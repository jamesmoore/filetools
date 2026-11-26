using FileTools.Drawing;
using System.Drawing;

namespace FileTools.BitmapDecoder
{

    public class BitmapToByteArrayGetPixel : IBitmapToByteArray, INativeBitmapToByteArray
	{
        public byte[] GetByteArray(Bitmap bitmap)
        {
            var s1 = bitmap.Size;

			using var memoryStream = new MemoryStream(s1.Height * s1.Width * 4);
			using var writer = new BinaryWriter(memoryStream);
			for (int y = 0; y < s1.Height; y++)
				for (int x = 0; x < s1.Width; x++)
				{
					var c1 = bitmap.GetPixel(x, y);
					writer.Write(c1.ToArgb());
				}
			writer.Flush();
			memoryStream.Position = 0;
			return memoryStream.ToArray();
		}

		public byte[] GetByteArray(string filename)
		{
			using var bmp1 = new Bitmap(filename);
			return GetByteArray(bmp1);
		}
	}
}
