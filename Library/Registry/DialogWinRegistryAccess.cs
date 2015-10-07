using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Win32;
using DigitalProduction.Forms;

namespace DigitalProduction.Registry
{
	/// <summary>
	/// DialogWinRegistry class.  Similar to the FormWinRegistryAccess class (and, indeed, is inherited from it), but
	/// is instead used for dialog boxes or similar forms opened from a main application form.  The primary difference
	/// being that this class stores it's setting is a sub-key of the main application key.
	/// </summary>
	public class DialogWinRegistryAccess : FormWinRegistryAccess
	{
		#region Members

		/// <summary>Dialog box name.</summary>
		private string									_dialogName;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DialogWinRegistryAccess(DPMForm owner, string dialogName) : base(owner)
		{
			_dialogName	= dialogName;
		}

		/// <summary>
		/// Constructor when the dialog box that is the owner is the top level dialog box.
		/// </summary>
		/// <param name="owner">DPMForm that is using this to access to the registry.</param>
		/// <param name="dialogName">The name of the dialog box.  Also used as a key to store values in the registry.</param>
		/// <param name="installHandler">Installation handler to add to the Install event.</param>
		public DialogWinRegistryAccess(DPMForm owner, string dialogName, InstallEventHandler installHandler) : base(owner, installHandler)
		{
			_dialogName	= dialogName;
		}

		#endregion

		#region Properties

		#endregion

		#region Methods

		/// <summary>
		/// Get the registry key associated with the application name.
		/// </summary>
		/// <returns>Returns the registry key if it could be accessed, null if an error occurs.</returns>
		protected override RegistryKey AppKey()
		{
			RegistryKey key;

			try
			{
				RegistryKey appKey = base.AppKey();

				if (appKey == null)
				{
					return null;
				}

				key = appKey.CreateSubKey("Dialogs");
				key = key.CreateSubKey(_dialogName);
			}
			catch
			{
				return null;
			}

			return key;
		}

		#endregion

	} // End class.
} // End namespace.