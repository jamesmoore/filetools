using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FileTools.Common;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace FileTools.Dupecheck
{

	public partial class frmEmptyDirectoryLister : frmDirectoryListerBase
	{

		public frmEmptyDirectoryLister(IFileSystem fileSystem)
		{
			InitializeComponent();
			this.fileSystem = fileSystem;
		}

		EmptyDirectoryFinder.EmptyDirectoryFinder dl;
		DirectoryRecurser dr;
		private readonly IFileSystem fileSystem;

		void dl_DirectoryRecurserAction(object sender, DirectoryRecurserEvent e)
		{
			if (e.EventType == DirectoryRecurserEventType.ScanDirectory)
			{
				this.ProgressMessage = e.DirectoryInfo.FullName.ToString();
			}
			else if (e.EventType == DirectoryRecurserEventType.ScanFile)
			{
				this.ProgressMessage = e.FileInfo.FullName.ToString();
			}
		}

		protected override void PopulateTreeView()
		{
			foreach (var di in dl.EmptyList.Where(p => FilterMatch(p.FullName)))
			{
				var tnSize = new TreeNode(di.FullName);
				this.treeView1.Nodes.Add(tnSize);
			}
		}

		protected async override Task ScanDirectories()
		{
			dl = new EmptyDirectoryFinder.EmptyDirectoryFinder();
			dr = new DirectoryRecurser(dl);
			dr.DirectoryRecurserAction += new EventHandler<DirectoryRecurserEvent>(dl_DirectoryRecurserAction);
			foreach (var di in this.multipleDirectorySelector1.Directories)
			{
				dr.Recurse(fileSystem.DirectoryInfo.New(di));
			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			List<TreeNode> removals = new List<TreeNode>();

			foreach (TreeNode nodeSize in this.treeView1.Nodes)
			{
				if (nodeSize.Checked)
				{
					var filename = nodeSize.Text;
					var di = fileSystem.DirectoryInfo.New(filename);
					try
					{
						di.DeleteDirectoryIfEmpty();
						removals.Add(nodeSize);
						dl.EmptyList.RemoveAll(p => p.FullName == filename);
					}
					catch (Exception)
					{

					}
				}
			}

			foreach (TreeNode node in removals)
			{
				this.treeView1.Nodes.Remove(node);
			}
		}




	}
}