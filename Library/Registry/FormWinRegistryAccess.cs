using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;
using DigitalProduction.Forms;

namespace DigitalProduction.WinRegistry
{
	#region Form position.

	/// <summary>
	/// Enumeration for the position of the form.
	/// </summary>
	public enum FormPosition
	{
		/// <summary>Position of left side of form.</summary>
		Left,

		/// <summary>Position of the top of the form.</summary>
		Top,

		/// <summary>Width of the form.</summary>
		Width,

		/// <summary>Height of the form.</summary>
		Height
	};

	#endregion

	/// <summary>
	/// WinRegistryAccess Class. A generic registry access to read and write to a Windows registry.
	/// It does common tasks for all application, such as get the CompanyKey and the ApplicationKey.
	/// A specific application should derive it's own registry writer from this to save any other
	/// specific data.
	/// 
	/// Note that the functions in this class cannot be static because most of them depend on the specific
	/// _owner of this instance.  Since this class is to be general enough to be used for all DPM applications
	/// the type of the owner is not know before hand.  Moreover, several instances of this class could
	/// exist at one time, each working for a different type of application.
	/// 
	/// This class supports multiple levels of access to the registry.  That is, this class can be used
	/// to access a set of registry keys and values for a form that is owned by another form.  If this
	/// WinRegistryAccess has a parent it requests the AppKey from it's parent.  If that WinRegistryAccess
	/// has a parent it does the same, and so on.
	/// </summary>
	public class FormWinRegistryAccess : WinRegistryAccess
	{
		#region Members / Variables / Delagates.

		/// <summary>
		/// Used to create a list of entries such that they can all be easily read or written at
		/// one time if needed.
		/// </summary>
		protected ArrayList							_registryentries			= new ArrayList();

		/// <summary>
		/// Digital Production Management base Form.  This allows commonality amongst all DPM applications.
		/// </summary>
		private readonly DPMForm					_owner;

		/// <summary>
		/// This can be used if the form using this registry access is a child of another form.
		/// This enables drilling down many levels in the registry when dialogs have children dialogs.
		/// </summary>
		private readonly FormWinRegistryAccess		_owneraccess;

		private readonly bool						_ischild;

		#endregion

		#region Construction / Destruction / Install.

		/// <summary>
		/// Constructor when the dialog box that is the owner is the top level dialog box.
		/// </summary>
		/// <param name="owner">DPMForm that is using this to access to the registry.</param>
		public FormWinRegistryAccess(DPMForm owner)
			: base(owner.DPMCompanyName, owner.AppName)
		{
			_owner			= owner;
			_owner.Install	+= new DPMForm.InstallDelegate(Install);
			_ischild		= false;
		}

		/// <summary>
		/// Constructor to be used when the dialog box that is using this to access the registry
		/// is owned by another DPMForm.
		/// </summary>
		/// <param name="owner">DPMForm that is using this to access to the registry.</param>
		/// <param name="ownerregaccess">WinRegistryAccess used by the DPMForm that is the owner of the form using this WinRegistyrAccess.</param>
		public FormWinRegistryAccess(DPMForm owner, FormWinRegistryAccess ownerregaccess)
			: base(owner.DPMCompanyName, owner.AppName)
		{
			_owner			= owner;
			_owner.Install	+= new DPMForm.InstallDelegate(Install);
			_owneraccess	= ownerregaccess;
			_ischild		= true;
		}

		/// <summary>
		/// Install function used by the delegate to do installation work.  Primarily used for debugging a setup
		/// routine should handle normal installation.
		/// </summary>
		// Note that install cannot be static because the application key is dependent on the specific
		// application that is using an instance of this class.
		private void Install()
		{
			RegistryKey appkey = AppKey();

			// Only write keys I am directly in charge of.
			if (appkey != null)
			{
				appkey.DeleteSubKeyTree("Recent Files", false);
			}
		}

		#endregion

		#region General properties.

		/// <summary>
		/// Get the owner of this registry access.
		/// </summary>
		public DPMForm Owner
		{
			get
			{
				return _owner;
			}
		}

		/// <summary>
		/// Get the registry access of the owner if available.
		/// </summary>
		public WinRegistryAccess OwnerRegAccess
		{
			get
			{
				return _owneraccess;
			}
		}

		#endregion

		#region Window state access.

		/// <summary>
		/// Return the key that holds window state information.
		/// </summary>
		/// <returns>Returns the registry key if it could be accessed, null if an error occurs.</returns>
		protected RegistryKey WindowStateKey()
		{
			RegistryKey regkey;

			try
			{
				RegistryKey appkey = AppKey();

				if (appkey == null)
				{
					return null;
				}

				// Open the Window State key.
				regkey = appkey.CreateSubKey("Window State");
			}
			catch
			{
				return null;
			}

			return regkey;
		}

		/// <value>
		/// Gets or sets the window position information from the registry.  Takes an array of
		/// four integers which represent the left, top, width, and height of the window.
		/// </value>
		public int[] WindowPosition
		{
			get
			{
				// Attempt to restore the position from the registry.
				RegistryKey regkey = WindowStateKey();

				int[] pos = new int[4];

				if (regkey == null)
				{
					pos[(int)FormPosition.Left]		= _owner.Left;
					pos[(int)FormPosition.Top]		= _owner.Top;
					pos[(int)FormPosition.Width]	= _owner.Width;
					pos[(int)FormPosition.Height]	= _owner.Height;
				}
				else
				{
					pos[(int)FormPosition.Left]		= System.Convert.ToInt32(regkey.GetValue("Left", _owner.Left));
					pos[(int)FormPosition.Top]		= System.Convert.ToInt32(regkey.GetValue("Top", _owner.Top));
					pos[(int)FormPosition.Width]	= System.Convert.ToInt32(regkey.GetValue("Width", _owner.Width));
					pos[(int)FormPosition.Height]	= System.Convert.ToInt32(regkey.GetValue("Height", _owner.Height));
				}

				return pos;
			}

			set
			{
				RegistryKey regkey = WindowStateKey();

				if (regkey != null)
				{
					regkey.SetValue("Left", value.GetValue((int)FormPosition.Left));
					regkey.SetValue("Top", value.GetValue((int)FormPosition.Top));
					regkey.SetValue("Width", value.GetValue((int)FormPosition.Width));
					regkey.SetValue("Height", value.GetValue((int)FormPosition.Height));
				}
			}
		}

		/// <value>
		/// Gets or sets the window state information from the registry.
		/// </value>
		public FormWindowState WindowState
		{
			get
			{
				RegistryKey regkey = WindowStateKey();

				if (regkey == null)
				{
					return FormWindowState.Maximized;
				}
				else
				{
					return (FormWindowState)regkey.GetValue("Window State", FormWindowState.Normal);
				}
			}

			set
			{
				RegistryKey regkey = WindowStateKey();

				if (regkey != null)
				{
					// Have to save an int to the registry otherwise a string gets written and
					// it is a lot easier to get a FormWindowState (enum) from an int than it
					// is from the string.  Plus, this is more flexible if Microsoft decides to add
					// more (or remove some) Window States.
					regkey.SetValue("Window State", System.Convert.ToInt32(value));
				}
			}
		}

		#endregion

		#region Recently used files.

		/// <summary>
		/// Return the key that holds recently used files.
		/// </summary>
		/// <returns>Returns the registry key if it could be accessed, null if an error occurs.</returns>
		protected RegistryKey RecentFilesKey()
		{
			RegistryKey regkey;

			try
			{
				RegistryKey appkey = AppKey();

				if (appkey == null)
				{
					return null;
				}

				// Open the Window State key.
				regkey = appkey.CreateSubKey("Recent Files");
			}
			catch
			{
				return null;
			}

			return regkey;
		}


		/// <value>
		/// Gets or sets the size of the recently used menus from the registry.  This is the number of menu
		/// items dispalyed, not the number of menu items allowed.
		/// </value>
		public uint NumberOfRecentlyUsedFiles
		{
			get
			{
				RegistryKey regkey = RecentFilesKey();

				if (regkey == null)
				{
					return 5;
				}
				else
				{
					return Convert.ToUInt32(regkey.GetValue("Size", 5));
				}
			}

			set
			{
				RegistryKey regkey = RecentFilesKey();

				if (regkey != null)
				{
					regkey.SetValue("Size", value);
				}
			}
		}

		/// <summary>
		/// Gets the recently used files from the registry.
		/// </summary>
		/// <param name="numberoffiles">The number of strings to return.  If an entry is not found for a file, a blank string is returned.</param>
		/// <returns>An array of strings the size of "numberoffiles".  Blank strings are returned for any entries that do not exist.</returns>
		public string[] GetRecentlyUsedFiles(uint numberoffiles)
		{
			RegistryKey regkey = RecentFilesKey();

			if (regkey == null)
			{
				return new string[1] { "" };
			}
			else
			{
				string[] files = new string[numberoffiles];

				for (int i = 0; i < numberoffiles; i++)
				{
					files[i] = (string)regkey.GetValue("File " + (i+1).ToString(), "");
				}

				return files;
			}
		}

		/// <value>
		/// Sets the the recently used menus from the registry.
		/// </value>
		public void SetRecentlyUsedFiles(string[] files)
		{
			RegistryKey regkey = RecentFilesKey();

			if (regkey == null)
			{
				return;
			}
			else
			{
				int numberoffiles = files.Length;
				for (int i = 0; i < numberoffiles; i++)
				{
					regkey.SetValue("File " + (i+1).ToString(), files[i]);
				}
			}
		}

		#endregion

	} // End class.
} // End namespace.