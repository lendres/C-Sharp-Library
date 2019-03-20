using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;
using DigitalProduction.Forms;

namespace DigitalProduction.Registry
{
	/// <summary>
	/// FormRegistryAccess Class. A class that handles additional registry access functions associated with application level
	/// forms.  For example, this class handles storing and retrieving of the window state of a form.
	/// </summary>
	public class FormWinRegistryAccess : WinRegistryAccess
	{
		#region Members

		/// <summary>
		/// Used to create a list of entries such that they can all be easily read or written at
		/// one time if needed.
		/// </summary>
		protected ArrayList							_registryentries			= new ArrayList();

		/// <summary>
		/// Digital Production Management base Form.  This allows commonality amongst all DPM applications.
		/// </summary>
		private readonly DPMForm					_owner;

		#endregion

		#region Construction

		/// <summary>
		/// Constructor when the dialog box that is the owner is the top level dialog box.
		/// </summary>
		/// <param name="owner">DPMForm that is using this to access to the registry.</param>
		public FormWinRegistryAccess(DPMForm owner) :
			base(owner.DPMCompanyName, owner.AppName)
		{
			_owner			= owner;
			this.Install	+= this.OnInstall;
		}

		/// <summary>
		/// Constructor when the dialog box that is the owner is the top level dialog box.
		/// </summary>
		/// <param name="owner">DPMForm that is using this to access to the registry.</param>
		/// <param name="installHandler">Installation handler to add to the Install event.</param>
		public FormWinRegistryAccess(DPMForm owner, InstallEventHandler installHandler) :
			base(owner.DPMCompanyName, owner.AppName, installHandler)
		{
			_owner			= owner;
			this.Install	+= this.OnInstall;
		}

		#endregion

		#region Installation

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
				//appkey.DeleteSubKeyTree("Recent Files", false);
			}
		}

		#endregion

		#region Properties

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

		#endregion

		#region Options

		/// <summary>
		/// Return the key that holds the options.
		/// </summary>
		/// <returns>Returns the registry key if it could be accessed, null if an error occurs.</returns>
		protected RegistryKey OptionsKey()
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
				regkey = appkey.CreateSubKey("Options");
			}
			catch
			{
				return null;
			}

			return regkey;
		}

		#endregion

		#region Window State Access

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

		#region Recently Used Files

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
		/// items displayed, not the number of menu items allowed.
		/// </value>
		public uint NumberOfRecentlyUsedFiles
		{
			get
			{
				return (uint)GetValue(RecentFilesKey(), "Size", 4);
			}

			set
			{
				SetValue(RecentFilesKey(), "Size", value);
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