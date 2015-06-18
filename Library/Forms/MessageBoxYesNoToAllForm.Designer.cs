using System.ComponentModel;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	partial class MessageBoxYesNoToAllForm
	{
		#region Members.

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;
		private TextBox txtbxMessage;
		private PictureBox pctboxIcon;
		private Button btnYes;
		private Button btnYesToAll;
		private Button btnNo;
		private Button btnNoToAll;
		private Button btnCancel;

		#endregion

		#region Disposing.

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Windows Form Designer generated code.

		private void InitializeComponent()
		{
			this.btnYes = new System.Windows.Forms.Button();
			this.btnYesToAll = new System.Windows.Forms.Button();
			this.btnNo = new System.Windows.Forms.Button();
			this.btnNoToAll = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtbxMessage = new System.Windows.Forms.TextBox();
			this.pctboxIcon = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pctboxIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// btnYes
			// 
			this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnYes.Location = new System.Drawing.Point(12, 77);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new System.Drawing.Size(75, 23);
			this.btnYes.TabIndex = 1;
			this.btnYes.Text = "&Yes";
			this.btnYes.UseVisualStyleBackColor = true;
			this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
			// 
			// btnYesToAll
			// 
			this.btnYesToAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnYesToAll.Location = new System.Drawing.Point(93, 77);
			this.btnYesToAll.Name = "btnYesToAll";
			this.btnYesToAll.Size = new System.Drawing.Size(75, 23);
			this.btnYesToAll.TabIndex = 2;
			this.btnYesToAll.Text = "Yes to &All";
			this.btnYesToAll.UseVisualStyleBackColor = true;
			this.btnYesToAll.Click += new System.EventHandler(this.btnYesToAll_Click);
			// 
			// btnNo
			// 
			this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNo.Location = new System.Drawing.Point(174, 77);
			this.btnNo.Name = "btnNo";
			this.btnNo.Size = new System.Drawing.Size(75, 23);
			this.btnNo.TabIndex = 3;
			this.btnNo.Text = "&No";
			this.btnNo.UseVisualStyleBackColor = true;
			this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
			// 
			// btnNoToAll
			// 
			this.btnNoToAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNoToAll.Location = new System.Drawing.Point(255, 77);
			this.btnNoToAll.Name = "btnNoToAll";
			this.btnNoToAll.Size = new System.Drawing.Size(75, 23);
			this.btnNoToAll.TabIndex = 4;
			this.btnNoToAll.Text = "No &to All";
			this.btnNoToAll.UseVisualStyleBackColor = true;
			this.btnNoToAll.Click += new System.EventHandler(this.btnNoToAll_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(336, 77);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtbxMessage
			// 
			this.txtbxMessage.BackColor = System.Drawing.SystemColors.Control;
			this.txtbxMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtbxMessage.Location = new System.Drawing.Point(61, 12);
			this.txtbxMessage.Multiline = true;
			this.txtbxMessage.Name = "txtbxMessage";
			this.txtbxMessage.Size = new System.Drawing.Size(350, 59);
			this.txtbxMessage.TabIndex = 0;
			this.txtbxMessage.TabStop = false;
			// 
			// pctboxIcon
			// 
			this.pctboxIcon.Location = new System.Drawing.Point(12, 12);
			this.pctboxIcon.Name = "pctboxIcon";
			this.pctboxIcon.Size = new System.Drawing.Size(32, 32);
			this.pctboxIcon.TabIndex = 6;
			this.pctboxIcon.TabStop = false;
			// 
			// MessageBoxYesNoToAllForm
			// 
			this.ClientSize = new System.Drawing.Size(423, 112);
			this.Controls.Add(this.pctboxIcon);
			this.Controls.Add(this.txtbxMessage);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnNoToAll);
			this.Controls.Add(this.btnNo);
			this.Controls.Add(this.btnYesToAll);
			this.Controls.Add(this.btnYes);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MessageBoxYesNoToAllForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			((System.ComponentModel.ISupportInitialize)(this.pctboxIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

	} // End class.
} // End namespace.
