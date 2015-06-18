using System;
using System.Threading;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Form with a progress bar.
	/// </summary>
	public class DPMProgressableForm : DPMForm
	{
		#region Members and delegates.

		/// <summary>
		/// Delegate for displaying a message.
		/// </summary>
		/// <param name="message">Message to display</param>
		/// <param name="icon">Icon to display with the message.</param>
		public delegate void DisplayMessageCallBack(string message, MessageBoxIcon icon);

		private DisplayMessageCallBack						_displaymessagecallback;

		/// <summary>
		/// Progress callback function.
		/// </summary>
		protected ProgressDialog.UpdateProgressCallBack		_progresscallback;
		
		private Thread										_processthread;
		private ProgressDialog								_progressdialog;
		private int											_lastprogressvalue;

		#endregion

		#region Construction.

		/// <summary>
		/// Constructor.
		/// </summary>
		public DPMProgressableForm() {}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="applicationname">Name of application.</param>
		public DPMProgressableForm(string applicationname) : base(applicationname)
		{
			Initialize();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="companyname">Company name.</param>
		/// <param name="applicationname">Name of application.</param>
		public DPMProgressableForm(string companyname, string applicationname): base(companyname, applicationname)
		{
			Initialize();
		}

		private void Initialize()
		{
			_progressdialog = new ProgressDialog();
			_progressdialog.Maximum = 100;
			_progresscallback = new ProgressDialog.UpdateProgressCallBack(_progressdialog.UpdateProgress);
			_displaymessagecallback = new DPMProgressableForm.DisplayMessageCallBack(InvokeMessage);
		}

		#endregion

		/// <summary>
		/// Display a message.
		/// </summary>
		/// <param name="message">Message to display</param>
		/// <param name="icon">Icon to display with the message.</param>
		protected void DisplayMessage(string message, MessageBoxIcon icon)
		{
			if (this.InvokeRequired)
			{
				this.Invoke((Delegate)this._displaymessagecallback, (object)message, (object)icon);
			}
			else
			{
				this.InvokeMessage(message, icon);
			}
		}

		/// <summary>
		/// For displaying a message when an invoke is required.
		/// </summary>
		/// <param name="message">Message to display</param>
		/// <param name="icon">Icon to display with the message.</param>
		protected void InvokeMessage(string message, MessageBoxIcon icon)
		{
			MessageBox.Show((IWin32Window)this, message, this.Text, MessageBoxButtons.OK, icon);
		}

		/// <summary>
		/// Update the progress bar.
		/// </summary>
		/// <param name="progress">Progress to report.</param>
		protected void ReportProgress(int progress)
		{
			if (progress <= this._lastprogressvalue)
			{
				return;
			}
			
			_lastprogressvalue = progress;

			if (_progressdialog == null)
			{
				return;
			}
			
			_progressdialog.BeginInvoke((Delegate)_progresscallback, new object[1] {(object)progress});
		}

		/// <summary>
		/// Start the processing thread.
		/// </summary>
		protected void StartProcessThread()
		{
			_progressdialog.ResetProgress();
			_progressdialog.StartTimer();
			_processthread = new Thread(new ThreadStart(this.RunProcessingThread));
			_processthread.IsBackground = true;
			_processthread.Start();
			if (_progressdialog.ShowDialog((IWin32Window)this) != DialogResult.Cancel)
			{
				return;
			}
			_processthread.Abort();
			HandleCancel();
		}

		private void RunProcessingThread()
		{
			while (!_progressdialog.Visible)
			{
				Thread.Sleep(100);
			}
			try
			{
				this.DoProcessing();
			}
			catch (ThreadAbortException ex)
			{
				string message = ex.Message;
				DisplayMessage("Processing aborted.\n\n", MessageBoxIcon.Asterisk);
			}
			catch (Exception ex)
			{
				DisplayMessage("Error processing the input file.\n\n" + ex.Message, MessageBoxIcon.Hand);
				try
				{
					_progressdialog.CloseOK();
					ExceptionCleanUp();
					return;
				}
				catch
				{
					return;
				}
			}
			_progressdialog.CloseOK();
		}

		/// <summary>
		/// Cancel handling.
		/// </summary>
		protected virtual void HandleCancel() {}

		/// <summary>
		/// Run the pocessing.
		/// </summary>
		protected virtual void DoProcessing() {}

		/// <summary>
		/// Processing clean up.
		/// </summary>
		protected virtual void ProcessingCleanUp() {}

		/// <summary>
		/// Exception clean up.
		/// </summary>
		protected virtual void ExceptionCleanUp() {}

	} // End class.
} // End namespace.