namespace FileTools.Dupecheck
{
	partial class frmEmptyDirectoryLister
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
			SuspendLayout();
			// btnDelete
			// 
			btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnDelete.Location = new System.Drawing.Point(706, 578);
			btnDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new System.Drawing.Size(88, 27);
			btnDelete.TabIndex = 9;
			btnDelete.Text = "Delete...";
			btnDelete.UseVisualStyleBackColor = true;
			btnDelete.Click += btnDelete_Click;
			// 
			// frmEmptyDirectoryLister
			// 
			Controls.Add(btnDelete);
			Name = "frmEmptyDirectoryLister";
			Text = "Empty Directories";
			Controls.SetChildIndex(btnDelete, 0);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button btnDelete;
	}
}