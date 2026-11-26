using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileTools.FileHashCalculator
{
    public interface IFileHashCalculator
    {
        Task<string> GetFileHash(IFileInfo file);
        bool SizeMatters { get; }
        string Description { get; }
    }
}
