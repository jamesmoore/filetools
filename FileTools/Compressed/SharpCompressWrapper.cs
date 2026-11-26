using FileTools.StreamHashCalculator;
using NLog;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace FileTools.Compressed
{
	public class SharpCompressWrapper(IFileSystem fileSystem) : ICompressedFileAPI
	{
		private readonly Logger logger = LogManager.GetCurrentClassLogger();

		public bool CompressDirectory(string filename, string directory, string format)
		{
			throw new NotImplementedException();
		}

		public bool Extract(string filename, string directory)
		{
			var options = new ExtractionOptions()
			{
				ExtractFullPath = true,
				Overwrite = true,
				PreserveAttributes = false,
				PreserveFileTime = true,
			};

			try
			{
				using var stream = fileSystem.File.OpenRead(filename);
				using var reader = ReaderFactory.OpenReader(stream);
				while (reader.MoveToNextEntry())
				{
					var entry = reader.Entry;
					if (!entry.IsDirectory)
					{
						reader.WriteEntryToDirectory(directory, options);
					}
				}
				return true;
			}
			catch(Exception ex)
			{
				logger.Error(ex);
				return false;
			} 
		}

		public CompressedFile List(string filename)
		{
			var compressedFile = new CompressedFile();
			using var stream = fileSystem.File.OpenRead(filename);
			using var entries = ArchiveFactory.OpenArchive(stream);
			var convertedEntries = entries.Entries.Where(entry => !entry.IsDirectory).Select(GetCompressedFileEntry);
			compressedFile.AddRange(convertedEntries);
			return compressedFile;
		}

		public async Task<bool> Test(string filename)
		{
			try
			{
				using var stream = fileSystem.File.OpenRead(filename);
				using var reader = ReaderFactory.OpenReader(stream);
				while (reader.MoveToNextEntry())
				{
					var entry = reader.Entry;
					if (!entry.IsDirectory)
					{
						using var ms = new MemoryStream();

						reader.WriteEntryTo(ms);

						ms.Position = 0;

						var calculator = new CRCHashCalculator();
                        var hash = await calculator.GetStreamHash(ms);
						var hashuint = (long)Convert.ToUInt32(hash, 16);
						var entryHash = entry.Crc;
						if (hashuint != entryHash) throw new InvalidOperationException();
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				logger.Error(ex);
				return false;
			}
		}

		private static CompressedFileEntry GetCompressedFileEntry(IEntry entry) => new CompressedFileEntry()
		{
			Crc = entry.Crc,
			Size = entry.Size,
			PackedSize = entry.CompressedSize,
			Modified = entry.LastModifiedTime ?? DateTime.MinValue,
			Path = entry.Key,
		};
	}
}
