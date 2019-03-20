using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading;
using DigitalProduction.Forms;

namespace DigitalProduction.Threading
{
	/// <summary>
	/// 
	/// </summary>
	public class ProgressDialogThreadingHelper
	{
		#region Members

		private ProgressDialog								_progressDialog;
		private Form										_parentForm;

		private DisplayMessageDelegate						_displayMessageDelegate;
		private ProgressWorkerDelegate						_workerDelegate;
		private ProgressCleanUpDelegate						_progressCleanUpDelegate;

		private ProgressBarStyle							_progressBarStyle				= ProgressBarStyle.Marquee;

		#endregion

		#region Construction

		/// <summary>
		/// Constructor.
		/// </summary>
		public ProgressDialogThreadingHelper(Form parentForm, ProgressWorkerDelegate workerDelegate, DisplayMessageDelegate displayMessageDelegate)
		{
			_parentForm					= parentForm;
			_workerDelegate				= workerDelegate;
			_displayMessageDelegate		= displayMessageDelegate;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public ProgressDialogThreadingHelper(Form parentForm, ProgressWorkerDelegate workerDelegate, DisplayMessageDelegate displayMessageDelegate, ProgressCleanUpDelegate progressCleanUpDelegate)
		{
			_parentForm					= parentForm;
			_workerDelegate				= workerDelegate;
			_displayMessageDelegate		= displayMessageDelegate;
			_progressCleanUpDelegate	= progressCleanUpDelegate;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Sets or gets the ProgressBarStyle to use in the ProgressDialog.
		/// </summary>
		public ProgressBarStyle ProgressBarStyle
		{
			get
			{
				return _progressBarStyle;
			}

			set
			{
				_progressBarStyle = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Runs the ProgressDialog and worker thread.
		/// </summary>
		/// <returns>True if successful, false otherwise.</returns>
		public bool Run()
		{

			CreateAndStartWorkerThread();

			if (_progressDialog.DialogResult == DialogResult.OK)
			{
				return true;
			}
			else
			{
				// Process thread did not finish correctly.
				return false;
			}
		}

		/// <summary>
		/// Create the ProgressDialog and Thread to run the processing on.
		/// </summary>
		private void CreateAndStartWorkerThread()
		{
			_progressDialog = new ProgressDialog(_progressBarStyle);
			_progressDialog.ResetProgress();
			_progressDialog.StartTimer();

			Thread processThread = new Thread(new ThreadStart(this.WorkerThread));
			processThread.IsBackground = true;
			processThread.Start();

			if (_progressDialog.ShowDialog(_parentForm) == DialogResult.Cancel)
			{
				processThread.Abort();
				try
				{
					if (_progressCleanUpDelegate != null)
					{
						_progressCleanUpDelegate();
					}
				}
				catch (Exception exception)
				{
					// Quiet exit.
				}
			}
		}
		/// <summary>
		/// Run the translation on a separate thread.
		/// </summary>
		private void WorkerThread()
		{
			// Wait for the dialog to open.
			while (!_progressDialog.Visible)
			{
				System.Threading.Thread.Sleep(100);
			}

			//try
			//{
				_workerDelegate();

				// Close the dialog and allow the other thread to continue.
				_progressDialog.Invoke(new ProgressDialog.CallBack(_progressDialog.CloseOK));
			//}
			//catch (Exception exception)
			//{
			//	// Close the dialog and allow the other thread to continue.
			//	//_progressDialog.Invoke(new ProgressDialog.CallBack(_progressDialog.Close));
			//	DisplayMessage(exception.Message, "Error", MessageBoxIcon.Error);
			//}
		}

		/// <summary>
		/// Invoke a message on the calling form.
		/// </summary>
		/// <param name="message">Message/text.</param>
		/// <param name="caption">Caption.</param>
		/// <param name="icon">MessageBoxIcon to show.</param>
		private void DisplayMessage(string message, string caption, MessageBoxIcon icon)
		{
			if (_parentForm.InvokeRequired)
			{
				// We need to use a callback.
				_parentForm.Invoke(_displayMessageDelegate, new object[] { message, caption, icon });
			}
			else
			{
				// Callback not required, just display the MessageBox.
				MessageBox.Show(_parentForm, message, caption, MessageBoxButtons.OK, icon);
			}
		}

		#endregion

	} // End class.
} // End namespace.