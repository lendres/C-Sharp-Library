using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Delegate template for install functions.
	/// </summary>
	public delegate void InstallEventHandler();

	/// <summary>
	/// Update the progress bar via a call back function from another thread.
	/// </summary>
	/// <param name="value">Value of the progress bar as an integer from 0-100.</param>
	public delegate void UpdateProgressCallBack(int value);

	/// <summary>
	/// Update the text (caption) on the form.
	/// </summary>
	/// <param name="caption">Text to display.</param>
	public delegate void UpdateCaptionCallBack(string caption);

	/// <summary>
	/// General call back delegate.  Can be used to update the progress bar, close the form, et cetera via a call back function from another thread.
	/// </summary>
	public delegate void CallBack();

	/// <summary>
	/// Delegate for a message callback function.
	/// </summary>
	/// <param name="message">Text to display in the message box.</param>
	/// <param name="caption">Text to display in the title bar of the message box.</param>
	/// <param name="icon">One of the System.Windows.Forms.MessagBoxIcon that specifies which icon to display in the message box.</param>
	public delegate void DisplayMessageDelegate(string message, string caption, MessageBoxIcon icon);

} // End namespace.