using FileTools.StreamHashCalculator;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileTools.FileHashCalculator
{
    class MP3MetadataHashCalculator(IHashCalculator inner) : IFileHashCalculator
    {
		public async Task<string> GetFileHash(IFileInfo file)
        {
            var tagLibFile = TagLib.File.Create(file.FullName);
            var tags = tagLibFile.Tag;
            var properties = tagLibFile.Properties;

            var duration = new TimeSpan(properties.Duration.Hours, properties.Duration.Minutes, properties.Duration.Seconds.RoundOff());
            var title = tags.Title;
            if (string.IsNullOrWhiteSpace(title))
            {
                title = Guid.NewGuid().ToString();
            }
            var summaryString = (tags.JoinedArtists + tags.Album + tags.Track + title + duration).Replace(" ", "").Replace("'","").ToLower();

            var bytes = GetBytes(summaryString);

			using var ms = new MemoryStream(bytes);
			var crc = await inner.GetStreamHash(ms);
			return crc;
		}

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
			Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
			Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

		public bool SizeMatters => false;

		public string Description => this.GetType().Name;
    }

    public static class ExtensionMethods
    {
        public static int RoundOff(this int i)
        {
            return ((int)Math.Round(i / 10.0)) * 10;
        }
    }
}
