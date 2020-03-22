namespace ControlsAndFormsTesting
{
	partial class Form1
	{
		#region Members

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		public System.Windows.Forms.Button btnNumericTextBox;
		private DigitalProduction.Forms.NumericTextBox numericTextBox2;
		private DigitalProduction.Forms.NumericTextBox numericTextBox3;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.GroupBox groupBoxXslt;
		private System.Windows.Forms.TextBox textBoxXsltFile;
		private System.Windows.Forms.Button buttonBrowseXslt;
		private System.Windows.Forms.GroupBox groupBoxTextWithInitialDirectory;
		private System.Windows.Forms.TextBox textBoxTextFile;
		private System.Windows.Forms.Button buttonBrowseText;
		private System.Windows.Forms.GroupBox groupBoxTextNoInitialDirectory;
		private System.Windows.Forms.TextBox textBoxTextNoInitialDirectory;
		private System.Windows.Forms.Button buttonBrowseNoInitialDirectory;
		private DigitalProduction.Forms.RotatedLabel rotatedLabel1;
		private System.Windows.Forms.Label label1;

		#endregion

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
			this.btnNumericTextBox = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.groupBoxXslt = new System.Windows.Forms.GroupBox();
			this.textBoxXsltFile = new System.Windows.Forms.TextBox();
			this.buttonBrowseXslt = new System.Windows.Forms.Button();
			this.groupBoxTextWithInitialDirectory = new System.Windows.Forms.GroupBox();
			this.textBoxTextFile = new System.Windows.Forms.TextBox();
			this.buttonBrowseText = new System.Windows.Forms.Button();
			this.groupBoxTextNoInitialDirectory = new System.Windows.Forms.GroupBox();
			this.textBoxTextNoInitialDirectory = new System.Windows.Forms.TextBox();
			this.buttonBrowseNoInitialDirectory = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.rotatedLabel1 = new DigitalProduction.Forms.RotatedLabel();
			this.numericTextBox3 = new DigitalProduction.Forms.NumericTextBox();
			this.numericTextBox2 = new DigitalProduction.Forms.NumericTextBox();
			this.groupBoxXslt.SuspendLayout();
			this.groupBoxTextWithInitialDirectory.SuspendLayout();
			this.groupBoxTextNoInitialDirectory.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnNumericTextBox
			// 
			this.btnNumericTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNumericTextBox.Location = new System.Drawing.Point(724, 39);
			this.btnNumericTextBox.Name = "btnNumericTextBox";
			this.btnNumericTextBox.Size = new System.Drawing.Size(75, 23);
			this.btnNumericTextBox.TabIndex = 1;
			this.btnNumericTextBox.Text = "Enter";
			this.btnNumericTextBox.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(299, 13);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(318, 20);
			this.textBox1.TabIndex = 5;
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox2.Location = new System.Drawing.Point(699, 12);
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(100, 20);
			this.textBox2.TabIndex = 6;
			// 
			// groupBoxXslt
			// 
			this.groupBoxXslt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxXslt.Controls.Add(this.textBoxXsltFile);
			this.groupBoxXslt.Controls.Add(this.buttonBrowseXslt);
			this.groupBoxXslt.Location = new System.Drawing.Point(12, 257);
			this.groupBoxXslt.Name = "groupBoxXslt";
			this.groupBoxXslt.Size = new System.Drawing.Size(786, 58);
			this.groupBoxXslt.TabIndex = 9;
			this.groupBoxXslt.TabStop = false;
			this.groupBoxXslt.Text = "XSLT With Initial Directory";
			// 
			// textBoxXsltFile
			// 
			this.textBoxXsltFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxXsltFile.Location = new System.Drawing.Point(12, 22);
			this.textBoxXsltFile.Name = "textBoxXsltFile";
			this.textBoxXsltFile.Size = new System.Drawing.Size(671, 20);
			this.textBoxXsltFile.TabIndex = 13;
			// 
			// buttonBrowseXslt
			// 
			this.buttonBrowseXslt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowseXslt.Location = new System.Drawing.Point(698, 20);
			this.buttonBrowseXslt.Name = "buttonBrowseXslt";
			this.buttonBrowseXslt.Size = new System.Drawing.Size(75, 23);
			this.buttonBrowseXslt.TabIndex = 12;
			this.buttonBrowseXslt.Text = "Browse";
			this.buttonBrowseXslt.UseVisualStyleBackColor = true;
			this.buttonBrowseXslt.Click += new System.EventHandler(this.buttonBrowse_Click);
			// 
			// groupBoxTextWithInitialDirectory
			// 
			this.groupBoxTextWithInitialDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxTextWithInitialDirectory.Controls.Add(this.textBoxTextFile);
			this.groupBoxTextWithInitialDirectory.Controls.Add(this.buttonBrowseText);
			this.groupBoxTextWithInitialDirectory.Location = new System.Drawing.Point(12, 193);
			this.groupBoxTextWithInitialDirectory.Name = "groupBoxTextWithInitialDirectory";
			this.groupBoxTextWithInitialDirectory.Size = new System.Drawing.Size(786, 58);
			this.groupBoxTextWithInitialDirectory.TabIndex = 14;
			this.groupBoxTextWithInitialDirectory.TabStop = false;
			this.groupBoxTextWithInitialDirectory.Text = "Text With Initial Directory";
			// 
			// textBoxTextFile
			// 
			this.textBoxTextFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTextFile.Location = new System.Drawing.Point(12, 22);
			this.textBoxTextFile.Name = "textBoxTextFile";
			this.textBoxTextFile.Size = new System.Drawing.Size(671, 20);
			this.textBoxTextFile.TabIndex = 13;
			// 
			// buttonBrowseText
			// 
			this.buttonBrowseText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowseText.Location = new System.Drawing.Point(698, 20);
			this.buttonBrowseText.Name = "buttonBrowseText";
			this.buttonBrowseText.Size = new System.Drawing.Size(75, 23);
			this.buttonBrowseText.TabIndex = 12;
			this.buttonBrowseText.Text = "Browse";
			this.buttonBrowseText.UseVisualStyleBackColor = true;
			this.buttonBrowseText.Click += new System.EventHandler(this.buttonBrowseText_Click);
			// 
			// groupBoxTextNoInitialDirectory
			// 
			this.groupBoxTextNoInitialDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxTextNoInitialDirectory.Controls.Add(this.textBoxTextNoInitialDirectory);
			this.groupBoxTextNoInitialDirectory.Controls.Add(this.buttonBrowseNoInitialDirectory);
			this.groupBoxTextNoInitialDirectory.Location = new System.Drawing.Point(12, 321);
			this.groupBoxTextNoInitialDirectory.Name = "groupBoxTextNoInitialDirectory";
			this.groupBoxTextNoInitialDirectory.Size = new System.Drawing.Size(786, 58);
			this.groupBoxTextNoInitialDirectory.TabIndex = 15;
			this.groupBoxTextNoInitialDirectory.TabStop = false;
			this.groupBoxTextNoInitialDirectory.Text = " No Initial Directory";
			// 
			// textBoxTextNoInitialDirectory
			// 
			this.textBoxTextNoInitialDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTextNoInitialDirectory.Location = new System.Drawing.Point(12, 22);
			this.textBoxTextNoInitialDirectory.Name = "textBoxTextNoInitialDirectory";
			this.textBoxTextNoInitialDirectory.Size = new System.Drawing.Size(671, 20);
			this.textBoxTextNoInitialDirectory.TabIndex = 13;
			// 
			// buttonBrowseNoInitialDirectory
			// 
			this.buttonBrowseNoInitialDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowseNoInitialDirectory.Location = new System.Drawing.Point(698, 20);
			this.buttonBrowseNoInitialDirectory.Name = "buttonBrowseNoInitialDirectory";
			this.buttonBrowseNoInitialDirectory.Size = new System.Drawing.Size(75, 23);
			this.buttonBrowseNoInitialDirectory.TabIndex = 12;
			this.buttonBrowseNoInitialDirectory.Text = "Browse";
			this.buttonBrowseNoInitialDirectory.UseVisualStyleBackColor = true;
			this.buttonBrowseNoInitialDirectory.Click += new System.EventHandler(this.buttonBrowseNoInitialDirectory_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(299, 36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(318, 16);
			this.label1.TabIndex = 17;
			this.label1.Text = "label111";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// rotatedLabel1
			// 
			this.rotatedLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rotatedLabel1.Angle = 300;
			this.rotatedLabel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.rotatedLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rotatedLabel1.Location = new System.Drawing.Point(6, 9);
			this.rotatedLabel1.Name = "rotatedLabel1";
			this.rotatedLabel1.Size = new System.Drawing.Size(287, 171);
			this.rotatedLabel1.TabIndex = 16;
			this.rotatedLabel1.Text = "test";
			this.rotatedLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// numericTextBox3
			// 
			this.numericTextBox3.AllowSeparator = false;
			this.numericTextBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericTextBox3.BeepOnInvalidKey = true;
			this.numericTextBox3.DecimalValue = 0D;
			this.numericTextBox3.DisplayAllDecimalPlaces = true;
			this.numericTextBox3.EnforceMaximumValue = false;
			this.numericTextBox3.EnforceMinimumValue = true;
			this.numericTextBox3.IntValue = 0;
			this.numericTextBox3.LimitDecimalPlaces = true;
			this.numericTextBox3.Location = new System.Drawing.Point(299, 71);
			this.numericTextBox3.MaximumValue = 100D;
			this.numericTextBox3.MinimumValue = 0D;
			this.numericTextBox3.Name = "numericTextBox3";
			this.numericTextBox3.NumberOfDecimalPlaces = ((uint)(3u));
			this.numericTextBox3.Size = new System.Drawing.Size(318, 20);
			this.numericTextBox3.TabIndex = 4;
			this.numericTextBox3.Text = "0.000";
			this.numericTextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericTextBox3.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox3_Validating);
			// 
			// numericTextBox2
			// 
			this.numericTextBox2.AllowSeparator = false;
			this.numericTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericTextBox2.BeepOnInvalidKey = true;
			this.numericTextBox2.DecimalValue = 0D;
			this.numericTextBox2.DisplayAllDecimalPlaces = true;
			this.numericTextBox2.EnforceMaximumValue = false;
			this.numericTextBox2.EnforceMinimumValue = true;
			this.numericTextBox2.IntValue = 0;
			this.numericTextBox2.LimitDecimalPlaces = true;
			this.numericTextBox2.Location = new System.Drawing.Point(700, 71);
			this.numericTextBox2.MaximumValue = 100D;
			this.numericTextBox2.MinimumValue = 0D;
			this.numericTextBox2.Name = "numericTextBox2";
			this.numericTextBox2.NumberOfDecimalPlaces = ((uint)(3u));
			this.numericTextBox2.ReadOnly = true;
			this.numericTextBox2.Size = new System.Drawing.Size(100, 20);
			this.numericTextBox2.TabIndex = 2;
			this.numericTextBox2.Text = "0.000";
			this.numericTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericTextBox2.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox3_Validating);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(812, 388);
			this.Controls.Add(this.rotatedLabel1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBoxTextNoInitialDirectory);
			this.Controls.Add(this.groupBoxTextWithInitialDirectory);
			this.Controls.Add(this.groupBoxXslt);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.numericTextBox3);
			this.Controls.Add(this.numericTextBox2);
			this.Controls.Add(this.btnNumericTextBox);
			this.Name = "Form1";
			this.Text = "Form1";
			this.groupBoxXslt.ResumeLayout(false);
			this.groupBoxXslt.PerformLayout();
			this.groupBoxTextWithInitialDirectory.ResumeLayout(false);
			this.groupBoxTextWithInitialDirectory.PerformLayout();
			this.groupBoxTextNoInitialDirectory.ResumeLayout(false);
			this.groupBoxTextNoInitialDirectory.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

	} // End class.
} // End namespace.