using FileTools.Drawing;
using FileTools.GIF;
using FileTools.StreamHashCalculator;
using System.Drawing;
using System.IO.Abstractions;

namespace FileTools.FileHashCalculator
{
	public class GifHashCalculator(IHashCalculator inner, INativeBitmapToByteArray bitmapToByteArray) : IFileHashCalculator
	{
		public bool SizeMatters => false;

		public async Task<string> GetFileHash(IFileInfo file)
		{
            string fullName = file.FullName;
            using var memoryStream = new MemoryStream();
			foreach (var bytes in GetBitmapBytes(fullName))
			{
				var temphash = await inner.GetStreamHash(new MemoryStream(bytes));
				memoryStream.Write(bytes, 0, bytes.Length);
			}
			memoryStream.Position = 0;
			return await inner.GetStreamHash(memoryStream);
		}

		public string Description => this.GetType().Name;


		public IEnumerable<byte[]> GetBitmapBytes(string fileName)
		{
            using var image = Image.FromFile(fileName);
            var frames = new GifSplitter().getFrames(image);
			foreach (var frame in frames)
			{
                var bytes = bitmapToByteArray.GetByteArray(frame);
				yield return bytes;
				frame.Dispose();
            }
        }
	}
}
