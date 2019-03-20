using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Provide summary.
	/// </summary>
	public partial class ProgressDialog
	{
		#region Members / Variables.

		private System.Windows.Forms.ProgressBar		pbarProgress;
		private System.Windows.Forms.Label				lblText;
		private System.Windows.Forms.Button				btnCancel;
		private System.Windows.Forms.Label				lblTimeElapsed;
		private System.Windows.Forms.Label				lblTimeElapsedLabel;

		private System.Windows.Forms.Timer				tmrClock;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container			components = null;

		#endregion

		#region Disposing.

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Windows Form Designer generated code.

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pbarProgress = new System.Windows.Forms.ProgressBar();
			this.lblText = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblTimeElapsed = new System.Windows.Forms.Label();
			this.lblTimeElapsedLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pbarProgress
			// 
			this.pbarProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbarProgress.Location = new System.Drawing.Point(8, 72);
			this.pbarProgress.Name = "pbarProgress";
			this.pbarProgress.Size = new System.Drawing.Size(290, 20);
			this.pbarProgress.TabIndex = 0;
			// 
			// lblText
			// 
			this.lblText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblText.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblText.Location = new System.Drawing.Point(8, 8);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(288, 23);
			this.lblText.TabIndex = 1;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(224, 104);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// lblTimeElapsed
			// 
			this.lblTimeElapsed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTimeElapsed.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblTimeElapsed.Location = new System.Drawing.Point(138, 40);
			this.lblTimeElapsed.Name = "lblTimeElapsed";
			this.lblTimeElapsed.Size = new System.Drawing.Size(160, 23);
			this.lblTimeElapsed.TabIndex = 4;
			this.lblTimeElapsed.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblTimeElapsedLabel
			// 
			this.lblTimeElapsedLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblTimeElapsedLabel.Location = new System.Drawing.Point(8, 40);
			this.lblTimeElapsedLabel.Name = "lblTimeElapsedLabel";
			this.lblTimeElapsedLabel.Size = new System.Drawing.Size(100, 23);
			this.lblTimeElapsedLabel.TabIndex = 3;
			this.lblTimeElapsedLabel.Text = "Time Elapsed:";
			// 
			// ProgressDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(306, 136);
			this.ControlBox = false;
			this.Controls.Add(this.lblTimeElapsed);
			this.Controls.Add(this.lblTimeElapsedLabel);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblText);
			this.Controls.Add(this.pbarProgress);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Progress";
			this.ResumeLayout(false);

		}

		#endregion

	} // End class.
} // End namespace.
