using SkiaSharp;

namespace FileTools.PHash
{
    public record Match(string File1, string File2, double Score)
    {
        private bool? ismatch;

        public bool IsExactMatch()
        {
            if (ismatch.HasValue) return ismatch.Value;

            bool isMatch;
            using (var bmp1 = SKBitmap.Decode(this.File1))
            using (var bmp2 = SKBitmap.Decode(this.File2))
            {
                isMatch = IsMatch(bmp1, bmp2);
            }

            ismatch = isMatch;

            return isMatch;
        }

        private bool IsMatch(SKBitmap bmp1, SKBitmap bmp2)
        {
            if (bmp1.Width != bmp2.Width || bmp1.Height != bmp2.Height)
                return false;

            var bytes1 = bmp1.GetPixelSpan();
            var bytes2 = bmp2.GetPixelSpan();

            return bytes1.SequenceEqual(bytes2);
        }
    }
}
