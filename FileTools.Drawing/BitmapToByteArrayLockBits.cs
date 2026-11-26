using FileTools.Drawing;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FileTools.BitmapDecoder
{
    public class BitmapToByteArrayLockBits : IBitmapToByteArray, INativeBitmapToByteArray
	{
        public byte[] GetByteArray(Bitmap bitmap)
        {
            BitmapData bmpdata = null;

			using var clone = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format32bppArgb);
			try
			{
				bmpdata = clone.LockBits(new Rectangle(0, 0, clone.Width, clone.Height), ImageLockMode.ReadOnly, clone.PixelFormat);
				int numbytes = bmpdata.Stride * clone.Height;
				byte[] bytedata = new byte[numbytes];
				IntPtr ptr = bmpdata.Scan0;
				Marshal.Copy(ptr, bytedata, 0, numbytes);
				return bytedata;
			}
			finally
			{
				if (bmpdata != null)
					clone.UnlockBits(bmpdata);
			}
		}

		public byte[] GetByteArray(string filename)
		{
			using var bmp1 = new Bitmap(filename);
			return GetByteArray(bmp1);
		}
	}
}
