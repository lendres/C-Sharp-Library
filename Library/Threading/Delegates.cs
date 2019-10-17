using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DigitalProduction.Threading
{
	#region Delegates

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