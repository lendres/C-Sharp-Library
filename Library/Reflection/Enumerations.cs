using System;
using System.ComponentModel;
using System.Reflection;

namespace DigitalProduction.Reflection
{
	/// <summary>
	/// Enumeration utilities.
	/// 
	/// Get description of an enumeration:
	///		Converts from a [Description("")] to a enum value.
	///		Grabs the [Description("")] from a enum value.
	/// 
	///		Based on code written by skot:
	///		http://www.codeproject.com/useritems/EnumDescriptionAttribute.asp
	/// </summary>
	public static class EnumUtils
	{
		#region Number of defined items.

		/// <summary>
		/// Gets the number of items defined within an enumeration definition.
		/// </summary>
		/// <typeparam name="T">Enumeration type that items are defined in.</typeparam>
		/// <returns>Number of items defined within an enumeration.</returns>
		public static int NumberOfDefinedItems<T>()
		{
			return Enum.GetNames(typeof(T)).Length;
		}

		#endregion

	} // End class.
} // End namespace.