using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace FileTools.PDFHasher
{
    public class GhostScriptLocator(IFileSystem fileSystem)
    {
        public string GetLocation()
        {
            const string Path1 = @"C:\Program Files\gs\";

            if (fileSystem.Directory.Exists(Path1))
            {
                var candidates = fileSystem.Directory.GetFiles(Path1, "gswin64c.exe", SearchOption.AllDirectories);
                var candidate = candidates.FirstOrDefault();
                return candidate;
            }
            else
            {
                return null;
            }
        }
    }
}
