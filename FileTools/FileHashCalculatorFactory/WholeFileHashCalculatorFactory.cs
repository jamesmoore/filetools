using FileTools.FileHashCalculator;
using FileTools.StreamHashCalculator;
using System.IO.Abstractions;

namespace FileTools.FileHashCalculatorFactory
{
    class WholeFileHashCalculatorFactory(IHashCalculator hashCalculator) : IFileHashCalculatorFactory
    {
		public IFileHashCalculator GetFileHashCalculator(IFileInfo info)
        {
            return
                new LoggingFileHashCalculator(
                    new DefaultFileHashCalculator(hashCalculator)
                );
        }
    }
}
