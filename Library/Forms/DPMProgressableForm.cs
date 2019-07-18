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
		#region Members

		private DisplayMessageCallBack						_displayMessageCallback;

		/// <summary>
		/// Progress callback function.
		/// </summary>
		protected UpdateProgressCallBack					_progressCallback;
		
		private Thread										_processThread;
		private ProgressDialog								_progressDialog;
		private int											_lastProgressValue;

		#endregion

		#region Construction

		/// <summary>
		/// Constructor.
		/// </summary>
		public DPMProgressableForm()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="applicationname">Name of application.</param>
		public DPMProgressableForm(string applicationname) :
			base(applicationname)
		{
			Initialize();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="companyname">Company name.</param>
		/// <param name="applicationname">Name of application.</param>
		public DPMProgressableForm(string companyname, string applicationname) :
			base(companyname, applicationname)
		{
			Initialize();
		}

		/// <summary>
		/// Initialization.
		/// </summary>
		private void Initialize()
		{
			_progressDialog						= new ProgressDialog();
			_progressDialog.ProgressBar.Maximum	= 100;
			_progressCallback					= new UpdateProgressCallBack(_progressDialog.UpdateProgress);
			_displayMessageCallback				= new DisplayMessageCallBack(InvokeMessage);
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

			_processThread				= new Thread(new ThreadStart(this.RunProcessingThread));
			_processThread.IsBackground	= true;

			_processThread.Start();

			if (_progressDialog.ShowDialog((IWin32Window)this) != DialogResult.Cancel)
			{
				return;
			}

			_processThread.Abort();
			HandleCancel();
		}

		/// <summary>
		/// Does the work of running the thread.
		/// </summary>
		private void RunProcessingThread()
		{
			// We have to pause until the dialog has appeared, otherwise we run the risk of trying to close it
			// before it opens.
			while (!_progressDialog.Visible)
			{
				Thread.Sleep(100);
			}

			try
			{
				DoProcessing();
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

			// Tell the dialog box to close in a happy way.
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