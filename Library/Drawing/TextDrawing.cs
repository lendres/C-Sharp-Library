using System.Drawing;
using System.Drawing.Drawing2D;

namespace DigitalProduction.Drawing
{
	/// <summary>
	/// A class to aid in drawing text.
	/// </summary>
	public class TextDrawing
	{
		#region Members

		private System.Drawing.Graphics		_graphics;
		private Font						_font		= new Font("Arial", 12);
		private Brush						_brush		= Brushes.Black;

		#endregion

		#region Construction / Destruction.

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphics">Graphics object to draw on.</param>
		public TextDrawing(System.Drawing.Graphics graphics)
		{
			_graphics = graphics;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphics">Graphics object to draw on.</param>
		/// <param name="font">Font to use when drawing.</param>
		public TextDrawing(System.Drawing.Graphics graphics, Font font)
		{
			_graphics	= graphics;
			_font		= font;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphics">Graphics object to draw on.</param>
		/// <param name="font">Font to use when drawing.</param>
		/// <param name="brush">Brush to use when drawing.</param>
		public TextDrawing(System.Drawing.Graphics graphics, Font font, Brush brush)
		{
			_graphics	= graphics;
			_font		= font;
			_brush		= brush;
		}

		#endregion

		#region Properties

		/// <value>
		/// Graphics to draw on.
		/// </value>
		public System.Drawing.Graphics Graphics
		{
			get
			{
				return _graphics;
			}

			set
			{
				if (value != null)
				{
					_graphics = value;
				}
			}
		}

		/// <value>
		/// Font to use when drawing.
		/// </value>
		public Font Font
		{
			get
			{
				return _font;
			}

			set
			{
				if (value != null)
				{
					_font = value;
				}
			}
		}

		/// <value>
		/// Brush to use when drawing.
		/// </value>
		public Brush Brush
		{
			get
			{
				return _brush;
			}

			set
			{
				if (value != null)
				{
					_brush = value;
				}
			}
		}

		#endregion

		#region Text Drawing

		/// <summary>
		/// Draw a string of text center horizontally between xstart and xstart+xwidth.
		/// </summary>
		/// <param name="text">Text to draw.</param>
		/// <param name="xstart">Starting x position of area to center text in.</param>
		/// <param name="xwidth">Width of area to center text in.</param>
		/// <param name="y">Y position on graphics to draw text.</param>
		public void DrawStringCenteredX(string text, float xstart, float xwidth, float y)
		{
			float textx = CenterTextHorizontally(_graphics, _font, text, xstart, xwidth);
			_graphics.DrawString(text, _font, _brush, textx, y);
		}

		/// <summary>
		/// Draw a string of text at position x,y rotated angle degrees clockwise.
		/// </summary>
		/// <param name="text">Text to draw.</param>
		/// <param name="angle">Angle, in degrees, to rotate text in clockwise direction.</param>
		/// <param name="x">X position on graphics to draw text.</param>
		/// <param name="y">Y position on graphics to draw text.</param>
		public void DrawStringRotated(string text, float angle, float x, float y)
		{
			// Save transformation matrix.
			Matrix matrix = _graphics.Transform;

			_graphics.TranslateTransform(x, y);
			_graphics.RotateTransform(angle);

			_graphics.DrawString(text, _font, _brush, 0f, 0f);

			// Restore transformation matrix.
			_graphics.Transform = matrix;
		}

		/// <summary>
		/// Draw a string of text rotated angle degrees clockwise and centered between x and x+width in the new
		/// rotated frame.
		/// </summary>
		/// <param name="text">Text to draw.</param>
		/// <param name="angle">Angle, in degrees, to rotate text in clockwise direction.</param>
		/// <param name="x">X position on graphics to draw text.</param>
		/// <param name="y">Y position on graphics to draw text.</param>
		/// <param name="width">Width of area to center text in.</param>
		public void DrawStringRotatedCenteredX(string text, float angle, float x, float y, float width)
		{
			float textx = CenterTextHorizontally(_graphics, _font, text, x, width);

			Matrix matrix = _graphics.Transform;

			// Translate to the point of the text.
			_graphics.TranslateTransform(x, y);

			// Rotate to the new angle.
			_graphics.RotateTransform(angle);

			// Translate along the x the offset amount to center text.
			_graphics.TranslateTransform(textx-x, 0f);

			// Draw the string.
			_graphics.DrawString(text, _font, _brush, 0f, 0f); //points[0]);

			// Restore transformation matrix.
			_graphics.Transform = matrix;
		}

		#endregion

		#region Justification

		/// <summary>
		/// Calculate the new X position needed to horizontally in the space between xstart and xstart+width.
		/// </summary>
		/// <param name="graphics">Graphics object text is going to be drawn on.</param>
		/// <param name="font">Font used to draw text.</param>
		/// <param name="text">Text to center.</param>
		/// <param name="xstart">X position text starts at.</param>
		/// <param name="width">Width of area to center text in.</param>
		private float CenterTextHorizontally(System.Drawing.Graphics graphics, Font font, string text, float xstart, float width)
		{
			SizeF size = graphics.MeasureString(text, font);
			return xstart + width/2 - size.Width/2;
		}

		#endregion

	} // End class.
} // End namespace.