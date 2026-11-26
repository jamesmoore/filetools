using System;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using FileTools;
using FileTools.Common;
using System.IO.Abstractions;
using FileToolsUI.SFV;

namespace FileToolsForm
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    partial class frmMain : System.Windows.Forms.Form
	{
		private string filename;
		SFVFileProcessor processor;
		private readonly IFileSystem fileSystem;

		public frmMain(IFileSystem fileSystem)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.fileSystem = fileSystem;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			this.txtFilename.Text = "";
			this.Text = "SFV Checker";
		
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
            DateTime dtStart = DateTime.Now;

            filename = this.txtFilename.Text;

			if(File.Exists(txtFilename.Text)||Directory.Exists(txtFilename.Text))
			{
                processor = new SFVFileProcessor(fileSystem);

                if (File.Exists(txtFilename.Text))
                {
                    processor.Load(txtFilename.Text);
                }
                else if (Directory.Exists(txtFilename.Text))
                {
                    var directory = fileSystem.DirectoryInfo.New(txtFilename.Text);

                    var dr = new DirectoryRecurser(processor);
                    dr.Recurse(directory);
                }

                this.lvResults.Items.Clear();

                // Loop through all the stuff in the processor and display it
                this.lvResults.Sorting = SortOrder.None;

                foreach (SFVFileInfo sfv in processor.List)
                {
                    SFVListViewItem sfvlvi = new SFVListViewItem(sfv);
                    this.lvResults.Items.Add(sfvlvi);
                    sfvlvi.EnsureVisible();
                }

                this.lvResults.Refresh();


                Thread t = new Thread(this.ProcesssListItems);
                t.Start();
                //ProcesssListItems();


                DateTime dtEnd = DateTime.Now;
                TimeSpan ts = new TimeSpan(dtEnd.Ticks - dtStart.Ticks);

                this.panelTime.Text = ts.TotalSeconds.ToString();

            }
		}

        public void ProcesssListItems()
        {
            foreach (SFVFileInfo sfvfileinfo in processor.List )
            {
                //this.panelScanning.Text = lvi.sfvfile.file.Name;
                sfvfileinfo.Process();
                //lvi.lvi.ListView.Refresh();
            }


            if (Directory.Exists(filename))
            {
                DirectoryInfo d = new DirectoryInfo(filename);
                processor.Save(d.FullName + @"\" + d.Name + ".sfv");
            }

            //delegateList del = new delegateList(ProcessListItems);
            //this.Invoke(del,new Object[] {sfvListViewItemList});
        }

//        public delegate void delegateList(List<SFVListViewItem> lvList);

        //public void ProcessListItems(List<SFVListViewItem> lvList)
        //{


        //}



//		long lFilesScanned;
//		long lFilesMissing;
//		long lFilesFailed;
//		long lFilesOK;
		
//		DateTime dtScanStart; 

		private void btnFileOpen_Click(object sender, System.EventArgs e)
		{
			this.openFileDialog1.ShowDialog(this);
		}

		private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.txtFilename.Text = openFileDialog1.FileName;
		}

		private void btnFolderBrowse_Click(object sender, System.EventArgs e)
		{
			this.folderBrowserDialog1.ShowDialog();
			if(folderBrowserDialog1.SelectedPath.Equals("")==false) 
			{
				this.txtFilename.Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void txtFilename_TextChanged(object sender, System.EventArgs e)
		{
			
//			FileInfo fi = new FileInfo(txtFilename.Text);
//			DirectoryInfo di = new DirectoryInfo(txtFilename.Text);

			if(File.Exists(txtFilename.Text)||Directory.Exists(txtFilename.Text)) 
			{
				if(txtFilename.Text.ToLower().EndsWith(".sfv")) 
					button1.Text="Check SFV";
				else
					button1.Text="Make SFV";
			}
			else 
			{
				button1.Text="Unknown";
			}
		}


		private void Form1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			String[] s = e.Data.GetFormats();
			String[] s2 = (String[])e.Data.GetData(DataFormats.FileDrop);
			if(s2.Length>0) 
			{
				this.txtFilename.Text = s2[0];
			}
		}

		private void Form1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//System.Diagnostics.Debug.Write(e.Data.ToString());
			e.Effect = DragDropEffects.All;
			//this.Text = "Drag Enter";
		}
        /*
		private  void AddItemToListview(SFVFileEventArgs s) 
		{
			lock(this) 
			{
				//ListViewItem lvi = null;

				if(chkShowFailed.Checked && (s.theEventType ==SFVEventType.SFVCheckedFailure||s.theEventType==SFVEventType.SFVCreateFail) ||
					chkShowMissing.Checked && s.theEventType==SFVEventType.SFVFileMissing||
					chkShowOK.Checked && (s.theEventType==SFVEventType.SFVCheckedSuccess||s.theEventType==SFVEventType.SFVCreated) ||
					s.theEventType == SFVEventType.SFVNewEntry ||
					s.theEventType == SFVEventType.SFVCreated) 
				{
				
					if(s.ListViewItem==null) 
					{
						s.ListViewItem = this.lvResults.Items.Add(s.RelativeFileName);
						s.ListViewItem.SubItems.Add("Status");
						if(s.theFile.Exists) 
						{
							s.ListViewItem.SubItems.Add(s.theFile.Length.ToString());
						}
						else 
						{
							s.ListViewItem.SubItems.Add("Unknown");
						}
						s.ListViewItem.SubItems.Add(s.SFVValue);
					}
			
					switch(s.theEventType) 
					{
						case SFVEventType.SFVCheckedSuccess:
							s.ListViewItem.SubItems[1].Text = "OK";
							break;
						case SFVEventType.SFVCheckedFailure:
							s.ListViewItem.SubItems[1].Text = "Failed";
							s.ListViewItem.ForeColor = Color.Red;
							s.ListViewItem.BackColor = Color.Yellow;
							break;
						case SFVEventType.SFVFileMissing:
							s.ListViewItem.SubItems[1].Text = "Missing";
							s.ListViewItem.ForeColor = Color.Blue;
							s.ListViewItem.BackColor = Color.Yellow;
							break;
						case SFVEventType.SFVCreateFail:
							s.ListViewItem.SubItems[1].Text = "Create fail";
							s.ListViewItem.ForeColor = Color.Red;
							break;
						case SFVEventType.SFVCreated:
							s.ListViewItem.SubItems[1].Text = "Created";
							s.ListViewItem.SubItems[3].Text = s.SFVValue;
							break;
						case SFVEventType.SFVNewEntry:
							s.ListViewItem.SubItems[1].Text = "Unknown";
							break;
					}
					s.ListViewItem.EnsureVisible();
				}
			}
		}
*/
		private void chkShowFailed_CheckedChanged(object sender, System.EventArgs e)
		{

		}

        private void lvResults_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.lvResults.ListViewItemSorter = new ListViewItemComparer(e.Column);
        }
	}

    class ListViewItemComparer : IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
        }
    }
}
