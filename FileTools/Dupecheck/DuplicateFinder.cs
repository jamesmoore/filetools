using System.Collections.Generic;
using FileTools.Common;
using System.Linq;
using FileTools.StreamHashCalculator;
using FileTools.HashStore;
using FileTools.FileHashCalculatorFactory;
using System.IO.Abstractions;
using System.Threading.Tasks;
using FileTools.FileHashCalculator;
using System;
using FileTools.PDFHasher;

namespace FileTools.Dupecheck
{
    public class DuplicateFinder(
            FileProcessingMethod method,
            IDuplicateFinderObserver observer,
            ICompressedFileAPI compressedFileAPI,
            IFileSystem fileSystem,
            IGhostScriptApi ghostScriptApi,
            IHashStore hashStore,
            bool matchFileName
            )
    {
        private const int ConsumerTasks = 10;

        public async Task<IList<DuplicateFileItem>> Process(IEnumerable<IFileInfo> fileList)
        {
            var hashCalculator = new SHA256HashCalculator();
            var calculatorFactoryFactory = new FileHashCalculatorAbstractFactory(fileSystem, compressedFileAPI, hashCalculator, ghostScriptApi);

            var calculatorFactory = new CachedFileHashCalculatorFactory(calculatorFactoryFactory.GetHashCalculatorFactory(method), hashStore);

            var progressStatus = new ProgressStatus(observer, fileList.Count(), 0);

            var originalFileList = fileList.Select(p => new CommandStruct(
                p,
                calculatorFactory.GetFileHashCalculator(p),
                progressStatus
            )).ToList();

            var sizeIndependentList =
                originalFileList.
                Where(p => p.Calculator.SizeMatters).
                GroupBy(p => p.FileItem.Length).
                Where(p => p.Count() == 1).
                SelectMany(p => p);
            var sizeSpecificList = originalFileList.Except(sizeIndependentList).ToList();

            progressStatus.UniqueFileCount = sizeSpecificList.Count;

            foreach(var item in sizeSpecificList.OrderBy(p => p.FileItem.GetHashCode()))
            {
                await item.ExecuteWithErrorHandler();
            }

            var hashList = sizeSpecificList.Where(p => string.IsNullOrWhiteSpace(p.Hash) == false).Select(p => new DuplicateFileItem(
                p.FileItem,
                p.Hash,
                p.Calculator.Description
                ));

            observer.SetProgressText("Finding unique CRCs");

            var dupeList = GetDuplicates(hashList).ToList();

            observer.SetProgressText("Dupes done");

            return dupeList;
        }

        private IEnumerable<DuplicateFileItem> GetDuplicates(IEnumerable<DuplicateFileItem> hashList)
        {
            return matchFileName == false ? GetDuplicateItemsByHash(hashList) : GetDuplicateItemsByHashAndFileName(hashList);
        }

        private static IEnumerable<DuplicateFileItem> GetDuplicateItemsByHash(IEnumerable<DuplicateFileItem> hashList)
        {
            return hashList.
                GroupBy(p => p.Hash).
                Where(p => p.Count() > 1).
                SelectMany(p => p);
        }

        private static IEnumerable<DuplicateFileItem> GetDuplicateItemsByHashAndFileName(IEnumerable<DuplicateFileItem> hashList)
        {
            return hashList.
                GroupBy(p => p.Hash).
                SelectMany(hashGroup =>
                    hashGroup.
                    GroupBy(p => p.FileItem.Name, StringComparer.OrdinalIgnoreCase).
                    Where(fileGroup => fileGroup.Count() > 1).
                    SelectMany(fileGroup => fileGroup));
        }
    }

    internal class CommandStruct(
        IFileInfo fileItem,
        IFileHashCalculator calculator,
        ProgressStatus progressStatus
        )
    {
        public string Hash { get; private set; } = null;
        public IFileInfo FileItem { get; } = fileItem;
        public IFileHashCalculator Calculator { get; } = calculator;

        public async Task<string> ExecuteWithErrorHandler()
        {
            try
            {
                progressStatus.NotifyStart(FileItem);
                var hash = await Calculator.GetFileHash(FileItem);
                this.Hash = hash;
                return hash;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                progressStatus.NotifyEnd(FileItem);
            }
        }
    }

    internal class ProgressStatus(IDuplicateFinderObserver observer, int originalCount, int uniqueFileCount)
    {
        private readonly DateTime startedAt = DateTime.UtcNow;

        private int position = 0;
        public int UniqueFileCount { get; set; } = uniqueFileCount;
        private int Percentage => (int)(100.0 * position / this.UniqueFileCount);

        public void NotifyStart(IFileInfo fileInfo)
        {
            position++;

            var timeTaken = DateTime.UtcNow - startedAt;

            var eta = timeTaken * UniqueFileCount / position;
            
            observer.SetProgressText($"{position} of {this.UniqueFileCount} (originally {originalCount}) {timeTaken:hh\\:mm\\:ss}/{eta:hh\\:mm\\:ss} {fileInfo.FullName} @ {fileInfo.Length} bytes");
        }

        public void NotifyEnd(IFileInfo fileInfo)
        {
            observer.SetProgressPercent(this.Percentage);
        }
    }
}
