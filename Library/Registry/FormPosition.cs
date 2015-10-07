using System.ComponentModel;

namespace DigitalProduction.Registry
{
	/// <summary>
	/// Add summary here.
	/// 
	/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
	/// 
	/// The "Length" enumeration can be used in loops as a convenient way of terminating a loop that does not have to be changed if
	/// the number of items in the enumeration changes.  The "Length" enumeration must be the last item.
	/// for (int i = 0; i &lt; (int)EnumType.Length; i++) {...}
	/// </summary>
	public enum FormPosition
	{
		/// <summary>Position of left side of form.</summary>
		[Description("Left")]
		Left,

		/// <summary>Position of the top of the form.</summary>
		[Description("Top")]
		Top,

		/// <summary>Width of the form.</summary>
		[Description("Width")]
		Width,

		/// <summary>Height of the form.</summary>
		[Description("Height")]
		Height

	} // End enum.
} // End namespace.