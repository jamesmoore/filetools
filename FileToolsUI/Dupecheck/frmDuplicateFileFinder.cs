using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FileTools.Common;
using System.Linq;
using FileTools.HashStore;
using System.IO.Abstractions;
using System.Threading.Tasks;
using FileTools.PDFHasher;

namespace FileTools.Dupecheck
{
    public partial class frmDuplicateFileFinder : frmDirectoryListerBase
    {
        private IList<DuplicateFileItem> duplicateFileList;
        private FileProcessingMethod ZipHandlingMethod;
        private bool MatchFilename;
        private readonly IHashStore hashStore;
        private readonly IFileSystem fileSystem;

        private ICompressedFileAPI compressedFileAPI { get; set; }
        public frmDuplicateFileFinder(
            IHashStore hashStore,
            ICompressedFileAPI compressedFileAPI,
            IFileSystem fileSystem
            )
        {
            InitializeComponent();
            var list = Enum.GetValues(typeof(FileProcessingMethod));
            this.ZipHandlingMethodComboBox.DataSource = list;
            this.AutocheckTypeCombo.Items.AddRange(new string[]
            {
                "Oldest",
                "Newest",
            });
            this.AutocheckTypeCombo.SelectedIndex = 0;
            this.hashStore = hashStore;
            this.compressedFileAPI = compressedFileAPI;
            this.fileSystem = fileSystem;
        }

        protected override void PreRunWorkerAsync()
        {
            this.ZipHandlingMethod = (FileProcessingMethod)this.ZipHandlingMethodComboBox.SelectedItem;
            this.MatchFilename = this.MatchFilenameCheckBox.Checked;
            base.PreRunWorkerAsync();
        }

        protected async override Task ScanDirectories()
        {
            try
            {
                var dl = new DuplicateRecurser();
                var recurser = new DirectoryRecurser(dl);
                recurser.DirectoryRecurserAction += new EventHandler<DirectoryRecurserEvent>(dl_DirectoryRecurserAction);
                foreach (var di in this.multipleDirectorySelector1.Directories)
                {
                    recurser.Recurse(fileSystem.DirectoryInfo.New(di));
                }

                var fileList = dl.FileList;

                var finder = new DuplicateFinder(
                    this.ZipHandlingMethod, 
                    new DuplicateFileFinderObserver(this), 
                    this.compressedFileAPI,
                    this.fileSystem,
                    new GhostScriptApi(new GhostScriptLocator(this.fileSystem)),
                    hashStore,
                    this.MatchFilename
                    );

                this.duplicateFileList = await finder.Process(fileList.Values.ToList());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetSummaryText(IList<DuplicateFileItem> dupelist)
        {
            var lookup = dupelist.ToLookup(p => p.Hash, p => p);

            long size = 0;
            foreach (var item in lookup)
            {
                var totalSize = item.Skip(1).Sum(p => p.FileItem.Length);
                size += totalSize;
            }

            var message = dupelist.Count + " duplicated files in " +
                dupelist.Select(p => p.Hash).Distinct().Count() + " groups " +
                size.ToFileSize() + " excess";
            return message;
        }

        protected override void PopulateTreeView()
        {
            if (this.duplicateFileList == null) return;
            var filtereditems = this.duplicateFileList.Where(item => this.FilterMatch(item.FileItem.FullName)).ToList();
            var lookup = filtereditems.ToLookup(p => p.Hash, p => p);
            var sequence = lookup.OrderBy(p => p.Select(q => q.FileItem.Length).Min());
            var treeNodes = sequence.Select(p => BuildSizeTreeNode(p.Key, p)).ToArray();
            this.treeView1.Nodes.AddRange(treeNodes);

            var message = GetSummaryText(filtereditems);
            this.ResultsMessage = message;
        }

        private static TreeNode BuildSizeTreeNode(string hash, IEnumerable<DuplicateFileItem> groupItem)
        {
            var minSize = groupItem.Min(p => p.FileItem.Length);
            var maxSize = groupItem.Max(p => p.FileItem.Length);

            var sizeNodeText = minSize == maxSize ? minSize.ToFileSize() : $"{minSize.ToFileSize()} + {(maxSize - minSize).ToFileSize()}";

            var tnFiles = groupItem.OrderBy(p => p.FileItem.Length).Select(fileItem => new TreeNode(fileItem.FileItem.FullName)
            {
                Tag = fileItem,
                ToolTipText = GetToolTipText(fileItem, minSize, maxSize)
            });

            var tnSize = new TreeNode(sizeNodeText, tnFiles.ToArray())
            {
                Tag = hash,
                ToolTipText = hash,
            };

            return tnSize;
        }

        private static string GetToolTipText(DuplicateFileItem duplicateFileItem, long minSize, long maxSize)
        {
            var fileInfo = duplicateFileItem.FileItem;
            return $@"Last write: {fileInfo.LastWriteTime}
Size: {fileInfo.Length.ToFileSize()} {(minSize != maxSize && fileInfo.Length != minSize ? (fileInfo.Length - minSize).ToFileSize() + " larger" : "")}
Hash: {duplicateFileItem.Hash}
Hash method: {duplicateFileItem.HashMethod}";
        }

        void dl_DirectoryRecurserAction(object sender, DirectoryRecurserEvent e)
        {
            if (e.EventType == DirectoryRecurserEventType.ScanDirectory)
            {
                this.ProgressMessage = e.DirectoryInfo.FullName.ToString();
            }
            else if (e.EventType == DirectoryRecurserEventType.ScanFile)
            {
                try
                {
                    this.ProgressMessage = e.FileInfo.FullName.ToString();
                }
                catch (System.IO.PathTooLongException ex)
                {
                    this.ProgressMessage = ex.ToString();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var nodeSize in this.treeView1.GetNodes())
                {
                    foreach (var nodeSFV in nodeSize.GetNodes().Where(p => p.Checked))
                    {
                        try
                        {
                            var item = (DuplicateFileItem)nodeSFV.Tag;
                            item.FileItem.DeleteFile();
                            this.duplicateFileList.Remove(item);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            finally
            {
                UpdateTreeView();
            }
        }

        private void btnAutoCheck_Click(object sender, EventArgs e)
        {
            foreach (TreeNode nodeSize in this.treeView1.Nodes)
            {
                var duplicates = nodeSize.GetNodes().Select(p => new { Node = p, FileItem = (DuplicateFileItem)p.Tag });

                var sorted = this.AutocheckTypeCombo.SelectedIndex == 0 ?
                    duplicates.OrderBy(p => p.FileItem.FileItem.LastWriteTime).ThenBy(p => p.FileItem.FileItem.CreationTime) :
                    duplicates.OrderByDescending(p => p.FileItem.FileItem.LastWriteTime).ThenByDescending(p => p.FileItem.FileItem.CreationTime);

                var asList = sorted.ToList();

                foreach (var item in asList)
                {
                    item.Node.Checked = item != asList.Last();
                }

            }
        }
    }
}
