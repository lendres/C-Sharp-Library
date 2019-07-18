using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Summary description for ProgressDialog.
	/// </summary>
	public partial class ProgressDialog : System.Windows.Forms.Form
	{
		#region Members

		private DateTime								_starttime;
		private DateTime								_endtime;
		private bool									_timing;
		private bool									_timingended;

		private Size									_defaultSize;
		private Size									_noCancelSize;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ProgressDialog()
		{
			Initialize();
		}

		/// <summary>
		/// Constructor for changing the progress bar style.
		/// </summary>
		public ProgressDialog(ProgressBarStyle style)
		{
			Initialize();
			this.pbarProgress.Style = style;
		}

		private void Initialize()
		{
			InitializeComponent();

			_defaultSize	= new Size(this.Size.Width + 10, this.Height + 32);
			_noCancelSize	= new Size(_defaultSize.Width, 140);

			// Animation speed when we use the Marquee style.
			// You must use:
			// progressdialog.ProgressBar.Style = ProgressBarStyle.Marquee;
			// for this to take effect.
			this.pbarProgress.MarqueeAnimationSpeed = 80;

			this.Cursor = Cursors.WaitCursor;

			this.pbarProgress.Step = 1;
			this.pbarProgress.Minimum = 0;

			this.lblText.Text = "Processing...";

			this.tmrClock = new Timer();
			this.tmrClock.Interval = 1000;
			this.tmrClock.Enabled = true;
			this.tmrClock.Tick += new EventHandler(tmrClock_Tick);

			ResetTimer();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Specifies if the cancel button should be shown.
		/// </summary>
		public bool AllowCancel
		{
			get
			{
				return this.btnCancel.Visible;
			}

			set
			{
				this.btnCancel.Visible = value;

				if (value)
				{
					this.Size = _defaultSize;
				}
				else
				{
					this.Size = _noCancelSize;
				}
			}
		}

		/// <summary>
		/// Get the time elapsed between the start time and end time (or, if timing has not ended, the time between
		/// the start time and now).
		/// </summary>
		public TimeSpan ElapsedTime
		{
			get
			{
				TimeSpan elapsedtime;

				if (_timing)
				{
					elapsedtime = DateTime.Now - _starttime;
				}
				else
				{
					elapsedtime = _endtime - _starttime;
				}

				return elapsedtime;
			}
		}

		/// <summary>
		/// Determines if the progress bar is visible.
		/// </summary>
		public ProgressBar ProgressBar
		{
			get
			{
				return this.pbarProgress;
			}
		}

		/// <summary>
		/// The maximum value to use for the progress bar.
		/// </summary>
		[Obsolete("Use ProgressDialog.ProgressBar.Maximum instead", true)]
		public int Maximum
		{
			set
			{
				this.pbarProgress.Maximum = value;
			}
		}

		/// <summary>
		/// The value of the progress bar.
		/// </summary>
		[Obsolete("Use ProgressDialog.ProgressBar.Value instead", true)]
		public int Value
		{
			get
			{
				return this.pbarProgress.Value;
			}

			set
			{
				this.pbarProgress.Value = value;
			}
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Updates the clock on the status bar.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void tmrClock_Tick(object sender, EventArgs e)
		{
			if (_timing | _timingended)
			{
				TimeSpan elapsedtime = this.ElapsedTime;
				string time = String.Format("{0:0} secs", elapsedtime.Seconds);

				if (elapsedtime.Minutes > 0)
				{
					time = String.Format("{0:0} mins  ", elapsedtime.Minutes) + time;
				}

				this.lblTimeElapsed.Text = time;
			}
			else
			{
				this.lblTimeElapsed.Text = "0 secs";
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Shows the form with the specified owner to the use.  Also resets the progress bar and starts the timer.
		/// </summary>
		/// <param name="owner">Any object that implements the System.Windows.Forms.IWin32Window and represents the top-level window that will own this form.</param>
		public DialogResult StartShowDialog(IWin32Window owner)
		{
			ResetProgress();
			StartTimer();

			return ShowDialog(owner);
		}

		/// <summary>
		/// Start timing.
		/// </summary>
		public void StartTimer()
		{
			this.lblTimeElapsed.Text = "0 secs";

			_starttime		= DateTime.Now;
			_timing			= true;
			_timingended	= false;
		}

		/// <summary>
		/// Stop the timer.  The time elapsed between the start and stop can then be retrieved.
		/// </summary>
		public void StopTimer()
		{
			_endtime		= DateTime.Now;
			_timing			= false;
			_timingended	= true;
		}

		/// <summary>
		/// Resets the timer to zero.
		/// </summary>
		public void ResetTimer()
		{
			this.lblTimeElapsed.Text = "0 secs";

			_starttime		= DateTime.Now;
			_endtime		= _starttime;
			_timing			= false;
			_timingended	= false;
		}

		/// <summary>
		/// Reset the progress bar.
		/// </summary>
		public void ResetProgress()
		{
			this.pbarProgress.Value		= 0;
		}

		/// <summary>
		/// Perform step.  Increments the progress bar.
		/// </summary>
		public void PerformStep()
		{
			this.pbarProgress.PerformStep();
		}

		/// <summary>
		/// Allow the option to close this dialog externally (from another class) and return DialogResult.OK
		/// as the result.  Using Close from another class always results in DialogResult.Cancel as the result.
		/// 
		/// Setting the dialog result will cause the closing of the form if it is shown model (using "ShowDialog" or "StartShowDialog").
		/// </summary>
		public void CloseOK()
		{
			this.DialogResult = DialogResult.OK;
			Close();
		}

		#endregion

		#region Call Backs

		/// <summary>
		/// Update the progress bar via a function.
		/// </summary>
		/// <param name="value">Value of the progress bar as an integer from 0-100.</param>
		/// <remarks>
		/// This can be used as an UpdateProgressCallBack delegate to update the progress bar 
		/// from another thread.
		/// </remarks>
		public void UpdateProgress(int value)
		{

			if (this.InvokeRequired)
			{
				// We need to use a callback.
				Action<int> setDelegate = this.SetProgressValue;
				this.Invoke(setDelegate, new object[] { value });
			}
			else
			{
				SetProgressValue(value);
			}
		}

		/// <summary>
		/// Update the progress bar.
		/// </summary>
		/// <param name="value">Value of the progress bar as an integer from 0-100.</param>
		public void SetProgressValue(int value)
		{
			this.pbarProgress.Value = value;
		}

		/// <summary>
		/// Sets the caption (title bar text) shown on the form.
		/// </summary>
		/// <param name="caption">Text to display.</param>
		[Obsolete("SetCaption has become SetMessage, SetCaption now sets the title bar caption instead of the label.")]
		public void SetCaption(string caption)
		{
			if (this.InvokeRequired)
			{
				// We need to use a callback.
				Action<string> setDelegate = this.SetCaptionText;
				this.Invoke(setDelegate, new object[] { caption });
			}
			else
			{
				SetCaptionText(caption);
			}
		}

		/// <summary>
		/// The actual work of setting the message.  We make a separate function so that it can be invoked from a delegate when
		/// an invoke is required.
		/// </summary>
		/// <param name="caption">Text to display.</param>
		private void SetCaptionText(string caption)
		{
			this.Text = caption;
		}

		/// <summary>
		/// Sets the message shown on the form (sets the label).
		/// </summary>
		/// <param name="message">Text to display.</param>
		public void SetMessage(string message)
		{
			if (this.InvokeRequired)
			{
				// We need to use a callback.
				Action<string> setDelegate = this.SetMessageText;
				this.Invoke(setDelegate, new object[] { message });
			}
			else
			{
				SetMessageText(message);
			}
		}

		/// <summary>
		/// The actual work of setting the message.  We make a separate function so that it can be invoked from a delegate when
		/// an invoke is required.
		/// </summary>
		/// <param name="message">Text to display.</param>
		private void SetMessageText(string message)
		{
			this.lblText.Text = message;
		}

		#endregion

	} // End class.
} // End namespace.