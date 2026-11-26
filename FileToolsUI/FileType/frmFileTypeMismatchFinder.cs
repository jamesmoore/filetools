using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using FileTools.Common;
using System.Linq;
using FileTools.FileType;
using FileTools;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileToolsUI.FileType
{
    public partial class frmFileTypeMismatchFinder : frmDirectoryListerBase
    {
        private IDictionary<string, List<IFileInfo>> MisMatches;
		private readonly IFileSystem fileSystem;

		public frmFileTypeMismatchFinder(
            IFileSystem fileSystem
            )
        {
            InitializeComponent();
			this.fileSystem = fileSystem;
           this.treeView1.AfterCheck += treeView1_AfterCheckCheckChildren;
		}

        protected async override Task ScanDirectories()
        {
            this.MisMatches = new Dictionary<string, List<IFileInfo>>();

            var finder = new FileTypeRecurser(this.MisMatches);

            var recurser = new DirectoryRecurser(finder);
            recurser.DirectoryRecurserAction += new EventHandler<DirectoryRecurserEvent>(dl_DirectoryRecurserAction);
            foreach (var di in this.multipleDirectorySelector1.Directories)
            {
                recurser.Recurse(fileSystem.DirectoryInfo.New(di));
            }

            this.ResultsMessage = "Number of mismatches: " + MisMatches.Select(p => p.Value).SelectMany(p => p).Count();
        }

        protected override void PopulateTreeView()
        {
            var treeNodes = new List<TreeNode>();

            foreach (var filetype in MisMatches.Keys)
            {
                var tntype = new TreeNode(filetype);
                treeNodes.Add(tntype);
                foreach (var fileItem in MisMatches[filetype].Where(item => this.FilterMatch(item.FullName)))
                {
                    var tnFile = new TreeNode(fileItem.FullName);
                    if (string.IsNullOrWhiteSpace(filetype) == false)
                    {
                        var corrected = Path.GetDirectoryName(fileItem.FullName) + @"\" + Path.GetFileNameWithoutExtension(fileItem.FullName) + filetype;
                        tnFile.Tag = corrected;
                        tnFile.ToolTipText = $"Suggested: {corrected}";
                    }
                    tntype.Nodes.Add(tnFile);
                }
            }

            this.treeView1.Nodes.AddRange(treeNodes.ToArray());
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

        private void btnRename_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (TreeNode filetype in this.treeView1.Nodes)
                {
                    foreach (TreeNode mismatchedName in filetype.Nodes)
                    {
                        if (mismatchedName.Checked)
                        {
                            var original = mismatchedName.Text;
                            var corrected = (string)mismatchedName.Tag;
                            if (string.IsNullOrWhiteSpace(corrected) == false &&  File.Exists(corrected) == false)
                            {
                                File.Move(original, corrected);

                                var files = MisMatches[filetype.Text];
                                files.RemoveAll(p => p.FullName == original);
                            }
                        }
                    }
                }
            }
            finally
            {
                UpdateTreeView();
            }
        }

        private void treeView1_AfterCheckCheckChildren(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown || e.Node.Nodes.Count == 0)
            {
                return;
            }

            foreach (TreeNode childNode in e.Node.Nodes)
            {
                childNode.Checked = e.Node.Checked;
            }
        }
    }
}
