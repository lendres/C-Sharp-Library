using System;

namespace DigitalProduction.XML
{
	/// <summary>
	/// An XML data handler.
	/// </summary>
	public class XMLHandler
	{
		#region Members

		private string					_elementname;
		private XMLHandlerFunction		_callback;
		private HandlerType				_type				= HandlerType.None;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public XMLHandler()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">HandlerType of this handler.</param>
		/// <param name="elementhandler">Function which handles the element if its found.</param>
		public XMLHandler(HandlerType type, XMLHandlerFunction elementhandler)
		{
			_callback	= elementhandler;
			_type		= type;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Name of the element this, Handler handles.
		/// </summary>
		public string ElementName
		{
			get
			{
				return _elementname;
			}

			set
			{
				_elementname = value;
			}
		}

		/// <summary>
		/// Handler type.
		/// </summary>
		public HandlerType Type
		{
			get
			{
				return _type;
			}

			set
			{
				_type = value;
			}
		}

		/// <summary>
		/// Callback function.
		/// </summary>
		public XMLHandlerFunction HandlerFunction
		{
			get
			{
				return _callback;
			}

			set
			{
				_callback = value;
			}
		}

		#endregion

		#region Functions

		#endregion

		#region IComparable Members

		/// <summary>
		/// Implements the CompareTo method of the IComparable interface.
		/// </summary>
		/// <param name="obj">An object of type XMLHandler.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared.  The return value 
		/// has these meanings:
		/// Less than zero: This instance is less than obj.
		/// Zero: This instance is equal to obj. 
		/// Greater than zero: This instance is greater than obj.
		/// </returns>
		public int CompareTo(object obj)
		{
			// Ensure we have an object of this type.
			if (obj is XMLHandler)
			{
				// Cast the input object and do the comparison.  The actual comparison is done by the CompareTo method
				// of the string.
				XMLHandler handler = (XMLHandler)obj;
				return _elementname.CompareTo(handler._elementname);
			}
			else
			{
				if (obj is string)
				{
					return _elementname.CompareTo((string)obj);
				}
				else
				{
					throw new ArgumentException("Object is not a XMLHandler.");
				}
			}
		}

		/// <summary>
		/// Equivalent function used as a predicate to determine if this FileExtension is equivalent to a second.
		/// </summary>
		/// <param name="obj">XMLHandler to compare to.</param>
		/// <returns>True if equivalent, false otherwise.</returns>
		override public bool Equals(object obj)
		{
			if (obj is XMLHandler)
			{
				return _elementname == ((XMLHandler)obj)._elementname;
			}

			return false;
		}

		/// <summary>
		/// Get a hash code.
		/// </summary>
		/// <returns>A hash code.</returns>
		override public int GetHashCode()
		{
			return _elementname.GetHashCode();
		}

		#endregion

	} // End class.
} // End namespace.