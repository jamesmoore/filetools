using FileTools.BitmapDecoder;
using FileTools.FileHashCalculator;
using FileTools.PDFHasher;
using FileTools.StreamHashCalculator;
using System;
using System.IO.Abstractions;

namespace FileTools.FileHashCalculatorFactory
{
	class ContentsFileHashCalculatorFactory : IFileHashCalculatorFactory
	{
		public ContentsFileHashCalculatorFactory(
			ICompressedFileAPI compressedFileAPI, 
			IHashCalculator hashCalculator,
			IFileSystem fileSystem,
            IGhostScriptApi ghostScriptApi
			)
		{
			zipFileCRCCalculator = new(() => new ZipFileCRCCalculator(hashCalculator, compressedFileAPI));
			bitmapHashCloneCalculator = new(() => new BitmapHashCloneCalculator(hashCalculator, bitmapDecoder.Value));
			gifHashCalculator = new(() => new SkiaSharpGifHashCalculator(hashCalculator));
			tagLibRemovalHashCalculator = new(() => new TagLibRemovalHashCalculator(hashCalculator));
			pDFHashCalculator = new(() => new PDFHashCalculator(fileSystem, ghostScriptApi, hashCalculator));
			defaultFileHashCalculator = new(() => new DefaultFileHashCalculator(hashCalculator));

		}

		private readonly Lazy<IBitmapToByteArray> bitmapDecoder = new(() => new SkiaSharpBitmapDecoder());

		private readonly Lazy<ZipFileCRCCalculator> zipFileCRCCalculator;
		private readonly Lazy<BitmapHashCloneCalculator> bitmapHashCloneCalculator;
		private readonly Lazy<SkiaSharpGifHashCalculator> gifHashCalculator;
		private readonly Lazy<TagLibRemovalHashCalculator> tagLibRemovalHashCalculator;
		private readonly Lazy<PDFHashCalculator> pDFHashCalculator;
		private readonly Lazy<DefaultFileHashCalculator> defaultFileHashCalculator;


		public IFileHashCalculator GetFileHashCalculator(IFileInfo info)
		{
			return new LoggingFileHashCalculator(GetInnerHashCalculator(info));
		}

		private IFileHashCalculator GetInnerHashCalculator(IFileInfo info)
		{
			return info.Extension.ToLower() switch
			{
				".zip" or ".7z" or ".rar" or ".epub" => zipFileCRCCalculator.Value,
				".jpg" or ".jpeg" or ".png" or ".webp" => bitmapHashCloneCalculator.Value,
				".gif" => gifHashCalculator.Value,
				".mp3" => tagLibRemovalHashCalculator.Value,
				".pdf" => pDFHashCalculator.Value,
				_ => defaultFileHashCalculator.Value,
			};
		}
	}
}
