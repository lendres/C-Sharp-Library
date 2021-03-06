﻿namespace DigitalProduction.Forms
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
			this.filesGroupBox = new System.Windows.Forms.GroupBox();
			this.selectFilesButton = new System.Windows.Forms.Button();
			this.removeFilesButton = new System.Windows.Forms.Button();
			this.filesListBox = new System.Windows.Forms.ListBox();
			this.clearAllButton = new System.Windows.Forms.Button();
			this.filesGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// filesGroupBox
			// 
			this.filesGroupBox.Controls.Add(this.selectFilesButton);
			this.filesGroupBox.Controls.Add(this.removeFilesButton);
			this.filesGroupBox.Controls.Add(this.filesListBox);
			this.filesGroupBox.Controls.Add(this.clearAllButton);
			this.filesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.filesGroupBox.Location = new System.Drawing.Point(0, 0);
			this.filesGroupBox.Name = "filesGroupBox";
			this.filesGroupBox.Size = new System.Drawing.Size(510, 248);
			this.filesGroupBox.TabIndex = 0;
			this.filesGroupBox.TabStop = false;
			this.filesGroupBox.Text = "Files";
			// 
			// selectFilesButton
			// 
			this.selectFilesButton.Location = new System.Drawing.Point(6, 19);
			this.selectFilesButton.Name = "selectFilesButton";
			this.selectFilesButton.Size = new System.Drawing.Size(80, 23);
			this.selectFilesButton.TabIndex = 7;
			this.selectFilesButton.Text = "Select Files...";
			this.selectFilesButton.UseVisualStyleBackColor = true;
			this.selectFilesButton.Click += new System.EventHandler(this.selectFilesButton_Click);
			// 
			// removeFilesButton
			// 
			this.removeFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.removeFilesButton.Enabled = false;
			this.removeFilesButton.Location = new System.Drawing.Point(333, 19);
			this.removeFilesButton.Name = "removeFilesButton";
			this.removeFilesButton.Size = new System.Drawing.Size(80, 23);
			this.removeFilesButton.TabIndex = 6;
			this.removeFilesButton.Text = "Remove";
			this.removeFilesButton.UseVisualStyleBackColor = true;
			this.removeFilesButton.Click += new System.EventHandler(this.removeFilesButton_Click);
			// 
			// filesListBox
			// 
			this.filesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.filesListBox.FormattingEnabled = true;
			this.filesListBox.Location = new System.Drawing.Point(6, 56);
			this.filesListBox.Name = "filesListBox";
			this.filesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.filesListBox.Size = new System.Drawing.Size(498, 186);
			this.filesListBox.TabIndex = 5;
			this.filesListBox.SelectedIndexChanged += new System.EventHandler(this.filesListBox_SelectedIndexChanged);
			// 
			// clearAllButton
			// 
			this.clearAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.clearAllButton.Enabled = false;
			this.clearAllButton.Location = new System.Drawing.Point(424, 19);
			this.clearAllButton.Name = "clearAllButton";
			this.clearAllButton.Size = new System.Drawing.Size(80, 23);
			this.clearAllButton.TabIndex = 8;
			this.clearAllButton.Text = "Clear All";
			this.clearAllButton.UseVisualStyleBackColor = true;
			this.clearAllButton.Click += new System.EventHandler(this.clearAllButton_Click);
			// 
			// MultipleFileSelectControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.filesGroupBox);
			this.Name = "MultipleFileSelectControl";
			this.Size = new System.Drawing.Size(510, 248);
			this.filesGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox filesGroupBox;
		private System.Windows.Forms.Button selectFilesButton;
		private System.Windows.Forms.Button removeFilesButton;
		private System.Windows.Forms.ListBox filesListBox;
		private System.Windows.Forms.Button clearAllButton;

	}
}
