using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FileTools.Common;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileTools.ZipTester
{

	public partial class frmZipTester : frmDirectoryListerBase
	{
		private ICompressedFileAPI compressedFileAPI { get; set; }


		public frmZipTester(ICompressedFileAPI compressedFileAPI, IFileSystem fileSystem)
		{
			InitializeComponent();
			this.cboCompressionMethod.SelectedIndex = 0;
			this.compressedFileAPI = compressedFileAPI;
			this.fileSystem = fileSystem;
		}

		ZipFileFinder dl;
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
			foreach (var di in dl.ZipList.Keys)
			{
				if (this.FilterMatch(di.FullName))
				{
					TreeNode tnSize = new TreeNode(di.FullName);
					tnSize.ToolTipText = dl.ZipList[di];
					this.treeView1.Nodes.Add(tnSize);
				}
			}
		}

		protected async override Task ScanDirectories()
		{
			dl = new ZipFileFinder();
			var dr = new DirectoryRecurser(dl);
			dr.DirectoryRecurserAction += new EventHandler<DirectoryRecurserEvent>(dl_DirectoryRecurserAction);
			foreach (var di in this.multipleDirectorySelector1.Directories)
			{
				dr.Recurse(fileSystem.DirectoryInfo.New(di));
			}
		}

		private async void btnTest_Click(object sender, EventArgs e)
		{
			List<TreeNode> removed = new List<TreeNode>();

			foreach (TreeNode nodeSize in this.treeView1.Nodes)
			{
				if (nodeSize.Checked)
				{
					string filename = nodeSize.Text;

					bool success = false;
					try
					{
						success = await compressedFileAPI.Test(filename);
						nodeSize.ToolTipText = "OK";
					}
					catch (Exception ex)
					{
						nodeSize.ToolTipText = ex.Message;
					}

					if (success)
					{
						nodeSize.ForeColor = Color.Gray;
						removed.Add(nodeSize);
					}
					else
					{
						nodeSize.BackColor = Color.Red;
					}
				}
			}

			foreach (TreeNode node in removed)
			{
				this.treeView1.Nodes.Remove(node);
			}

		}

		private void btnRecompress_Click(object sender, EventArgs e)
		{
			try
			{

				this.treeView1.BeginUpdate();

				foreach (TreeNode nodeSize in this.treeView1.Nodes)
				{
					if (nodeSize.Checked)
					{
						string originalFile = nodeSize.Text;

						bool success = false;
						string workingDirectory = "C:\\" + Path.GetRandomFileName();
						try
						{
							string directory = workingDirectory; // +"\\" + Path.GetFileNameWithoutExtension(filename);

							string format = this.cboCompressionMethod.Text;
							string destinationFile = workingDirectory + "\\" + Path.GetFileNameWithoutExtension(originalFile) + "." + format;

							Directory.CreateDirectory(directory);

							success = compressedFileAPI.Extract(originalFile, directory);

							success = compressedFileAPI.CompressDirectory(destinationFile, directory, format);

							long originalLength = (new FileInfo(originalFile)).Length;
							long newLength = (new FileInfo(destinationFile)).Length;

							if (newLength >= originalLength && this.chkSizeCheck.Checked)
							{
								nodeSize.ForeColor = Color.Gray;
							}
							else
							{
								// delete original
								string originalPath = Path.GetDirectoryName(originalFile);
								File.SetAttributes(originalFile, File.GetAttributes(originalFile) & ~FileAttributes.ReadOnly);
								File.Delete(originalFile);
								// move new to original path
								string finaldestinationfile = originalPath + "\\" + Path.GetFileName(destinationFile);
								File.Move(destinationFile, finaldestinationfile);
								nodeSize.Text = finaldestinationfile;
								nodeSize.ForeColor = Color.Green;
							}

							nodeSize.ToolTipText = "Original size: " + originalLength + "\r\n" +
								"New size: " + newLength + "\r\n" +
								"Change: " + (newLength - originalLength) + "\r\n" +
								"% change: " + (100M * ((newLength - originalLength) / (decimal)originalLength)).ToString(".00");


						}
						catch (Exception ex)
						{
							nodeSize.ToolTipText = ex.Message;
						}
						finally
						{
							if (Directory.Exists(workingDirectory))
							{
								var mdrw = new MakeDirectoryReadWrite();
								var dr = new DirectoryRecurser(mdrw);
								dr.Recurse(fileSystem.DirectoryInfo.New(workingDirectory));
								Directory.Delete(workingDirectory, true);
							}
						}

						if (success == false)
						{
							nodeSize.BackColor = Color.Red;
						}
					}
				}
			}
			finally
			{
				this.treeView1.EndUpdate();
			}
		}

	}
}