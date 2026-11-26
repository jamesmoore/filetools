using System.Collections.Generic;
using System.IO.Abstractions;
using FileTools.Common;

namespace FileTools.EmptyDirectoryFinder
{
	public class EmptyDirectoryFinder : IRecursionHandler
    {
		public List<IDirectoryInfo> EmptyList { get; } = new List<IDirectoryInfo>();

		public void ProcessDirectory(IDirectoryInfo di)
        {
            try
            {
                if (di.GetDirectories().Length == 0 && di.GetFiles().Length == 0)
                {
                    EmptyList.Add(di);
                }
            }
            catch(System.IO.IOException) {

            }
        }

        public void ProcessFile(IDirectoryInfo root, IFileInfo fi)
        {

        }
    }
}
