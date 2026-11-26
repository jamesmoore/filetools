using System;
using System.Threading.Tasks;

namespace FileTools.HashStore
{
    public interface IHashStore : IDisposable
    {
		void Clear();
		string GetHash(string key);
        Task StoreHash(string key, string hash);
        void ClearFailed();
    }
}