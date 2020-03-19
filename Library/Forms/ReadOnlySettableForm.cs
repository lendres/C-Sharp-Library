using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// A form that can have all it's controls disabled to create a read only form.
	/// </summary>
	public partial class ReadOnlySettableForm : Form
	{
		#region Members

		private bool			_readOnly			= false;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ReadOnlySettableForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Constructor to set read only state.
		/// </summary>
		/// <param name="readOnly">Specifies if the controls should be set to read only.</param>
		public ReadOnlySettableForm(bool readOnly)
		{
			InitializeComponent();
			_readOnly = readOnly;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Form Load event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void ReadOnlySettableForm_Load(object sender, System.EventArgs eventArgs)
		{
			// Have to do this after the derived class calls "InitializeComponent" otherwise all the read only
			// settings get overwritten.  We handle this by waiting until the form is loading, then setting the
			// read only state of the controls.
			if (_readOnly)
			{
				SetReadOnly();
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Set the form to read only.
		/// </summary>
		public virtual void SetReadOnly()
		{
			_readOnly	=  true;
			this.Text	+= " (Read Only)";

			SetControlsToReadOnly(this);

			if (this.AcceptButton != null && this.AcceptButton is Button)
			{
				((Button)this.AcceptButton).Visible = false;
			}

			if (this.CancelButton != null && this.CancelButton is Button)
			{
				((Button)this.CancelButton).Text	= "Close";
				((Button)this.CancelButton).Enabled	= true;
			}
		}

		/// <summary>
		/// Sets the controls to read only.
		/// </summary>
		/// <param name="rootControl">Control to set child controls to read only.</param>
		private void SetControlsToReadOnly(Control rootControl)
		{
			// Normal control.
			foreach (Control childControl in rootControl.Controls)
			{
				switch (childControl)
				{
					// For containers and controls that are static (can be changed) we ignore.
					case GroupBox	groupBox:
					case Panel		panel:
					case TabControl	tabControl:
					{
						// Search the children controls.
						SetControlsToReadOnly(childControl);
						break;
					}
					case Label		label:
					{
						break;
					}

					case TextBox	textBox:
					{
						textBox.ReadOnly = true;
						break;
					}

					case NumericUpDown numericUpDown:
					{
						numericUpDown.ReadOnly = true;
						break;
					}

					default:
					{
						childControl.Enabled = false;
						break;
					}
				}
			}
		}

		#endregion

	} // End class.
} // End namespace.
