using Dapper;
using Microsoft.Data.Sqlite;
using NLog;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FileTools.HashStore
{
    public class SqliteHashStore : IHashStore, IDisposable
	{
		private readonly DbPath dbPath;
		private readonly bool keepAlive;
		const string TableName = "HashTable";
		private SqliteTransaction transaction;
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();
		private readonly string insertSql;

		public SqliteHashStore(DbPath dbPath, bool keepAlive = false)
		{
			this.dbPath = dbPath;
			this.keepAlive = keepAlive;
			this.ConnectionString = new Lazy<string>(() => GetConnectionString());

			dbPath.CreateIfMissing();

			var columns = new string[]
			{
				"HashKey",
				"HashValue",
			};

			var tabledef = columns.Select(p => (
				Column: p,
				Parameter: "@" + p.Replace("[", "").Replace("]", ""),
				DataType: "TEXT",
				IsPrimaryKey: p == "HashKey"));

			insertSql = $@"INSERT INTO {TableName}({columns.ToCommaSeparated()}) 
                VALUES ( {tabledef.Select(p => p.Parameter).ToCommaSeparated()} )
				ON CONFLICT(HashKey) DO UPDATE SET
					HashValue = excluded.HashValue
				WHERE {TableName}.HashValue IS NULL;";

			// Setup table if absent https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/types
			ExecuteOnConnection(connection => connection.Execute(@$"CREATE TABLE IF NOT EXISTS {TableName} (
				{tabledef.Select(p => $"{p.Column} {p.DataType}{(p.IsPrimaryKey ? " PRIMARY KEY" : "")}").ToCommaSeparated()}
				);"));
		}

		private string GetConnectionString()
		{
			var path = dbPath.GetPath();
			logger.Info($"Using db at {path}");
			var connectionString = $"Data Source={path}";
			return connectionString;
		}

		private readonly Lazy<string> ConnectionString;

		private SqliteConnection GetConnection()
		{
			logger.Info($"Opening connection");
			var connectionString = ConnectionString.Value;
			var connection = new SqliteConnection(connectionString);
			connection.Open();
			return connection;
		}

		private SqliteConnection sqliteConnection;

		private T ExecuteOnConnection<T>(Func<SqliteConnection, T> func)
		{
			if (this.transaction != null) 
			{
				return func(transaction.Connection);
			}
			else if (keepAlive)
			{
				sqliteConnection ??= GetConnection();
				return func(sqliteConnection);
			}
			else
			{
				using var connection = GetConnection();
				return func(connection);
			}
		}

        public string GetHash(string key)
		{
			var reader = ExecuteOnConnection(connection => {

				var hash = connection.QueryFirstOrDefault<string>(
				$@"SELECT HashValue
				FROM {TableName}
				WHERE HashKey = @key;",
				new { key },
				this.transaction);

				return hash;

				});

			return reader;
		}

		public async Task StoreHash(string key, string hash)
		{
            ExecuteOnConnection(connection => connection.Execute(
				insertSql,
				new
				{
					HashKey = key,
					HashValue = hash,
				},
				this.transaction
			));
		}

		public void BeginTransaction()
		{
			this.transaction ??= (sqliteConnection ?? GetConnection()).BeginTransaction();
		}

		public void CommitTransaction()
		{
			if (this.transaction != null)
			{
				var connection = this.transaction.Connection;
				this.transaction.Commit();
				this.transaction.Dispose();
				this.transaction = null;
				if (keepAlive == false)
				{
					connection.Close();
					connection.Dispose();
				}
			}
		}

		public void Dispose()
		{
			logger.Info("Data source dispose");
			this.CommitTransaction();
			sqliteConnection?.Close();
			sqliteConnection?.Dispose();
		}

		public void Clear()
		{
			ExecuteOnConnection(connection => connection.Execute(@$"DELETE FROM {TableName};"));
		}

		public void ClearFailed()
		{
			ExecuteOnConnection(connection => connection.Execute(@$"DELETE FROM {TableName} where HashValue IS NULL;"));
		}
	}
}
