using System;

namespace APS
{
	/// <summary>
	/// A class for formatting strings.
	/// </summary>
	public static class Format
	{
		#region Members

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		static Format()
		{
		}

		#endregion

		#region Properties

		#endregion

		#region Methods

		/// <summary>
		/// Removes white spaces from the specified string.
		/// </summary>
		/// <param name="input">The input string.</param>
		public static string RemoveWhiteSpace(string input)
		{
			int index		= 0;
			int inputlen	= input.Length;
			char[] newarr	= new char[inputlen];

			for (int i = 0; i < inputlen; i++)
			{
				char tmp = input[i];

				if (!char.IsWhiteSpace(tmp))
				{
					newarr[index] = tmp;
					index++;
				}
			}

			return new string(newarr, 0, index);
		}

		/// <summary>
		/// Determines whether the specified string has any white space characters.
		/// </summary>
		/// <param name="input">The input string.</param>
		public static bool HasWhiteSpace(string input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				if (char.IsWhiteSpace(input[i]))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns the DateTime as a formatted string with factions of seconds included.
		/// </summary>
		/// <param name="dateTime">DateTime to format.</param>
		public static string DateTimeWithPreciseSeconds(DateTime dateTime)
		{
			return dateTime.ToString("M/dd/yyyy h:mm:ss.fffffff");
		}

		/// <summary>
		/// Extracts just the major and minor version numbers from a software version number.
		/// </summary>
		/// <param name="softwareVersion">A string that is a software version in the form of X.X.XX.XX</param>
		/// <example>
		/// If the version number is "1.2.3.4" this will return "1.2" as a string.
		/// </example>
		public static string MajorMinorVersionNumber(string softwareVersion)
		{
			string[] splitVersion = softwareVersion.Split('.');
			return splitVersion[0] + "." + splitVersion[1];
		}

		#endregion

	} // End class.
} // End namespace.