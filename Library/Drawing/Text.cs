using System;
using System.Drawing;

namespace DigitalProduction.Drawing
{
	/// <summary>
	/// Summary description for DPMText.
	/// </summary>
	public class DPMText
	{
		#region Members

		#endregion

		#region Construction

		/// <summary>
		/// Constructor.
		/// </summary>
		public DPMText()
		{
		}

		#endregion

		#region Justification

		/// <summary>
		/// Calculate the new x position needed to horizontally in the space between xstart and xstart+width.
		/// </summary>
		/// <param name="graphics">Graphics object text is going to be drawn on.</param>
		/// <param name="font">Font used to draw text.</param>
		/// <param name="text">Text to center.</param>
		/// <param name="xstart">X position text starts at.</param>
		/// <param name="width">Width of area to center text in.</param>
		public static float CenterTextHorizontally(Graphics graphics, Font font, string text, float xstart, float width)
		{
			SizeF size = graphics.MeasureString(text, font);
			return xstart + width/2 - size.Width/2;
		}

		#endregion

	} // End class.
} // End namespace.