using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;
using DigitalProduction.Forms;

namespace DigitalProduction.Registry
{
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
		#region Events

		/// <summary>
		/// Install event.
		/// </summary>
		public event InstallEventHandler				Install;

		#endregion

		#region Members

		/// <summary>Used to create a list of entries such that they can all be easily read or written at one time if needed.</summary>
		protected ArrayList								_registryEntries			= new ArrayList();

		/// <summary>Company name.</summary>
		protected string								_companyName;

		/// <summary>Software/application name.</summary>
		protected string								_softwareName;

		#endregion

		#region Construction and Installation

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="companyName">Company name used to access the company registry key.</param>
		/// <param name="softwareName">Software name used to access the software registry key.</param>
		public WinRegistryAccess(string companyName, string softwareName)
		{
			Initialize(companyName, softwareName);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public WinRegistryAccess(WinRegistryAccess winRegistryAcess)
		{
			Initialize(winRegistryAcess._companyName, winRegistryAcess._softwareName);
		}

		/// <summary>
		/// Constructor when the dialog box that is the owner is the top level dialog box.
		/// </summary>
		/// <param name="companyName">Company name used to access the company registry key.</param>
		/// <param name="softwareName">Software name used to access the software registry key.</param>
		/// <param name="installHandler">Installation handler to add to the Install event.</param>
		public WinRegistryAccess(string companyName, string softwareName, InstallEventHandler installHandler)
		{
			Initialize(companyName, softwareName);

			this.Install += installHandler;
		}

		/// <summary>
		/// Common construction code.
		/// </summary>
		/// <param name="companyName">Company name used to access the company registry key.</param>
		/// <param name="softwareName">Software name used to access the software registry key.</param>
		private void Initialize(string companyName, string softwareName)
		{
			_companyName		= companyName;
			_softwareName		= softwareName;

			CreateRegistryEntries();
			ReadRegistryEntries();

			this.Install += this.OnInstall;
		}

		/// <summary>
		/// Install function used by the delegate to do installation work.  Primarily used for debugging a setup
		/// routine should handle normal installation.
		/// </summary>
		// Note that install cannot be static because the application key is dependent on the specific
		// application that is using an instance of this class.
		private void OnInstall()
		{
			RegistryKey appkey = AppKey();

			// Only write keys I am directly in charge of.
			if (appkey != null)
			{
				appkey.SetValue("Installed", true);
			}
		}

		#endregion

		#region Virtual Functions - Derived Classes Should Override These.

		/// <summary>
		/// Default creation of registry entries.  Derived classes should override this to
		/// create there own entries.
		/// </summary>
		protected virtual void CreateRegistryEntries()
		{
		}

		#endregion

		#region Registry Entry Functions

		/// <summary>
		/// Write all the registry entries stored in the registry entries array list.
		/// </summary>
		protected void WriteRegistryEntries()
		{
			for (int i = 0; i < _registryEntries.Count; i++)
			{
				RegistryEntry regent = (RegistryEntry)_registryEntries[i];

				SetValue(regent.RegKeyFunction(), regent.Name, regent.Value);
			}
		}

		/// <summary>
		/// Read all the registry entries stored in the registry entries array list.
		/// </summary>
		protected void ReadRegistryEntries()
		{
			for (int i = 0; i < _registryEntries.Count; i++)
			{
				RegistryEntry regent = (RegistryEntry)_registryEntries[i];
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
			if (index > _registryEntries.Count)
			{
				return null;
			}
			return (RegistryEntry)_registryEntries[index];
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

		#region General Properties

		/// <value>
		/// Checks the registry key to see if the installed key has been set to true.  This
		/// is primarily used to check to see if Install needs to be run when doing debugging.
		/// </value>
		public bool Installed
		{
			get
			{
				return GetValue(AppKey(), "Installed", false);
			}
		}

		#endregion

		#region Company and Application Name Access

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
				RegistryKey userSoftwareKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software");

				// Open the company key.
				companykey = userSoftwareKey.CreateSubKey(_companyName);
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

				// Open the app key.
				appkey = companykey.CreateSubKey(_softwareName);
			}
			catch
			{
				return null;
			}

			return appkey;
		}

		#endregion

		#region General Access to Values

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

		/// <summary>
		/// Get a registry value.
		/// </summary>
		/// <param name="key">Registry key that the value is located in.</param>
		/// <param name="valuename">Name of the value to get.</param>
		/// <param name="defaultvalue">Default value to use.</param>
		/// <returns>Returns the value stored in registry value if it exists, otherwise return the default value.</returns>
		public DateTime GetValue(RegistryKey key, string valuename, DateTime defaultvalue)
		{
			if (key == null)
			{
				return defaultvalue;
			}

			return System.Convert.ToDateTime(key.GetValue(valuename, defaultvalue));
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

		#endregion

		#region Event Raising

		/// <summary>
		/// Trigger the installation event.  Primarily used to simplify debugging.
		/// </summary>
		public void RaiseInstallEvent()
		{
			if (Install != null)
			{
				RegistryKey appkey = AppKey();

				// Only write keys I am directly in charge of.
				if (appkey == null)
				{
					Install();
				}
				else
				{
					bool installed = GetValue(appkey, "Installed", false);
					if (!installed)
					{
						Install();
					}
				}
			}
		}

		#endregion

	} // End class.
} // End namespace.