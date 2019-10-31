using System;
using MsdnMag;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.Win32;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// A message box that incorporates a check box to indicate that the message should not be displayed again.
	/// </summary>
	public class MessageBoxDNS
	{
		#region Delegates

		/// <summary>
		/// Delegate template to use if you want this message box to access the registry key through a function.  The function
		/// must store the value of registryvalue if setvalue is true.  If setvalue is false the function must put the stored
		/// value into registryvalue.
		/// </summary>
		/// <example>
		/// new MessageBoxDNS.RegistryValueDelegate(MyRegFunction)
		/// </example>
		/// 
		public delegate void RegistryValueDelegate(ref bool registryvalue, bool setvalue);

		#endregion

		#region Members

		/// <summary>
		/// Function template to store the registry value.
		/// </summary>
		protected RegistryValueDelegate	_registryaccessfunction		= null;

		/// <summary>
		/// Hook onto window events.
		/// </summary>
		protected LocalCbtHook			_cbt						= null;

		/// <summary>
		/// Windows handle of this window.
		/// </summary>
		protected IntPtr				_hwnd						= IntPtr.Zero;

		/// <summary>
		/// Check box that is created on the message box.
		/// </summary>
		protected IntPtr				_checkbox					= IntPtr.Zero;

		/// <summary>
		/// Is this window active.
		/// </summary>
		protected bool					_bInit						= false;

		/// <summary>
		/// Is the check box added to the message box checked?
		/// </summary>
		protected bool					_boxischecked				= false;

		/// <summary>
		/// Registry key location.
		/// </summary>
		protected string				_registrykey				= null;

		/// <summary>
		/// Registry value (entry).
		/// </summary>
		protected string				_registryvalue				= null;

		/// <summary>
		/// Default text for the Do Not Show check box.
		/// </summary>
		protected static string			_DoNotShowAgainMessage		= "Don't show this message again.";

		#endregion

		#region Construction

		/// <summary>
		/// Constructor.  Use this constructor to access a registry key by providing the key and value as strings.
		/// </summary>
		/// <param name="registrykey">The registry key used to hold the registry value which is used to store the do not show again value.</param>
		/// <param name="registryvalue">Registry value which stores the result of the do not show again check box.</param>
		/// <example>
		/// MessageBoxDNS dialog = new MessageBoxDNS(@"Software\My Company\My App", "Dont Show Again");
		/// dialog.Show(this, "Warning: Something", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
		/// </example>
		public MessageBoxDNS(string registrykey, string registryvalue)
		{
			_registrykey = registrykey;
			_registryvalue = registryvalue;

			_cbt = new LocalCbtHook();
			_cbt.WindowCreated += new LocalCbtHook.CbtEventHandler(WndCreated);
			_cbt.WindowDestroyed += new LocalCbtHook.CbtEventHandler(WndDestroyed);
			_cbt.WindowActivated += new LocalCbtHook.CbtEventHandler(WndActivated);
		}

		/// <summary>
		/// Constructor.  Use this constructor to access a registry key (or any other place that the value can be stored) by
		/// providing a function of the form RegistryValueDelegate.
		/// </summary>
		/// <param name="registryaccessfunction">Function that sets and gets a stored boolean.</param>
		/// <example>
		/// MessageBoxDNS dialog = new MessageBoxDNS(new MessageBoxDNS.RegistryValueDelegate(MyFunction));
		/// dialog.Show(this, "Warning: Something", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
		/// </example>
		public MessageBoxDNS(RegistryValueDelegate registryaccessfunction)
		{
			_registryaccessfunction = registryaccessfunction;

			_cbt = new LocalCbtHook();
			_cbt.WindowCreated += new LocalCbtHook.CbtEventHandler(WndCreated);
			_cbt.WindowDestroyed += new LocalCbtHook.CbtEventHandler(WndDestroyed);
			_cbt.WindowActivated += new LocalCbtHook.CbtEventHandler(WndActivated);
		}

		#endregion

		#region Properties

		/// <summary>
		/// The string shown in the check box at the bottom of the message box.  A default is normally used to simplify usage and
		/// promote continuity for all message boxes shown.  Read/write.
		/// </summary>
		public static string DNSCheckBoxText
		{
			get
			{
				return _DoNotShowAgainMessage;
			}

			set
			{
				_DoNotShowAgainMessage = value;
			}
		}

		#endregion

		#region Show Functions

		//IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options

		/// <summary>
		/// Show the message box with the specified settings.
		/// </summary>
		/// <param name="owner">The IWin32Window the message box will display in front of.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box.</param>
		/// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
		/// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
		/// <param name="defaultButton">One of the MessageBoxDefaultButton values the specifies the default button for the message box.</param>
		public DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
		{
			bool registryvalue = false;

			// If a registry access function is present, use that to get the registry value.
			if (_registryaccessfunction != null)
			{
				// Get the registry value using the access function.
				_registryaccessfunction(ref registryvalue, false);
			}
			else
			{
				// Get the registry value through the specified key and value.
				RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(_registrykey);
				try
				{
					registryvalue = Convert.ToBoolean(regKey.GetValue(_registryvalue, false));
				}
				catch
				{
					// Ignore exception and proceed using default (false).
				}
			}

			if (registryvalue)
			{
				// Return the default dialog result for the buttons and defaultButton specified.
				return DefaultDialogResult(buttons, defaultButton);
			}


			// Show the message box.
			_cbt.Install();
			DialogResult dialogresult = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton);
			_cbt.Uninstall();

			// Set the registry value and return the result.
			if (_registryaccessfunction != null)
			{
				_registryaccessfunction(ref _boxischecked, true);
			}
			else
			{
				// Get the registry value through the specified key and value.
				RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(_registrykey);
				regKey.SetValue(_registryvalue, _boxischecked);
			}

			return dialogresult;
		}

		/// <summary>
		/// Show the message box with the specified settings.
		/// </summary>
		/// <param name="owner">The IWin32Window the message box will display in front of.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box.</param>
		/// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
		/// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
		public DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return Show(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
		}

		/// <summary>
		/// Show the message box with the specified settings.
		/// </summary>
		/// <param name="owner">The IWin32Window the message box will display in front of.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box.</param>
		/// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
		public DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
		{
			return Show(owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
		}

		/// <summary>
		/// Show the message box with the specified settings.
		/// </summary>
		/// <param name="owner">The IWin32Window the message box will display in front of.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box.</param>
		public DialogResult Show(IWin32Window owner, string text, string caption)
		{
			return Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
		}

		/// <summary>
		/// Show the message box with the specified settings.
		/// </summary>
		/// <param name="owner">The IWin32Window the message box will display in front of.</param>
		/// <param name="text">The text to display in the message box. </param>
		public DialogResult Show(IWin32Window owner, string text)
		{
			return Show(owner, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Window created.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void WndCreated(object sender, CbtEventArgs eventArgs)
		{
			if (eventArgs.IsDialogWindow)
			{
				_bInit = false;
				_hwnd = eventArgs.Handle;
			}
		}

		/// <summary>
		/// Window destroyed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void WndDestroyed(object sender, CbtEventArgs eventArgs)
		{
			if (eventArgs.Handle == _hwnd)
			{
				_bInit = false;
				_hwnd = IntPtr.Zero;

				// Check to see if the check box is checked.
				if (BST_CHECKED == (int)SendMessage(_checkbox, BM_GETCHECK, IntPtr.Zero, IntPtr.Zero))
				{
					_boxischecked = true;
				}
				else
				{
					_boxischecked = false;
				}
			}
		}

		/// <summary>
		/// Window activated.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void WndActivated(object sender, CbtEventArgs eventArgs)
		{
			if (_hwnd != eventArgs.Handle)
			{
				return;
			}

			// Not the first time
			if (_bInit)
			{
				return;
			}
			else
			{
				_bInit = true;
			}

			// Get the current font, either from the static text window
			// or the message box itself.
			IntPtr hFont;
			IntPtr hwndText = GetDlgItem(_hwnd, 0xFFFF);
			if (hwndText != IntPtr.Zero)
			{
				hFont = SendMessage(hwndText, WM_GETFONT, IntPtr.Zero, IntPtr.Zero);
			}
			else
			{
				hFont = SendMessage(_hwnd, WM_GETFONT, IntPtr.Zero, IntPtr.Zero);
			}

			Font fCur = Font.FromHfont(hFont);

			// Get the x coordinate for the check box.  Align it with the icon if possible,
			// or one character height in.
			int x = 0;
			IntPtr hwndIcon = GetDlgItem(_hwnd, 0x0014);
			if (hwndIcon != IntPtr.Zero)
			{
				RECT rcIcon = new RECT();
				GetWindowRect(hwndIcon, rcIcon);
				POINT pt = new POINT();
				pt.x = rcIcon.left;
				pt.y = rcIcon.top;
				ScreenToClient(_hwnd, pt);
				x = pt.x;
			}
			else
			{
				x = (int)fCur.GetHeight();
			}

			// Get the y coordinate for the check box, which is the bottom of the
			// current message box client area.
			RECT rc = new RECT();
			GetClientRect(_hwnd, rc);
			int y = rc.bottom - rc.top;

			// Resize the message box with room for the check box.
			GetWindowRect(_hwnd, rc);
			MoveWindow(_hwnd, rc.left, rc.top, rc.right-rc.left, rc.bottom-rc.top + (int)fCur.GetHeight()*2, true);

			// Create the check box.
			_checkbox = CreateWindowEx(0, "button", _DoNotShowAgainMessage, BS_AUTOCHECKBOX|WS_CHILD|WS_VISIBLE|WS_TABSTOP,
				x, y, rc.right-rc.left-x, (int)fCur.GetHeight(),
				_hwnd, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

			// Set the font of the check box.
			SendMessage(_checkbox, WM_SETFONT, hFont, new IntPtr(1));
		}

		/// <summary>
		/// Determines what the default dialog result should be for a message box with the specified buttons and default button.  Used
		/// when the message box is not displayed because the Do Not Show check box was previously checked.
		/// </summary>
		/// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
		/// <param name="defaultButton"></param>
		private DialogResult DefaultDialogResult(MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
		{
			DialogResult result = DialogResult.OK;

			switch (buttons)
			{
				case MessageBoxButtons.AbortRetryIgnore:
				switch (defaultButton)
				{
					case MessageBoxDefaultButton.Button1:
					result = DialogResult.Abort;
					break;

					case MessageBoxDefaultButton.Button2:
					result = DialogResult.Retry;
					break;

					case MessageBoxDefaultButton.Button3:
					result = DialogResult.Ignore;
					break;
				}
				break;

				case MessageBoxButtons.OK:
				result = DialogResult.OK;
				break;

				case MessageBoxButtons.OKCancel:
				switch (defaultButton)
				{
					case MessageBoxDefaultButton.Button1:
					result = DialogResult.OK;
					break;

					case MessageBoxDefaultButton.Button2:
					result = DialogResult.Cancel;
					break;

					case MessageBoxDefaultButton.Button3:
					result = DialogResult.OK;
					break;
				}
				break;

				case MessageBoxButtons.RetryCancel:
				switch (defaultButton)
				{
					case MessageBoxDefaultButton.Button1:
					result = DialogResult.Retry;
					break;

					case MessageBoxDefaultButton.Button2:
					result = DialogResult.Cancel;
					break;

					case MessageBoxDefaultButton.Button3:
					result = DialogResult.Retry;
					break;
				}
				break;

				case MessageBoxButtons.YesNo:
				switch (defaultButton)
				{
					case MessageBoxDefaultButton.Button1:
					result = DialogResult.Yes;
					break;

					case MessageBoxDefaultButton.Button2:
					result = DialogResult.No;
					break;

					case MessageBoxDefaultButton.Button3:
					result = DialogResult.Yes;
					break;
				}
				break;

				case MessageBoxButtons.YesNoCancel:
				switch (defaultButton)
				{
					case MessageBoxDefaultButton.Button1:
					result = DialogResult.Yes;
					break;

					case MessageBoxDefaultButton.Button2:
					result = DialogResult.No;
					break;

					case MessageBoxDefaultButton.Button3:
					result = DialogResult.Cancel;
					break;
				}
				break;
			} // End switch on buttons shown.

			return result;
		}

		#endregion

		#region Win32 Imports

		private const int WS_VISIBLE		= 0x10000000;
		private const int WS_CHILD			= 0x40000000;
		private const int WS_TABSTOP        = 0x00010000;
		private const int WM_SETFONT		= 0x00000030;
		private const int WM_GETFONT		= 0x00000031;
		private const int BS_AUTOCHECKBOX	= 0x00000003;
		private const int BM_GETCHECK       = 0x00F0;
		private const int BST_CHECKED       = 0x0001;

		/// <summary>
		/// Destroy a windows.  Imported from user32.dll.
		/// </summary>
		/// <param name="hwnd">Windows handle of window.</param>
		[DllImport("user32.dll")]
		protected static extern void DestroyWindow(IntPtr hwnd);

		/// <summary>
		/// Get an item on a dialog.  Imported from user32.dll.
		/// </summary>
		/// <param name="hwnd">Windows handle of window.</param>
		/// <param name="id">ID of dialog item.</param>
		[DllImport("user32.dll")]
		protected static extern IntPtr GetDlgItem(IntPtr hwnd, int id);

		/// <summary>
		/// Get rectangle of the window.  Imported from user32.dll.
		/// </summary>
		/// <param name="hwnd">Windows handle of window.</param>
		/// <param name="rc">Rectangle to store info in.</param>
		[DllImport("user32.dll")]
		protected static extern int GetWindowRect(IntPtr hwnd, RECT rc);

		/// <summary>
		/// Get rectangle of client.  Imported from user32.dll.
		/// </summary>
		/// <param name="hwnd">Windows handle of window.</param>
		/// <param name="rc">Rectangle to store info in.</param>
		[DllImport("user32.dll")]
		protected static extern int GetClientRect(IntPtr hwnd, RECT rc);

		/// <summary>
		/// Move window.  Imported from user32.dll.
		/// </summary>
		/// <param name="hwnd">Windows handle of window.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="nWidth"></param>
		/// <param name="nHeight"></param>
		/// <param name="bRepaint"></param>
		[DllImport("user32.dll")]
		protected static extern void MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

		/// <summary>
		/// Convert a screen point to a client point.  Imported from user32.dll.
		/// </summary>
		/// <param name="hwnd">Windows handle of window.</param>
		/// <param name="pt">Point to convert.</param>
		[DllImport("user32.dll")]
		protected static extern int ScreenToClient(IntPtr hwnd, POINT pt);

		/// <summary>
		/// Windows message box.  Imported from user32.dll.
		/// </summary>
		/// <param name="hwnd">Windows handle of window.</param>
		/// <param name="text">Text.</param>
		/// <param name="caption">Caption.</param>
		/// <param name="options">Options.</param>
		[DllImport("user32.dll", EntryPoint="MessageBox")]
		protected static extern int _MessageBox(IntPtr hwnd, string text, string caption, int options);

		/// <summary>
		/// Send a message to a window.  Imported from user32.dll.
		/// </summary>
		/// <param name="hwnd">Windows handle of window.</param>
		/// <param name="msg">Message to send.</param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		[DllImport("user32.dll")]
		protected static extern IntPtr SendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// Create a window.  Imported from user32.dll.  Returns a Windows handle to created window.
		/// </summary>
		/// <param name="dwExStyle">Extended window style.</param>
		/// <param name="lpClassName">Registered class name.</param>
		/// <param name="lpWindowName">Window name.</param>
		/// <param name="dwStyle">Window style.</param>
		/// <param name="x">Horizontal position of window.</param>
		/// <param name="y">Vertical position of window.</param>
		/// <param name="nWidth">Window width.</param>
		/// <param name="nHeight">Window height.</param>
		/// <param name="hWndParent">Handle to parent or owner window.</param>
		/// <param name="hMenu">Menu handle or child identifier.</param>
		/// <param name="hInstance">Handle to application instance.</param>
		/// <param name="lpParam">Window-creation data.</param>
		[DllImport("user32.dll")]
		protected static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName,
			int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu,
			IntPtr hInstance, IntPtr lpParam
		);

		/// <summary>
		/// Point.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			/// <summary>X position of window.</summary>
			public int x;

			/// <summary>Y position of window.</summary>
			public int y;
		}

		/// <summary>
		/// Rectangle.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public class RECT
		{
			/// <summary>Left position of window.</summary>
			public int left;

			/// <summary>Top position of window.</summary>
			public int top;

			/// <summary>Right position of window.</summary>
			public int right;

			/// <summary>Bottom position of window.</summary>
			public int bottom;
		}

		#endregion

	} // End class.
} // End namespace.