using System.Windows.Forms;
namespace FileToolsForm
{
	partial class frmMain
	{
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button btnFileOpen;
		private System.Windows.Forms.Button btnFolderBrowse;
		private System.Windows.Forms.ListView lvResults;
		private System.Windows.Forms.StatusStrip statusBar1;
		private System.Windows.Forms.ToolStripStatusLabel panelScanned;
		private System.Windows.Forms.ToolStripStatusLabel panelOK;
		private System.Windows.Forms.ToolStripStatusLabel panelMissing;
		private System.Windows.Forms.ToolStripStatusLabel panelFailed;
		private System.Windows.Forms.ToolStripStatusLabel panelTime;
		private System.Windows.Forms.ColumnHeader colhdrStatus;
		private System.Windows.Forms.ColumnHeader colhdrFile;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ColumnHeader colhdrSize;
		private Panel panel1;
		private Panel panel2;
		private ToolStripStatusLabel panelScanning;
		private System.Windows.Forms.ColumnHeader colhdrSFV;

		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.btnFileOpen = new System.Windows.Forms.Button();
			this.btnFolderBrowse = new System.Windows.Forms.Button();
			this.lvResults = new System.Windows.Forms.ListView();
			this.colhdrFile = new System.Windows.Forms.ColumnHeader();
			this.colhdrStatus = new System.Windows.Forms.ColumnHeader();
			this.colhdrSize = new System.Windows.Forms.ColumnHeader();
			this.colhdrSFV = new System.Windows.Forms.ColumnHeader();
			this.statusBar1 = new System.Windows.Forms.StatusStrip();
			this.panelScanning = new System.Windows.Forms.ToolStripStatusLabel();
			this.panelScanned = new System.Windows.Forms.ToolStripStatusLabel();
			this.panelOK = new System.Windows.Forms.ToolStripStatusLabel();
			this.panelMissing = new System.Windows.Forms.ToolStripStatusLabel();
			this.panelFailed = new System.Windows.Forms.ToolStripStatusLabel();
			this.panelTime = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.statusBar1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtFilename
			// 
			this.txtFilename.Location = new System.Drawing.Point(10, 20);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(504, 23);
			this.txtFilename.TabIndex = 0;
			this.txtFilename.Text = "D:\\programs\\programs.sfv";
			this.txtFilename.TextChanged += new System.EventHandler(this.txtFilename_TextChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(590, 17);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(111, 30);
			this.button1.TabIndex = 2;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
			// 
			// btnFileOpen
			// 
			this.btnFileOpen.Location = new System.Drawing.Point(523, 17);
			this.btnFileOpen.Name = "btnFileOpen";
			this.btnFileOpen.Size = new System.Drawing.Size(29, 30);
			this.btnFileOpen.TabIndex = 5;
			this.btnFileOpen.Text = "...";
			this.btnFileOpen.Click += new System.EventHandler(this.btnFileOpen_Click);
			// 
			// btnFolderBrowse
			// 
			this.btnFolderBrowse.Location = new System.Drawing.Point(557, 17);
			this.btnFolderBrowse.Name = "btnFolderBrowse";
			this.btnFolderBrowse.Size = new System.Drawing.Size(29, 30);
			this.btnFolderBrowse.TabIndex = 6;
			this.btnFolderBrowse.Text = "...";
			this.btnFolderBrowse.Click += new System.EventHandler(this.btnFolderBrowse_Click);
			// 
			// lvResults
			// 
			this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.colhdrFile,
			this.colhdrStatus,
			this.colhdrSize,
			this.colhdrSFV});
			this.lvResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvResults.FullRowSelect = true;
			this.lvResults.HideSelection = false;
			this.lvResults.Location = new System.Drawing.Point(0, 0);
			this.lvResults.Name = "lvResults";
			this.lvResults.Size = new System.Drawing.Size(588, 370);
			this.lvResults.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvResults.TabIndex = 7;
			this.lvResults.UseCompatibleStateImageBehavior = false;
			this.lvResults.View = System.Windows.Forms.View.Details;
			this.lvResults.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvResults_ColumnClick);
			// 
			// colhdrFile
			// 
			this.colhdrFile.Text = "File";
			this.colhdrFile.Width = 377;
			// 
			// colhdrStatus
			// 
			this.colhdrStatus.Text = "Status";
			// 
			// colhdrSize
			// 
			this.colhdrSize.Text = "Size";
			this.colhdrSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// colhdrSFV
			// 
			this.colhdrSFV.Text = "SFV";
			this.colhdrSFV.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colhdrSFV.Width = 69;
			// 
			// statusBar1
			// 
			this.statusBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.panelScanning,
			this.panelScanned,
			this.panelOK,
			this.panelMissing,
			this.panelFailed,
			this.panelTime});
			this.statusBar1.Location = new System.Drawing.Point(0, 429);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(588, 22);
			this.statusBar1.TabIndex = 8;
			this.statusBar1.Text = "statusBar1";
			// 
			// panelScanning
			// 
			this.panelScanning.Name = "panelScanning";
			this.panelScanning.Size = new System.Drawing.Size(0, 17);
			// 
			// panelScanned
			// 
			this.panelScanned.Name = "panelScanned";
			this.panelScanned.Size = new System.Drawing.Size(0, 17);
			// 
			// panelOK
			// 
			this.panelOK.Name = "panelOK";
			this.panelOK.Size = new System.Drawing.Size(0, 17);
			// 
			// panelMissing
			// 
			this.panelMissing.Name = "panelMissing";
			this.panelMissing.Size = new System.Drawing.Size(0, 17);
			// 
			// panelFailed
			// 
			this.panelFailed.Name = "panelFailed";
			this.panelFailed.Size = new System.Drawing.Size(0, 17);
			// 
			// panelTime
			// 
			this.panelTime.Name = "panelTime";
			this.panelTime.Size = new System.Drawing.Size(0, 17);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.txtFilename);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Controls.Add(this.btnFileOpen);
			this.panel1.Controls.Add(this.btnFolderBrowse);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(588, 59);
			this.panel1.TabIndex = 9;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.lvResults);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 59);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(588, 370);
			this.panel2.TabIndex = 10;
			// 
			// frmMain
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
			this.ClientSize = new System.Drawing.Size(588, 451);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.statusBar1);
			this.Name = "frmMain";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
			this.statusBar1.ResumeLayout(false);
			this.statusBar1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}