using System.Drawing;
using System.Drawing.Imaging;

namespace FileTools.GIF
{
	public class GifSplitter
    {
        public IEnumerable<Bitmap> getFrames(Image originalImg)
        {
            var numberOfFrames = originalImg.GetFrameCount(FrameDimension.Time);

            for (int i = 0; i < numberOfFrames; i++)
            {
                originalImg.SelectActiveFrame(FrameDimension.Time, i);
                yield return originalImg.Clone() as Bitmap;
            }
        }

    }
}
