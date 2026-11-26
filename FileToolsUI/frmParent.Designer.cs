namespace FileTools
{
    partial class frmParent
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmParent));
			toolStrip = new System.Windows.Forms.ToolStrip();
			newToolStripButton = new System.Windows.Forms.ToolStripButton();
			toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			tsbPHash = new System.Windows.Forms.ToolStripButton();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			toolStripDropDownCache = new System.Windows.Forms.ToolStripDropDownButton();
			clearCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			clearFailedCachesEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			statusStrip = new System.Windows.Forms.StatusStrip();
			toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			ToolTip = new System.Windows.Forms.ToolTip(components);
			toolStripButtonLogs = new System.Windows.Forms.ToolStripButton();
			toolStrip.SuspendLayout();
			statusStrip.SuspendLayout();
			SuspendLayout();
			// 
			// toolStrip
			// 
			toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { newToolStripButton, toolStripButton2, toolStripButton1, toolStripButton3, toolStripButton4, tsbPHash, toolStripSeparator1, toolStripDropDownCache, toolStripButtonLogs });
			toolStrip.Location = new System.Drawing.Point(0, 0);
			toolStrip.Name = "toolStrip";
			toolStrip.Size = new System.Drawing.Size(951, 25);
			toolStrip.TabIndex = 1;
			toolStrip.Text = "ToolStrip";
			// 
			// newToolStripButton
			// 
			newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			newToolStripButton.Image = (System.Drawing.Image)resources.GetObject("newToolStripButton.Image");
			newToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
			newToolStripButton.Name = "newToolStripButton";
			newToolStripButton.Size = new System.Drawing.Size(30, 22);
			newToolStripButton.Text = "SFV";
			newToolStripButton.Click += newToolStripButton_Click;
			// 
			// toolStripButton2
			// 
			toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
			toolStripButton2.ImageTransparentColor = System.Drawing.Color.Black;
			toolStripButton2.Name = "toolStripButton2";
			toolStripButton2.Size = new System.Drawing.Size(61, 22);
			toolStripButton2.Text = "Duplicate";
			toolStripButton2.Click += toolStripButton2_Click;
			// 
			// toolStripButton1
			// 
			toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
			toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripButton1.Name = "toolStripButton1";
			toolStripButton1.Size = new System.Drawing.Size(124, 22);
			toolStripButton1.Text = "Empty Directory Scan";
			toolStripButton1.Click += toolStripButton1_Click;
			// 
			// toolStripButton3
			// 
			toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			toolStripButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripButton3.Image");
			toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripButton3.Name = "toolStripButton3";
			toolStripButton3.Size = new System.Drawing.Size(61, 22);
			toolStripButton3.Text = "Zip Tester";
			toolStripButton3.Click += toolStripButton3_Click;
			// 
			// toolStripButton4
			// 
			toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			toolStripButton4.Image = (System.Drawing.Image)resources.GetObject("toolStripButton4.Image");
			toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripButton4.Name = "toolStripButton4";
			toolStripButton4.Size = new System.Drawing.Size(106, 22);
			toolStripButton4.Text = "FileTypeMismatch";
			toolStripButton4.Click += toolStripButton4_Click;
			// 
			// tsbPHash
			// 
			tsbPHash.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			tsbPHash.Image = (System.Drawing.Image)resources.GetObject("tsbPHash.Image");
			tsbPHash.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbPHash.Name = "tsbPHash";
			tsbPHash.Size = new System.Drawing.Size(45, 22);
			tsbPHash.Text = "PHash";
			tsbPHash.Click += tsbPHash_Click;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripDropDownCache
			// 
			toolStripDropDownCache.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			toolStripDropDownCache.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { clearCacheToolStripMenuItem, clearFailedCachesEntriesToolStripMenuItem });
			toolStripDropDownCache.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownCache.Image");
			toolStripDropDownCache.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripDropDownCache.Name = "toolStripDropDownCache";
			toolStripDropDownCache.Size = new System.Drawing.Size(53, 22);
			toolStripDropDownCache.Text = "Cache";
			toolStripDropDownCache.ToolTipText = "Cache";
			// 
			// clearCacheToolStripMenuItem
			// 
			clearCacheToolStripMenuItem.Name = "clearCacheToolStripMenuItem";
			clearCacheToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			clearCacheToolStripMenuItem.Text = "Clear Cache";
			clearCacheToolStripMenuItem.Click += clearCacheToolStripMenuItem_Click;
			// 
			// clearFailedCachesEntriesToolStripMenuItem
			// 
			clearFailedCachesEntriesToolStripMenuItem.Name = "clearFailedCachesEntriesToolStripMenuItem";
			clearFailedCachesEntriesToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			clearFailedCachesEntriesToolStripMenuItem.Text = "Clear failed caches entries";
			clearFailedCachesEntriesToolStripMenuItem.Click += clearFailedCachesEntriesToolStripMenuItem_Click;
			// 
			// statusStrip
			// 
			statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel });
			statusStrip.Location = new System.Drawing.Point(0, 662);
			statusStrip.Name = "statusStrip";
			statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			statusStrip.Size = new System.Drawing.Size(951, 22);
			statusStrip.TabIndex = 2;
			statusStrip.Text = "StatusStrip";
			// 
			// toolStripStatusLabel
			// 
			toolStripStatusLabel.Name = "toolStripStatusLabel";
			toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
			toolStripStatusLabel.Text = "Status";
			// 
			// toolStripButtonLogs
			// 
			toolStripButtonLogs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			toolStripButtonLogs.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonLogs.Image");
			toolStripButtonLogs.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripButtonLogs.Name = "toolStripButtonLogs";
			toolStripButtonLogs.Size = new System.Drawing.Size(36, 22);
			toolStripButtonLogs.Text = "Logs";
			toolStripButtonLogs.Click += toolStripButtonLogs_Click;
			// 
			// frmParent
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(951, 684);
			Controls.Add(statusStrip);
			Controls.Add(toolStrip);
			Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			IsMdiContainer = true;
			Name = "frmParent";
			Text = "FileTools";
			toolStrip.ResumeLayout(false);
			toolStrip.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
		#endregion

		private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton tsbPHash;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownCache;
		private System.Windows.Forms.ToolStripMenuItem clearCacheToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearFailedCachesEntriesToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButtonLogs;
	}
}



