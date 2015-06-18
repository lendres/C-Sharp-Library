﻿using System.Xml.Serialization;
using System.Xml;

namespace DigitalProduction.XML.Serialization
{
	/// <summary>
	/// Add serialization to a dictionary.
	/// 
	/// From:
	/// http://stackoverflow.com/questions/495647/serialize-class-containing-dictionary-member
	/// </summary>
	/// <typeparam name="KeyType">Dictionary key type.</typeparam>
	/// <typeparam name="ValueType">Dictionary value type.</typeparam>
	[XmlRoot("item")]
	public class SerializableKeyValuePair<KeyType, ValueType>
	{
		#region Members

		/// <summary>Dictionary key.</summary>
		[XmlAttribute("key")]
		public KeyType Key;

		/// <summary>Dictionary value.</summary>
		[XmlElement("value")]
		public ValueType Value;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SerializableKeyValuePair() {}

		/// <summary>
		/// Constructor for initialzing values.
		/// </summary>
		/// <param name="key">Dictionary key.</param>
		/// <param name="value">Dictionary value.</param>
		public SerializableKeyValuePair(KeyType key, ValueType value)
		{
			Key		= key;
			Value	= value;
		}

		#endregion

	} // End class.

} // End namespace.