using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileTools;
using FileTools.Common;
using FileTools.Dupecheck;
using FileTools.FileHashCalculator;
using FileTools.HashStore;
using FileTools.PHash;
using NLog;
using static FileTools.PHash.PHashCorrelationFinder;

namespace FileToolsUI.PHash
{
    public partial class frmPHash : frmDirectoryListerBase
    {
        private readonly IHashStore hashStore;
        private readonly IFileSystem fileSystem;
        private IList<Match> matches;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public frmPHash(IHashStore hashStore, IFileSystem fileSystem)
        {
            InitializeComponent();
            this.hashStore = hashStore;
            this.fileSystem = fileSystem;
            this.treeView1.AfterSelect += TreeView1_DoubleClick;
        }

        private void TreeView1_DoubleClick(object sender, EventArgs e)
        {

            TreeView tv = sender as TreeView;
            TreeNode selected = tv.SelectedNode;
            if (selected == null)
                return;

            if (selected.Nodes.Count > 0)
            {
                var match = selected.Tag as Match;
                if (match != null)
                {
                    this.lblMatch.Text = "...";
                    var isMatch = match.IsExactMatch();
                    this.lblMatch.Text = isMatch ? "Exact match" : "Not exact";
                }
            }
        }



        protected async override Task ScanDirectories()
        {
            var dl = new DuplicateRecurser();
            var recurser = new DirectoryRecurser(dl);
            recurser.DirectoryRecurserAction += new EventHandler<DirectoryRecurserEvent>(dl_DirectoryRecurserAction);
            foreach (var di in this.multipleDirectorySelector1.Directories)
            {
                recurser.Recurse(fileSystem.DirectoryInfo.New(di));
            }

            var files = dl.FileList;
            var graphicFiles = files.Where(p => IsGraphicFile(p.Value));
            var fileCount = graphicFiles.Count();
            var hasher = new CachedFileHashCalculator(new PHashCalculator(), hashStore);
            var hashesTasks = graphicFiles.Select(
                (p, i) =>
                {
                    this.ProgressMessage = $"pHashing {i + 1} of {fileCount} {p.Key}";
                    this.ProgressPercent = (100 * (i + 1)) / fileCount;
                    return new
                    {
                        Filename = p.Key,
                        Hash = NewMethod(p, hasher),
                    };
                }
            ).ToDictionary(p => p.Filename, p => p.Hash);

            await Task.WhenAll(hashesTasks.Select(p => p.Value));

            hashStore.Dispose();

            var hashes = hashesTasks.Where(p => p.Value.Status == TaskStatus.RanToCompletion).Select(p => new FilePhash(p.Key, p.Value.Result)).ToList();

            var invalidHashes = hashes.Where(p => p.PHash == null).ToList();
            foreach (var hash in invalidHashes)
            {
                logger.Info($"Excluding file with invalid hash {hash.FileName}");
                hashes.Remove(hash);
            }

            this.ResultsMessage = $"Got {hashes.Count} hashes, matching for correlation";

            var minscore = (double)this.minpHash.Value / 100;

            var combinations = new PHashCorrelationFinder().BuildCombinations(hashes, this.LeftHandFilterTextbox.Text, this.RightHandFilterTextbox.Text);

            this.ResultsMessage = $"Got {hashes.Count} hashes, matching {combinations.Count()} combinations for correlation";

            matches = new PHashCorrelationFinder().GetMatches(combinations).Where(p => p.Score > minscore).OrderByDescending(p => p.Score).ToList();

            this.ResultsMessage = $"Done. {matches.Count} perceptual matches";
        }

        private async Task<string> NewMethod(KeyValuePair<string, IFileInfo> p, CachedFileHashCalculator hasher)
        {
            try
            {
                return await hasher.GetFileHash(p.Value);
            }
            catch (Exception e)
            {
                logger.Error(e, $"Error hashing file {p.Key}");
                return null;
            }
        }

        protected override void PopulateTreeView()
        {
            if (this.matches == null) return;

            var treeNodes = this.matches.Where(p =>
                this.FilterMatch(p.File1) || this.FilterMatch(p.File2)
            ).Select(MatchToTreeNode).ToArray();

            this.treeView1.Nodes.AddRange(treeNodes);
        }

        private TreeNode MatchToTreeNode(Match item)
        {
            var tnMatch = new TreeNode((item.Score * 100).ToString("0.##") + "%")
            {
                Tag = item
            };

            var tnFile1 = FileToTreeNode(item.File1);
            tnMatch.Nodes.Add(tnFile1);
            var tnFile2 = FileToTreeNode(item.File2);
            tnMatch.Nodes.Add(tnFile2);
            return tnMatch;
        }

        private TreeNode FileToTreeNode(string filename)
        {
            var fileInfo = fileSystem.FileInfo.New(filename);
            var tnFile1 = new TreeNode(filename);
            tnFile1.Tag = filename;
            tnFile1.ToolTipText = GetToolTipText(fileInfo);
            return tnFile1;
        }

        private static string GetToolTipText(IFileInfo fileInfo)
        {
            return $"Last write: {fileInfo.LastWriteTime}\r\nSize: {fileInfo.Length:N0}";
        }

        private bool IsGraphicFile(IFileInfo fileInfo)
        {
            var extension = fileInfo.Extension.ToLower();
            return extension is ".jpg" or
                ".jpeg" or
                ".png" or
                ".gif" or
                ".webp";
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
                foreach (TreeNode nodeSize in this.treeView1.Nodes)
                {
                    foreach (TreeNode nodeSFV in nodeSize.GetNodes().Where(p => p.Checked))
                    {
                        var filename = nodeSFV.Text;
                        FileInfo fi = new FileInfo(filename);
                        fi.Attributes = FileAttributes.Normal;
                        fi.Delete();

                        var matchesToRemove = matches.Where(p => p.File1 == filename || p.File2 == filename).ToList();

                        foreach (var toremove in matchesToRemove)
                        {
                            this.matches.Remove(toremove);
                        }
                    }
                }
            }
            finally
            {
                UpdateTreeView();
            }
        }
    }
}
