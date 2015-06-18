using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Runtime.InteropServices;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Text box that only allows numeric values.
	/// 
	/// From: http://msdn.microsoft.com/en-us/library/dd183783%28v=vs.90%29.aspx
	/// </summary>
	public partial class NumericTextBox : TextBox
	{
		#region DLL imports.

		[DllImport("user32.dll")]
		static extern void MessageBeep(uint uType);

		const uint MB_OK                = 0x00000000;
		const uint MB_ICONHAND          = 0x00000010;
		const uint MB_ICONQUESTION      = 0x00000020;
		const uint MB_ICONEXCLAMATION   = 0x00000030;
		const uint MB_ICONASTERISK      = 0x00000040;

		#endregion

		#region Members.

		// Options.
		bool				_limitdecimalplaces				= true;
		uint				_decimalplaces					= 2;
		bool				_displayalldecimalplaces		= false;
		bool				_allowdecimal					= true;
		bool				_enforceminimumvalue			= false;
		double				_minimumvalue					= 0;
		bool				_enforcemaximumvalue			= false;
		double				_maximumvalue					= 100;
		bool				_allowseparator					= false;
		bool				_beeponinvalidkey				= true;

		// Some variables for tracking during formatting.
		bool				_skipevent						= false;
		int					_separatorcount					= 0;

		// Variables for formatting the string.
		NumberFormatInfo	_numberformatinfo;
		char				_decimalseparator;
		char				_groupseparator;
		char				_negativesign;
		int[]				_groupsize;

		#endregion

		#region Construction.

		/// <summary>
		/// Construction.
		/// </summary>
		public NumericTextBox()
		{
			_numberformatinfo	= System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
			_decimalseparator	= _numberformatinfo.NumberDecimalSeparator.ToCharArray()[0];
			_groupseparator		= _numberformatinfo.NumberGroupSeparator.ToCharArray()[0];
			_groupsize			= _numberformatinfo.NumberGroupSizes;
			_negativesign		= _numberformatinfo.NegativeSign.ToCharArray()[0];
		}

		#endregion

		#region Event handling.

		/// <summary>
		/// Restricts the entry of characters to digits (including hexadecimal), the negative sign, the decimal point,
		/// and editing keystrokes (backspace).  
		/// </summary>
		/// <param name="eventargs">Event arguments.</param>
		protected override void OnKeyPress(KeyPressEventArgs eventargs)
		{
			// Allow base class to do any work.
			base.OnKeyPress(eventargs);

			// If the control is set to "ReadOnly," we don't allow text to be entered.
			if (this.ReadOnly)
			{
				eventargs.Handled = true;
				return;
			}

			eventargs.Handled = HandleNewKey(eventargs.KeyChar);
		}

		/// <summary>
		/// Insert the new key if it is valid, otherwise ignore it.
		/// </summary>
		/// <param name="keyinput">Key, as a string, to try to insert into the text.</param>
		/// <returns>True if the key has been handled (inserted or ignored) by this method, otherwise false is returned and the base control can handle it.</returns>
		private bool HandleNewKey(char keyinput)
		{
			bool validkey = false;
			return HandleNewKey(keyinput, out validkey);
		}

		/// <summary>
		/// Insert the new key if it is valid, otherwise ignore it.
		/// </summary>
		/// <param name="keyinput">Key, as a string, to try to insert into the text.</param>
		/// <param name="validkey">Output that returns true if the key was valid for insertion, false otherwise.</param>
		/// <returns>True if the key has been handled (inserted or ignored) by this method, otherwise false is returned and the base control can handle it.</returns>
		private bool HandleNewKey(char keyinput, out bool validkey)
		{
			// Allows Backspace key to pass through and operate normally.
			if (keyinput == '\b')
			{
				validkey = true;
				return false;
			}

			// Allows Return/Enter key to pass through and operate normally.
			if (keyinput == '\r')
			{
				CancelEventArgs cancelEventArguments = new CancelEventArgs();
				OnValidating(cancelEventArguments);
				if (!cancelEventArguments.Cancel)
				{
					EventArgs eventArguments = new EventArgs();
					OnValidated(new EventArgs());
				}
				
				validkey = true;
				return false;
			}

			// Allows one decimal separator as input. 
			if (keyinput.Equals(_decimalseparator))
			{
				if (_allowdecimal)
				{
					UpdateText(keyinput);
				}
				validkey = true;
				return true;
			}

			// If it's not a digit, it passes.
			// Allows negative sign if the negative sign is the initial character.
			if (!(Char.IsDigit(keyinput) || keyinput.Equals(_negativesign)))
			{
				validkey = false;
				return true;
			}

			// Insert the new key into the text.
			string updatedtext = InsertNewKeyIntoString(keyinput, this.SelectionStart);

			// If it's a digit, it passes.
			// Allows negative sign if the negative sign is the initial character.
			if (updatedtext == "-")
			{
				UpdateText(keyinput);
				validkey = true;
				return true;
			}

			bool passedconversion;
			double newvalue = ConvertValueToDouble(updatedtext, out passedconversion);
			if (passedconversion)
			{
				// Enforce minimum value.
				if (_enforceminimumvalue)
				{
					if (newvalue < _minimumvalue)
					{
						DoBeep();
						validkey = false;
						return true;
					}
				}

				// Enforce maximum value.
				if (_enforcemaximumvalue)
				{
					if (newvalue > _maximumvalue)
					{
						DoBeep();
						validkey = false;
						return true;
					}
				}

				// All is good, so update the text.
				UpdateText(keyinput);
				validkey = true;
				return true;
			}

			// Consume this invalid key and beep.
			DoBeep();
			validkey = false;
			return false;
		}

		/// <summary>
		/// Beep on an error, if the option is enabled.
		/// </summary>
		private void DoBeep()
		{
			if (_beeponinvalidkey)
			{
				//System.Media.SystemSounds.Exclamation.Play();
				MessageBeep(MB_ICONEXCLAMATION);
			}
		}

		/// <summary>
		/// Key down event.
		/// </summary>
		/// <param name="eventargs">Event arguments.</param>
		protected override void OnKeyDown(KeyEventArgs eventargs)
		{
			base.OnKeyDown(eventargs);

			// Paste event.
			if (eventargs.Control && eventargs.KeyCode == Keys.V)
			{
				// If the control is set to "ReadOnly," we don't allow text to be entered.
				if (this.ReadOnly)
				{
					eventargs.Handled = true;
					return;
				}

				char[] clipboardtext	= Clipboard.GetText().ToCharArray();
				bool cont				= true;
				int i					= 0;

				// Loop over each character and process it to see if it can be added to the control's text.
				while (cont)
				{
					HandleNewKey(clipboardtext[i++], out cont);
					cont = cont && i < clipboardtext.Length;
				}

				eventargs.Handled = true;
				return;
			}

			// Copy event.
			if (eventargs.Control && eventargs.KeyCode == Keys.C)
			{
				Clipboard.SetText(this.Text);
				eventargs.Handled = true;
				return;
			}
		}

		/// <summary>
		/// Key up event.
		/// </summary>
		/// <param name="eventargs">Event arguments.</param>
		protected override void OnKeyUp(KeyEventArgs eventargs)
		{
			base.OnKeyUp(eventargs);

			if (_skipevent)
			{
				_skipevent = false;
				return;
			}
			// Restores text box to Numeric mode if it was 
			// changed for decimal entry.
			//InputModeEditor.SetInputMode(this, InputMode.Numeric);
		}
		
		/// <summary>
		/// When validating, pad any remaining decimal places with zeros.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnValidating(CancelEventArgs e)
		{
			EnforceAllDecimalPlaces();

 			base.OnValidating(e);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Ensure all the decimal places are correct when setting the text.
		/// </summary>
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				EnforceAllDecimalPlaces();
			}
		}

		/// <summary>
		/// If true, the default "beep" sound will be emitted if an invalid key is pressed.
		/// </summary>
		[Category("Numeric Behavior")]
		public bool BeepOnInvalidKey
		{
			get
			{
				return _beeponinvalidkey;
			}

			set
			{
				_beeponinvalidkey = value;
			}
		}

		/// <summary>
		/// Specifies if the number of decimal places is limited.  If they are limited, then only the number
		/// of decimal digits specified by NumberOfDecimalPlaces is shown on the control.
		/// </summary>
		[Category("Numeric Behavior")]
		public bool LimitDecimalPlaces
		{
			get
			{
				return _limitdecimalplaces;
			}

			set
			{
				_limitdecimalplaces	= value;
				_allowdecimal		= !_limitdecimalplaces || (_limitdecimalplaces && _decimalplaces > 0);
			}
		}

		/// <summary>
		/// If LimitDecimalPlaces is true, then this is the number of decimal digits allowed.
		/// </summary>
		[Category("Numeric Behavior")]
		[Description("If LimitDecimalPlaces is true, then this is the number of decimal digits allowed.")]
		public uint NumberOfDecimalPlaces
		{
			get
			{
				return _decimalplaces;
			}

			set
			{
				_decimalplaces = value;
				_allowdecimal = !_limitdecimalplaces || (_limitdecimalplaces && _decimalplaces > 0);
			}
		}

		/// <summary>
		/// If DisplayAllDecimalPlaces is true, then zeros will pad any decimal places not filled by the user.
		/// </summary>
		[Category("Numeric Behavior")]
		public bool DisplayAllDecimalPlaces
		{
			get
			{
				return _displayalldecimalplaces;
			}

			set
			{
				_displayalldecimalplaces = value;
			}
		}

		/// <summary>
		/// Restricts the user to entering values equal to or above the minimum value.
		/// </summary>
		[Category("Numeric Behavior")]
		public bool EnforceMinimumValue
		{
			get
			{
				return _enforceminimumvalue;
			}

			set
			{
				_enforceminimumvalue = value;
			}
		}

		/// <summary>
		/// Minimum value allowed in the text box.  Only enforces if "EnforceMinimumValue" is true.
		/// </summary>
		[Category("Numeric Behavior")]
		public double MinimumValue
		{
			get
			{
				return _minimumvalue;
			}

			set
			{
				_minimumvalue = value;
			}
		}
		
		/// <summary>
		/// Restricts the user to entering values equal to or below the maximum value.
		/// </summary>
		[Category("Numeric Behavior")]
		public bool EnforceMaximumValue
		{
			get
			{
				return _enforcemaximumvalue;
			}

			set
			{
				_enforcemaximumvalue = value;
			}
		}

		/// <summary>
		/// Minimum value allowed in the text box.  Only enforces if "EnforceMinimumValue" is true.
		/// </summary>
		[Category("Numeric Behavior")]
		public double MaximumValue
		{
			get
			{
				return _maximumvalue;
			}

			set
			{
				_maximumvalue = value;
			}
		}

		/// <summary>
		/// Specifies is the separator character is allowed.
		/// </summary>
		[Category("Numeric Behavior")]
		public bool AllowSeparator
		{
			get
			{
				return _allowseparator;
			}

			set
			{
				_allowseparator = value;
			}
		}

		/// <summary>
		/// Return value as an integer (int).
		/// </summary>
		[Browsable(false)]
		public int IntValue
		{
			get
			{
				// Trying to convert it to an int using int.Parse did not seem to work.  Instead, parse it as a double, then convert to the int.
				return (int)this.DecimalValue;
			}

			set
			{
				this.Text = value.ToString();
			}
		}

		/// <summary>
		/// Return value as a decimal.
		/// </summary>
		[Browsable(false)]
		public double DecimalValue
		{
			get
			{
				if (this.Text == "")
				{
					return 0;
				}

				bool passed;
				return ConvertValueToDouble(this.Text, out passed);
			}

			set
			{
				this.Text = value.ToString();
			}
		}

		#endregion

		#region Methods.

		/// <summary>
		/// Convert the value to a double and catch any errors.
		/// </summary>
		/// <param name="text">Text to try to convert.</param>
		/// <param name="passed">Flag to indicate if the conversion was successful.</param>
		/// <returns>The converted value as a double.  If the conversion fails, a FormatException is thrown.</returns>
		private double ConvertValueToDouble(string text, out bool passed)
		{
			passed = true;
			try
			{
				return Double.Parse(text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign);
			}
			catch (FormatException exception)
			{
				passed = false;
				MessageBox.Show("An invalid format: " + exception.Message.ToString());
				return 0;
			}
		}

		/// <summary>
		/// Checks current input characters and updates control with valid characters only.  Eliminates all digits to the
		/// right of extraneous decimal characters.
		/// </summary>
		/// <param name="newkey">New key (char) entered.</param>
		public void UpdateText(char newkey)
		{
			_separatorcount = 0;

			// Rebuild the string in the text box starting with the existing text, removing any selected text, and inserting the new character.
			int caretposition = this.SelectionStart;

			string input = InsertNewKeyIntoString(newkey, caretposition);

			 // The "caretposition" is the position of the caret (cursor) before modifying
			// the text.  Since we inserted a character, we increment it by one.
			caretposition++;

			string	updatedtext	= "";
			int		csize		= 0;

			// char[] tokens = new char[] { decimalSeparator.ToCharArray()[0] }; 
			// NOTE: Supports decimalSeparator with a length == 1. 
			char token = _decimalseparator;
			string[] groups = input.Split(token);

			// Returning input to left of decimal. 
			char[] inputchars = groups[0].ToCharArray();

			// Reversing input to handle separators. 
			char[] reversedinputchars = inputchars;
			Array.Reverse(reversedinputchars);
			StringBuilder stringbuilder = new StringBuilder();

			bool validkey = false;

			for (int i = 0; i < reversedinputchars.Length; i++)
			{
				if (reversedinputchars[i].ToString().Equals(_groupseparator))
				{
					continue;
				}

				if (reversedinputchars[i].ToString().Equals(_negativesign))
				{
					// Ignore negative sign unless processing first character. 
					if (i < reversedinputchars.Length-1)
					{
						// Character was a negative sign and we are ignoring it.  For this special case we do not
						// want to increment the carat, so de-increment it so that the incrementing later will restore
						// the original position.
						caretposition--;
						continue;
					}
					stringbuilder.Insert(0, reversedinputchars[i].ToString());
					i++;
					continue;
				}

				if (_allowseparator)
				{
					// NOTE: Does not support multiple groupSeparator sizes. 
					if (csize > 0 && csize % _groupsize[0] == 0)
					{
						stringbuilder.Insert(0, _groupseparator);
						_separatorcount++;
					}
				}

				// Maintaining correct group size for digits. 
				if (Char.IsDigit(reversedinputchars[i]))
				{
					// Increment cSize only after processing groupSeparators.
					csize++;
					validkey = true;
				}

				if (validkey)
				{
					stringbuilder.Insert(0, reversedinputchars[i].ToString());
				}

				validkey = false;
			}

			updatedtext = stringbuilder.ToString();

			// Handle decimal places.
			if (groups.Length > 1)
			{
				char[] rightofdecimals			= groups[1].ToCharArray();
				StringBuilder stringbuilder2	= new StringBuilder();

				// Allow for limiting the number of decimal places.  We calculated the maximum digits allowed, then count
				// each found digit.  We don't want to loose digits if someone tries to insert an invalid character.  For
				// example if we had "123.45" with two allowed decimal places and someone tries to insert a non-digit like
				// "123.4g5" we want to make sure we return "123.45" and not "123.4".  So we count the "founddigits" instead
				// of just counting entries in "rightofdecimals" when limiting decimal places.
				int maxdigits	= rightofdecimals.Length;
				int founddigits	= 0;
				if (_limitdecimalplaces)
				{
					// If the number of is limited, we specify that as max digits.  Note, this might
					// mean maxdigits > rightofdecimals.Length, but in that case, the loop below will
					// terminate before maxdigits are found (this is desired behavior).
					maxdigits = (int)_decimalplaces;
				}

				// Copy any digits to the output.
				for (int i = 0; i < rightofdecimals.Length; i++)
				{
					if (Char.IsDigit(rightofdecimals[i]))
					{
						stringbuilder2.Append(rightofdecimals[i]);
						founddigits++;
					}

					// If we found as many digits as we allow, we are done.
					if (founddigits == maxdigits)
					{
						break;
					}
				}

				updatedtext += _decimalseparator + stringbuilder2.ToString();
			}

			// Updates text box. 
			this.Text = updatedtext;

			// Update cursor position.
			this.SelectionStart = caretposition;
		}

		/// <summary>
		/// Insert the new key character into the text displayed on the text box.  Selected text will be overwritten.
		/// </summary>
		/// <param name="newkey">Character to insert.</param>
		/// <param name="position">Position to insert the character at.</param>
		/// <returns>A new string with the selected text removed and the new character inserted.</returns>
		private string InsertNewKeyIntoString(char newkey, int position)
		{
			string input		= this.Text;
			int selectionsize	= this.SelectionLength;
			input				= input.Substring(0, position) + newkey.ToString() + input.Substring(position+selectionsize, input.Length-position-selectionsize);
			return input;
		}

		/// <summary>
		/// Ensures the number of decimal places shown is correct.
		/// </summary>
		private void EnforceAllDecimalPlaces()
		{
			// If we are enforcing a minimum number of decimal places, we enforce it here.
			if (_displayalldecimalplaces)
			{
				string[] groups = this.Text.Split(_decimalseparator);

				string righthandside = "";
				if (groups.Length > 1)
				{
					righthandside = groups[1];
				}
				if (righthandside.Length < _decimalplaces)
				{
					righthandside = righthandside.PadRight((int)_decimalplaces, '0');
					this.Text = groups[0] + _decimalseparator + righthandside;
				}
			}
		}

		#endregion

	} // End class.
} // End namespace.