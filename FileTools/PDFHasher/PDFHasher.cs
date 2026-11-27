using FileTools.StreamHashCalculator;
using FileTools.Streams;
using NLog;
using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace FileTools.PDFHasher
{
    public class PDFHasher(
        IFileSystem fileSystem,
        IGhostScriptApi ghostScriptApi,
        IHashCalculator hashCalculator)
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public async Task<string> GetPDFContentsHash(string filename)
        {
            var tempPath = GetTemporaryDirectory();

            if (ghostScriptApi.IsAvailable == false)
            {
                logger.Warn($"PDF hashing disabled for {filename}");
                return null;
            }

            var result = ghostScriptApi.ExtractPdfToPath(filename, tempPath);

            try
            {
                if (result.ExitCode != 0)
                {
                    throw new Exception(result.Output);
                }
                else
                {
                    var files = fileSystem.Directory.GetFiles(tempPath);

                    if (files.Length == 0)
                    {
                        throw new InvalidOperationException($"No PDF files exported: {result.Output}");
                    }
                    else
                    {
                        // Ghostscript pads page numbers to at least four digits.
                        var streams = files.OrderBy(path => path.Length)
                            .ThenBy(path => path, StringComparer.Ordinal)
                            .Select(fileSystem.File.OpenRead);
                        using var combined = new StreamOfStreams(streams);
                        var hash = await hashCalculator.GetStreamHash(combined);
                        return hash;
                    }
                }
            }
            finally
            {
                fileSystem.Directory.Delete(tempPath, true);
            }
        }

        public string GetTemporaryDirectory()
        {
            var tempDirectory = fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), fileSystem.Path.GetRandomFileName());
            fileSystem.Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
