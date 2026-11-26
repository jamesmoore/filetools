using System.IO;
using System.IO.Abstractions;

namespace FileTools.FileType
{
    public class FileType(string name, string extension, FileTypeMatcher matcher)
	{

		public string Name { get; } = name;

		public string Extension { get; } = extension;

		public static FileType Unknown { get; } = new FileType("unknown", string.Empty, null);

		public bool Matches(Stream stream)
        {
            return matcher == null || matcher.Matches(stream);
        }

        public bool ExtensionMatches(IFileInfo fi)
        {
            return Extension.Equals(fi.Extension, System.StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
