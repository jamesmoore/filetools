using FileTools.FileHashCalculator;
using System.IO.Abstractions;

namespace FileTools.FileHashCalculatorFactory
{
    public interface IFileHashCalculatorFactory
    {
        IFileHashCalculator GetFileHashCalculator(IFileInfo info);
    }
}