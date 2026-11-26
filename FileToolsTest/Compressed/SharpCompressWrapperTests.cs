using FileTools.Compressed;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace FileToolsTest.Compressed
{
	[TestClass]
	public class SharpCompressWrapperTests
	{
		private const string testArchive = "test-archive.zip";

		[TestMethod]
		public void SharpCompressWrapper_List_Test()
		{
			var sut = new SharpCompressWrapper(new FileSystem());
			var file1 = sut.List(testArchive);
			Assert.IsNotNull(file1);
			Assert.IsFalse(file1.Count == 0);

			var sut2 = new _7zwrapper("C:\\Program Files\\7z");
			var file2 = sut.List(testArchive);
			Assert.IsNotNull(file2);
			Assert.IsFalse(file2.Count == 0);

			Assert.AreEqual(file1.Count, file2.Count);

			CollectionAssert.AreEqual(file1.CRC, file2.CRC);
		}

		[TestMethod]
		public void SharpCompressWrapper_Extract_Test()
		{
			const string directory = @".\testextract";
			var sut = new SharpCompressWrapper(new FileSystem());

			Directory.CreateDirectory(directory);

			var file1 = sut.Extract(testArchive, directory);
			Assert.IsTrue(file1);

			Directory.Delete(directory, true);
		}

		[TestMethod]
		public async Task SharpCompressWrapper_Test_Test()
		{ 
			var sut = new SharpCompressWrapper(new FileSystem());
			var workingResult = await sut.Test(testArchive);
			Assert.IsTrue(workingResult);

			var brokenArchiveName = "broken-" + testArchive;

			File.Copy(testArchive, brokenArchiveName, true);

			using (var stream = File.Open(brokenArchiveName, FileMode.Open))
			{
				var corruptionData = Enumerable.Range(0, 10).Select(p => (byte)p).ToArray();
				stream.Position = 1024;
				stream.Write(corruptionData, 0, corruptionData.Length);
			}

			var brokenResult = await sut.Test(brokenArchiveName);
			Assert.IsFalse(brokenResult);

			File.Delete(brokenArchiveName);
		}
	}
}
