using System.IO;

namespace FileTools.FileType
{
    public class RangeFileTypeMatcher(FileTypeMatcher matcher, int minimumStartLocation, int maximumStartLocation) : FileTypeMatcher
    {
		protected override bool MatchesPrivate(Stream stream)
        {
            for (var i = minimumStartLocation; i < maximumStartLocation; i++)
            {
                // Might want to check if i >= stream.Length.
                stream.Position = i;
                if (matcher.Matches(stream, resetPosition: false))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
