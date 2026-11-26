using FileTools.StreamHashCalculator;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileTools.FileHashCalculator
{
    class ZipFileCRCCalculator(IHashCalculator inner, ICompressedFileAPI compressedFileAPI) : IFileHashCalculator
    {
		public async Task<string> GetFileHash(IFileInfo file)
        {
            var zipcontents = compressedFileAPI.List(file.FullName);
            var signature = zipcontents.CRC;

			using var ms = new MemoryStream(signature);
			return await inner.GetStreamHash(ms);
		}


		public bool SizeMatters => false;
		public string Description => this.GetType().Name;

    }
}
