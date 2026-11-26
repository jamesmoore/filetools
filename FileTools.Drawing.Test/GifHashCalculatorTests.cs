using System.IO.Abstractions;
using FileTools.FileHashCalculator;
using FileTools.StreamHashCalculator;
using FileTools.BitmapDecoder;

namespace FileTools.Drawing.Test
{
    [TestClass]
    public sealed class GifHashCalculatorTests
    {
        [DataTestMethod]
        [DataRow("0000.gif", 13, "k2dySRxCLZIMiUMXo/ZX5aKN0PeGbptqa4Me7I3nxR0=")]
        [DataRow("tumblr_pew3zxiCR71rnbw6mo1_1280.gif", 484, "tdaN1D5MCaWE7aqpIplc5dFx9VZHO3ArylgSWdjpLlk=")]
        [DataRow("tumblr_pal4j5uMXB1rnbw6mo1_1280.gif", 299, "+sM9WPqn+T4Z1OcB9af5md6upYliWz138/CGBogeH7c=")]
        public async Task GifHashCalculator_Test(string gif, int framesCount, string expectedHash)
        {
            var fs = new FileSystem();

            var gifFile = fs.FileInfo.New(gif);

            var sut = new GifHashCalculator(new SHA256HashCalculator(), new BitmapToByteArrayGetPixel());
            var hash = await sut.GetFileHash(gifFile);
            Assert.AreEqual(expectedHash,hash);
        }
    }
}
