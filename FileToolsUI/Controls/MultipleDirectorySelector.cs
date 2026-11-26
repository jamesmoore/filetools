using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FileTools.Controls
{
    public partial class MultipleDirectorySelector : UserControl
    {
        public event EventHandler ScanStart;

        public MultipleDirectorySelector()
        {
            InitializeComponent();
        }

        private void btnDirectoryAdd_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (this.folderBrowserDialog1.SelectedPath != "")
            {
                this.lstDirectories.Items.Add(this.folderBrowserDialog1.SelectedPath);
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            var selected = this.lstDirectories.SelectedItems.OfType<object>().ToList();
            foreach (var selecteditem in selected)
            {
                this.lstDirectories.Items.Remove(selecteditem);
            }
        }

        private void lstDirectories_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var droppedPaths = (string[])e.Data.GetData(DataFormats.FileDrop);

            var directories = droppedPaths
                .Select(ResolveDirectory)
                .Where(path => path != null)
                .Cast<string>();

            var existing = this.lstDirectories.Items
                .Cast<string>()
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            this.lstDirectories.Items.AddRange(
                directories.Where(existing.Add).ToArray());
        }

        private static string ResolveDirectory(string path)
        {
            if (Directory.Exists(path))
                return path;

            if (!path.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
                return null;

            Type shellType = Type.GetTypeFromProgID("WScript.Shell");
            if (shellType == null)
                return null;

            dynamic shell = Activator.CreateInstance(shellType);

            try
            {
                dynamic shortcut = shell.CreateShortcut(path);
                string targetPath = shortcut.TargetPath;

                return Directory.Exists(targetPath)
                    ? targetPath
                    : null;
            }
            finally
            {
                if (shell != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shell);
            }
        }

        private void lstDirectories_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (this.ScanStart != null)
            {
                ScanStart(this, new EventArgs());
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableButton
        {
            get { return this.btnScan.Enabled; }
            set {  this.btnScan.Enabled = value;}
        }

        public IEnumerable<string> Directories
        {
            get
            {
                foreach (string s in this.lstDirectories.Items)
                {
                    yield return s;
                }
            }
        }
    }
}