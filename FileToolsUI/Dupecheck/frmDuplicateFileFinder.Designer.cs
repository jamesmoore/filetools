namespace FileTools.Dupecheck
{
    partial class frmDuplicateFileFinder
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
			btnDelete = new System.Windows.Forms.Button();
			ZipHandlingMethodComboBox = new System.Windows.Forms.ComboBox();
           MatchFilenameCheckBox = new System.Windows.Forms.CheckBox();
			btnAutoCheck = new System.Windows.Forms.Button();
			AutocheckTypeCombo = new System.Windows.Forms.ComboBox();
			SuspendLayout();

			// 
			// btnDelete
			// 
			btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnDelete.Location = new System.Drawing.Point(870, 588);
			btnDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new System.Drawing.Size(72, 27);
			btnDelete.TabIndex = 12;
			btnDelete.Text = "Delete";
			btnDelete.UseVisualStyleBackColor = true;
			btnDelete.Click += btnDelete_Click;
			// 
			// ZipHandlingMethodComboBox
			// 
			ZipHandlingMethodComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			ZipHandlingMethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			ZipHandlingMethodComboBox.FormattingEnabled = true;
			ZipHandlingMethodComboBox.Location = new System.Drawing.Point(690, 24);
			ZipHandlingMethodComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			ZipHandlingMethodComboBox.Name = "ZipHandlingMethodComboBox";
			ZipHandlingMethodComboBox.Size = new System.Drawing.Size(140, 23);
			ZipHandlingMethodComboBox.TabIndex = 13;
			// 
         // MatchFilenameCheckBox
			// 
			MatchFilenameCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			MatchFilenameCheckBox.AutoSize = true;
			MatchFilenameCheckBox.Location = new System.Drawing.Point(557, 27);
			MatchFilenameCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			MatchFilenameCheckBox.Name = "MatchFilenameCheckBox";
			MatchFilenameCheckBox.Size = new System.Drawing.Size(108, 19);
			MatchFilenameCheckBox.TabIndex = 16;
			MatchFilenameCheckBox.Text = "Match filename";
			MatchFilenameCheckBox.UseVisualStyleBackColor = true;
			// 
			// btnAutoCheck
			// 
			btnAutoCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnAutoCheck.Location = new System.Drawing.Point(666, 588);
			btnAutoCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnAutoCheck.Name = "btnAutoCheck";
			btnAutoCheck.Size = new System.Drawing.Size(88, 27);
			btnAutoCheck.TabIndex = 14;
			btnAutoCheck.Text = "AutoCheck";
			btnAutoCheck.UseVisualStyleBackColor = true;
			btnAutoCheck.Click += btnAutoCheck_Click;
			// 
			// AutocheckTypeCombo
			// 
			AutocheckTypeCombo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			AutocheckTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			AutocheckTypeCombo.FormattingEnabled = true;
			AutocheckTypeCombo.Location = new System.Drawing.Point(761, 588);
			AutocheckTypeCombo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			AutocheckTypeCombo.Name = "AutocheckTypeCombo";
			AutocheckTypeCombo.Size = new System.Drawing.Size(102, 23);
			AutocheckTypeCombo.TabIndex = 15;
			// 
			// frmDuplicateFileFinder
			// 
           Controls.Add(MatchFilenameCheckBox);
			Controls.Add(AutocheckTypeCombo);
			Controls.Add(btnAutoCheck);
			Controls.Add(ZipHandlingMethodComboBox);
			Controls.Add(btnDelete);
			Name = "frmDuplicateFileFinder";
			Text = "Duplicate file finder";
			Controls.SetChildIndex(multipleDirectorySelector1, 0);
			Controls.SetChildIndex(btnDelete, 0);
			Controls.SetChildIndex(treeView1, 0);
			Controls.SetChildIndex(ZipHandlingMethodComboBox, 0);
			Controls.SetChildIndex(btnAutoCheck, 0);
			Controls.SetChildIndex(AutocheckTypeCombo, 0);
            Controls.SetChildIndex(MatchFilenameCheckBox, 0);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ComboBox ZipHandlingMethodComboBox;
        private System.Windows.Forms.CheckBox MatchFilenameCheckBox;
        private System.Windows.Forms.Button btnAutoCheck;
        private System.Windows.Forms.ComboBox AutocheckTypeCombo;
    }
}