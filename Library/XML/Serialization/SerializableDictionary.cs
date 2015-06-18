﻿using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;

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
	[XmlRoot("dictionary")]
	public class SerializableDictionary<KeyType, ValueType> : Dictionary<KeyType, ValueType>, IXmlSerializable
	{
		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SerializableDictionary() {}

		#endregion
		
		#region XML

		/// <summary>
		/// Get the schema.
		/// </summary>
		/// <returns>Null.  This object does not have a schema.</returns>
		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Read XML.
		/// </summary>
		/// <param name="reader">XmlReader.</param>
		public void ReadXml(XmlReader reader)
		{
			XDocument document = null;
			using (XmlReader subtreereader = reader.ReadSubtree())
			{
				document = XDocument.Load(subtreereader);
			}
			XmlSerializer serializer = new XmlSerializer(typeof(SerializableKeyValuePair<KeyType, ValueType>));
			foreach (XElement item in document.Descendants(XName.Get("item")))
			{
				using (XmlReader itemReader =  item.CreateReader())
				{
					var keyvaluepair = serializer.Deserialize(itemReader) as SerializableKeyValuePair<KeyType, ValueType>;
					Add(keyvaluepair.Key, keyvaluepair.Value);
				}
			}
			reader.ReadEndElement();
		}

		/// <summary>
		/// Write XML.
		/// </summary>
		/// <param name="writer">XmlWriter.</param>
		public void WriteXml(System.Xml.XmlWriter writer)
		{
			XmlSerializer serializer			= new XmlSerializer(typeof(SerializableKeyValuePair<KeyType, ValueType>));
			XmlSerializerNamespaces namespaces	= new XmlSerializerNamespaces();
			namespaces.Add("", "");

			foreach (KeyType key in this.Keys)
			{
				ValueType value		= this[key];
				var keyvaluepair	= new SerializableKeyValuePair<KeyType, ValueType>(key, value);
				serializer.Serialize(writer, keyvaluepair, namespaces);
			}
		}

		#endregion

	} // End class.
} // End namespace.