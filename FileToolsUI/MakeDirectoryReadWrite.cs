using FileTools.Common;
using System.IO.Abstractions;

namespace FileTools
{
	public class MakeDirectoryReadWrite : IRecursionHandler
    {
        public void ProcessDirectory(IDirectoryInfo di)
        {
            di.ClearAttributes();
        }

        public void ProcessFile(IDirectoryInfo di, IFileInfo fi)
        {
            fi.ClearAttributes();
        }
    }
}
