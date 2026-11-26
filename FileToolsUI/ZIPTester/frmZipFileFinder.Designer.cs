namespace FileTools.ZipTester
{
	partial class frmZipTester
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
			btnTest = new System.Windows.Forms.Button();
			btnRecompress = new System.Windows.Forms.Button();
			cboCompressionMethod = new System.Windows.Forms.ComboBox();
			chkSizeCheck = new System.Windows.Forms.CheckBox();
			SuspendLayout();
			// 
			// btnTest
			// 
			btnTest.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnTest.Location = new System.Drawing.Point(899, 586);
			btnTest.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnTest.Name = "btnTest";
			btnTest.Size = new System.Drawing.Size(43, 27);
			btnTest.TabIndex = 9;
			btnTest.Text = "Test";
			btnTest.UseVisualStyleBackColor = true;
			btnTest.Click += btnTest_Click;
			// 
			// btnRecompress
			// 
			btnRecompress.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnRecompress.Location = new System.Drawing.Point(805, 586);
			btnRecompress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnRecompress.Name = "btnRecompress";
			btnRecompress.Size = new System.Drawing.Size(88, 27);
			btnRecompress.TabIndex = 13;
			btnRecompress.Text = "Recompress";
			btnRecompress.UseVisualStyleBackColor = true;
			btnRecompress.Click += btnRecompress_Click;
			// 
			// cboCompressionMethod
			// 
			cboCompressionMethod.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			cboCompressionMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cboCompressionMethod.FormattingEnabled = true;
			cboCompressionMethod.Items.AddRange(new object[] { "7z", "zip" });
			cboCompressionMethod.Location = new System.Drawing.Point(750, 587);
			cboCompressionMethod.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			cboCompressionMethod.Name = "cboCompressionMethod";
			cboCompressionMethod.Size = new System.Drawing.Size(46, 23);
			cboCompressionMethod.TabIndex = 14;
			// 
			// chkSizeCheck
			// 
			chkSizeCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			chkSizeCheck.AutoSize = true;
			chkSizeCheck.Location = new System.Drawing.Point(662, 593);
			chkSizeCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkSizeCheck.Name = "chkSizeCheck";
			chkSizeCheck.Size = new System.Drawing.Size(81, 19);
			chkSizeCheck.TabIndex = 15;
			chkSizeCheck.Text = "Check size";
			chkSizeCheck.UseVisualStyleBackColor = true;
			// 
			// frmZipTester
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(chkSizeCheck);
			Controls.Add(btnTest);
			Controls.Add(btnRecompress);
			Controls.Add(cboCompressionMethod);
			Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
			Name = "frmZipTester";
			Text = "frmZipTester";
			Controls.SetChildIndex(cboCompressionMethod, 0);
			Controls.SetChildIndex(btnRecompress, 0);
			Controls.SetChildIndex(btnTest, 0);
			Controls.SetChildIndex(chkSizeCheck, 0);
			Controls.SetChildIndex(multipleDirectorySelector1, 0);
			Controls.SetChildIndex(treeView1, 0);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Button btnRecompress;
		private System.Windows.Forms.ComboBox cboCompressionMethod;
		private System.Windows.Forms.CheckBox chkSizeCheck;
	}
}