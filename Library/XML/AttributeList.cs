using System;
using System.Collections.Generic;

namespace DigitalProduction.XML
{
	/// <summary>
	/// Summary not provided for the class AttributeList.
	/// </summary>
	public class AttributeList : IEnumerable<Attribute>
	{
		#region Enumerations.

		private List<Attribute>		_attributes;

		#endregion

		#region Members / Variables / Delegates.



		#endregion

		#region Construction.

		/// <summary>
		/// Default constructor.
		/// </summary>
		public AttributeList()
		{
			_attributes = new List<Attribute>();
		}

		#endregion

		#region Properties.

		/// <summary>
		/// Number of Attributes.
		/// </summary>
		public int Count
		{
			get
			{
				return _attributes.Count;
			}
		}

		#endregion

		#region Functions.

		/// <summary>
		/// Add an Attribute.
		/// </summary>
		/// <param name="attribute">Attribute to add.</param>
		public void Add(Attribute attribute)
		{
			_attributes.Add(attribute);
		}

		/// <summary>
		/// Get an attribute.
		/// </summary>
		/// <param name="number">Which Attribute to get.</param>
		/// <returns>The Attribute at position "number" if it exists, null otherwise.</returns>
		public Attribute GetAttribute(int number)
		{
			if (number < _attributes.Count || number >= 0)
			{
				return _attributes[number];
			}

			return null;
		}

		#endregion

		#region IEnumerable / IEnumerable<Attribute> Members

		/// <summary>
		/// Get an enumerator.
		/// </summary>
		/// <returns>An enumerator.</returns>
		public IEnumerator<Attribute> GetEnumerator()
		{
 			return new AttributeEnumerator(_attributes);
		}

		/// <summary>
		/// Get an enumerator.
		/// </summary>
		/// <returns>An enumerator.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new AttributeEnumerator(_attributes);
		}

		#endregion

} // End class.
} // End namespace.