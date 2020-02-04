using DigitalProduction.Forms;
using System;
using System.Threading;
using System.Windows.Forms;

namespace DigitalProduction.Threading
{
	/// <summary>
	/// A helper class for displaying the progress dialog form.
	/// </summary>
	public class ProgressDialogThreadingHelper
	{
		#region Members

		private ProgressDialog								_progressDialog					= new ProgressDialog();
		private Form										_parentForm;

		private DisplayMessageDelegate						_displayMessageDelegate;
		private ProgressWorkerDelegate						_workerDelegate;
		private ProgressCleanUpDelegate						_cancelCleanUpDelegate;

		#endregion

		#region Construction

		/// <summary>
		/// Constructor used when passing a ProgressDialogThreadingHelper from a form to a class that is going to do the main work.
		/// </summary>
		/// <param name="parentForm">The parent form that this dialog box will show in front of.</param>
		/// <param name="displayMessageDelegate">A delegate used to display messages in the main form.</param>
		public ProgressDialogThreadingHelper(Form parentForm, DisplayMessageDelegate displayMessageDelegate)
		{
			_parentForm							= parentForm;
			_displayMessageDelegate				= displayMessageDelegate;
			_progressDialog.ProgressBar.Style	= ProgressBarStyle.Marquee;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parentForm">The parent form that this dialog box will show in front of.</param>
		/// <param name="workerDelegate">A delegate which is a function that does the main work.</param>
		/// <param name="displayMessageDelegate">A delegate used to display messages in the main form.</param>
		public ProgressDialogThreadingHelper(Form parentForm, ProgressWorkerDelegate workerDelegate, DisplayMessageDelegate displayMessageDelegate) :
			this(parentForm, displayMessageDelegate)
		{
			_workerDelegate				= workerDelegate;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parentForm">The parent form that this dialog box will show in front of.</param>
		/// <param name="workerDelegate">A delegate which is a function that does the main work.</param>
		/// <param name="displayMessageDelegate">A delegate used to display messages in the main form.</param>
		/// <param name="cancelCleanUpDelegate">A delegate that is used to clean up resources if the "Cancel" button is pressed while the worker thread is running.</param>
		public ProgressDialogThreadingHelper(Form parentForm, ProgressWorkerDelegate workerDelegate, DisplayMessageDelegate displayMessageDelegate, ProgressCleanUpDelegate cancelCleanUpDelegate) :
			this(parentForm, workerDelegate, displayMessageDelegate)
		{
			_cancelCleanUpDelegate		= cancelCleanUpDelegate;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Sets or gets the worker delegate.
		/// 
		/// Example uses:
		///		No argument required:
		///			progressDialogThreadingHelper.WorkerDelegate = this.SerializeWorker;
		///			
		///		Argument required:
		///			progressDialogThreadingHelper.WorkerDelegate = new ProgressWorkerDelegate(() => base.ReadFileAndSetControls(path));
		/// </summary>
		public ProgressWorkerDelegate WorkerDelegate
		{
			get
			{
				return _workerDelegate;
			}

			set
			{
				_workerDelegate	= value;
			}
		}

		/// <summary>
		/// Sets or gets the cancel clean up delegate.
		/// </summary>
		public ProgressCleanUpDelegate CancelCleanUpDelegate
		{
			get
			{
				return _cancelCleanUpDelegate;
			}

			set
			{
				_cancelCleanUpDelegate = value;
			}
		}

		/// <summary>
		/// Sets or gets the ProgressBarStyle to use in the ProgressDialog.
		/// </summary>
		public ProgressDialog ProgressDialog
		{
			get
			{
				return _progressDialog;
			}

			set
			{
				_progressDialog = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Runs the ProgressDialog and worker thread.
		///
		/// Returns true if successful, false otherwise.
		/// </summary>
		public bool Run()
		{

			CreateAndStartWorkerThread();

			try
			{
				if (_progressDialog.DialogResult == DialogResult.OK)
				{
					return true;
				}
				else
				{
					// Process thread did not finish correctly (canceled).
					return false;
				}
			}
			catch (Exception exception)
			{
				_progressDialog.Close();
				MessageBox.Show("An error occurred while calculating the solution.\n\n" + exception.Message, "Solution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		/// <summary>
		/// Create the ProgressDialog and Thread to run the processing on.
		/// </summary>
		private void CreateAndStartWorkerThread()
		{
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
					if (_cancelCleanUpDelegate != null)
					{
						_cancelCleanUpDelegate();
					}
				}
				catch
				{
					// Catch all and quietly exit.
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

			_workerDelegate();

			// Close the dialog and allow the other thread to continue.
			_progressDialog.Invoke(new CallBack(_progressDialog.CloseOK));
		}

		/// <summary>
		/// 
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