using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using FileTools.StreamHashCalculator;

namespace FileTools.FileHashCalculator
{
    public class DefaultFileHashCalculator(IHashCalculator inner) : IFileHashCalculator
    {
		public async Task<string> GetFileHash(IFileInfo file)
        {
			using var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 1000000);
			var returnValue = await inner.GetStreamHash(fs);
			return returnValue;
		}

		public bool SizeMatters => true;

		public string Description => this.GetType().Name;

    }
}
