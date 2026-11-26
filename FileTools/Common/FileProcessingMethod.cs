using System;

namespace FileTools.Common
{
    [Flags]
    public enum FileProcessingMethod
    {
        /// <summary>
        /// Treat zip files as a single file
        /// </summary>
        WholeFile = 0,
        /// <summary>
        /// Treat zip files intelligently for CRC
        /// </summary>
        Contents = 1,
        /// <summary>
        /// Treat zip files as a directory as individual files
        /// </summary>
        Directory = 2,
        Metadata = 4,
    }
}
