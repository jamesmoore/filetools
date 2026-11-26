using FileTools.Common;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace FileTools.FileType
{
    public class FileTypeRecurser(IDictionary<string, List<IFileInfo>> misMatches) : IRecursionHandler
    {
		public void ProcessDirectory(IDirectoryInfo di)
        {

        }

        public void ProcessFile(IDirectoryInfo root, IFileInfo fi)
        {
            var ftc = new FileTypeChecker();
            using var stream = new FileStream(fi.FullName, FileMode.Open);
			var matches = ftc.GetFileTypes(stream);
            if (matches.Any(p => p.ExtensionMatches(fi)))
            {
                return;
            }
            else
            {
                foreach (var match in matches)
                {
                    if (misMatches.ContainsKey(match.Extension) == false)
                    {
                        misMatches.Add(match.Extension, []);
                    }
                    misMatches[match.Extension].Add(fi);
                }
            }
        }
    }
}
