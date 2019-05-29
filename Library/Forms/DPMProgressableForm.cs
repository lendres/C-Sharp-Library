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
		#region Delegates

		/// <summary>
		/// Delegate for displaying a message.
		/// </summary>
		/// <param name="message">Message to display</param>
		/// <param name="icon">Icon to display with the message.</param>
		public delegate void DisplayMessageCallBack(string message, MessageBoxIcon icon);

		#endregion

		#region Members

		private DisplayMessageCallBack						_displayMessageCallback;

		/// <summary>
		/// Progress callback function.
		/// </summary>
		protected ProgressDialog.UpdateProgressCallBack		_progressCallback;
		
		private Thread										_processThread;
		private ProgressDialog								_progressDialog;
		private int											_lastProgressValue;

		#endregion

		#region Construction

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
			_progressDialog = new ProgressDialog();
			_progressDialog.Maximum = 100;
			_progressCallback = new ProgressDialog.UpdateProgressCallBack(_progressDialog.UpdateProgress);
			_displayMessageCallback = new DPMProgressableForm.DisplayMessageCallBack(InvokeMessage);
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
				this.Invoke((Delegate)this._displayMessageCallback, (object)message, (object)icon);
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
			if (progress <= this._lastProgressValue)
			{
				return;
			}
			
			_lastProgressValue = progress;

			if (_progressDialog == null)
			{
				return;
			}
			
			_progressDialog.BeginInvoke((Delegate)_progressCallback, new object[1] {(object)progress});
		}

		/// <summary>
		/// Start the processing thread.
		/// </summary>
		protected void StartProcessThread()
		{
			_progressDialog.ResetProgress();
			_progressDialog.StartTimer();
			_processThread = new Thread(new ThreadStart(this.RunProcessingThread));
			_processThread.IsBackground = true;
			_processThread.Start();
			if (_progressDialog.ShowDialog((IWin32Window)this) != DialogResult.Cancel)
			{
				return;
			}
			_processThread.Abort();
			HandleCancel();
		}

		private void RunProcessingThread()
		{
			while (!_progressDialog.Visible)
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
					_progressDialog.CloseOK();
					ExceptionCleanUp();
					return;
				}
				catch
				{
					return;
				}
			}
			_progressDialog.CloseOK();
		}

		/// <summary>
		/// Cancel handling.
		/// </summary>
		protected virtual void HandleCancel() {}

		/// <summary>
		/// Run the processing.
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