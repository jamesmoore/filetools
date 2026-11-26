using FileTools.Phash;
using NLog;
using Shipwreck.Phash;
using SkiaSharp;
using System.Diagnostics;
using System.IO.Abstractions;

namespace FileTools.FileHashCalculator
{
    public class PHashCalculator : IFileHashCalculator
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public bool SizeMatters => true;

        public Task<string> GetFileHash(IFileInfo file)
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                using var bitmap = SKBitmap.Decode(file.FullName);
                var hash = ImagePhash.ComputeDigest(new SKBitmapByteImage(bitmap));
                var base64Hash = hash.Coefficients.ToBase64();
                logger.Info($"Phash for {file.FullName} done in {sw.ElapsedMilliseconds}ms");
                return Task.FromResult(base64Hash);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error phashing file {file.FullName}");
            }
            return Task.FromResult<string>(null);
        }

        public string Description => this.GetType().Name;
    }
}
