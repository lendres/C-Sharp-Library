using DigitalProduction.Forms;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ControlsAndFormsTesting
{
	public partial class Form1 : Form
	{
		#region Members

		private double				_numericTextBoxValue;

		#endregion

		#region Construction

		/// <summary>
		/// Default contstructor.
		/// </summary>
		public Form1()
		{
			InitializeComponent();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Value in the numberic text box.
		/// </summary>
		public double NumericTextBoxValue
		{
			get
			{
				return _numericTextBoxValue;
			}
			set
			{
				_numericTextBoxValue = value;
			}
		}

		#endregion

		#region Event Handlers

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

		private void buttonBrowseText_Click(object sender, EventArgs e)
		{
			string initialDirectory = @"C:\Temp";

			// We use restore directory set to false so that the current directory is left as the initial directory above.
			string path = FileSelect.BrowseForAFile(this, "Text files (*.txt)|*.txt|All files (*.*)|*.*", "Select File", initialDirectory, false);

			if (path != "")
			{
				this.textBoxTextFile.Text = path;
			}
		}

		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			string initialDirectory = @"C:\Projects\WellDrill\60513-welldrill\source\WellDrill\ProgramData Files";

			string path = FileSelect.BrowseForAFile(this, "XSLT files (*.xslt)|*.xslt|All files (*.*)|*.*", "Select Transformation File", initialDirectory, true);

			if (path != "")
			{
				this.textBoxXsltFile.Text = path;
			}
		}

		private void buttonBrowseNoInitialDirectory_Click(object sender, EventArgs e)
		{
			string path = FileSelect.BrowseForAFile(this, "Text files (*.txt)|*.txt|All files (*.*)|*.*", "Select File", false);

			if (path != "")
			{
				this.textBoxTextNoInitialDirectory.Text = path;
			}
		}

		#endregion

		#region Methods

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

		#endregion

	} // End class.
} // End namespace.