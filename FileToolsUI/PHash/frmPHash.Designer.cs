namespace FileToolsUI.PHash
{
	partial class frmPHash
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
			lblMatch = new System.Windows.Forms.Label();
			btnDelete = new System.Windows.Forms.Button();
			minpHash = new System.Windows.Forms.NumericUpDown();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			LeftHandFilterTextbox = new System.Windows.Forms.TextBox();
			RightHandFilterTextbox = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)minpHash).BeginInit();
			SuspendLayout();
			// 
			// treeView1
			// 
			treeView1.LineColor = System.Drawing.Color.Black;
			// 
			// lblMatch
			// 
			lblMatch.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblMatch.AutoSize = true;
			lblMatch.Location = new System.Drawing.Point(470, 593);
			lblMatch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblMatch.Name = "lblMatch";
			lblMatch.Size = new System.Drawing.Size(16, 15);
			lblMatch.TabIndex = 12;
			lblMatch.Text = "...";
			// 
			// btnDelete
			// 
			btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnDelete.Location = new System.Drawing.Point(854, 584);
			btnDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new System.Drawing.Size(88, 27);
			btnDelete.TabIndex = 13;
			btnDelete.Text = "Delete";
			btnDelete.UseVisualStyleBackColor = true;
			btnDelete.Click += btnDelete_Click;
			// 
			// minpHash
			// 
			minpHash.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			minpHash.Location = new System.Drawing.Point(817, 130);
			minpHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			minpHash.Name = "minpHash";
			minpHash.Size = new System.Drawing.Size(64, 23);
			minpHash.TabIndex = 14;
			minpHash.Value = new decimal(new int[] { 98, 0, 0, 0 });
			// 
			// label2
			// 
			label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(703, 132);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(92, 15);
			label2.TabIndex = 15;
			label2.Text = "Min match level";
			// 
			// label3
			// 
			label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(703, 167);
			label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(84, 15);
			label3.TabIndex = 16;
			label3.Text = "Left hand filter";
			// 
			// LeftHandFilterTextbox
			// 
			LeftHandFilterTextbox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			LeftHandFilterTextbox.Location = new System.Drawing.Point(817, 163);
			LeftHandFilterTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			LeftHandFilterTextbox.Name = "LeftHandFilterTextbox";
			LeftHandFilterTextbox.Size = new System.Drawing.Size(116, 23);
			LeftHandFilterTextbox.TabIndex = 17;
			// 
			// RightHandFilterTextbox
			// 
			RightHandFilterTextbox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			RightHandFilterTextbox.Location = new System.Drawing.Point(817, 193);
			RightHandFilterTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			RightHandFilterTextbox.Name = "RightHandFilterTextbox";
			RightHandFilterTextbox.Size = new System.Drawing.Size(116, 23);
			RightHandFilterTextbox.TabIndex = 19;
			// 
			// label4
			// 
			label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(703, 197);
			label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(92, 15);
			label4.TabIndex = 18;
			label4.Text = "Right hand filter";
			// 
			// frmPHash
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(957, 647);
			Controls.Add(RightHandFilterTextbox);
			Controls.Add(label4);
			Controls.Add(LeftHandFilterTextbox);
			Controls.Add(label3);
			Controls.Add(label2);
			Controls.Add(minpHash);
			Controls.Add(btnDelete);
			Controls.Add(lblMatch);
			Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
			Name = "frmPHash";
			Text = "frmPHash";
			Controls.SetChildIndex(multipleDirectorySelector1, 0);
			Controls.SetChildIndex(treeView1, 0);
			Controls.SetChildIndex(lblMatch, 0);
			Controls.SetChildIndex(btnDelete, 0);
			Controls.SetChildIndex(minpHash, 0);
			Controls.SetChildIndex(label2, 0);
			Controls.SetChildIndex(label3, 0);
			Controls.SetChildIndex(LeftHandFilterTextbox, 0);
			Controls.SetChildIndex(label4, 0);
			Controls.SetChildIndex(RightHandFilterTextbox, 0);
			((System.ComponentModel.ISupportInitialize)minpHash).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblMatch;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.NumericUpDown minpHash;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox LeftHandFilterTextbox;
		private System.Windows.Forms.TextBox RightHandFilterTextbox;
		private System.Windows.Forms.Label label4;
	}
}