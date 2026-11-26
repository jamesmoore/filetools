using Shipwreck.Phash.Imaging;
using SkiaSharp;

namespace FileTools.Phash
{
    internal class SKBitmapByteImage : IByteImage
    {
        private readonly SKBitmap bitmap;
        private readonly byte[,] Luminance;

        public SKBitmapByteImage(SKBitmap sKBitmap)
        {
            bitmap = sKBitmap;
            Luminance = sKBitmap.ToLuminanceMatrix();
        }

        public byte this[int x, int y] => Luminance[x,y];

        public int Width => bitmap.Width;

        public int Height => bitmap.Height;
    }
}
