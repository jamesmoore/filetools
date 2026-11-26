using FileTools.FileHashCalculator;
using FileTools.StreamHashCalculator;
using System.IO.Abstractions;

namespace FileTools.FileHashCalculatorFactory
{
    class MetaDataFileHashCalculatorFactory(IHashCalculator hashCalculator) : IFileHashCalculatorFactory
    {
		public IFileHashCalculator GetFileHashCalculator(IFileInfo info) => info.Extension.ToLower() switch
		{
			".mp3" or ".flac" => new MP3MetadataHashCalculator(hashCalculator),
			_ => new DefaultFileHashCalculator(hashCalculator),
		};
	}
}
