using FileTools.StreamHashCalculator;
using FileTools.Streams;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace FileTools.FileHashCalculator
{
    public class SkiaSharpGifHashCalculator(IHashCalculator inner) : IFileHashCalculator
    {
        public bool SizeMatters => false;

        public string Description => this.GetType().Name;

        public async Task<string> GetFileHash(IFileInfo file)
        {
            var bytes = GetBitmapBytes(file.FullName).Select(p => new MemoryStream(p));
            var compositeStream = new StreamOfStreams(bytes);
            return await inner.GetStreamHash(compositeStream);
        }

        public IEnumerable<byte[]> GetBitmapBytes(string fileName)
        {
            using SKCodec codec = SKCodec.Create(fileName);
            var imageInfo = new SKImageInfo(codec.Info.Width, codec.Info.Height);

            // create bitmap stub
            using var skBitmap = new SKBitmap(imageInfo);
            var pixelsPtr = skBitmap.GetPixels();

            for (int PageIndex = 0; PageIndex < codec.FrameCount; PageIndex++)
            {
                // decode particular frame into the bitmap
                var result = codec.GetPixels(imageInfo, pixelsPtr, new SKCodecOptions(PageIndex));

                if (result != SKCodecResult.Success)
                {
                    throw new InvalidOperationException($"{result}: Unable to get pixels of frame {PageIndex} of {fileName}");
                }

                yield return skBitmap.Bytes;
            }
        }
    }
}
