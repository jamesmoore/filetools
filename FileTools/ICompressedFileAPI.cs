using System.Threading.Tasks;
using FileTools.Compressed;

namespace FileTools
{
    public interface ICompressedFileAPI
    {
        bool CompressDirectory(string filename, string directory, string format);
        bool Extract(string filename, string directory);
        CompressedFile List(string filename);
        Task<bool> Test(string filename);
    }
}