using System;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using FileTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileToolsTest
{
    [TestClass]
    public class ExtensionMethodsTests
    {
        [TestMethod]
        public void DeleteFileDoesNotWriteAttributesWhenDeletionSucceeds()
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "keep.txt"), "keep");
            try
            {
                var file = DispatchProxy.Create<IFileInfo, AttributeDeniedFile>();
                var proxy = (AttributeDeniedFile)(object)file;
                proxy.Directory = new FileSystem().DirectoryInfo.New(path);

                file.DeleteFile();

                Assert.IsTrue(proxy.Deleted);
            }
            finally
            {
                Directory.Delete(path, true);
            }
        }

        [TestMethod]
        public void DeleteFileDeletesReadOnlyFileAndEmptyParent()
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var child = Path.Combine(path, "empty-after-delete");
            Directory.CreateDirectory(child);
            File.WriteAllText(Path.Combine(path, "keep.txt"), "keep");
            var filePath = Path.Combine(child, "delete.txt");
            File.WriteAllText(filePath, "delete");
            File.SetAttributes(filePath, FileAttributes.ReadOnly);
            try
            {
                new FileSystem().FileInfo.New(filePath).DeleteFile();

                Assert.IsFalse(File.Exists(filePath));
                Assert.IsFalse(Directory.Exists(child));
                Assert.IsTrue(Directory.Exists(path));
            }
            finally
            {
                if (File.Exists(filePath)) File.SetAttributes(filePath, FileAttributes.Normal);
                Directory.Delete(path, true);
            }
        }

        public class AttributeDeniedFile : DispatchProxy
        {
            public IDirectoryInfo Directory { get; set; }
            public bool Deleted { get; private set; }

            protected override object Invoke(MethodInfo targetMethod, object[] args)
            {
                switch (targetMethod.Name)
                {
                    case "get_Directory": return Directory;
                    case "Delete": Deleted = true; return null;
                    case "set_Attributes": throw new UnauthorizedAccessException("Attribute writes denied by share.");
                    default: throw new InvalidOperationException(targetMethod.Name);
                }
            }
        }
    }
}
