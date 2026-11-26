namespace FileTools.HashStore
{
    public interface IHashStoreFactory
    {
        IHashStore GetHashStore();
    }
}