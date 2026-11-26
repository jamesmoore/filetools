using System.IO.Abstractions;
using System.Threading.Tasks;
using FileTools.PDFHasher;
using FileTools.StreamHashCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileToolsTest.PDFHasher
{
	[TestClass]
    public class PDFHasherTest
    {
        [TestMethod]
        public async Task TestPDFHasher()
        {
            var fileSystem = new FileSystem();
            var sut = new FileTools.PDFHasher.PDFHasher(fileSystem, new GhostScriptApi(new GhostScriptLocator(fileSystem)), new SHA256HashCalculator());
            var hash1 = await sut.GetPDFContentsHash(@"./PDFHasher/lemorted.pdf");
            var hash2 = await sut.GetPDFContentsHash(@"./PDFHasher/lemorted_optimized.pdf");
            Assert.IsFalse(string.IsNullOrWhiteSpace(hash1));
            Assert.AreEqual(hash1, hash2);

            Assert.AreEqual("3VLwm0zdneAq+NTr4g8n8/auTmiN/UK3ADQL/AgDw50=", hash1);
        }
    }
}
