using FileTools.Common;
using FileTools.PDFHasher;
using FileTools.StreamHashCalculator;
using System;
using System.IO.Abstractions;

namespace FileTools.FileHashCalculatorFactory
{
	public class FileHashCalculatorAbstractFactory(
		IFileSystem fileSystem,
		ICompressedFileAPI compressedFileAPI, 
		IHashCalculator hashCalculator,
		IGhostScriptApi ghostScriptApi
		)
	{
		public IFileHashCalculatorFactory GetHashCalculatorFactory(FileProcessingMethod fileProcessingMethod) => fileProcessingMethod switch
		{
			FileProcessingMethod.WholeFile => new WholeFileHashCalculatorFactory(hashCalculator),
			FileProcessingMethod.Contents => new ContentsFileHashCalculatorFactory(
				compressedFileAPI, 
				hashCalculator,
				fileSystem,
				ghostScriptApi
				),
			FileProcessingMethod.Metadata => new MetaDataFileHashCalculatorFactory(hashCalculator),
			_ => throw new NotImplementedException(),
		};
	}
}
