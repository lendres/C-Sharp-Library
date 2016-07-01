using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DigitalProduction.Threading
{
	#region Delegates

	/// <summary>
	/// Delegate for a message callback function.
	/// </summary>
	/// <param name="message">Text to display in the message box.</param>
	/// <param name="caption">Text to display in the title bar of the message box.</param>
	/// <param name="icon">One of the System.Windows.Forms.MessagBoxIcon that specifies which icon to display in the message box.</param>
	public delegate void DisplayMessageDelegate(string message, string caption, MessageBoxIcon icon);

	/// <summary>
	/// Delegate that has a worker function (to run on a separate thread).
	/// </summary>
	public delegate void ProgressWorkerDelegate();

	/// <summary>
	/// Delegate that does any required clean up in case of a ProgressDialog getting canceled.
	/// </summary>
	public delegate void ProgressCleanUpDelegate();

	#endregion

} // End namespace.