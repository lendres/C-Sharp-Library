#pragma warning disable 0618
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, UnmanagedCode = true)]
namespace System.Windows.Forms
{
	/// <summary>
	/// A class for changing the default text on a message box.
	/// </summary>
	public class MessageBoxManager
	{
		#region Enumerations

		/// <summary>
		/// The buttons on a message box are numbered from 1, not zero, so we start counting from 1 in this enumeration.
		/// </summary>
		enum Button
		{
			OK			= 1,
			Cancel,
			Abort,
			Retry,
			Ignore,
			Yes,
			No,
			Size,
		}

		#endregion

		#region DLL imports.

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

		[DllImport("user32.dll")]
		private static extern IntPtr SetWindowsHookEx(int idhook, HookProc lpfn, IntPtr hinstance, int threadid);

		[DllImport("user32.dll")]
		private static extern int UnhookWindowsHookEx(IntPtr idhook);

		[DllImport("user32.dll")]
		private static extern IntPtr CallNextHookEx(IntPtr idhook, int ncode, IntPtr wparam, IntPtr lparam);

		[DllImport("user32.dll", EntryPoint = "GetWindowTextLengthW", CharSet = CharSet.Unicode)]
		private static extern int GetWindowTextLength(IntPtr hwnd);

		[DllImport("user32.dll", EntryPoint = "GetWindowTextW", CharSet = CharSet.Unicode)]
		private static extern int GetWindowText(IntPtr hwnd, StringBuilder text, int maxLength);

		[DllImport("user32.dll")]
		private static extern int EndDialog(IntPtr hdlg, IntPtr nresult);

		[DllImport("user32.dll")]
		private static extern bool EnumChildWindows(IntPtr hwndparent, EnumChildProc lpenumfunc, IntPtr lparam);

		[DllImport("user32.dll", EntryPoint = "GetClassNameW", CharSet = CharSet.Unicode)]
		private static extern int GetClassName(IntPtr hwnd, StringBuilder lpclassname, int nmaxcount);

		[DllImport("user32.dll")]
		private static extern int GetDlgCtrlID(IntPtr hwndCtl);

		[DllImport("user32.dll")]
		private static extern IntPtr GetDlgItem(IntPtr hDlg, int niddlgitem);

		[DllImport("user32.dll", EntryPoint = "SetWindowTextW", CharSet = CharSet.Unicode)]
		private static extern bool SetWindowText(IntPtr hwnd, string lpstring);

		#endregion

		#region Structures.

		/// <summary>
		/// Defines the message parameters passed to a WH_CALLWNDPROCRET hook procedure,
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct CWPRETSTRUCT
		{
			/// <summary>The return value of the window procedure that processed the message specified by the message value.</summary>
			public IntPtr lresult;

			/// <summary>Additional information about the message. The exact meaning depends on the message value.</summary>
			public IntPtr lparam;

			/// <summary>Additional information about the message. The exact meaning depends on the message value.</summary>
			public IntPtr wparam;

			/// <summary>The message. </summary>
			public uint   message;

			/// <summary>A handle to the window that processed the message specified by the message value.</summary>
			public IntPtr hwnd;
		};

		#endregion

		#region Members/Delegates.

		private delegate IntPtr HookProc(int ncode, IntPtr wparam, IntPtr lparam);
		private delegate bool EnumChildProc(IntPtr hwnd, IntPtr lparam);

		private const int		WH_CALLWNDPROCRET			= 12;
		private const int		WM_DESTROY					= 0x0002;
		private const int		WM_INITDIALOG				= 0x0110;
		private const int		WM_TIMER					= 0x0113;
		private const int		WM_USER						= 0x400;
		private const int		DM_GETDEFID					= WM_USER + 0;

		private static HookProc			_hookproc;
		private static EnumChildProc	_enumproc;

		[ThreadStatic]
		private static IntPtr			_hhook;

		[ThreadStatic]
		private static int				_buttonnumber;

		// The first entry is not used, we just put in a spacing so the array matches the enumeration.
		private static string[]			_buttontexts		= new string[(int)Button.Size] {"Not Used", "&OK", "&Cancel", "&Abort", "&Retry", "&Ignore", "&Yes", "&No"};

		#endregion

		#region Constructor.

		static MessageBoxManager()
		{
			_hookproc = new HookProc(MessageBoxHookProc);
			_enumproc = new EnumChildProc(MessageBoxEnumProc);
			_hhook = IntPtr.Zero;
		}

		#endregion

		#region Properties.

		/// <summary>
		/// OK text.
		/// </summary>
		public static string OK
		{
			get
			{
				return MessageBoxManager._buttontexts[(int)Button.OK];
			}
			set
			{
				MessageBoxManager._buttontexts[(int)Button.OK] = value;
			}
		}

		/// <summary>
		/// Cancel text.
		/// </summary>
		public static string Cancel
		{
			get
			{
				return MessageBoxManager._buttontexts[(int)Button.Cancel];
			}
			set
			{
				MessageBoxManager._buttontexts[(int)Button.Cancel] = value;
			}
		}

		/// <summary>
		/// Abort text.
		/// </summary>
		public static string Abort
		{
			get
			{
				return MessageBoxManager._buttontexts[(int)Button.Abort];
			}
			set
			{
				MessageBoxManager._buttontexts[(int)Button.Abort] = value;
			}
		}

		/// <summary>
		/// Retry text.
		/// </summary>
		public static string Retry
		{
			get
			{
				return MessageBoxManager._buttontexts[(int)Button.Retry];
			}
			set
			{
				MessageBoxManager._buttontexts[(int)Button.Retry] = value;
			}
		}

		/// <summary>
		/// Ignore text.
		/// </summary>
		public static string Ignore
		{
			get
			{
				return MessageBoxManager._buttontexts[(int)Button.Ignore];
			}
			set
			{
				MessageBoxManager._buttontexts[(int)Button.Ignore] = value;
			}
		}

		/// <summary>
		/// Yes text.
		/// </summary>
		public static string Yes
		{
			get
			{
				return MessageBoxManager._buttontexts[(int)Button.Yes];
			}
			set
			{
				MessageBoxManager._buttontexts[(int)Button.Yes] = value;
			}
		}

		/// <summary>
		/// No text.
		/// </summary>
		public static string No
		{
			get
			{
				return MessageBoxManager._buttontexts[(int)Button.No];
			}
			set
			{
				MessageBoxManager._buttontexts[(int)Button.No] = value;
			}
		}

		#endregion

		#region Register/Deregister.

		/// <summary>
		/// Enables MessageBoxManager functionality
		/// </summary>
		/// <remarks>
		/// MessageBoxManager functionality is enabled on current thread only.
		/// Each thread that needs MessageBoxManager functionality has to call this method.
		/// </remarks>
		public static void Register()
		{
			if (_hhook != IntPtr.Zero)
			{
				throw new NotSupportedException("One hook per thread allowed.");
			}
			_hhook = SetWindowsHookEx(WH_CALLWNDPROCRET, _hookproc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
		}

		/// <summary>
		/// Disables MessageBoxManager functionality
		/// </summary>
		/// <remarks>
		/// Disables MessageBoxManager functionality on current thread only.
		/// </remarks>
		public static void Unregister()
		{
			if (_hhook != IntPtr.Zero)
			{
				UnhookWindowsHookEx(_hhook);
				_hhook = IntPtr.Zero;
			}
		}

		#endregion

		#region Functions.

		private static IntPtr MessageBoxHookProc(int ncode, IntPtr wparam, IntPtr lparam)
		{
			if (ncode < 0)
			{
				return CallNextHookEx(_hhook, ncode, wparam, lparam);
			}

			CWPRETSTRUCT msg	= (CWPRETSTRUCT)Marshal.PtrToStructure(lparam, typeof(CWPRETSTRUCT));
			IntPtr hook			= _hhook;

			if (msg.message == WM_INITDIALOG)
			{
				int windowtextlength	= GetWindowTextLength(msg.hwnd);
				StringBuilder classname	= new StringBuilder(10);

				GetClassName(msg.hwnd, classname, classname.Capacity);
				if (classname.ToString() == "#32770")
				{
					_buttonnumber = 0;
					EnumChildWindows(msg.hwnd, _enumproc, IntPtr.Zero);
					if (_buttonnumber == 1)
					{
						IntPtr hbutton = GetDlgItem(msg.hwnd, (int)Button.Cancel);
						if (hbutton != IntPtr.Zero)
						{
							SetWindowText(hbutton, OK);
						}
					}
				}
			}

			return CallNextHookEx(hook, ncode, wparam, lparam);
		}

		private static bool MessageBoxEnumProc(IntPtr hwnd, IntPtr lparam)
		{
			StringBuilder classname = new StringBuilder(10);
			GetClassName(hwnd, classname, classname.Capacity);

			if (classname.ToString() == "Button")
			{
				Button controlid = (Button)GetDlgCtrlID(hwnd);
				switch (controlid)
				{
					case Button.OK:
					{
						SetWindowText(hwnd, OK);
						break;
					}
					case Button.Cancel:
					{
						SetWindowText(hwnd, Cancel);
						break;
					}
					case Button.Abort:
					{
						SetWindowText(hwnd, Abort);
						break;
					}
					case Button.Retry:
					{
						SetWindowText(hwnd, Retry);
						break;
					}
					case Button.Ignore:
					{
						SetWindowText(hwnd, Ignore);
						break;
					}
					case Button.Yes:
					{
						SetWindowText(hwnd, Yes);
						break;
					}
					case Button.No:
					{
						SetWindowText(hwnd, No);
						break;
					}

				}
				_buttonnumber++;
			}

			return true;
		}

		#endregion

	} // End class.
} // End namespace.