using System;
using System.Runtime.InteropServices;

namespace DigitalProduction.Gaming
{
	/// <summary>
	/// Provides access to cards.dll on a Windows machine.  Handles initialization and destruction
	/// of resources and provides an easy to use interface.  This class can be used to draw cards from
	/// the class Card.cs located in this same library.
	/// </summary>
	public class CardsDLLAccess : IDisposable
	{
		#region Enumerations.

		/// <summary>
		/// Image to show on the backs of cards when they are face down.
		/// </summary>
		public enum BackStyle : int
		{
			/// <summary>CrossHatch in Windows XP; CrossHatch Pre-Windows XP.</summary>
			CrossHatch		= 53,

			/// <summary>Sky in Windows XP; Weave 1 Pre-Windows XP.</summary>
			Sky				= 54,

			/// <summary>Mineral Windows XP; Weave 2 Pre-Windows XP.</summary>
			Mineral			= 55,

			/// <summary>Fish Windows XP; Robot Pre-Windows XP.</summary>
			Fish			= 56,

			/// <summary>Frog Windows XP; Flowers Pre-Windows XP.</summary>
			Frog			= 57,

			/// <summary>Flower Windows XP; Vine 1 Pre-Windows XP.</summary>
			Flower			= 58,

			/// <summary>Islang Windows XP; Vine 2 Pre-Windows XP.</summary>
			Island			= 59,

			/// <summary>Squares Windows XP; Fish 1 Pre-Windows XP.</summary>
			Squares			= 60,

			/// <summary>Magenta Windows XP; Fish 2 Pre-Windows XP.</summary>
			Magenta			= 61,

			/// <summary>Desert Moon Windows XP; Shells Pre-Windows XP.</summary>
			DesertMoon		= 62,

			/// <summary>Astronaut Windows XP; Castle Pre-Windows XP.</summary>
			Astronaut		= 63,

			/// <summary>Lines Windows XP; Island Pre-Windows XP.</summary>
			Lines			= 64,

			/// <summary>ToyCars Windows XP; Cardhand Pre-Windows XP.</summary>
			ToyCars			= 65,
		}

		/// <summary>
		/// Types of place holders where a stack of cards will go, but there isn't any now.
		/// </summary>
		public enum PlaceHolder : int
		{
			/// <summary>X style place holder.</summary>
			X				= 67,	// X Place holder.

			/// <summary>O style place holder.</summary>
			O				= 68	// O Place holder.
		}

		private enum Mode : int
		{
			FaceUp			= 0,	// For cards 0-51 draws in order A,A,A,A,2,2,2...; Don't use for 53-68.  Color does nothing.
			FaceDown		= 1,	// For cards 1-52 draws in order A,2,3,4...; Draws card backs for 53-68.  Color changes clubs, spades.
			Negative		= 2,	// Draws card inverses.  Effected by color.
			Ghost			= 3,	// Draws card backs of hatched.  Effected by color.
			Solid			= 4,	// Draws a solid color square of the specified color.  Effective by color.
			PlaceHolder		= 5,	// Draw an empty hatched card position.  Similar to ghost but NOT effected by color.
			X				= 6,	// Draw an X card place holder.
			O				= 7		// Draw an O card place holder.
		}

		#endregion

		#region Members / Variables.

		private BackStyle			_backstyle;
		private int					_width;
		private int					_height;
		private int					_color;

		#endregion

		#region DLL Import fuctions.

		/// <summary>
		/// Initializes the cards.dll library.
		/// </summary>
		/// <param name="width">Width of cards in pixels.  Changing width doesn't seem to have an effect.</param>
		/// <param name="height">Height of cards in pixels.  Changing height doesn't seem to have an effect.</param>
		/// <returns>Returns true if successful, false if an error occurs.</returns>
		[DllImport("cards.dll")]
		private static extern bool cdtInit(ref int width, ref int height);

		/// <summary>
		/// Cleans up resources allocated with call to cdtInit().
		/// </summary>
		[DllImport("cards.dll")]
		private static extern void cdtTerm();

		/// <summary>
		/// Draw a card.
		/// </summary>
		/// <param name="hdc">Device context to draw with.  Get the device context from Graphics.GetHdc()</param>
		/// <param name="x">X location, in pixels, of the upper left corner of the card being drawn.</param>
		/// <param name="y">Y location, in pixels, of the upper left corner of the card being drawn.</param>
		/// <param name="card">Card to draw.  This depends on the mode.  See enumerations for more info.</param>
		/// <param name="mode">Mode to draw in.  See enumeration for more information.</param>
		/// <param name="color">
		/// Color to use when drawing card.
		/// </param>
		/// <returns>Returns true if successful, false if an error occurs.</returns>
		[DllImport("cards.dll")]
		private static extern bool cdtDraw(IntPtr hdc, int x, int y, int card, int mode, int color);

		/// <summary>
		/// Draw a card the size of dx and dy in pixels.  Warning - drawing cards at the non-standard size greatly distorts them.
		/// </summary>
		/// <param name="hdc">Device context to draw with.  Get the device context from Graphics.GetHdc()</param>
		/// <param name="x">X location, in pixels, of the upper left corner of the card being drawn.</param>
		/// <param name="y">Y location, in pixels, of the upper left corner of the card being drawn.</param>
		/// <param name="dx">X size, in pixels, to draw the card.</param>
		/// <param name="dy">Y size, in pixels, to draw the card.</param>
		/// <param name="card">Card to draw.  This depends on the mode.  See enumerations for more info.</param>
		/// <param name="mode">Mode to draw in.  See enumeration for more information.</param>
		/// <param name="color">Color to use when drawing card.</param>
		/// <returns>Returns true if successful, false if an error occurs.</returns>
		[DllImport("cards.dll")]
		private static extern bool cdtDrawExt(IntPtr hdc, int x, int y, int dx, int dy, int card, int mode, int color);

		/// <summary>
		/// Animate a card back by displaying different frames.  This only works for some card backs.
		/// </summary>
		/// <param name="hdc">Device context to draw with.  Get the device context from Graphics.GetHdc()</param>
		/// <param name="cardback">Card back to animate.  See enumerations for more information.</param>
		/// <param name="x">X location, in pixels, of the upper left corner of the card being drawn.</param>
		/// <param name="y">Y location, in pixels, of the upper left corner of the card being drawn.</param>
		/// <param name="frame">Frame to use in animation.  Different card backs have different number of frames.  See enumerations for more information.</param>
		/// <returns>Returns true if successful, false if an error occurs.</returns>
		[DllImport("cards.dll")]
		private static extern bool cdtAnimate(IntPtr hdc, int cardback, int x, int y, int frame);

		#endregion

		#region Construction / Destruction.

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CardsDLLAccess()
		{
			_width = 71;
			_height = 93;
			_color = 16777215;

			// Initialize cards.dll.
			if(!cdtInit(ref _width, ref _height))
			{
				throw new Exception("Cards.dll can not load.  Please make sure it is installed.");
			}

			_backstyle = BackStyle.Mineral;
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		public void Dispose()
		{
			// Clean up resources allocated in constructor.
			cdtTerm();
		}

		#endregion

		#region Properties.

		/// <value>
		/// Get or set the card back style.
		/// </value>
		public BackStyle CardBackStyle
		{
			get
			{
				return _backstyle;
			}
			set
			{
				if (value >= BackStyle.CrossHatch && value <= BackStyle.ToyCars)
				{
					_backstyle = value;
				}
			}
		}

		/// <value>
		/// Get the card drawing width in pixels.
		/// </value>
		public int CardWidth
		{
			get
			{
				return _width;
			}
		}

		/// <value>
		/// Get the card drawing height in pixels.
		/// </value>
		public int CardHeight
		{
			get
			{
				return _height;
			}
		}

		/// <value>
		/// Color used in drawing.  Read/write.
		/// </value>
		public int Color
		{
			get
			{
				return _color;
			}

			set
			{
				if (value >= 0)
				{
					_color = value;
				}
			}
		}

		#endregion

		#region Drawing functions.

		/// <summary>
		/// Draw a card face down (so back is showing).
		/// </summary>
		/// <param name="hdc">Device context to draw with.  Get the device context from Graphics.GetHdc()</param>
		/// <param name="x">X position to draw card at (in pixels).</param>
		/// <param name="y">Y position to draw card at (in pixels).</param>
		/// <returns>True if drawing succeeded, false otherwise.</returns>
		public bool DrawCardFaceDown(IntPtr hdc, int x, int y)
		{
			return cdtDraw(hdc, x, y, (int)_backstyle, (int)Mode.FaceDown, _color);
		}

		/// <summary>
		/// Draw a card face up.
		/// </summary>
		/// <remarks>
		/// The mode (FaceUp) used causes the cards in "cards.dll" to be drawn in A,A,A,A,2,2,2,...K,K format.
		/// </remarks>
		/// <param name="hdc">Device context to draw with.  Get the device context from Graphics.GetHdc()</param>
		/// <param name="card">Card to draw.</param>
		/// <param name="x">X position to draw card at (in pixels).</param>
		/// <param name="y">Y position to draw card at (in pixels).</param>
		/// <returns>True if drawing succeeded, false otherwise.</returns>
		public bool DrawCardFaceUp(IntPtr hdc, Card card, int x, int y)
		{
			return cdtDraw(hdc, x, y, CardDLLPosition(card), (int)Mode.FaceUp, _color);
		}

		/// <summary>
		/// Draw a card face up.
		/// </summary>
		/// <param name="hdc">Device context to draw with.  Get the device context from Graphics.GetHdc()</param>
		/// <param name="card">Integer position in cards.dll of card to draw.</param>
		/// <param name="x">X position to draw card at (in pixels).</param>
		/// <param name="y">Y position to draw card at (in pixels).</param>
		/// <returns>True if drawing succeeded, false otherwise.</returns>
		public bool DrawCardFaceUp(IntPtr hdc, int card, int x, int y)
		{
			return cdtDraw(hdc, x, y, card, (int)Mode.FaceUp, _color);
		}

		/// <summary>
		/// Draw a card face up as a negative.
		/// </summary>
		/// <param name="hdc">Device context to draw with.  Get the device context from Graphics.GetHdc()</param>
		/// <param name="card">Card to draw.</param>
		/// <param name="x">X position to draw card at (in pixels).</param>
		/// <param name="y">Y position to draw card at (in pixels).</param>
		/// <returns>True if drawing succeeded, false otherwise.</returns>
		public bool DrawNegativeCard(IntPtr hdc, Card card, int x, int y)
		{
			return cdtDraw(hdc, x, y, CardDLLPosition(card), (int)Mode.Negative, _color);
		}

		/// <summary>
		/// Draw a card face up as a negative.
		/// </summary>
		/// <param name="hdc">Device context to draw with.  Get the device context from Graphics.GetHdc()</param>
		/// <param name="card">Integer position in cards.dll of card to draw.</param>
		/// <param name="x">X position to draw card at (in pixels).</param>
		/// <param name="y">Y position to draw card at (in pixels).</param>
		/// <returns>True if drawing succeeded, false otherwise.</returns>
		public bool DrawNegativeCard(IntPtr hdc, int card, int x, int y)
		{
			return cdtDraw(hdc, x, y, card, (int)Mode.Negative, _color);
		}

		/// <summary>
		/// Draw a X card place holder.
		/// </summary>
		/// <param name="hdc">Device context to draw with.  Get the device context from Graphics.GetHdc()</param>
		/// <param name="x">X position to draw card at (in pixels).</param>
		/// <param name="y">Y position to draw card at (in pixels).</param>
		/// <returns>True if drawing succeeded, false otherwise.</returns>
		public bool DrawX(IntPtr hdc, int x, int y)
		{
			return cdtDraw(hdc, x, y, (int)PlaceHolder.X, (int)Mode.FaceDown, _color);
		}

		/// <summary>
		/// Draw a O card place holder.
		/// </summary>
		/// <param name="hdc">Device context to draw with.  Get the device context from Graphics.GetHdc()</param>
		/// <param name="x">X position to draw card at (in pixels).</param>
		/// <param name="y">Y position to draw card at (in pixels).</param>
		/// <returns>True if drawing succeeded, false otherwise.</returns>
		public bool DrawO(IntPtr hdc, int x, int y)
		{
			return cdtDraw(hdc, x, y, (int)PlaceHolder.O, (int)Mode.FaceDown, _color);
		}

		#endregion

		#region Card positions in DLL functions.

		/// <summary>
		/// Returns the position in cards.dll that the card is at.
		/// </summary>
		/// <param name="rank">The rank (face value) of the card who's position is desired.</param>
		/// <param name="suit">The suit of the card who's position is desired.</param>
		/// <returns>Integer position of card in cards.dll.</returns>
		public static int CardDLLPosition(CardRank rank, CardSuit suit)
		{
			return (int)rank*4 + (int)suit;
		}

		/// <summary>
		/// Returns the position in cards.dll that the card is at.
		/// </summary>
		/// <param name="card">The card who's position is desired.</param>
		/// <returns>Integer position of card in cards.dll.</returns>
		public static int CardDLLPosition(Card card)
		{
			return CardDLLPosition(card.Rank, card.Suit);
		}

		/// <summary>
		/// Return a new Card which represents the card in a given position of the "cards.dll."
		/// </summary>
		/// <param name="position">Position in the "cards.dll" to create a card from.</param>
		/// <returns>A new Card which is the same as the card in the position in the "cards.dll"</returns>
		public static Card CardInDLLPosition(int position)
		{
			int suit = position % 4;
			int rank = (position-suit) / 4;

            return new Card((CardRank)rank, (CardSuit)suit);
		}

		#endregion

	} // End class.
} // End namespace.