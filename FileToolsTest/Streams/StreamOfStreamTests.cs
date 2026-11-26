using FileTools.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace FileToolsTest.Streams
{
	[TestClass]
	public class StreamOfStreamTests
	{
		[TestMethod]
		public void StreamOfStreamTests_Oversized_Destination()
		{
			var stream1 = new MemoryStream(Enumerable.Range(0, 5).Select(p => (byte)p).ToArray());
			var stream2 = new MemoryStream(Enumerable.Range(100, 10).Select(p => (byte)p).ToArray());
			var stream3 = new MemoryStream(Enumerable.Range(200, 15).Select(p => (byte)p).ToArray());

			var sut = new StreamOfStreams(
			[
				stream1,
				stream2,
				stream3
			]);

			var dest = new byte[100];

			var read = sut.Read(dest, 0, dest.Length);

			Assert.AreEqual(30, read);
			Assert.AreEqual(0, dest[0]);
			Assert.AreEqual(1, dest[1]);
			Assert.AreEqual(214, dest[29]);
		}

		[TestMethod]
		public void StreamOfStreamTests_Oversized_Destination_With_Offset()
		{
			var stream1 = new MemoryStream(Enumerable.Range(0, 5).Select(p => (byte)p).ToArray());
			var stream2 = new MemoryStream(Enumerable.Range(100, 10).Select(p => (byte)p).ToArray());
			var stream3 = new MemoryStream(Enumerable.Range(200, 15).Select(p => (byte)p).ToArray());

			var sut = new StreamOfStreams(
			[
				stream1,
				stream2,
				stream3
			]);

			var dest = new byte[100];

			var read = sut.Read(dest, 2, dest.Length);

			Assert.AreEqual(30, read);
			Assert.AreEqual(0, dest[0 + 2]);
			Assert.AreEqual(1, dest[1 + 2]);
			Assert.AreEqual(214, dest[29 + 2]);
		}

		[TestMethod]
		public void StreamOfStreamTests_Undersized_Destination()
		{
			var stream1 = new MemoryStream(Enumerable.Range(0, 5).Select(p => (byte)p).ToArray());
			var stream2 = new MemoryStream(Enumerable.Range(100, 10).Select(p => (byte)p).ToArray());
			var stream3 = new MemoryStream(Enumerable.Range(200, 15).Select(p => (byte)p).ToArray());

			var sut = new StreamOfStreams(
			[
				stream1,
				stream2,
				stream3
			]);

			var dest = new byte[10];

			var read = sut.Read(dest, 0, dest.Length);

			Assert.AreEqual(10, read);
			Assert.AreEqual(0, dest[0]);
			Assert.AreEqual(1, dest[1]);
			Assert.AreEqual(104, dest[9]);
		}

		[TestMethod]
		public void StreamOfStreamTests_Empty()
		{
			var sut = new StreamOfStreams(
			[
			]);

			var dest = new byte[10];

			var read = sut.Read(dest, 0, dest.Length);

			Assert.AreEqual(0, read);
		}
	}
}
