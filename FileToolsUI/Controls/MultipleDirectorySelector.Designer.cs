namespace FileTools.Controls
{
    partial class MultipleDirectorySelector
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstDirectories = new System.Windows.Forms.ListBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnDirectoryAdd = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // lstDirectories
            // 
            this.lstDirectories.AllowDrop = true;
            this.lstDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDirectories.FormattingEnabled = true;
            this.lstDirectories.Location = new System.Drawing.Point(0, 0);
            this.lstDirectories.Name = "lstDirectories";
            this.lstDirectories.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstDirectories.Size = new System.Drawing.Size(675, 82);
            this.lstDirectories.TabIndex = 7;
            this.lstDirectories.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstDirectories_DragDrop);
            this.lstDirectories.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstDirectories_DragEnter);
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Location = new System.Drawing.Point(681, 58);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 6;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnDirectoryAdd
            // 
            this.btnDirectoryAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDirectoryAdd.Location = new System.Drawing.Point(681, 0);
            this.btnDirectoryAdd.Name = "btnDirectoryAdd";
            this.btnDirectoryAdd.Size = new System.Drawing.Size(75, 23);
            this.btnDirectoryAdd.TabIndex = 8;
            this.btnDirectoryAdd.Text = "Add...";
            this.btnDirectoryAdd.UseVisualStyleBackColor = true;
            this.btnDirectoryAdd.Click += new System.EventHandler(this.btnDirectoryAdd_Click);
            // 
            // Remove
            // 
            this.Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Remove.Location = new System.Drawing.Point(681, 29);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(75, 23);
            this.Remove.TabIndex = 9;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // MultipleDirectorySelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstDirectories);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.btnDirectoryAdd);
            this.Controls.Add(this.Remove);
            this.Name = "MultipleDirectorySelector";
            this.Size = new System.Drawing.Size(762, 84);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstDirectories;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnDirectoryAdd;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}
