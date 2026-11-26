using System.IO.Abstractions;

namespace FileTools.Common
{
	public interface IRecursionHandler
    {
        void ProcessDirectory(IDirectoryInfo di);
        void ProcessFile(IDirectoryInfo root, IFileInfo fi);
    }
}
