using SkiaSharp;

namespace FileTools.Phash
{
    internal static class SKBitmapExtensionMethods
    {
        public static byte[,] ToLuminanceMatrix(this SKBitmap original)
        {
            // Convert to a known format if needed
            SKBitmap bitmap = original.ColorType == SKColorType.Bgra8888
                ? original
                : original.Copy(SKColorType.Bgra8888);

            int width = bitmap.Width;
            int height = bitmap.Height;
            var luminance = new byte[width, height];
            var bytes = bitmap.GetPixelSpan();
            int bytesPerPixel = 4;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = (y * width + x) * bytesPerPixel;

                    byte blue = bytes[index + 0];
                    byte green = bytes[index + 1];
                    byte red = bytes[index + 2];

                    byte lum = (byte)((77 * red + 150 * green + 29 * blue) >> 8);
                    luminance[x, y] = lum;
                }
            }

            return luminance;
        }

    }
}
