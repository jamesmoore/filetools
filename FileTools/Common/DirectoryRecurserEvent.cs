using System;
using System.IO.Abstractions;

namespace FileTools.Common
{
    public enum DirectoryRecurserEventType
    {
        ScanDirectory,
        ScanFile,
    }

    public class DirectoryRecurserEvent(DirectoryRecurserEventType type) : EventArgs
    {
		public DirectoryRecurserEventType EventType { get; set; } = type;

		public IDirectoryInfo DirectoryInfo { get; set; }

        public IFileInfo FileInfo { get; set; }

    }


}
