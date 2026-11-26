using FileTools.StreamHashCalculator;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileTools.FileHashCalculator
{

    class TagLibRemovalHashCalculator(IHashCalculator inner) : IFileHashCalculator
    {
		public async Task<string> GetFileHash(IFileInfo file)
        {
            var bytes = File.ReadAllBytes(file.FullName);

            var tempFileName = Path.GetTempFileName().ToLower();
            var tempFileNamemp3 = tempFileName.Replace(".tmp", ".mp3");
            File.WriteAllBytes(tempFileNamemp3, bytes);

            using (var tagLibFile = TagLib.File.Create(tempFileNamemp3))
            {
                tagLibFile.RemoveTags(TagLib.TagTypes.AllTags);
                tagLibFile.Tag.Pictures = new TagLib.IPicture[0];
                tagLibFile.Save();
            }
            var newBytes = File.ReadAllBytes(tempFileNamemp3);
            File.Delete(tempFileNamemp3);
            File.Delete(tempFileName);

            Debug.WriteLine(file.Name + "\t" + newBytes.Length + "\t" + newBytes.Length.ToString("X"));

			using var ms = new MemoryStream(newBytes);
			var crc = await inner.GetStreamHash(ms);
			return crc;
		}

		public bool SizeMatters => false;
		public string Description => this.GetType().Name;

    }

}
