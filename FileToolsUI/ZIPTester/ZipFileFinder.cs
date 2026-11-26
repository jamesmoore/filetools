using System.Collections.Generic;
using System.IO.Abstractions;
using FileTools.Common;

namespace FileTools.ZipTester
{
    public class ZipFileFinder : IRecursionHandler
    {
        public Dictionary<IFileInfo, string> ZipList { get; set; } = new Dictionary<IFileInfo, string>();

        public void ProcessDirectory(IDirectoryInfo di)
        {

        }

        public void ProcessFile(IDirectoryInfo root, IFileInfo fi)
        {
            var extensions = new List<string>()
            {
                ".zip",".arj" ,".rar" ,".7z", ".epub", ".cbr", ".cbz"
            };

            if (extensions.Contains(fi.Extension.ToLower()))
            {
                ZipList.Add(fi, "Untested");
            }
        }


    }
}
