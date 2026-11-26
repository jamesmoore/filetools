using System;
using System.IO;

namespace FileTools.FileType
{
    public abstract class FileTypeMatcher
    {
        public bool Matches(Stream stream, bool resetPosition = true)
        {
            ArgumentNullException.ThrowIfNull(stream);
            if (!stream.CanRead || (stream.Position != 0 && !stream.CanSeek))
            {
                throw new ArgumentException("File contents must be a readable stream", "stream");
            }
            if (stream.Position != 0 && resetPosition)
            {
                stream.Position = 0;
            }

            return MatchesPrivate(stream);
        }

        protected abstract bool MatchesPrivate(Stream stream);
    }
}
