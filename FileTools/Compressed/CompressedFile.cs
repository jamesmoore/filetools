using System;
using System.Collections.Generic;
using System.Linq;

namespace FileTools.Compressed
{
    public class CompressedFile : List<CompressedFileEntry>
    {
        public byte[] CRC
        {
            get
            {
                var strings = this.Where(p => p.Path.ToLower() != "file_id.diz").OrderBy(p => p.Crc).Select(p => p.Crc.ToString()).ToArray();
                var singleString = string.Join(",", strings);
                var bytes = GetBytes(singleString);
                return bytes;
            }
        }

        byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

    }

}
