using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Provides parallel behavior to the MessageBox class but adds "Yes to All" and "No to All" options.  This is the form that is diplayed.
	/// This class should not be used directory.  It is used by the MessabeBoxYesNoToAll and, in combination, they provide the behavior which
	/// parallels the standard MessageBox while adding the new controls.
	/// 
	/// 
	/// </summary>
	internal partial class MessageBoxYesNoToAllForm : Form
	{
		#region Members

		private MessageBoxYesNoToAll.Result			_dialogresult				= MessageBoxYesNoToAll.Result.Cancel;
		private int									_xspacing					= 81;
		private Point								_firstbuttonlocation;
		private int									_yoffset;
		private Button[]							_buttons;

		#endregion

		#region Construction

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="text">Message to display.</param>
		public MessageBoxYesNoToAllForm(string text)
		{
			InitializeComponent();

			this.txtbxMessage.Text = text;
			Initialize();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="text">Message to display.</param>
		/// <param name="caption">Title of message box.</param>
		public MessageBoxYesNoToAllForm(string text, string caption)
		{
			InitializeComponent();

			this.txtbxMessage.Text	= text;
			this.Text				= caption;
			Initialize();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="text">Message to display.</param>
		/// <param name="caption">Title of message box.</param>
		/// <param name="buttons">Which buttons to show.</param>
		public MessageBoxYesNoToAllForm(string text, string caption, MessageBoxYesNoToAll.Buttons buttons)
		{
			InitializeComponent();

			this.txtbxMessage.Text	= text;
			this.Text				= caption;
			Initialize();

			SetButtons(buttons);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="text">Message to display.</param>
		/// <param name="caption">Title of message box.</param>
		/// <param name="buttons">Which buttons to show.</param>
		/// <param name="icon">Icon to show.</param>
		public MessageBoxYesNoToAllForm(string text, string caption, MessageBoxYesNoToAll.Buttons buttons, MessageBoxYesNoToAll.Icon icon)
		{
			InitializeComponent();

			this.txtbxMessage.Text	= text;
			this.Text				= caption;
			SetIcon(icon);
			Initialize();

			SetButtons(buttons);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="text">Message to display.</param>
		/// <param name="caption">Title of message box.</param>
		/// <param name="buttons">Which buttons to show.</param>
		/// <param name="icon">Icon to show.</param>
		/// <param name="defaultbutton">Which button is default.</param>
		public MessageBoxYesNoToAllForm(string text, string caption, MessageBoxYesNoToAll.Buttons buttons, MessageBoxYesNoToAll.Icon icon, MessageBoxYesNoToAll.DefaultButton defaultbutton)
		{
			InitializeComponent();

			this.txtbxMessage.Text	= text;
			this.Text				= caption;
			SetIcon(icon);
			Initialize();

			SetDefaultButton(defaultbutton, this.SetButtons(buttons));
		}

		/// <summary>
		/// Common construction/initialization.
		/// </summary>
		private void Initialize()
		{
			this.ActiveControl		= (Control)this.pctboxIcon;
			_firstbuttonlocation	= this.btnCancel.Location;
			_yoffset				= this.Size.Height - this.btnCancel.Location.Y;
			_buttons				= new Button[5] {this.btnYes, this.btnYesToAll, this.btnNo, this.btnNoToAll, this.btnCancel};
		}

		#endregion

		#region Properties

		/// <summary>
		/// Dialog result.
		/// </summary>
		public MessageBoxYesNoToAll.Result Result
		{
			get
			{
				return _dialogresult;
			}
			set
			{
				_dialogresult = value;
			}
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Yes button pressed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void btnYes_Click(object sender, EventArgs e)
		{
			_dialogresult = MessageBoxYesNoToAll.Result.Yes;
			Close();
		}

		/// <summary>
		/// Yes to All button pressed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void btnYesToAll_Click(object sender, EventArgs e)
		{
			_dialogresult = MessageBoxYesNoToAll.Result.YesToAll;
			Close();
		}

		/// <summary>
		/// No button pressed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void btnNo_Click(object sender, EventArgs e)
		{
			_dialogresult = MessageBoxYesNoToAll.Result.No;
			Close();
		}

		/// <summary>
		/// No to All button pressed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void btnNoToAll_Click(object sender, EventArgs e)
		{
			_dialogresult = MessageBoxYesNoToAll.Result.NoToAll;
			Close();
		}

		/// <summary>
		/// Cancel button pressed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void btnCancel_Click(object sender, EventArgs e)
		{
			_dialogresult = MessageBoxYesNoToAll.Result.Cancel;
			Close();
		}

		#endregion

		#region Control Manipulation (spacing, visible, default)

		/// <summary>
		/// Creates the image on the control from the specified icon.
		/// </summary>
		/// <param name="icon">Icon to show.</param>
		private void SetIcon(MessageBoxYesNoToAll.Icon icon)
		{
			switch (icon)
			{
				case MessageBoxYesNoToAll.Icon.Asterisk:
				{
					this.pctboxIcon.Image = (Image)SystemIcons.Asterisk.ToBitmap();
					break;
				}
				case MessageBoxYesNoToAll.Icon.Error:
				{
					this.pctboxIcon.Image = (Image)SystemIcons.Error.ToBitmap();
					break;
				}
				case MessageBoxYesNoToAll.Icon.Exclamation:
				{
					this.pctboxIcon.Image = (Image)SystemIcons.Exclamation.ToBitmap();
					break;
				}
				case MessageBoxYesNoToAll.Icon.Information:
				{
					this.pctboxIcon.Image = (Image)SystemIcons.Information.ToBitmap();
					break;
				}
				case MessageBoxYesNoToAll.Icon.Question:
				{
					this.pctboxIcon.Image = (Image)SystemIcons.Question.ToBitmap();
					break;
				}
				case MessageBoxYesNoToAll.Icon.Warning:
				{
					this.pctboxIcon.Image = (Image)SystemIcons.Warning.ToBitmap();
					break;
				}
			}
		}

		/// <summary>
		/// Sets which buttons are shown on the control.
		/// </summary>
		/// <param name="buttons">Which buttons are shown.</param>
		/// <returns>An array of bools indicating which buttons are t</returns>
		private bool[] SetButtons(MessageBoxYesNoToAll.Buttons buttons)
		{
			bool[] visiblebuttons = ResetButtons();
			switch (buttons)
			{
				case MessageBoxYesNoToAll.Buttons.YesToAllNo:
				{
					visiblebuttons[0] = true;
					visiblebuttons[1] = true;
					visiblebuttons[2] = true;
					break;
				}
				case MessageBoxYesNoToAll.Buttons.YesToAllNoCancel:
				{
					visiblebuttons[0] = true;
					visiblebuttons[1] = true;
					visiblebuttons[2] = true;
					visiblebuttons[4] = true;
					break;
				}
				case MessageBoxYesNoToAll.Buttons.YesNoToAll:
				{
					visiblebuttons[0] = true;
					visiblebuttons[2] = true;
					visiblebuttons[3] = true;
					break;
				}
				case MessageBoxYesNoToAll.Buttons.YesNoToAllCancel:
				{
					visiblebuttons[0] = true;
					visiblebuttons[2] = true;
					visiblebuttons[3] = true;
					visiblebuttons[4] = true;
					break;
				}
				case MessageBoxYesNoToAll.Buttons.YesToAllNoToAll:
				{
					visiblebuttons[0] = true;
					visiblebuttons[1] = true;
					visiblebuttons[2] = true;
					visiblebuttons[3] = true;
					break;
				}
				case MessageBoxYesNoToAll.Buttons.YesToAllNoToAllCancel:
				{
					visiblebuttons[0] = true;
					visiblebuttons[1] = true;
					visiblebuttons[2] = true;
					visiblebuttons[3] = true;
					visiblebuttons[4] = true;
					break;
				}
			}

			for (int i = 0; i < visiblebuttons.Length; ++i)
			{
				_buttons[i].Visible = visiblebuttons[i];
			}
			this.ControlBox = visiblebuttons[4];
			SpaceButtons(true, visiblebuttons);
			return visiblebuttons;
		}

		private bool[] ResetButtons()
		{
			bool[] visiblebuttons = new bool[5];
			for (int i = 0; i < visiblebuttons.Length; i++)
			{
				visiblebuttons[i] = false;
			}
			SpaceButtons(false, visiblebuttons);
			return visiblebuttons;
		}

		private void SpaceButtons(bool onlyvisible, bool[] visiblebuttons)
		{
			Point point					= _firstbuttonlocation;
			point.Y						= this.Size.Height - _yoffset;

			for (int i = _buttons.Length-1; i > -1; i--)
			{
				if (!onlyvisible || visiblebuttons[i])
				{
					_buttons[i].Location = point;

					// Calculate next point that a control would be displayed at.
					point.X = point.X - _xspacing;
				}
			}
		}

		private void SetDefaultButton(MessageBoxYesNoToAll.DefaultButton defaultbutton, bool[] visiblebuttons)
		{
			int num = 0;
			for (int i = 0; i < _buttons.Length; i++)
			{
				// Look for the specified button to set it as the default.  We only want to count the ones that are visible.
				// Uses the short-circuit functionality of "if" function for counting (num++).
				if (visiblebuttons[i] && (MessageBoxYesNoToAll.DefaultButton)num++ == defaultbutton)
				{
					this.AcceptButton = (IButtonControl)_buttons[i];
					break;
				}
			}
		}

		#endregion

	} // End class.
} // End namespace.