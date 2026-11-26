using FileTools.HashStore;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileTools.FileHashCalculator
{
    public class CachedFileHashCalculator(IFileHashCalculator inner, IHashStore hashStore) : IFileHashCalculator
    {
		public bool SizeMatters => inner.SizeMatters;

		public async Task<string> GetFileHash(IFileInfo file)
		{
			var key = GenerateKey(file);
			return await hashStore.GetOrAddAsync(key, () => inner.GetFileHash(file));
		}

		private string GenerateKey(IFileInfo file)
        {
            var filepath = file.FullName.Substring(Path.GetPathRoot(file.FullName).Length);
            var filecode = $"{inner.Description}_{filepath}_{file.Length}_{file.LastWriteTimeUtc.Ticks}";
            return filecode;
        }

        public string Description => inner.Description + " (cached)";
    }
}
