using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;

using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;

using GotDotNet.XInclude;

namespace DigitalProduction.XML
{
	/// <summary>
	/// 
	/// </summary>
	public static class XsltTools
	{
		#region Members

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		static XsltTools()
		{
		}

		#endregion

		#region Properties

		#endregion

		#region Methods

		/// <summary>
		/// Perform the transformation.
		/// </summary>
		/// <param name="inputFile">Input (XML) file.</param>
		/// <param name="xsltFile">Transformation (XSLT) file.</param>
		/// <param name="outputFile">Output file.</param>
		public static void Transform(string inputFile, string xsltFile, string outputFile)
		{
			XIncludingReader xIncludingReader	= new XIncludingReader(inputFile);
			XPathDocument xPathDocument			= new XPathDocument(xIncludingReader);

			XslCompiledTransform xslTransform	= new XslCompiledTransform(true);
			xslTransform.Load(xsltFile);

			System.IO.StreamWriter streamWriter = new StreamWriter(outputFile, false, System.Text.Encoding.ASCII);
			xslTransform.Transform(xPathDocument, new XsltArgumentList(), streamWriter);

			xIncludingReader.Close();
			streamWriter.Close();
		}

		#endregion

	} // End class.
} // End namespace.