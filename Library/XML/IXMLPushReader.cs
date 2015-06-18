using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalProduction.XML
{
	/// <summary>
	/// Provides an interface for creating classes that use the XMLTextProcessor to read xml files using a
	/// push model.
	/// </summary>
	public interface IXMLPushReader
	{
		/// <summary>
		/// Read the element that this class corresponds to.
		/// </summary>
		/// <param name="xmlprocessor">XMLTextProcessor doing the reading.</param>
		void ReadXML(XMLTextProcessor xmlprocessor);

	} // End interface.
} // End namespace.
