using System;
using System.Windows.Forms;
using FileToolsForm;
using FileTools.Dupecheck;
using FileTools.ZipTester;
using System.Configuration;
using FileToolsUI.FileType;
using FileToolsUI.PHash;
using FileTools.HashStore;
using System.IO.Abstractions;
using FileTools.Compressed;
using System.Diagnostics;

namespace FileTools
{
	public partial class frmParent : Form
	{
        private readonly IHashStore hashStoreFactory;
		private readonly IFileSystem fileSystem;
		private readonly ICompressedFileAPI compressedFileAPI;

		public frmParent(IHashStore hashStore, IFileSystem fileSystem)
		{
			InitializeComponent();
			var path = ConfigurationManager.AppSettings["7zPath"];
			//if (fileSystem.Directory.Exists(path) == false)
			//{
			//	throw new ApplicationException($"7z path in .config file {path} could not be found.");
			//}
			//this.compressedFileAPI = new _7zwrapper(path);
			this.fileSystem = fileSystem;
			this.compressedFileAPI = new SharpCompressWrapper(fileSystem);
			this.hashStoreFactory = hashStore;
		}

		private void newToolStripButton_Click(object sender, EventArgs e)
		{
			var childForm = new frmMain(fileSystem);
			ShowChildForm(childForm);
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			var childForm = new frmDuplicateFileFinder(hashStoreFactory, this.compressedFileAPI, fileSystem);
			ShowChildForm(childForm);
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			var childForm = new frmEmptyDirectoryLister(fileSystem);
			ShowChildForm(childForm);
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			var childForm = new frmZipTester(this.compressedFileAPI, fileSystem);
			ShowChildForm(childForm);
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			var childForm = new frmFileTypeMismatchFinder(fileSystem);
			ShowChildForm(childForm);
		}

		private void tsbPHash_Click(object sender, EventArgs e)
		{
			var childForm = new frmPHash(hashStoreFactory, fileSystem);
			ShowChildForm(childForm);
		}

		private void ShowChildForm(Form childForm)
		{
			childForm.MdiParent = this;
			childForm.WindowState = FormWindowState.Maximized;
			childForm.Show();
		}

		private void clearCacheToolStripMenuItem_Click(object sender, EventArgs e)
		{
            hashStoreFactory.Clear();
		}

		private void clearFailedCachesEntriesToolStripMenuItem_Click(object sender, EventArgs e)
		{
            hashStoreFactory.ClearFailed();
		}

		private void toolStripButtonLogs_Click(object sender, EventArgs e)
		{
			const string path = ".\\Logs";
			if (fileSystem.Directory.Exists(path))
			{
				Process.Start(new ProcessStartInfo() { 
					FileName = "explorer.exe",
					Arguments = path,
				});
			}
		}
	}
}
