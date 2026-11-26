using FileTools.Common;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace FileTools.Dupecheck
{
    public class DuplicateRecurser : IRecursionHandler
    {
        public Dictionary<string, IFileInfo> FileList { get; set; }

        public DuplicateRecurser()
        {
            this.FileList = new Dictionary<string, IFileInfo>();

        }

        public void ProcessDirectory(IDirectoryInfo di)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void ProcessFile(IDirectoryInfo root, IFileInfo fi)
        {
            try
            {
                var filename = fi.FullName;
                if (this.FileList.ContainsKey(filename) == false)
                {
                    this.FileList.Add(filename, fi);
                }
            }
            catch (System.IO.PathTooLongException)
            {

            }
        }
    }
}
