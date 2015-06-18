using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace DigitalProduction.XML
{
	/// <summary>
	/// Summary description for XMLTextProcessor.
	/// </summary>
	public class XMLTextProcessor
	{
		#region Members / Variables.

		/// <summary>Base stream that reads the file.</summary>
		private FileStream					_inputstream;

		/// <summary>XML reader that reads the file.</summary>
		private XmlTextReader				_xmlreader;

		/// <summary>The name of the top element.</summary>
		private string						_topelement;

		/// <summary>Flag to indicate the first call to process.</summary>
		private bool						_firstcall;

		#endregion

		#region Construction / Destruction.

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="file">XML file to process.</param>
		public XMLTextProcessor(string file)
		{
			Open(file);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="file">XML file to process.</param>
		/// <param name="topelement">Name of the top element in the file.</param>
		public XMLTextProcessor(string file, string topelement)
		{
			Open(file, topelement);
		}

		#endregion

		#region Open and closing the input.

		/// <summary>
		/// Open the input and read up to the first element.
		/// </summary>
		/// <param name="file">XML file to process.</param>
		private void Open(string file)
		{
			Open(file, "");
		}

		/// <summary>
		/// Open the input and read up to the first element.
		/// </summary>
		/// <param name="file">XML file to process.</param>
		/// <param name="topelement">Name of the top element in the file.</param>
		private void Open(string file, string topelement)
		{
			try
			{
				// File input stream.
				_inputstream = new FileStream(file, FileMode.Open, FileAccess.Read);

				// If the file is empty, we are done with this.
				if (_inputstream.Length == 0)
				{
					_xmlreader = null;
					return;
				}

				// Create the xml reader.
				_xmlreader = new XmlTextReader(_inputstream);
				_xmlreader.WhitespaceHandling = WhitespaceHandling.None;

				// Read the header stuff and get to the first element.
				while (_xmlreader.NodeType != XmlNodeType.Element)
				{
					_xmlreader.Read();
				}
			}
			catch
			{
				Close();
				return;
			}

			// Store the top level node name and reset parameters.
			_topelement	= _xmlreader.LocalName;
			_firstcall	= true;

			// If a top level element was specified we will perform a check to ensure we found what we are looking
			// for, otherwise we will assume we found it or don't care what it is.
			//
			// This is mainly implemented to let calls to the XMLTextProcessor constructor using the old constructor
			// that required the top level element name, a requirement which has since been dropped.  But as long as
			// we have the top element for the old style constructor calls, we might as well perform a check to make
			// sure we are getting what we expect.
			if (topelement != "")
			{
				// Do the check.
				if (_topelement != topelement)
				{
					// Did we open the right file?
					string message	=  "Top level element found in XML file did not match the expected element name.";
					message			+= "\n\nExpected: "	+ topelement;
					message			+= "\nFound: "		+ _topelement;
					throw new Exception(message);
				}
			}
		}

		/// <summary>
		/// Clean up in case of any errors.
		/// </summary>
		public void Close()
		{
			if (_inputstream != null)
			{
				try
				{
					_inputstream.Close();
					_inputstream.Dispose();
				}
				catch
				{
				}
				_inputstream = null;
			}

			if (_xmlreader != null)
			{
				try
				{
					_xmlreader.Close();
				}
				catch
				{
				}
				_xmlreader = null;
			}
		}

		#endregion

		#region Properties.

		/// <summary>
		/// The base input stream used.  Read only.
		/// </summary>
		public FileStream FileStream
		{
			get
			{
				return _inputstream;
			}
		}

		/// <summary>
		/// The text reader used.  Read only.
		/// </summary>
		public XmlTextReader XmlTextReader
		{
			get
			{
				return _xmlreader;
			}
		}

		/// <summary>
		/// Is the XML file open for reading?
		/// </summary>
		public bool IsFileOpen
		{
			get
			{
				return _xmlreader != null;
			}
		}

		/// <summary>
		/// Returns the information necessary to display a message to the user so that they can figure out what
		/// went wrong with their input file.  Read only.
		/// </summary>
		public string ErrorInformation
		{
			get
			{
				string message = "\n\nFile: " + _inputstream.Name;
                message += "\n\nLine: " + _xmlreader.LineNumber;
                message += "\n\nPosition: " + _xmlreader.LinePosition;
				return message;
			}
		}

		#endregion

		#region Process.

		#region Public access and cleaning up.

		/// <summary>
		/// Process the body of the current element.
		/// </summary>
		/// <param name="handlers">
		/// An instance of XMLHandlerList which has the handlers for elements that this element contains.
		/// </param>
		public void Process(XMLHandlerList handlers)
		{
			Process(handlers, null);
		}

		/// <summary>
		/// Process the body of the current element.
		/// </summary>
		/// <param name="handlers">
		/// An instance of XMLHandlerList which has the handlers for elements that this element contains.
		/// </param>
		/// <param name="data">Optional data passed to the handler.</param>
		/// <remarks>
		/// This function is really just a wrapper around the RunProcess which does the real work.  We just use this function
		/// to do the error handling.
		/// </remarks>
		public void Process(XMLHandlerList handlers, object data)
		{
			try
			{
				RunProcess(handlers, data);
			}
			catch (Exception e)
			{
				// Try to clean up.
				Close();
				throw e;
			}
		}

		#endregion

		#region Run process.

		/// <summary>
		/// Process the body of the current element.
		/// </summary>
		/// <param name="handlers">
		/// An instance of XMLHandlerList which has the handlers for elements that this element contains.
		/// </param>
		/// <param name="data">Optional data passed to the handler.</param>
		private void RunProcess(XMLHandlerList handlers, object data)
		{
			if (_xmlreader == null)
			{
				return;
			}

			// Don't want to read threw empty elements and into the next element.
			if (_xmlreader.IsEmptyElement)
			{
				return;
			}

			string topelement = _xmlreader.LocalName;

			if (_firstcall)
			{
				topelement = _topelement;
				_firstcall = false;
			}
			
			_xmlreader.Read();

			while (_xmlreader.LocalName != topelement && !_xmlreader.EOF)
			{
				switch (_xmlreader.NodeType)
				{
					case XmlNodeType.Element:
					{
						// Ensure that there is something to read here.
						handlers.ProcessElement(_xmlreader.LocalName, this, data);
						break;
					}

					case XmlNodeType.Text:
					{
						// Handle the text.
						handlers.Process(HandlerType.Text, this, data);

						// If the handler processed the next we have read into something else (like an element)
						// and we don't want to call read again or we will read over it.
						if (_xmlreader.NodeType != XmlNodeType.Text)
						{
							continue;
						}

						break;
					}

					case XmlNodeType.EndElement:
					{
						while (_xmlreader.NodeType == XmlNodeType.EndElement)
						{
							_xmlreader.Read();							
						}
						continue;
					}

					case XmlNodeType.Attribute:
					case XmlNodeType.CDATA:
					case XmlNodeType.Comment:
					case XmlNodeType.Document:
					case XmlNodeType.DocumentFragment:
					case XmlNodeType.DocumentType:
					case XmlNodeType.Entity:
					case XmlNodeType.EndEntity:
					case XmlNodeType.EntityReference:
					case XmlNodeType.None:
					case XmlNodeType.Notation:
					case XmlNodeType.ProcessingInstruction:
					case XmlNodeType.SignificantWhitespace:
					case XmlNodeType.Whitespace:
					case XmlNodeType.XmlDeclaration:
					default:
					{
						break;
					}

				} // End switch.

				_xmlreader.Read();

			} // End while.
		}

		#endregion

		#endregion

		#region Attribute reading.

		/// <summary>
		/// Read an attribute from the file and convert it to the indicated data type.
		/// </summary>
		/// <param name="name">Name of the attribute to read.</param>
		/// <param name="defaultvalue">Value to return if nothing is found or an error occurs.</param>
		/// <returns>
		/// Attribute converted to the indicated data type if possible, otherwise the default value.
		/// </returns>
		public int GetAttribute(string name, int defaultvalue)
		{
			string val = _xmlreader.GetAttribute(name);
			int convval = defaultvalue;
			try
			{
				convval = System.Convert.ToInt32(val);
			}
			catch
			{
            }

			return convval;
		}

		/// <summary>
		/// Read an attribute from the file and convert it to the indicated data type.
		/// </summary>
		/// <param name="name">Name of the attribute to read.</param>
		/// <param name="defaultvalue">Value to return if nothing is found or an error occurs.</param>
		/// <returns>
		/// Attribute converted to the indicated data type if possible, otherwise the default value.
		/// </returns>
		public double GetAttribute(string name, double defaultvalue)
		{
			string val = _xmlreader.GetAttribute(name);
			double convval = defaultvalue;
			try
			{
				convval = System.Convert.ToDouble(val);
			}
			catch
			{
			}

			return convval;
		}

		/// <summary>
		/// Read an attribute from the file and convert it to the indicated data type.
		/// </summary>
		/// <param name="name">Name of the attribute to read.</param>
		/// <param name="defaultvalue">Value to return if nothing is found or an error occurs.</param>
		/// <returns>
		/// Attribute converted to the indicated data type if possible, otherwise the default value.
		/// </returns>
		public bool GetAttribute(string name, bool defaultvalue)
		{
			string val = _xmlreader.GetAttribute(name);
			bool convval = defaultvalue;
			try
			{
				convval = System.Convert.ToBoolean(val);
			}
			catch
			{
			}

			return convval;
		}

		/// <summary>
		/// Read an attribute from the file and convert it to the indicated data type.
		/// </summary>
		/// <param name="name">Name of the attribute to read.</param>
		/// <param name="defaultvalue">Value to return if nothing is found or an error occurs.</param>
		/// <returns>
		/// Attribute converted to the indicated data type if possible, otherwise the default value.
		/// </returns>
		public string GetAttribute(string name, string defaultvalue)
		{
			string val = _xmlreader.GetAttribute(name);

			if (val == null)
			{
				return defaultvalue;
			}
			else
			{
				return val;
			}
		}

		/// <summary>
		/// Read an attribute from the file and convert it to the indicated data type.
		/// </summary>
		/// <param name="name">Name of the attribute to read.</param>
		/// <param name="defaultvalue">Value to return if nothing is found or an error occurs.</param>
		/// <returns>
		/// Attribute converted to the indicated data type if possible, otherwise the default value.
		/// </returns>
		public object GetAttribute(string name, object defaultvalue)
		{
			object val = _xmlreader.GetAttribute(name);

			if (val == null)
			{
				return defaultvalue;
			}
			else
			{
				return val;
			}		
		}

		/// <summary>
		/// Extracts all the attributes as a name, value pair and moves to the next element.
		/// </summary>
		/// <returns>The attributes as a name, value pair.  The values are returned as strings.</returns>
		/// <remarks>
		/// This function moves to the next elements so if you want to do additional work with the attributes, do it first.
		/// </remarks>
		public AttributeList GetAttributes()
		{
			AttributeList attributes = new AttributeList();

			if (_xmlreader.HasAttributes)
			{
				_xmlreader.MoveToFirstAttribute();
				attributes.Add(new Attribute(_xmlreader.Name, _xmlreader.Value));

				while (_xmlreader.MoveToNextAttribute())
				{
					attributes.Add(new Attribute(_xmlreader.Name, _xmlreader.Value));
				} // End while attribute.

				// Move the reader back to the beginning of the element so it is back were we started
				// and the main processing loop doesn't have a hissy.
				_xmlreader.MoveToElement();

			} // End if attrbutes.

			return attributes;
		}

		#endregion

		#region Get element data.

		/// <summary>
		/// Read the element data as a string.
		/// </summary>
		/// <returns>The element data.</returns>
		public string GetElementString(string defaultvalue)
		{
			string val = _xmlreader.ReadString();

			if (val == null)
			{
				return defaultvalue;
			}
			else
			{
				return val;
			}
		}

		/// <summary>
		/// Read the element data as the indicated type.
		/// </summary>
		/// <returns>The element data converted to the indicated type.</returns>
		public int GetElementString(int defaultvalue)
		{
			string val = _xmlreader.ReadString();
			int convval = defaultvalue;
			try
			{
				convval = System.Convert.ToInt32(val);
			}
			catch
			{
			}

			return convval;
		}
		
		/// <summary>
		/// Read the element data as the indicated type.
		/// </summary>
		/// <returns>The element data converted to the indicated type.</returns>
		public double GetElementString(double defaultvalue)
		{
			string val = _xmlreader.ReadString();
			double convval = defaultvalue;
			try
			{
				convval = System.Convert.ToDouble(val);
			}
			catch
			{
			}

			return convval;
		}

		/// <summary>
		/// Read the element data as the indicated type.
		/// </summary>
		/// <returns>The element data converted to the indicated type.</returns>
		public bool GetElementString(bool defaultvalue)
		{
			string val = _xmlreader.ReadString();
			bool convval = defaultvalue;
			try
			{
				convval = System.Convert.ToBoolean(val);
			}
			catch
			{
			}

			return convval;
		}

		#endregion

	} // End class.
} // End namespace.