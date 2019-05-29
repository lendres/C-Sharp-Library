using System.ComponentModel;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Provides a parallel form and Show methods to MessageBox but adds "Yes to All" and "No to All" options.
	/// 
	/// A delegate is provided which allows the MessageBoxYesNoToAll set and get the result of the dialog call.  This allow the
	/// "Yes to All" and "No to All" options work by storing the value and using it for later calls.  Note, the stored value must
	/// be reset in order to re-display the message box.
	/// 
	/// Allows for the option to return "Yes to All" as "Yes" and "No to All" as "No."  This means the calling function does not need
	/// to check both "Yes/No to All" and "Yes/No" to determine if it's work should be done.
	/// </summary>
	public class MessageBoxYesNoToAll
	{
		#region Enumerations.

		/// <summary>
		/// Results of the dialog.
		/// </summary>
		public enum Result
		{
			/// <summary>Yes.</summary>
			[Description("Yes")]
			Yes,

			/// <summary>Yes to All button was pushed.</summary>
			[Description("Yes to All")]
			YesToAll,

			/// <summary>No button was pushed.</summary>
			[Description("No")]
			No,

			/// <summary>No to All button was pushed.</summary>
			[Description("No to All")]
			NoToAll,

			/// <summary>Cancel button was pushed.</summary>
			[Description("Cancel")]
			Cancel,
		}

		/// <summary>
		/// Buttons shown on the dialog.
		/// </summary>
		public enum Buttons
		{
			/// <summary>Yes, Yes to All, No.</summary>
			YesToAllNo,

			/// <summary>Yes to All, No, Cancel.</summary>
			YesToAllNoCancel,

			/// <summary>Yes, No, No to All.</summary>
			YesNoToAll,

			/// <summary>Yes, No, No to All, Cancel.</summary>
			YesNoToAllCancel,

			/// <summary>Yes, Yes to All, No, No to All.</summary>
			YesToAllNoToAll,

			/// <summary>Yes, Yes to All, No, No to All, Cancel.</summary>
			YesToAllNoToAllCancel,
		}

		/// <summary>
		/// Icon shown on the dialog.
		/// </summary>
		public enum Icon
		{
			/// <summary>Asterisk icon.</summary>
			Asterisk,

			/// <summary>Error icon.</summary>
			Error,

			/// <summary>Exclamation icon.</summary>
			Exclamation,

			/// <summary>Hand icon.</summary>
			Hand,

			/// <summary>Information icon.</summary>
			Information,

			/// <summary>No icon.</summary>
			None,

			/// <summary>Question icon.</summary>
			Question,

			/// <summary>Warning icon.</summary>
			Warning,
		}

		/// <summary>
		/// Button that is the default on the form (button activated if "enter" is pressed).
		/// </summary>
		public enum DefaultButton
		{
			/// <summary>Button 1.</summary>
			Button1,

			/// <summary>Button 2.</summary>
			Button2,

			/// <summary>Button 3.</summary>
			Button3,

			/// <summary>Button 4.</summary>
			Button4,

			/// <summary>Button 5.</summary>
			Button5,
		}

		#endregion

		#region Members and Delegates.

		/// <summary>
		/// Delegate signature for the function used to save and retrieve the result of showing the dialog box.
		/// </summary>
		/// <param name="result">Result of showing the dialog box.</param>
		/// <param name="setvalue">If true, the result must be saved.  If fase, the result must be retrieved and stored in the "result" parameter.</param>
		public delegate void StoreResultDelegate(ref MessageBoxYesNoToAll.Result result, bool setvalue);

		/// <summary>
		/// Delegate function used to store the result of the dialog display and to access the result for the previous display.
		/// </summary>
		protected MessageBoxYesNoToAll.StoreResultDelegate		_storeresultfunction;

		private bool											_returnonlyyesno;

		#endregion

		#region Construction.

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="storeresultfunction"></param>
		public MessageBoxYesNoToAll(MessageBoxYesNoToAll.StoreResultDelegate storeresultfunction)
		{
			_storeresultfunction = storeresultfunction;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="storeresultfunction"></param>
		/// <param name="returnonlyyesno">If true, "Yes to All" is returned as "Yes" and "No to All" is returned as "No."</param>
		public MessageBoxYesNoToAll(MessageBoxYesNoToAll.StoreResultDelegate storeresultfunction, bool returnonlyyesno)
		{
			_storeresultfunction	= storeresultfunction;
			_returnonlyyesno		= returnonlyyesno;
		}

		#endregion

		#region Show functions.

		/// <summary>
		/// Show a MessageBoxYesNoToAll dialog box and return the result.  The result is also stored for later retrieval.
		/// </summary>
		/// <param name="owner">Owner to show in front of.</param>
		/// <param name="text">Message to display.</param>
		public MessageBoxYesNoToAll.Result Show(IWin32Window owner, string text)
		{
			return Show(owner, text, "", MessageBoxYesNoToAll.Buttons.YesToAllNoCancel, MessageBoxYesNoToAll.Icon.Exclamation, MessageBoxYesNoToAll.DefaultButton.Button1);
		}

		/// <summary>
		/// Show a MessageBoxYesNoToAll dialog box and return the result.  The result is also stored for later retrieval.
		/// </summary>
		/// <param name="owner">Owner to show in front of.</param>
		/// <param name="text">Message to display.</param>
		/// <param name="caption">Title of message box.</param>
		public MessageBoxYesNoToAll.Result Show(IWin32Window owner, string text, string caption)
		{
			return Show(owner, text, caption, MessageBoxYesNoToAll.Buttons.YesToAllNoCancel, MessageBoxYesNoToAll.Icon.Exclamation, MessageBoxYesNoToAll.DefaultButton.Button1);
		}

		/// <summary>
		/// Show a MessageBoxYesNoToAll dialog box and return the result.  The result is also stored for later retrieval.
		/// </summary>
		/// <param name="owner">Owner to show in front of.</param>
		/// <param name="text">Message to display.</param>
		/// <param name="caption">Title of message box.</param>
		/// <param name="buttons">Which buttons to show.</param>
		public MessageBoxYesNoToAll.Result Show(IWin32Window owner, string text, string caption, MessageBoxYesNoToAll.Buttons buttons)
		{
			return Show(owner, text, caption, buttons, MessageBoxYesNoToAll.Icon.Exclamation, MessageBoxYesNoToAll.DefaultButton.Button1);
		}

		/// <summary>
		/// Show a MessageBoxYesNoToAll dialog box and return the result.  The result is also stored for later retrieval.
		/// </summary>
		/// <param name="owner">Owner to show in front of.</param>
		/// <param name="text">Message to display.</param>
		/// <param name="caption">Title of message box.</param>
		/// <param name="buttons">Which buttons to show.</param>
		/// <param name="icon">Icon to show.</param>
		public MessageBoxYesNoToAll.Result Show(IWin32Window owner, string text, string caption, MessageBoxYesNoToAll.Buttons buttons, MessageBoxYesNoToAll.Icon icon)
		{
			return Show(owner, text, caption, buttons, icon, MessageBoxYesNoToAll.DefaultButton.Button1);
		}

		/// <summary>
		/// Show a MessageBoxYesNoToAll dialog box and return the result.  The result is also stored for later retrieval.
		/// </summary>
		/// <param name="owner">Owner to show in front of.</param>
		/// <param name="text">Message to display.</param>
		/// <param name="caption">Title of message box.</param>
		/// <param name="buttons">Which buttons to show.</param>
		/// <param name="icon">Icon to show.</param>
		/// <param name="defaultbutton">Which button is default.</param>
		public MessageBoxYesNoToAll.Result Show(IWin32Window owner, string text, string caption, MessageBoxYesNoToAll.Buttons buttons, MessageBoxYesNoToAll.Icon icon, MessageBoxYesNoToAll.DefaultButton defaultbutton)
		{
			MessageBoxYesNoToAll.Result result1 = MessageBoxYesNoToAll.Result.Cancel;
			_storeresultfunction(ref result1, false);

			switch (result1)
			{
				case MessageBoxYesNoToAll.Result.YesToAll:
				case MessageBoxYesNoToAll.Result.NoToAll:
				{
					return CheckResult(result1);
				}
				default:
				{
					MessageBoxYesNoToAllForm boxYesNoToAllForm = new MessageBoxYesNoToAllForm(text, caption, buttons, icon, defaultbutton);
					boxYesNoToAllForm.ShowDialog(owner);
					MessageBoxYesNoToAll.Result result2 = boxYesNoToAllForm.Result;
					_storeresultfunction(ref result2, true);
					return CheckResult(result2);
				}
			}
		}

		#endregion

		#region  Helper functions.

		/// <summary>
		/// Performs the duty of converting "Yes to All" to "Yes" and "No to All" to "No" if that option is selected.
		/// </summary>
		/// <param name="dialogresult">Input dialog result.</param>
		/// <returns>Dialog result correct based on if the conversion option is selected.</returns>
		private MessageBoxYesNoToAll.Result CheckResult(MessageBoxYesNoToAll.Result dialogresult)
		{
			// Checks option to return "Yes" instead of "YesToAll" and "No" instead of "NoToAll".
			if (_returnonlyyesno)
			{
				switch (dialogresult)
				{
					case MessageBoxYesNoToAll.Result.YesToAll:
					{
						return MessageBoxYesNoToAll.Result.Yes;
					}
					case MessageBoxYesNoToAll.Result.NoToAll:
					{
						return MessageBoxYesNoToAll.Result.No;
					}
				}
			}
			return dialogresult;
		}

		#endregion

	} // End class.
} // End namespace.
