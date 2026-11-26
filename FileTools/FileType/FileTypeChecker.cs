using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileTools.FileType
{
    public class FileTypeChecker
    {
        private static readonly FileTypeMatcher zipFileTypeMatcher = new ExactFileTypeMatcher([0x50, 0x4b, 0x03, 0x04,]);
        private static readonly FileTypeMatcher sevenzipFileTypeMatcher = new ExactFileTypeMatcher([0x37, 0x7a, 0xbc, 0xaf, 0x27, 0x1c]);
        private static readonly ExactFileTypeMatcher ebook = new ExactFileTypeMatcher("BOOKMOBI"u8.ToArray());
        private static readonly ExactFileTypeMatcher rarMatcher = new ExactFileTypeMatcher("Rar!"u8.ToArray());
        private static readonly ExactFileTypeMatcher cfbfMatcher = new ExactFileTypeMatcher([0xD0, 0xCF, 0x11, 0xE0]);
        private static readonly ExactFileTypeMatcher misxMatcher = new ExactFileTypeMatcher("MSix"u8.ToArray());
        private static readonly ExactFileTypeMatcher paintDotNetMatcher = new ExactFileTypeMatcher("PDN3"u8.ToArray());
        private static readonly ExactFileTypeMatcher tiffLittleEndianMatcher = new ExactFileTypeMatcher([0x49, 0x49, 0x2A, 0x00]);
        private static readonly ExactFileTypeMatcher tiffBigEndianMatcher = new ExactFileTypeMatcher([0x4D, 0x4D, 0x00, 0x2A]);
        private static readonly ExactFileTypeMatcher sqliteMatcher = new ExactFileTypeMatcher("SQLite format 3\0"u8.ToArray());

        /// <summary>
        /// See https://www.garykessler.net/library/file_sigs.html
        /// https://www.nationalarchives.gov.uk/PRONOM/Format/proFormatSearch.aspx?status=detailReport&id=1144&strPageToDisplay=signatures
        /// </summary>
        private static readonly IList<FileType> knownFileTypes =
        [
            new FileType("Bitmap", ".bmp", new ExactFileTypeMatcher([0x42, 0x4d])),
            new FileType("Portable Network Graphic", ".png", new ExactFileTypeMatcher([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A])),
            new FileType("JPEG", ".jpg",new FuzzyFileTypeMatcher([0xFF, 0xD8, 0xFF])), // , 0xE0, null, null, 0x4A, 0x46, 0x49, 0x46, 0x00})),
            new FileType("Graphics Interchange Format 87a", ".gif",new ExactFileTypeMatcher("GIF87a"u8.ToArray())),
            new FileType("Graphics Interchange Format 89a", ".gif",new ExactFileTypeMatcher("GIF89a"u8.ToArray())),
            new FileType("Portable Document Format", ".pdf", new RangeFileTypeMatcher(new ExactFileTypeMatcher("%PDF"u8.ToArray()), 0, 1019)),
            new FileType("Mobi", ".mobi", new RangeFileTypeMatcher(ebook, 60, 61)),
            new FileType("AZW3", ".azw3", new RangeFileTypeMatcher(ebook, 60, 61)),
            new FileType("Webm", ".webm", new ExactFileTypeMatcher([0x1A, 0x45, 0xDF, 0xA3])),
            new FileType("MP4", ".mp4",new FuzzyFileTypeMatcher([00, 00, 00, null, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56])), // ftypMSNV
            new FileType("MP4", ".mp4",new FuzzyFileTypeMatcher([00, 00, 00, null, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D])), // ftypisom
            new FileType("QuickTime Movie", ".mov",new FuzzyFileTypeMatcher([00, 00, 00, null, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20])), // ftypqt
            new FileType("AVIF", ".avif",new FuzzyFileTypeMatcher([00, 00, 00, null, 0x66, 0x74, 0x79, 0x70, 0x61, 0x76, 0x69, 0x66])),
            new FileType("M4V", ".m4v",new FuzzyFileTypeMatcher([00, 00, 00, null, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32])), //ftypmp42
            new FileType("xml", ".xml",new ExactFileTypeMatcher("<?xml"u8.ToArray())),
            new FileType("svg", ".svg",new ExactFileTypeMatcher("<svg "u8.ToArray())),
            new FileType("Epub", ".epub", zipFileTypeMatcher),
            new FileType("Zip", ".zip", zipFileTypeMatcher),
            new FileType("Windows Installer Package", ".msi", cfbfMatcher),
            new FileType("Windows App Package", ".msix", misxMatcher), // "Msix"
            new FileType("Windows App Bundle", ".msixbundle", misxMatcher), // "Msix"
            new FileType("Microsoft Word docx", ".docx", zipFileTypeMatcher),
            new FileType("Microsoft Word 97-2003 Binary File", ".doc", cfbfMatcher),
            new FileType("Microsoft Excel xlsx", ".xlsx", zipFileTypeMatcher),
            new FileType("Microsoft Excel 97-2003 Binary File", ".xls", cfbfMatcher),
            new FileType("Microsoft Powerpoint pptx", ".pptx", zipFileTypeMatcher),
            new FileType("Comic Book Zip Archive", ".cbz", zipFileTypeMatcher),
            new FileType("7zip", ".7z", sevenzipFileTypeMatcher),
            new FileType("Comic Book Zip Archive", ".cb7", sevenzipFileTypeMatcher),
            new FileType("WebP", ".webp",new FuzzyFileTypeMatcher([0x52, 0x49, 0x46, 0x46, null, null, null, null, 0x57, 0x45, 0x42, 0x50])),
            new FileType("URL", ".url",new ExactFileTypeMatcher("[InternetShortcut]"u8.ToArray())),
            new FileType("Macromedia Flash", ".swf",new ExactFileTypeMatcher("CWS"u8.ToArray())),
            new FileType("Windows Executable", ".exe", new ExactFileTypeMatcher("MZ"u8.ToArray())), // "MZ"
            new FileType("Windows Dynamic Link Library", ".dll", new ExactFileTypeMatcher("MZ"u8.ToArray())), // "MZ"
            new FileType("Waveform Audio", ".wav", new FuzzyFileTypeMatcher([0x52, 0x49, 0x46, 0x46, null, null, null, null, 0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20])), // "RIFF....WAVEfmt "
            new FileType("AVI File", ".avi", new FuzzyFileTypeMatcher([0x52, 0x49, 0x46, 0x46, null, null, null, null, 0x41, 0x56, 0x49, 0x20, 0x4C, 0x49, 0x53, 0x54])),// "RIFF....AVI LIST"
            new FileType("WinRAR Compressed Archive", ".rar", rarMatcher), // "Rar!"
            new FileType("Comic Book RAR Archive", ".cbr", rarMatcher),
            // new FileType("MS-DOS Executable", ".com", new ExactFileTypeMatcher([0xCD, 0x21])), // Int 21h DOS call instruction
            new FileType("Rich Text Format", ".rtf",new ExactFileTypeMatcher(@"{\rtf"u8.ToArray())),
            new FileType("Paint.NET Image", ".pdn", paintDotNetMatcher),
            new FileType("GZip Compressed Archive", ".gz", new ExactFileTypeMatcher([0x1F, 0x8B])),
            new FileType("BZip2 Compressed Archive", ".bz2", new ExactFileTypeMatcher("BZh"u8.ToArray())),
            new FileType("XZ Compressed Archive", ".xz", new ExactFileTypeMatcher([0xFD, 0x37, 0x7A, 0x58, 0x5A, 0x00])),
            new FileType("FLAC Audio", ".flac", new ExactFileTypeMatcher("fLaC"u8.ToArray())),
            new FileType("Ogg Media", ".ogg", new ExactFileTypeMatcher("OggS"u8.ToArray())),
            new FileType("Icon", ".ico", new ExactFileTypeMatcher([0x00, 0x00, 0x01, 0x00])),
            new FileType("Tagged Image File Format", ".tif", tiffLittleEndianMatcher),
            new FileType("Tagged Image File Format", ".tiff", tiffLittleEndianMatcher),
            new FileType("Tagged Image File Format", ".tif", tiffBigEndianMatcher),
            new FileType("Tagged Image File Format", ".tiff", tiffBigEndianMatcher),
            new FileType("Adobe Photoshop Document", ".psd", new ExactFileTypeMatcher("8BPS"u8.ToArray())),
            new FileType("SQLite Database", ".sqlite", sqliteMatcher),
            new FileType("Web Open Font Format", ".woff", new ExactFileTypeMatcher("wOFF"u8.ToArray())),
            new FileType("Web Open Font Format 2", ".woff2", new ExactFileTypeMatcher("wOF2"u8.ToArray())),
            // Unreliable
            new FileType("MPEG Audio (Layer III)", ".mp3", new FuzzyFileTypeMatcher([null, null, 0xFF, 0xFB])) // ID3 tag start marker
        ];

        public IEnumerable<FileType> GetFileTypes(Stream stream)
        {
            var matches = knownFileTypes.Where(fileType => fileType.Matches(stream)).DefaultIfEmpty(FileType.Unknown);
            return matches;
        }
    }
}
