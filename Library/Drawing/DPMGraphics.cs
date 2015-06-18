using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DigitalProduction.Drawing
{
	/// <summary>
	/// Summary description for DPMGraphics.
	/// </summary>
	public class DPMGraphics
	{
		#region Members / Variables.

		private Graphics		_graphics;
		private Font			_font		= new Font("Arial", 12);
		private Brush			_brush		= Brushes.Black;

		#endregion

		#region Construction / Destruction.

		/// <summary>
		/// Construtor.
		/// </summary>
		/// <param name="graphics">Graphics object to draw on.</param>
		public DPMGraphics(Graphics graphics)
		{
			_graphics = graphics;
		}

		/// <summary>
		/// Construtor.
		/// </summary>
		/// <param name="graphics">Graphics object to draw on.</param>
		/// <param name="font">Font to use when drawing.</param>
		public DPMGraphics(Graphics graphics, Font font)
		{
			_graphics = graphics;
			_font = font;
		}

		/// <summary>
		/// Construtor.
		/// </summary>
		/// <param name="graphics">Graphics object to draw on.</param>
		/// <param name="font">Font to use when drawing.</param>
		/// <param name="brush">Brush to use when drawing.</param>
		public DPMGraphics(Graphics graphics, Font font, Brush brush)
		{
			_graphics = graphics;
			_font = font;
			_brush = brush;
		}

		#endregion

		#region Properties.

		/// <value>
		/// Graphics to draw on.
		/// </value>
		public Graphics Graphics
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

		#region Draw text.

		/// <summary>
		/// Draw a string of text center horizontally between xstart and xstart+xwidth.
		/// </summary>
		/// <param name="text">Text to draw.</param>
		/// <param name="xstart">Starting x position of area to center text in.</param>
		/// <param name="xwidth">Width of area to center text in.</param>
		/// <param name="y">Y position on graphics to draw text.</param>
		public void DrawStringCenteredX(string text, float xstart, float xwidth, float y)
		{
			float textx = DPMText.CenterTextHorizontally(_graphics, _font, text, xstart, xwidth);
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
			float textx = DPMText.CenterTextHorizontally(_graphics, _font, text, x, width);

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

	} // End class.
} // End namespace.