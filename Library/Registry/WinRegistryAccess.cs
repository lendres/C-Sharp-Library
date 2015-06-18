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
	public class WinRegistryAccess
	{
		#region Members / Variables / Delagates.

		/// <summary>
		/// Used to create a list of entries such that they can all be easily read or written at
		/// one time if needed.
		/// </summary>
		protected ArrayList						_registryentries			= new ArrayList();

		/// <summary>
		/// Digital Production Management base Form.  This allows commonality amongst all DPM applications.
		/// </summary>
		private readonly DPMForm				_owner;

		/// <summary>
		/// This can be used if the form using this registry access is a child of another form.
		/// This enables drilling down many levels in the registry when dialogs have children dialogs.
		/// </summary>
 		private readonly WinRegistryAccess		_owneraccess;

		private readonly bool					_ischild;

		#endregion

		#region Construction / Destruction / Install.

		/// <summary>
		/// Constructor when the dialog box that is the owner is the top level dialog box.
		/// </summary>
		/// <param name="owner">DPMForm that is using this to access to the registry.</param>
		public WinRegistryAccess(DPMForm owner)
		{
			_owner			= owner;
			_owner.Install	+= new DPMForm.InstallDelegate(Install);
			_ischild		= false;

			CreateRegistryEntries();
			ReadRegistryEntries();
		}

		/// <summary>
		/// Constructor to be used when the dialog box that is using this to access the registry
		/// is owned by another DPMForm.
		/// </summary>
		/// <param name="owner">DPMForm that is using this to access to the registry.</param>
		/// <param name="ownerregaccess">WinRegistryAccess used by the DPMForm that is the owner of the form using this WinRegistyrAccess.</param>
		public WinRegistryAccess(DPMForm owner, WinRegistryAccess ownerregaccess)
		{
			_owner			= owner;
			_owner.Install	+= new DPMForm.InstallDelegate(Install);
			_owneraccess	= ownerregaccess;
			_ischild		= true;

			CreateRegistryEntries();
			ReadRegistryEntries();
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
				appkey.SetValue("Installed", true);
				appkey.DeleteSubKeyTree("Recent Files", false);
			}
		}

		#endregion

		#region Virtual functions, derived classes should override these.

		/// <summary>
		/// Default creation of registy entries.  Derived classes should override this to
		/// create there own entries.
		/// </summary>
		protected virtual void CreateRegistryEntries() {}

		#endregion

		#region Registry entries functions.

		/// <summary>
		/// Write all the registry entries stored in the registry entries array list.
		/// </summary>
		protected void WriteRegistryEntries()
		{
			for (int i = 0; i < _registryentries.Count; i++)
			{
				RegistryEntry regent = (RegistryEntry)_registryentries[i];

				SetValue(regent.RegKeyFunction(), regent.Name, regent.Value);
			}
		}

		/// <summary>
		/// Read all the registry entries stored in the registry entries array list.
		/// </summary>
		protected void ReadRegistryEntries()
		{
			for (int i = 0; i < _registryentries.Count; i++)
			{
				RegistryEntry regent = (RegistryEntry)_registryentries[i];
				regent.Value = GetValue(regent.RegKeyFunction(), regent.Name, regent.Value);
			}
		}

		/// <summary>
		/// Get a registry entry from the array.
		/// </summary>
		/// <param name="index">index of the registry entry desired.</param>
		/// <returns>The registry entry associated with index if possible, otherwise null.</returns>
		protected RegistryEntry GetRegEntry(int index)
		{
			if (index > _registryentries.Count)
			{
				return null;
			}
			return (RegistryEntry)_registryentries[index];
		}

		/// <summary>
		/// Get a value from the registry entries stored in arraylist and return
		/// as a boolean.
		/// </summary>
		/// <param name="index">Index in arraylist that entry is stored at.</param>
		/// <returns>The value of the registry entry as a boolean.</returns>
		public bool GetValueAsBoolean(int index)
		{
			RegistryEntry regentry = GetRegEntry(index);

			return System.Convert.ToBoolean(regentry.Value);
		}

		/// <summary>
		/// Get a value from the registry entries stored in arraylist and return
		/// as an int.
		/// </summary>
		/// <param name="index">Index in arraylist that entry is stored at.</param>
		/// <returns>The value of the registry entry as an int.</returns>
		public int GetValueAsInt32(int index)
		{
			RegistryEntry regentry = GetRegEntry(index);

			return System.Convert.ToInt32(regentry.Value);
		}

		/// <summary>
		/// Get a value from the registry entries stored in arraylist and return
		/// as a string.
		/// </summary>
		/// <param name="index">Index in arraylist that entry is stored at.</param>
		/// <returns>The value of the registry entry as a string.</returns>
		public string GetValueAsString(int index)
		{
			RegistryEntry regentry = GetRegEntry(index);

			return System.Convert.ToString(regentry.Value);
		}

		/// <summary>
		/// Get a value from the registry entries stored in arraylist and return
		/// as a double.
		/// </summary>
		/// <param name="index">Index in arraylist that entry is stored at.</param>
		/// <returns>The value of the registry entry as a double.</returns>
		public double GetValueAsDouble(int index)
		{
			RegistryEntry regentry = GetRegEntry(index);

			return System.Convert.ToDouble(regentry.Value);
		}

		/// <summary>
		/// Sets the value of a registry entry stored in the arraylist.
		/// </summary>
		/// <param name="index">Index in arraylist that entry is stored at.</param>
		/// <param name="setvalue">Value to set.</param>
		public void SetValue(int index, object setvalue)
		{
			RegistryEntry regentry = GetRegEntry(index);

			regentry.Value = setvalue;
			RegistryKey key = regentry.RegKeyFunction();
			
			if (key != null)
			{
				key.SetValue(regentry.Name, setvalue);
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

		/// <value>
		/// Checks the registry key to see if the installed key has been set to true.  This
		/// is primarily used to check to see if Install needs to be run when doing debugging.
		/// </value>
		public bool Installed
		{
			get
			{
				RegistryKey appkey = AppKey();

				if (appkey != null)
				{
					return System.Convert.ToBoolean(appkey.GetValue("Installed", false));
				}
				else
				{
					return false;
				}
			}
		}

		#endregion

		#region Company and application name access.

		/// <summary>
		/// Get the registry key associated with the company name.
		/// </summary>
		/// <returns>Returns the registry key if it could be accessed, null if an error occurs.</returns>
		protected RegistryKey CompanyKey()
		{
			RegistryKey companykey;

			try
			{
				// Have to open all registry keys that are going to be written too (or a subkey is going
				// to be written too) with CreateSubKey.  I.e. cannot do 
				// Registry.CurrentUser.OpenSubKey("Software").CreateSubKey("Digital Production Management") because "Software" will
				// be opened read only.
				RegistryKey cUserSoftware = Registry.CurrentUser.CreateSubKey("Software");

				// Open the company key.
				companykey = cUserSoftware.CreateSubKey(_owner.DPMCompanyName);
			}
			catch
			{
				return null;
			}

			return companykey;
		}

		/// <summary>
		/// Get the registry key associated with the application name.
		/// </summary>
		/// <returns>Returns the registry key if it could be accessed, null if an error occurs.</returns>
		protected virtual RegistryKey AppKey()
		{
			RegistryKey appkey;

			try
			{
				RegistryKey companykey = CompanyKey();

				if (companykey == null)
				{
					return null;
				}

				if (_ischild)
				{
					appkey = _owneraccess.AppKey();

					if (appkey != null)
					{
						appkey = appkey.CreateSubKey("Dialogs");
						appkey = appkey.CreateSubKey(_owner.AppName);
					}
				}
				else
				{
					// Open the app key.
					appkey = companykey.CreateSubKey(_owner.AppName);
				}
			}
			catch
			{
				return null;
			}

			return appkey;
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

				if(regkey == null)
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
				return new string[1] {""};
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

		#region General access to values.

		/// <summary>
		/// Get a registry value.
		/// </summary>
		/// <param name="key">Registry key that the value is located in.</param>
		/// <param name="valuename">Name of the value to get.</param>
		/// <param name="defaultvalue">Default value to use.</param>
		/// <returns>Returns the value stored in registry value if it exists, otherwise return the default value.</returns>
		public bool GetValue(RegistryKey key, string valuename, bool defaultvalue)
		{
			if (key == null)
			{
				return defaultvalue;
			}

			return System.Convert.ToBoolean(key.GetValue(valuename, defaultvalue));
		}

		/// <summary>
		/// Get a registry value.
		/// </summary>
		/// <param name="key">Registry key that the value is located in.</param>
		/// <param name="valuename">Name of the value to get.</param>
		/// <param name="defaultvalue">Default value to use.</param>
		/// <returns>Returns the value stored in registry value if it exists, otherwise return the default value.</returns>
		public int GetValue(RegistryKey key, string valuename, int defaultvalue)
		{
			if (key == null)
			{
				return defaultvalue;
			}

			return System.Convert.ToInt32(key.GetValue(valuename, defaultvalue));
		}

		/// <summary>
		/// Get a registry value.
		/// </summary>
		/// <param name="key">Registry key that the value is located in.</param>
		/// <param name="valuename">Name of the value to get.</param>
		/// <param name="defaultvalue">Default value to use.</param>
		/// <returns>Returns the value stored in registry value if it exists, otherwise return the default value.</returns>
		public object GetValue(RegistryKey key, string valuename, object defaultvalue)
		{
			if (key == null)
			{
				return defaultvalue;
			}

			return key.GetValue(valuename, defaultvalue);
		}

		/// <summary>
		/// Set a registry value.
		/// </summary>
		/// <param name="key">Registry key that the value is located in.</param>
		/// <param name="valuename">Name of the value to get.</param>
		/// <param name="setvalue">Value to set the registry value to.</param>
		public void SetValue(RegistryKey key, string valuename, object setvalue)
		{
			if (key != null)
			{
				key.SetValue(valuename, setvalue);
			}
		}

		/// <summary>
		/// Get a registry value.
		/// </summary>
		/// <param name="key">Registry key that the value is located in.</param>
		/// <param name="valuename">Name of the value to get.</param>
		/// <param name="defaultvalue">Default value to use.</param>
		/// <returns>Returns the value stored in registry value if it exists, otherwise return the default value.</returns>
		public double GetValue(RegistryKey key, string valuename, double defaultvalue)
		{
			if (key == null)
			{
				return defaultvalue;
			}

			return System.Convert.ToDouble(key.GetValue(valuename, defaultvalue));
		}

		/// <summary>
		/// Get a registry value.
		/// </summary>
		/// <param name="key">Registry key that the value is located in.</param>
		/// <param name="valuename">Name of the value to get.</param>
		/// <param name="defaultvalue">Default value to use.</param>
		/// <returns>Returns the value stored in registry value if it exists, otherwise return the default value.</returns>
		public string GetValue(RegistryKey key, string valuename, string defaultvalue)
		{
			if (key == null)
			{
				return defaultvalue;
			}

			return System.Convert.ToString(key.GetValue(valuename, defaultvalue));
		}

		#endregion

	} // End class.
} // End namespace.