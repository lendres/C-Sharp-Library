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
	/// Delegate for displaying a message.
	/// </summary>
	/// <param name="message">Message to display</param>
	/// <param name="icon">Icon to display with the message.</param>
	public delegate void DisplayMessageCallBack(string message, MessageBoxIcon icon);

} // End namespace.