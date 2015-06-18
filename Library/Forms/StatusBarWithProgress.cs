using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace DigitalProduction.Forms
{
	/// <summary>
	/// A status bar that contains a progress bar.
	/// </summary>
	public class StatusBarWithProgress : StatusBar
	{
		#region Members / Variables.

		private int				_progressbarpanel	= 1;

		/// <summary>Progress bar displayed on the status bar.</summary>
		public ProgressBar		ProgressBar			= new ProgressBar();

		#endregion

		#region Construction.

		/// <summary>
		/// Constructor.
		/// </summary>
		public StatusBarWithProgress()
		{
			ProgressBar.Hide();
			//this.Controls.Add(ProgressBar);
			ProgressBar.Parent = this;
			this.DrawItem += new StatusBarDrawItemEventHandler(this.Reposition);
		}

		#endregion

		#region Properties.

		/// <summary>
		/// Set the panel number that the status bar is to appear in.  Panels use zeroth based numbering.
		/// </summary>
		public int SetProgressBarPanel
		{
			get
			{
				return _progressbarpanel;
			}

			set 
			{
				System.Collections.IEnumerator enumer = this.Panels.GetEnumerator();
				int i = -1;
				while (enumer.MoveNext()) i++;

				if (value < 0 || value > i)
				{
					return;
				}

				_progressbarpanel = value;
				this.Panels[_progressbarpanel].Alignment = HorizontalAlignment.Left;
				//this.Panels[_progressbarpanel].AutoSize = StatusBarPanelAutoSize.None;
				this.Panels[_progressbarpanel].Style = StatusBarPanelStyle.OwnerDraw;
			}
		}

		#endregion

		#region Event handlers.

		/// <summary>
		/// Handles repositioning of form.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="sbdevent">Event arguments.</param>
		private void Reposition(object sender, StatusBarDrawItemEventArgs sbdevent)
		{
			int xadjust = (int)((sbdevent.Panel.Width - sbdevent.Bounds.Width)/2.0);
			ProgressBar.Location = new System.Drawing.Point(sbdevent.Bounds.X-xadjust, sbdevent.Bounds.Y+1);
			ProgressBar.Size = new System.Drawing.Size(sbdevent.Bounds.Width, sbdevent.Bounds.Height-2);
			//ProgressBar.Show();
		}

		#endregion

	} // End class.
} // End namespace.