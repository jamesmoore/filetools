using System;
using SkiaSharp;

namespace FileTools.BitmapDecoder
{
    public class SkiaSharpBitmapDecoder : IBitmapToByteArray
    {
        public byte[] GetByteArray(string filename)
        {
            using var x = SKBitmap.Decode(filename);
            if (x == null)
            {
                throw new InvalidOperationException($"Unable to decode image");
            }
            return x.Bytes;
        }
    }
}
