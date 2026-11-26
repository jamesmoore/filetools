using FileTools;
using FileTools.Common;
using FileTools.Compressed;
using FileTools.Dupecheck;
using FileTools.HashStore;
using FileTools.PDFHasher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace FileToolsTest.Dupecheck
{
    [TestClass]
    public class DuplicateFinderTests
    {
        [TestMethod]
        public async Task Process_WhenMatchFilenameIsDisabled_ReturnsFilesWithDifferentNames()
        {
            using var temporaryDirectory = new TemporaryDirectory();
            var fileSystem = new FileSystem();
            var first = CreateFile(fileSystem, Path.Combine(temporaryDirectory.Path, "one.txt"), "same-content");
            var second = CreateFile(fileSystem, Path.Combine(temporaryDirectory.Path, "two.txt"), "same-content");

            var sut = CreateSut(fileSystem, false);

            var result = await sut.Process([first, second]);

            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(new[] { first.FullName, second.FullName }, result.Select(p => p.FileItem.FullName).ToArray());
        }

        [TestMethod]
        public async Task Process_WhenMatchFilenameIsEnabled_OnlyReturnsFilesWithSameNames()
        {
            using var temporaryDirectory = new TemporaryDirectory();
            var fileSystem = new FileSystem();
            var first = CreateFile(fileSystem, Path.Combine(temporaryDirectory.Path, "A", "same.txt"), "same-content");
            var second = CreateFile(fileSystem, Path.Combine(temporaryDirectory.Path, "B", "same.txt"), "same-content");
            var third = CreateFile(fileSystem, Path.Combine(temporaryDirectory.Path, "C", "different.txt"), "same-content");

            var sut = CreateSut(fileSystem, true);

            var result = await sut.Process([first, second, third]);

            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(new[] { first.FullName, second.FullName }, result.Select(p => p.FileItem.FullName).ToArray());
        }

        private static DuplicateFinder CreateSut(IFileSystem fileSystem, bool matchFilename)
        {
            return new DuplicateFinder(
                FileProcessingMethod.WholeFile,
                new TestObserver(),
                new TestCompressedFileApi(),
                fileSystem,
                new TestGhostScriptApi(),
                new TestHashStore(),
                matchFilename);
        }

        private static IFileInfo CreateFile(IFileSystem fileSystem, string path, string contents)
        {
            fileSystem.Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            fileSystem.File.WriteAllText(path, contents);
            return fileSystem.FileInfo.New(path);
        }

        private sealed class TemporaryDirectory : IDisposable
        {
            public TemporaryDirectory()
            {
                Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(Path);
            }

            public string Path { get; }

            public void Dispose()
            {
                if (Directory.Exists(Path))
                {
                    Directory.Delete(Path, true);
                }
            }
        }

        private sealed class TestObserver : IDuplicateFinderObserver
        {
            public void SetProgressPercent(int i)
            {
            }

            public void SetProgressText(string file)
            {
            }
        }

        private sealed class TestCompressedFileApi : ICompressedFileAPI
        {
            public bool CompressDirectory(string filename, string directory, string format)
            {
                throw new NotSupportedException();
            }

            public bool Extract(string filename, string directory)
            {
                throw new NotSupportedException();
            }

            public CompressedFile List(string filename)
            {
                throw new NotSupportedException();
            }

            public Task<bool> Test(string filename)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class TestGhostScriptApi : IGhostScriptApi
        {
            public bool IsAvailable => false;

            public (int ExitCode, string Output) ExtractPdfToPath(string filename, string tempPath)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class TestHashStore : IHashStore
        {
            private readonly Dictionary<string, string> hashes = [];

            public void Clear()
            {
                hashes.Clear();
            }

            public void ClearFailed()
            {
            }

            public void Dispose()
            {
            }

            public string GetHash(string key)
            {
                return hashes.TryGetValue(key, out var value) ? value : null;
            }

            public Task StoreHash(string key, string hash)
            {
                hashes[key] = hash;
                return Task.CompletedTask;
            }
        }
    }
}
