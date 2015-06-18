using System;

namespace DigitalProduction.XML
{
	/// <summary>
	/// Summary not provided for the class Attribute.
	/// </summary>
	public class Attribute
	{
		#region Members / Variables / Delegates.

		private string		_name;
		private string		_value;

		#endregion

		#region Construction.

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Attribute() {}

		/// <summary>
		/// Constructor.
		/// </summary>
		public Attribute(string name, string value)
		{
			_name	= name;
			_value	= value;
		}

		#endregion

		#region Properties.

		/// <summary>
		/// Name of the attribute.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
			}
		}

		/// <summary>
		/// Value of the attribute.
		/// </summary>
		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		#endregion

	} // End class.
} // End namespace.