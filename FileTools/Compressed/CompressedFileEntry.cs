using System;

namespace FileTools.Compressed
{
    public class CompressedFileEntry
    {
        public string Path { get; set; }

        public string Folder { get; set; }

        public long Size { get; set; }

        public long PackedSize { get; set; }

        public DateTime Modified { get; set; }

        public string Attributes { get; set; }

        public long Crc { get; set; }

        //Path = FONTINTR.FF
        //Folder = -
        //Size = 1128
        //Packed Size = 1040
        //Modified = 1994-09-14 11:45:18
        //Attributes = ....A
        //Encrypted = -
        //Comment =
        //CRC = 894B3240
        //Method = Deflate
        //Host OS = FAT

    }

}
