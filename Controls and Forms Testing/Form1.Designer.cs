namespace ControlsAndFormsTesting
{
	partial class Form1
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
			this.btnNumericTextBox = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.numericTextBox3 = new DigitalProduction.Forms.NumericTextBox();
			this.numericTextBox2 = new DigitalProduction.Forms.NumericTextBox();
			this.SuspendLayout();
			// 
			// btnNumericTextBox
			// 
			this.btnNumericTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNumericTextBox.Location = new System.Drawing.Point(416, 39);
			this.btnNumericTextBox.Name = "btnNumericTextBox";
			this.btnNumericTextBox.Size = new System.Drawing.Size(75, 23);
			this.btnNumericTextBox.TabIndex = 1;
			this.btnNumericTextBox.Text = "Enter";
			this.btnNumericTextBox.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(209, 13);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 20);
			this.textBox1.TabIndex = 5;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(391, 12);
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(100, 20);
			this.textBox2.TabIndex = 6;
			// 
			// numericTextBox3
			// 
			this.numericTextBox3.AllowSeparator = false;
			this.numericTextBox3.BeepOnInvalidKey = true;
			this.numericTextBox3.DecimalValue = 0D;
			this.numericTextBox3.DisplayAllDecimalPlaces = true;
			this.numericTextBox3.EnforceMaximumValue = false;
			this.numericTextBox3.EnforceMinimumValue = true;
			this.numericTextBox3.IntValue = 0;
			this.numericTextBox3.LimitDecimalPlaces = true;
			this.numericTextBox3.Location = new System.Drawing.Point(209, 110);
			this.numericTextBox3.MaximumValue = 100D;
			this.numericTextBox3.MinimumValue = 0D;
			this.numericTextBox3.Name = "numericTextBox3";
			this.numericTextBox3.NumberOfDecimalPlaces = ((uint)(3u));
			this.numericTextBox3.Size = new System.Drawing.Size(100, 20);
			this.numericTextBox3.TabIndex = 4;
			this.numericTextBox3.Text = "0";
			this.numericTextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericTextBox3.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox3_Validating);
			// 
			// numericTextBox2
			// 
			this.numericTextBox2.AllowSeparator = false;
			this.numericTextBox2.BeepOnInvalidKey = true;
			this.numericTextBox2.DecimalValue = 0D;
			this.numericTextBox2.DisplayAllDecimalPlaces = true;
			this.numericTextBox2.EnforceMaximumValue = false;
			this.numericTextBox2.EnforceMinimumValue = true;
			this.numericTextBox2.IntValue = 0;
			this.numericTextBox2.LimitDecimalPlaces = true;
			this.numericTextBox2.Location = new System.Drawing.Point(392, 110);
			this.numericTextBox2.MaximumValue = 100D;
			this.numericTextBox2.MinimumValue = 0D;
			this.numericTextBox2.Name = "numericTextBox2";
			this.numericTextBox2.NumberOfDecimalPlaces = ((uint)(3u));
			this.numericTextBox2.ReadOnly = true;
			this.numericTextBox2.Size = new System.Drawing.Size(100, 20);
			this.numericTextBox2.TabIndex = 2;
			this.numericTextBox2.Text = "0";
			this.numericTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericTextBox2.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox3_Validating);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(504, 277);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.numericTextBox3);
			this.Controls.Add(this.numericTextBox2);
			this.Controls.Add(this.btnNumericTextBox);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Button btnNumericTextBox;
		private DigitalProduction.Forms.NumericTextBox numericTextBox2;
		private DigitalProduction.Forms.NumericTextBox numericTextBox3;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;

	}
}