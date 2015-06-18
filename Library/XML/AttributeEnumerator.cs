using System;
using System.Collections;
using System.Collections.Generic;

namespace DigitalProduction.XML
{
	/// <summary>
	/// Summary not provided for the class AttributeEnumerator.
	/// </summary>
	public class AttributeEnumerator : IEnumerator, IEnumerator<Attribute>
	{
		#region Members / Variables / Delegates.

		private List<Attribute>	_attributes;

		// Enumerators are positioned before the first element until the first MoveNext() call.
		private int				_position = -1;

		#endregion

		#region Construction.

		/// <summary>
		/// Default constructor.
		/// </summary>
		public AttributeEnumerator(List<Attribute> attributes)
		{
			_attributes = attributes;
		}

		#endregion

		#region Properties.

		/// <summary>
		/// Get the current entry.
		/// </summary>
		object IEnumerator.Current
		{
			get
			{
				try
				{
					return _attributes[_position];
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}

		Attribute IEnumerator<Attribute>.Current
		{
			get
			{
				try
				{
					return _attributes[_position];
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}

		#endregion

		#region Functions.

		/// <summary>
		/// Move to the next entry.
		/// </summary>
		/// <returns>True is there is another entry, false otherwise.</returns>
		bool IEnumerator.MoveNext()
		{
			_position++;
			return (_position < _attributes.Count);
		}

		/// <summary>
		/// Reset to the beginning of the entries.
		/// </summary>
		void IEnumerator.Reset()
		{
			_position = -1;
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose() {}

		#endregion

	} // End class.
} // End namespace.