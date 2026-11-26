using System.IO.Abstractions;

namespace FileTools.HashStore
{
    public class HashStoreFactory(IFileSystem fileSystem) : IHashStoreFactory
    {
        public IHashStore GetHashStore()
        {
            return new SqliteHashStore(new DbPath(fileSystem, new DataPath(fileSystem)), true);
        }
    }
}