using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlsAndFormsTesting
{
	public partial class Form1 : Form
	{
		private double				_numerictextboxvalue;

		public Form1()
		{
			InitializeComponent();
		}

		public double NumericTextBoxValue
		{
			get
			{
				return _numerictextboxvalue;
			}
			set
			{
				_numerictextboxvalue = value;
			}
		}

		/// <summary>
		/// Paint the control with an error formatting.
		/// </summary>
		/// <param name="textBox">TextBox to format as an error.</param>
		protected void DisplayError(TextBox textBox)
		{
			textBox.BackColor	= System.Drawing.Color.Red;
			textBox.ForeColor	= System.Drawing.Color.White;
			textBox.Font		= new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
		}

		/// <summary>
		/// Clear the error formatting.
		/// </summary>
		/// <param name="textBox">TextBox to clear the error formatting of.</param>
		protected void ClearError(TextBox textBox)
		{
			textBox.BackColor	= System.Drawing.SystemColors.Window;
			textBox.ForeColor	= System.Drawing.Color.Black;
			textBox.Font		= new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
		}

		private void numericTextBox3_Validating(object sender, CancelEventArgs e)
		{
			if (this.numericTextBox3.DecimalValue > 50)
			{
				ClearError(this.numericTextBox3);
			}
			else
			{
				DisplayError(this.numericTextBox3);
				e.Cancel = true;
			}
		}

	} // End class.
} // End namespace.