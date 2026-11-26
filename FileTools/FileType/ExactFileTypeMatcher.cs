using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileTools.FileType
{
    public class ExactFileTypeMatcher(IEnumerable<byte> bytes) : FileTypeMatcher
    {
        private readonly byte[] bytes = bytes.ToArray();

		protected override bool MatchesPrivate(Stream stream)
        {
            foreach (var b in this.bytes)
            {
                if (stream.ReadByte() != b)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
