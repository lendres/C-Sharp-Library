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

		private PictureBox picboxLogo;
		private Label lblProductName;
		private Label lblVersion;
		private Label lblCopyright;
		private Label lblCompanyName;
		private RichTextBox txtbxDescription;
		private Button btnOK;
		private Label lblDescription;
		private Label lblReportErrors;
		private LinkLabel lnkReportErrors;

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
			this.picboxLogo = new PictureBox();
			this.lblProductName = new Label();
			this.lblVersion = new Label();
			this.lblCopyright = new Label();
			this.lblCompanyName = new Label();
			this.txtbxDescription = new RichTextBox();
			this.btnOK = new Button();
			this.lblReportErrors = new Label();
			this.lnkReportErrors = new LinkLabel();
			this.lblDescription = new Label();
			//this.picboxLogo.BeginInit();
			this.SuspendLayout();
			this.picboxLogo.Image = (Image)componentResourceManager.GetObject("picboxLogo.Image");
			this.picboxLogo.Location = new Point(3, 2);
			this.picboxLogo.Name = "picboxLogo";
			this.picboxLogo.Size = new Size(142, 303);
			this.picboxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
			this.picboxLogo.TabIndex = 26;
			this.picboxLogo.TabStop = false;
			this.lblProductName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.lblProductName.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.lblProductName.ForeColor = Color.FromArgb(192, 0, 0);
			this.lblProductName.Location = new Point(150, 9);
			this.lblProductName.Margin = new Padding(6, 0, 3, 0);
			this.lblProductName.Name = "lblProductName";
			this.lblProductName.Size = new Size(293, 17);
			this.lblProductName.TabIndex = 27;
			this.lblProductName.Text = "Product Name";
			this.lblProductName.TextAlign = ContentAlignment.MiddleLeft;
			this.lblVersion.Location = new Point(151, 28);
			this.lblVersion.Margin = new Padding(6, 0, 3, 0);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new Size(293, 17);
			this.lblVersion.TabIndex = 25;
			this.lblVersion.Text = "Version";
			this.lblCopyright.Location = new Point(151, 67);
			this.lblCopyright.Margin = new Padding(6, 0, 3, 0);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new Size(293, 17);
			this.lblCopyright.TabIndex = 28;
			this.lblCopyright.Text = "Copyright";
			this.lblCopyright.TextAlign = ContentAlignment.MiddleLeft;
			this.lblCompanyName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.lblCompanyName.ForeColor = Color.FromArgb(0, 0, 192);
			this.lblCompanyName.Location = new Point(151, 53);
			this.lblCompanyName.Margin = new Padding(6, 0, 3, 0);
			this.lblCompanyName.Name = "lblCompanyName";
			this.lblCompanyName.Size = new Size(293, 17);
			this.lblCompanyName.TabIndex = 29;
			this.lblCompanyName.Text = "Company Name";
			this.lblCompanyName.TextAlign = ContentAlignment.MiddleLeft;
			this.txtbxDescription.Location = new Point(154, 123);
			this.txtbxDescription.Margin = new Padding(6, 3, 3, 3);
			this.txtbxDescription.Name = "txtbxDescription";
			this.txtbxDescription.ReadOnly = true;
			this.txtbxDescription.ScrollBars = RichTextBoxScrollBars.Vertical;
			this.txtbxDescription.Size = new Size(293, 117);
			this.txtbxDescription.TabIndex = 30;
			this.txtbxDescription.TabStop = false;
			this.txtbxDescription.Text = "";
			this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.btnOK.DialogResult = DialogResult.OK;
			this.btnOK.Location = new Point(372, 272);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new Size(75, 23);
			this.btnOK.TabIndex = 31;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new EventHandler(this.btnOK_Click);
			this.lblReportErrors.Location = new Point(151, 243);
			this.lblReportErrors.Margin = new Padding(6, 0, 3, 0);
			this.lblReportErrors.Name = "lblReportErrors";
			this.lblReportErrors.Size = new Size(103, 17);
			this.lblReportErrors.TabIndex = 32;
			this.lblReportErrors.Text = "Report errors to:";
			this.lblReportErrors.TextAlign = ContentAlignment.MiddleLeft;
			this.lnkReportErrors.AutoSize = true;
			this.lnkReportErrors.Location = new Point(236, 245);
			this.lnkReportErrors.Name = "lnkReportErrors";
			this.lnkReportErrors.Size = new Size(141, 13);
			this.lnkReportErrors.TabIndex = 33;
			this.lnkReportErrors.TabStop = true;
			this.lnkReportErrors.Text = "someone@hugheschris.com";
			this.lnkReportErrors.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkReportErrors_LinkClicked);
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
			this.Controls.Add((Control)this.lblCompanyName);
			this.Controls.Add((Control)this.lnkReportErrors);
			this.Controls.Add((Control)this.lblReportErrors);
			this.Controls.Add((Control)this.picboxLogo);
			this.Controls.Add((Control)this.lblProductName);
			this.Controls.Add((Control)this.lblVersion);
			this.Controls.Add((Control)this.lblCopyright);
			this.Controls.Add((Control)this.txtbxDescription);
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