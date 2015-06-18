using System;
using Microsoft.Win32;

namespace DigitalProduction
{
	/// <summary>
	/// Template for function to access the registry key.
	/// </summary>
	public delegate RegistryKey	RegistryEntryDelegate();

	/// <summary>
	/// Holds data associated with a registry function and a function that gets they key
	/// where the registry entry is stored.
	/// </summary>
	public class RegistryEntry
	{
		#region Members / Variables.

		/// <summary>
		/// Function used to access registry key.
		/// </summary>
		public readonly RegistryEntryDelegate		RegKeyFunction;

		private readonly string						_name;
		private object								_value;

		#endregion

		#region Construction / Destruction.

		/// <summary>
		/// Use constructor to assign name and key function, plus to simplify creation.
		/// </summary>
		/// <param name="keyfunction">
		/// A function of type RegistryEntryDelegate which returns the RegistryKey
		/// that the RegistryEntry is on.
		/// </param>
		/// <param name="name">Name of the RegistryEntry.  Used as the name of the RegistryValue.</param>
		/// <param name="val">Default value to use.</param>
		public RegistryEntry(RegistryEntryDelegate keyfunction, string name, object val)
		{
			RegKeyFunction	= keyfunction;
			_name			= name;
			_value			= val;
		}

		#endregion

		#region Properties.

		/// <value>
		/// Get or set name.
		/// </value>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		/// <value>
		/// Get or set a registry value.
		/// </value>
		public object Value
		{
			get
			{
				return _value;
			}

			set
			{
				_value = value;

				RegistryKey key = this.RegKeyFunction();

				if (key != null)
				{
					try
					{
						key.SetValue(_name, value);
					}
					catch
					{
					}
				}
			}
		}

		#endregion

	} // End class.
} // End namespace.