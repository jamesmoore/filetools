using System.Drawing;
using FileTools.BitmapDecoder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileToolsTest.BitmapDecoder
{
    [TestClass]
    public class BitmapToByteArrayTests
    {
        [DataTestMethod]
        [DataRow("a796618f8b2dc09003412e16f8d5946a747a5780cb7a6e6217f10b62d7270194.png")]
        [DataRow("barratts-new-houses.jpg")]
        public void BitmapToByteArrayTest(string filename)
        {
            using var bmp1 = new Bitmap(filename);
            var array1 = new BitmapToByteArrayGetPixel().GetByteArray(bmp1);
            var array2 = new SkiaSharpBitmapDecoder().GetByteArray(filename);
            Assert.IsTrue(array1.Length > 0);
            Assert.IsTrue(array2.Length > 0);
            CollectionAssert.AreEqual(array1, array2);
		}
    }
}
