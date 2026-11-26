using NLog;
using System;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileTools.FileHashCalculator
{
    public class LoggingFileHashCalculator(IFileHashCalculator inner) : IFileHashCalculator
    {
		private readonly Logger logger = LogManager.GetCurrentClassLogger();

		public bool SizeMatters => inner.SizeMatters;

        public string Description => inner.Description;

        public async Task<string> GetFileHash(IFileInfo file)
        {
            var s = new Stopwatch();
            s.Start();
            try
            {
                var returnValue = await inner.GetFileHash(file);
                s.Stop();
                logger.Info($"{inner.GetType().Name} calculated hash for \"{file.FullName}\", {file.Length} bytes, {s.Elapsed.TotalMilliseconds} ms");
                return returnValue;
            }
            catch (Exception ex)
            {
                s.Stop();
                logger.Error(ex, $"{inner.GetType().Name} {ex.Message} exception for \"{file.FullName}\", {file.Length} bytes, {s.Elapsed.TotalMilliseconds} ms");
                throw;
            }
        }
    }
}
