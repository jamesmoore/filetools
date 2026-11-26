using FileTools.PDFHasher;
using FileTools.StreamHashCalculator;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileTools.FileHashCalculator
{
	internal class PDFHashCalculator(IFileSystem fileSystem, IGhostScriptApi ghostScriptApi, IHashCalculator hasher) : IFileHashCalculator
	{
		public bool SizeMatters => false;

		public string Description => "PDF exported content hash";

		public async Task<string> GetFileHash(IFileInfo file)
		{
			var pdfHasher = new PDFHasher.PDFHasher(fileSystem, ghostScriptApi, hasher);
			return await pdfHasher.GetPDFContentsHash(file.FullName);
		}
	}
}
