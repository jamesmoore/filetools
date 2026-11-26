using System.Drawing;

namespace FileTools.Drawing
{
	public interface INativeBitmapToByteArray
	{
		byte[] GetByteArray(Bitmap bitmap);
	}
}
