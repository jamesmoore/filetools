using System.IO;
using System.Threading.Tasks;

namespace FileTools.StreamHashCalculator
{
    public interface IHashCalculator
    {
        Task<string> GetStreamHash(Stream file);
    }
}
