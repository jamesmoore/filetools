using FileTools.PDFHasher;
using FileTools.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileToolsTest.PDFHasher
{
	[TestClass]
	public class CombinationStreamsTest
	{
		[TestMethod]
		public void CombinedStreamsTest()
        {
            var memoryStreams = GetTestStreams();

            var combinedStreams = new CombinationStream(memoryStreams.ToList());

            Assert.AreEqual(100, combinedStreams.Length);

            var testArray = new byte[50];
            combinedStreams.ReadExactly(testArray);
            Assert.AreEqual(0, testArray[0]);
            Assert.AreEqual(49, testArray[49]);
            combinedStreams.ReadExactly(testArray);
            Assert.AreEqual(50, testArray[0]);
            Assert.AreEqual(99, testArray[49]);

            var finalRead = combinedStreams.Read(testArray);
            Assert.AreEqual(0, finalRead);
        }

        [TestMethod]
        public void CombinedStreams_Empty_Test()
        {
            var memoryStreams = GetTestStreams();

            var combinedStreams = new CombinationStream(Enumerable.Empty<Stream>());

            Assert.AreEqual(0, combinedStreams.Length);

            var testArray = new byte[50];
            var read = combinedStreams.Read(testArray);
            Assert.AreEqual(0, read);
        }

        [TestMethod]
        public void StreamOfStreamsTest()
        {
            var memoryStreams = GetTestStreams();

            var combinedStreams = new StreamOfStreams(memoryStreams);

            Assert.Throws<NotImplementedException>(() => combinedStreams.Length);

            var testArray = new byte[50];
            combinedStreams.ReadExactly(testArray);
            Assert.AreEqual(0, testArray[0]);
            Assert.AreEqual(49, testArray[49]);
            combinedStreams.ReadExactly(testArray);
            Assert.AreEqual(50, testArray[0]);
            Assert.AreEqual(99, testArray[49]);

            var finalRead = combinedStreams.Read(testArray);
            Assert.AreEqual(0, finalRead);
        }

        private static IEnumerable<Stream> GetTestStreams()
        {
            return Enumerable.Range(0, 10).Select((p) =>
            {
                var ms = new MemoryStream();
                ms.Write(Enumerable.Range(p * 10, 10).Select(q => (byte)q).ToArray());
                ms.Position = 0;
                return ms;
            });
        }
    }
}
