using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace FileTools
{

    public partial class frmDirectoryListerBase : Form
    {
        public frmDirectoryListerBase()
        {
            InitializeComponent();
            this.toolStripProgressBar1.Minimum = 0;
            this.toolStripProgressBar1.Maximum = 100;
            this.treeView1.MouseUp += this.treeView1_MouseUp;
        }

        private bool IsMatch(string s)
        {
            string filter = this.txtFilter.Text;

            if (string.IsNullOrEmpty(filter)) return false;

            if (checkRegex.Checked)
            {
                var regex = new Regex(filter, RegexOptions.IgnoreCase);
                return regex.IsMatch(s);
            }
            else
            {
                return s.ToLower().Contains(filter.ToLower());
            }
        }

        protected bool FilterMatch(string s)
        {
            if (this.btnFilter.Checked == false)
            {
                return true;
            }
            else
            {
                return this.IsMatch(s);
            }
        }

        protected async virtual Task ScanDirectories()
        {

        }

        protected void UpdateTreeView()
        {
            Debug.Assert(treeView1.InvokeRequired == false);
            this.treeView1.BeginUpdate();
            this.treeView1.Nodes.Clear();
            this.treeView1.CheckBoxes = true;
            this.treeView1.ShowNodeToolTips = true;
            this.PopulateTreeView();
            this.ApplyCheckState();
            this.ApplyHighlightState();
            try
            {
                this.treeView1.ExpandAll();
            }
            catch (Exception ex)
            {

            }
            this.treeView1.EndUpdate();
        }


        protected virtual void PopulateTreeView()
        {

        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TreeView tv = sender as TreeView;
            TreeNode selected = tv.SelectedNode;
            if (selected == null)
                return;

            if (selected.Nodes.Count == 0)
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = selected.Text,
                        UseShellExecute = true
                    };

                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string filter = this.txtFilter.Text;

            bool enablebuttons = false;

            if (checkRegex.Checked)
            {
                try
                {
                    var regex = new Regex(filter, RegexOptions.IgnoreCase);
                    enablebuttons = true;
                    this.txtFilter.ForeColor = Color.Black;
                }
                catch (ArgumentException)
                {
                    this.txtFilter.ForeColor = Color.Red;
                }
            }
            else
            {
                enablebuttons = true;
            }

            this.btnCheck.Enabled = enablebuttons;
            this.btnFilter.Enabled = enablebuttons;
            ApplyHighlightState();
        }

        private void btnFilter_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateTreeView();
        }

        private void TreeNodeCheckRecursive(TreeNodeCollection tnCollection, Action<TreeNode> processorDelegate)
        {
            foreach (TreeNode node in tnCollection)
            {
                processorDelegate(node);
                TreeNodeCheckRecursive(node.Nodes, processorDelegate);
            }
        }

        private void ApplyHighlightState()
        {
            TreeNodeCheckRecursive(this.treeView1.Nodes, node2 =>
            {
                node2.BackColor = IsMatch(node2.Text) ? Color.Yellow : Color.Transparent;
            }
            );
        }

        private void btnCheck_CheckedChanged(object sender, EventArgs e)
        {
            ApplyCheckState();
        }

        private void ApplyCheckState()
        {
            TreeNodeCheckRecursive(this.treeView1.Nodes, node2 =>
            {
                node2.Checked = this.btnCheck.Checked && this.IsMatch(node2.Text);
            });
        }

        protected virtual void PreRunWorkerAsync()
        {

        }

        private async void multipleDirectorySelector1_ScanStart(object sender, EventArgs e)
        {
            var startTime = DateTime.Now;
            this.PreRunWorkerAsync();
            this.ProgressMessage = string.Empty;
            this.ResultsMessage = string.Empty;
            this.ProgressPercent = 0;


            //await this.ScanDirectories();
            //this.UpdateTreeView();
            //var timeDiff = DateTime.Now - startTime;
            //this.ProgressMessage = "Done in " + timeDiff.ToString();

            this.multipleDirectorySelector1.EnableButton = false;

            await Task.Run(this.ScanDirectories).ContinueWith(
                async (Task task1) =>
                {
                    this.UpdateTreeView();
                    var timeDiff = DateTime.Now - startTime;
                    this.ProgressMessage = $"Done in {timeDiff:hh\\:mm\\:ss}";
                    this.multipleDirectorySelector1.EnableButton = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());

            //var task = new Task(() => this.ScanDirectories());
            //var uiThreadScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            //var task2 = task.ContinueWith((continuation) =>
            //{
            //this.UpdateTreeView();
            //var timeDiff = DateTime.Now - startTime;
            //this.ProgressMessage = "Done in " + timeDiff.ToString();
            //}, uiThreadScheduler);
            //task.Start();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ProgressMessage { get; set; } = string.Empty;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ResultsMessage { get; set; } = string.Empty;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ProgressPercent { get; set; } = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            //lock (this.ResultsMessage)
            //{
            this.tsResults.Text = this.ResultsMessage;
            //}
            //lock (this.ProgressMessage)
            //{
            this.tsStatus.Text = this.ProgressMessage;
            //}
            this.toolStripProgressBar1.Value = ProgressPercent;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            e.Node.ForeColor = e.Node.Checked ? Color.Red : Color.Black;
        }

        private void treeView1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
                if (node != null)
                {

                    //var m_OldSelectNode = treeView1.SelectedNode;
                    treeView1.SelectedNode = node;

                    contextMenuStrip.Show(treeView1, e.X, e.Y);

                    //// Find the appropriate ContextMenu depending on the selected node.
                    //switch (Convert.ToString(node.Tag))
                    //{
                    //    case "TextFile":
                    //        mnuTextFile.Show(treeView1, p);
                    //        break;
                    //    case "File":
                    //        mnuFile.Show(treeView1, p);
                    //        break;
                    //}
                    //if (m_OldSelectNode != null)
                    //{
                    //    treeView1.SelectedNode = m_OldSelectNode;
                    //    m_OldSelectNode = null;
                    //}
                }
            }
        }

        private void OpenFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fullname = treeView1.SelectedNode.Text;
            if (File.Exists(fullname))
            {
                var argument = $"/select, \"{fullname}\"";
                Process.Start("explorer.exe", argument);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fullname = treeView1.SelectedNode.Text;
            if (fullname.Contains(" "))
            {
                fullname = "\"" + fullname + "\"";
            }
            Clipboard.SetText(fullname);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtFilter.Clear();
            this.btnCheck.Checked = false;
            this.btnFilter.Checked = false;
        }
    }
}