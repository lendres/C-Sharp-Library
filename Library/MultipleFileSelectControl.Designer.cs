namespace DigitalProduction
{
	partial class MultipleFileSelectControl
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
			this.removeFilesButton = new System.Windows.Forms.Button();
			this.filesListBox = new System.Windows.Forms.ListBox();
			this.selectFilesButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// removeFilesButton
			// 
			this.removeFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.removeFilesButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.removeFilesButton.Location = new System.Drawing.Point(430, 0);
			this.removeFilesButton.Name = "removeFilesButton";
			this.removeFilesButton.Size = new System.Drawing.Size(80, 23);
			this.removeFilesButton.TabIndex = 3;
			this.removeFilesButton.Text = "Remove";
			this.removeFilesButton.UseVisualStyleBackColor = true;
			// 
			// filesListBox
			// 
			this.filesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.filesListBox.FormattingEnabled = true;
			this.filesListBox.Location = new System.Drawing.Point(0, 36);
			this.filesListBox.Name = "filesListBox";
			this.filesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.filesListBox.Size = new System.Drawing.Size(510, 212);
			this.filesListBox.TabIndex = 2;
			// 
			// selectFilesButton
			// 
			this.selectFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.selectFilesButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.selectFilesButton.Location = new System.Drawing.Point(0, 0);
			this.selectFilesButton.Name = "selectFilesButton";
			this.selectFilesButton.Size = new System.Drawing.Size(80, 23);
			this.selectFilesButton.TabIndex = 4;
			this.selectFilesButton.Text = "Select Files...";
			this.selectFilesButton.UseVisualStyleBackColor = true;
			this.selectFilesButton.Click += new System.EventHandler(this.selectFilesButton_Click);
			// 
			// MultipleFileSelectControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.selectFilesButton);
			this.Controls.Add(this.removeFilesButton);
			this.Controls.Add(this.filesListBox);
			this.Name = "MultipleFileSelectControl";
			this.Size = new System.Drawing.Size(510, 248);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button removeFilesButton;
		private System.Windows.Forms.ListBox filesListBox;
		private System.Windows.Forms.Button selectFilesButton;
	}
}
