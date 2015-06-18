using System;
using System.Collections.Generic;

namespace DigitalProduction.XML
{
	/// <summary>
	/// List of handlers for element.
	/// </summary>
	public delegate void XMLHandlerFunction(XMLTextProcessor xmlprocessor, object data);

	#region Enumerations.

	/// <summary>Type of handler.</summary>
	public enum HandlerType : int
	{
		/// <summary>First element in enumeration.  Used in loops to provide access to first element without hard coding element name.</summary>
		Start,

		/// <summary>Handles a specific element.</summary>
		Element		= Start,

		/// <summary>If a specific element handler is not specified for the read element, the default handler will be used.</summary>
		Default,

		/// <summary>Handles reading text between an element's start and end tags.</summary>
		Text,

		/// <summary>Default.</summary>
		None,

		/// <summary>One past the last element in this enumeration list.</summary>
		End,

		/// <summary>The number of enumerations in this enumeration list.</summary>
		Count		= End
	}

	#endregion

	/// <summary>
	/// A list of handlers.
	/// </summary>
	public class XMLHandlerList
	{
		#region Members / Variables.

		private Dictionary<string, XMLHandler>			_handlers;
		private XMLHandler[]							_uniquehandlers;

		#endregion

		#region Construction / Destruction.

		/// <summary>
		/// Constructor.
		/// </summary>
		public XMLHandlerList()
		{
			_handlers		= new Dictionary<string, XMLHandler>();
			_uniquehandlers	= new XMLHandler[(int)HandlerType.Count];
		}

		#endregion

		#region Functions.

		/// <summary>
		/// Add an element handler.
		/// </summary>
		/// <param name="elementname">Name of the element to handle.</param>
		/// <param name="elementhandler">Function which handles the element if it is found.</param>
		public void AddHandler(string elementname, XMLHandlerFunction elementhandler)
		{
			XMLHandler handler		= new XMLHandler();
			handler.Type			= HandlerType.Element;
			handler.ElementName		= elementname;
			handler.HandlerFunction	= elementhandler;

			_handlers.Add(handler.ElementName, handler);
		}

		/// <summary>
		/// Add a handler of a specific type.
		/// </summary>
		/// <param name="type">HandlerType added.</param>
		/// <param name="elementhandler">Function which handles the element if its found.</param>
		public void AddHandler(HandlerType type, XMLHandlerFunction elementhandler)
		{
			switch (type)
			{
				case HandlerType.Default:
				case HandlerType.Text:
				{
					_uniquehandlers[(int)type] = new XMLHandler(type, elementhandler);
					break;
				}

				default:
				{
					throw new ArgumentException("The handler type specified cannot be added through this function.\n\nHandler type specified: " + type.ToString());
				}
			}
		}

		/// <summary>
		/// Add a list of handlers to this handler list.
		/// </summary>
		/// <param name="xmlhandlerlist">XMLHandlerList to add handlers from.</param>
		public void AddHandlers(XMLHandlerList xmlhandlerlist)
		{
			foreach (KeyValuePair<string, XMLHandler> handlerpair in xmlhandlerlist._handlers)
			{
				_handlers.Add(handlerpair.Key, handlerpair.Value);
			}
		}

		/// <summary>
		/// See if the element has an associated handler.  If it does call the handler.
		/// </summary>
		/// <param name="elementname">Name of the element to look for.</param>
		/// <param name="xmlprocessor">XML processor that is doing the processing.</param>
		/// <param name="data">Optional data passed to the handler.</param>
		public void ProcessElement(string elementname, XMLTextProcessor xmlprocessor, object data)
		{
			// Try to find the element name.
			if (_handlers.ContainsKey(elementname))
			{
				_handlers[elementname].HandlerFunction(xmlprocessor, data);
				return;
			}

			Process(HandlerType.Default, xmlprocessor, data);
		}

		/// <summary>
		/// See if the element has an associated handler.  If it does call the handler.
		/// </summary>
		/// <param name="handler">The HandlerType to look for.</param>
		/// <param name="xmlprocessor">XML processor that is doing the processing.</param>
		/// <param name="data">Optional data passed to the handler.</param>
		public void Process(HandlerType handler, XMLTextProcessor xmlprocessor, object data)
		{
			if (_uniquehandlers[(int)handler] != null)
			{
				_uniquehandlers[(int)handler].HandlerFunction(xmlprocessor, data);
			}
		}

		#endregion

	} // End class.
} // End Namespace.