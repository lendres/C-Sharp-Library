using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace DigitalProduction.Forms
{
	partial class AboutForm1
	{
		#region Members.

		private IContainer components		= null;

		private PictureBox pictureBoxLogo;
		private Label labelProductName;
		private Label labelVersion;
		private Label labelCopyright;
		private Label labelCompanyName;
		private RichTextBox textBoxxDescription;
		private Button btnOK;
		private Label lblDescription;
		private Label lblReportErrors;
		private LinkLabel linkReportErrors;

		#endregion

		#region Disposing.

		/// <summary>
		/// Dispose of form.
		/// </summary>
		/// <param name="disposing">Disposing of form.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Windows Form Designer generated code.

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AboutForm1));
			this.pictureBoxLogo = new PictureBox();
			this.labelProductName = new Label();
			this.labelVersion = new Label();
			this.labelCopyright = new Label();
			this.labelCompanyName = new Label();
			this.textBoxxDescription = new RichTextBox();
			this.btnOK = new Button();
			this.lblReportErrors = new Label();
			this.linkReportErrors = new LinkLabel();
			this.lblDescription = new Label();
			//this.picboxLogo.BeginInit();
			this.SuspendLayout();
			this.pictureBoxLogo.Image = (Image)componentResourceManager.GetObject("picboxLogo.Image");
			this.pictureBoxLogo.Location = new Point(3, 2);
			this.pictureBoxLogo.Name = "picboxLogo";
			this.pictureBoxLogo.Size = new Size(142, 303);
			this.pictureBoxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBoxLogo.TabIndex = 26;
			this.pictureBoxLogo.TabStop = false;
			this.labelProductName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.labelProductName.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.labelProductName.ForeColor = Color.FromArgb(192, 0, 0);
			this.labelProductName.Location = new Point(150, 9);
			this.labelProductName.Margin = new Padding(6, 0, 3, 0);
			this.labelProductName.Name = "lblProductName";
			this.labelProductName.Size = new Size(293, 17);
			this.labelProductName.TabIndex = 27;
			this.labelProductName.Text = "Product Name";
			this.labelProductName.TextAlign = ContentAlignment.MiddleLeft;
			this.labelVersion.Location = new Point(151, 28);
			this.labelVersion.Margin = new Padding(6, 0, 3, 0);
			this.labelVersion.Name = "lblVersion";
			this.labelVersion.Size = new Size(293, 17);
			this.labelVersion.TabIndex = 25;
			this.labelVersion.Text = "Version";
			this.labelCopyright.Location = new Point(151, 67);
			this.labelCopyright.Margin = new Padding(6, 0, 3, 0);
			this.labelCopyright.Name = "lblCopyright";
			this.labelCopyright.Size = new Size(293, 17);
			this.labelCopyright.TabIndex = 28;
			this.labelCopyright.Text = "Copyright";
			this.labelCopyright.TextAlign = ContentAlignment.MiddleLeft;
			this.labelCompanyName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.labelCompanyName.ForeColor = Color.FromArgb(0, 0, 192);
			this.labelCompanyName.Location = new Point(151, 53);
			this.labelCompanyName.Margin = new Padding(6, 0, 3, 0);
			this.labelCompanyName.Name = "lblCompanyName";
			this.labelCompanyName.Size = new Size(293, 17);
			this.labelCompanyName.TabIndex = 29;
			this.labelCompanyName.Text = "Company Name";
			this.labelCompanyName.TextAlign = ContentAlignment.MiddleLeft;
			this.textBoxxDescription.Location = new Point(154, 123);
			this.textBoxxDescription.Margin = new Padding(6, 3, 3, 3);
			this.textBoxxDescription.Name = "txtbxDescription";
			this.textBoxxDescription.ReadOnly = true;
			this.textBoxxDescription.ScrollBars = RichTextBoxScrollBars.Vertical;
			this.textBoxxDescription.Size = new Size(293, 117);
			this.textBoxxDescription.TabIndex = 30;
			this.textBoxxDescription.TabStop = false;
			this.textBoxxDescription.Text = "";
			this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.btnOK.DialogResult = DialogResult.OK;
			this.btnOK.Location = new Point(372, 272);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new Size(75, 23);
			this.btnOK.TabIndex = 31;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new EventHandler(this.buttonOK_Click);
			this.lblReportErrors.Location = new Point(151, 243);
			this.lblReportErrors.Margin = new Padding(6, 0, 3, 0);
			this.lblReportErrors.Name = "lblReportErrors";
			this.lblReportErrors.Size = new Size(103, 17);
			this.lblReportErrors.TabIndex = 32;
			this.lblReportErrors.Text = "Report errors to:";
			this.lblReportErrors.TextAlign = ContentAlignment.MiddleLeft;
			this.linkReportErrors.AutoSize = true;
			this.linkReportErrors.Location = new Point(236, 245);
			this.linkReportErrors.Name = "lnkReportErrors";
			this.linkReportErrors.Size = new Size(141, 13);
			this.linkReportErrors.TabIndex = 33;
			this.linkReportErrors.TabStop = true;
			this.linkReportErrors.Text = "someone@hugheschris.com";
			this.linkReportErrors.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkReportErrors_Clicked);
			this.lblDescription.Location = new Point(154, 103);
			this.lblDescription.Margin = new Padding(6, 0, 3, 0);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new Size(103, 17);
			this.lblDescription.TabIndex = 34;
			this.lblDescription.Text = "Description:";
			this.lblDescription.TextAlign = ContentAlignment.MiddleLeft;
			this.AutoScaleDimensions = new SizeF(6f, 13f);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(456, 307);
			this.Controls.Add((Control)this.lblDescription);
			this.Controls.Add((Control)this.labelCompanyName);
			this.Controls.Add((Control)this.linkReportErrors);
			this.Controls.Add((Control)this.lblReportErrors);
			this.Controls.Add((Control)this.pictureBoxLogo);
			this.Controls.Add((Control)this.labelProductName);
			this.Controls.Add((Control)this.labelVersion);
			this.Controls.Add((Control)this.labelCopyright);
			this.Controls.Add((Control)this.textBoxxDescription);
			this.Controls.Add((Control)this.btnOK);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutBox1";
			this.Padding = new Padding(9);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.Text = "About";
			//this.picboxLogo.EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

	} // End class.
} // End namespace.