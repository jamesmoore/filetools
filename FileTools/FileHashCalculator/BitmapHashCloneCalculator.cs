using FileTools.BitmapDecoder;
using FileTools.StreamHashCalculator;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileTools.FileHashCalculator
{
    public class BitmapHashCloneCalculator(IHashCalculator inner, IBitmapToByteArray bitmapToByteArray) : IFileHashCalculator
    {
		public bool SizeMatters => false;

        public async Task<string> GetFileHash(IFileInfo file)
        {
			using var memoryStream = new MemoryStream(bitmapToByteArray.GetByteArray(file.FullName));
			return await inner.GetStreamHash(memoryStream);
		}
        public string Description => this.GetType().Name;

    }
}
