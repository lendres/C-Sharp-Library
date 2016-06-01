using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DigitalProduction.Threading
{
	#region Delegates

	public delegate void DisplayMessageDelegate(string message, string caption, MessageBoxIcon icon);
	public delegate void ProgressWorkerDelegate();

	#endregion

} // End namespace.