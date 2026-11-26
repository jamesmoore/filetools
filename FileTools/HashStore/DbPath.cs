using System.IO.Abstractions;

namespace FileTools.HashStore
{
	public class DbPath(IFileSystem fileSystem, DataPath dataPath)
	{
		public string GetPath() => fileSystem.Path.Combine(dataPath.GetPath(), "filehash.db");

		internal void CreateIfMissing()
		{
			dataPath.CreateIfMissing();
		}
	}
}
