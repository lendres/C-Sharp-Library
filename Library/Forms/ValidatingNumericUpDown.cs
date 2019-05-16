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
	public partial class ValidatingNumericUpDown : NumericUpDown
	{
		#region DLL Imports

		[DllImport("user32.dll")]
		static extern void MessageBeep(uint uType);

		const uint MB_OK                = 0x00000000;
		const uint MB_ICONHAND          = 0x00000010;
		const uint MB_ICONQUESTION      = 0x00000020;
		const uint MB_ICONEXCLAMATION   = 0x00000030;
		const uint MB_ICONASTERISK      = 0x00000040;

		#endregion

		#region Members

		bool				_beepOnError				= false;

		#endregion

		#region Construction

		/// <summary>
		/// Construction.
		/// </summary>
		public ValidatingNumericUpDown()
		{
			InitializeComponent();
		}

		#endregion

		#region Event Handling

		/// <summary>
		/// When validating, pad any remaining decimal places with zeros.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnValidating(CancelEventArgs e)
		{
			base.OnValidating(e);
		}

		#endregion

		#region Properties

		///// <summary>
		///// Allows up/down controls to be hidden.
		///// </summary>
		//[Category("Numeric Behavior")]
		//public bool UpDownControlsVisable
		//{
		//	get
		//	{
		//		return this.Controls[0].Visible;
		//	}

		//	set
		//	{
		//		// Option to hide up/down controls.  The NumericUpDown control contains two controls,
		//		// the text box and the up/down buttons.  The up/down buttons are the first (0) control.
		//		//this.BackColor = System.Drawing.SystemColors.Control;
		//		this.Controls[0].Visible = value;
		//		this.Controls[0].Enabled = value;
		//		//this.Controls[1].Width += this.Controls[0].Width;
		//		//this.Controls[1].Dock = DockStyle.Fill;
		//		this.Controls[1].BackColor = Color.AliceBlue;
		//		this.Controls[0].BackColor = System.Drawing.SystemColors.Control;
		//		//this.Controls[0].Parent.BackColor = System.Drawing.SystemColors.Control;
		//	}
		//}

		/// <summary>
		/// If true, the default "beep" sound will be emitted if an invalid key is pressed.
		/// </summary>
		[Category("Numeric Behavior")]
		public bool BeepOnError
		{
			get
			{
				return _beepOnError;
			}

			set
			{
				_beepOnError = value;
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

		#region Methods

		/// <summary>
		/// Beep on an error, if the option is enabled.
		/// </summary>
		private void DoBeep()
		{
			if (_beepOnError)
			{
				//System.Media.SystemSounds.Exclamation.Play();
				MessageBeep(MB_ICONEXCLAMATION);
			}
		}

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

		#endregion

	} // End class.
} // End namespace.