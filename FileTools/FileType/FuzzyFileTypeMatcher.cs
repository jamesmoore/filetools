using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileTools.FileType
{
    public class FuzzyFileTypeMatcher(IEnumerable<byte?> bytes) : FileTypeMatcher
    {
        private readonly byte?[] bytes = bytes.ToArray();

		protected override bool MatchesPrivate(Stream stream)
        {
            foreach (var b in this.bytes)
            {
                var c = stream.ReadByte();
                if (c == -1 || (b.HasValue && c != b.Value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
