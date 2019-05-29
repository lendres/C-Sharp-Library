using System;
using System.Collections;

namespace DigitalProduction.Gaming
{
	#region Enumerations.

	/// <summary>
	/// The suit of a cards.
	/// </summary>
	public enum CardSuit : int
	{
		/// <summary>Suit of the card is Clubs</summary>
		Clubs,

		/// <summary>Suit of the card is Diamonds.</summary>
		Diamonds,

		/// <summary>Suit of the card is Hearts.</summary>
		Hearts,

		/// <summary>Suit of the card is Spades.</summary>
		Spades
	}

	/// <summary>
	/// The rank (face value) of cards.
	/// </summary>
	public enum CardRank : int
	{
		/// <summary>The rank (face value) of the card is 1</summary>
		Ace,

		/// <summary>The rank (face value) of the card is 2</summary>
		Two,

		/// <summary>The rank (face value) of the card is 3</summary>
		Three,

		/// <summary>The rank (face value) of the card is 4</summary>
		Four,

		/// <summary>The rank (face value) of the card is 5</summary>
		Five,

		/// <summary>The rank (face value) of the card is 6</summary>
		Six,

		/// <summary>The rank (face value) of the card is 7</summary>
		Seven,

		/// <summary>The rank (face value) of the card is 8</summary>
		Eight,

		/// <summary>The rank (face value) of the card is 9</summary>
		Nine,

		/// <summary>The rank (face value) of the card is 10</summary>
		Ten,

		/// <summary>The rank (face value) of the card is Jack</summary>
		Jack,

		/// <summary>The rank (face value) of the card is Queen</summary>
		Queen,

		/// <summary>The rank (face value) of the card is King</summary>
		King,

		/// <summary>The rank of the card is not valid.</summary>
		End
	}

	#endregion

	/// <summary>
	/// A simple class for holding a card.  Card face values are based on the rules of Blackjack.
	/// Derive a new class from this class is you need different values.
	/// </summary>
	public class Card
	{
		#region Members

		private CardRank	_rank;		// Face value of the card.
		private CardSuit	_suit;		// Suit of the card.

		/// <summary>
		/// Array which is used to convert the face value of the card to a numeric value.  If these 
		/// are not the values for what ever type of game you are using this for then over write these
		/// in the derived class.  Must also override Value property.
		/// </summary>
		protected static int[] _cardvalues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Card()
		{
			_rank = CardRank.Ace;
			_suit = CardSuit.Clubs;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cardrank">Rank of card (face value).</param>
		/// <param name="cardsuit">Suit of card.</param>
		public Card(CardRank cardrank, CardSuit cardsuit)
		{
			_rank = cardrank;
			_suit = cardsuit;
		}

		#endregion

		#region Properties

		/// <value>
		/// Get the rank of the card.
		/// </value>
		public CardRank Rank
		{
			get
			{
				return _rank;
			}
		}

		/// <value>
		/// Get the suit of the card.
		/// </value>
		public CardSuit Suit
		{
			get
			{
				return _suit;
			}
		}

		/// <value>
		/// Return the card's value based on the default card values (based on Blackjack rules).
		/// </value>
		public int Value
		{
			get
			{
				return _cardvalues[(int)_rank];
			}
		}

		#endregion

		#region Static Properties

		/// <summary>
		/// The values (points) associated with the cards.
		/// </summary>
		public static int[] CardValues
		{
			get
			{
				return _cardvalues;
			}

			set
			{
				if (value.Length != 13)
				{
					throw new ArgumentException("Error setting the card values, the supplied array is the wrong length.");
				}

				_cardvalues = value;
			}
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Convert a string to a CardSuit.  CardSuit associated with CardSuit if found, otherwise CardSuit.Clubs.
		/// </summary>
		/// <param name="cardsuit">String which represents the CardSuit.</param>
		public static CardSuit GetCardSuit(string cardsuit)
		{
			for (CardSuit i = DigitalProduction.Gaming.CardSuit.Clubs; i <= DigitalProduction.Gaming.CardSuit.Spades; i++)
			{
				if (i.ToString() == cardsuit)
				{
					return i;
				}
			}

			return DigitalProduction.Gaming.CardSuit.Clubs;
		}

		/// <summary>
		/// Convert a string to a CardRank.  CardRank associated with CardRank if found, otherwise CardRank.Ace.
		/// </summary>
		/// <param name="cardrank">String which represents the CardRank.</param>
		public static CardRank GetCardRank(string cardrank)
		{
			for (CardRank i = DigitalProduction.Gaming.CardRank.Ace; i <= DigitalProduction.Gaming.CardRank.Ten; i++)
			{
				if (i.ToString() == cardrank)
				{
					return i;
				}
			}

			return CardRank.End;
		}

		/// <summary>
		/// Convert a card rank to a integer value.
		/// </summary>
		/// <param name="cardrank">CardRank to find the value of.</param>
		/// <returns>An integer which represents the value of the card.</returns>
		public static int GetCardValue(CardRank cardrank)
		{
			return _cardvalues[(int)cardrank];
		}

		/// <summary>
		/// Get the CardRank of a card based on the value of the card.  CardRank of CardValue if found, CardRank.End if not found.
		/// </summary>
		/// <param name="cardvalue">Value of card.</param>
		public static CardRank GetCardRank(int cardvalue)
		{
			for (int i = 0; i < _cardvalues.Length; i++)
			{
				if (cardvalue == _cardvalues[i])
				{
					return (CardRank)i;
				}
			}
			return CardRank.End;
		}

		/// <summary>
		/// Used to create "decks" of cards.  Cards are created in order and must be "shuffled."
		/// </summary>
		/// <param name="cards">Array to put cards in as they are created.</param>
		/// <param name="number_of_decks">Number of decks to created.</param>
		public static void CreateCards(ref ArrayList cards, int number_of_decks)
		{
			cards = new ArrayList();

			for (int deck = 0; deck < number_of_decks; deck++)
			{
				for (CardSuit suit = DigitalProduction.Gaming.CardSuit.Clubs; suit <= DigitalProduction.Gaming.CardSuit.Spades; suit++)
				{
					for (CardRank rank = DigitalProduction.Gaming.CardRank.Ace; rank <= DigitalProduction.Gaming.CardRank.King; rank++)
					{
						cards.Add(new Card(rank, suit));
					}
				}
			}
		}

		/// <summary>
		/// Used to create Spanish "decks" of cards.  Spanish decks do not have the 10's in them.
		/// Cards are created in order and must be "shuffled."
		/// </summary>
		/// <param name="cards">Array to put cards in as they are created.</param>
		/// <param name="number_of_decks">Number of decks to created.</param>
		public static void CreateSpanishCards(ref ArrayList cards, int number_of_decks)
		{
			cards = new ArrayList();

			for (int deck = 0; deck < number_of_decks; deck++)
			{
				for (CardSuit suit = DigitalProduction.Gaming.CardSuit.Clubs; suit <= DigitalProduction.Gaming.CardSuit.Spades; suit++)
				{
					for (CardRank rank = DigitalProduction.Gaming.CardRank.Ace; rank <= DigitalProduction.Gaming.CardRank.King; rank++)
					{
						if (rank != DigitalProduction.Gaming.CardRank.Ten)
						{
							cards.Add(new Card(rank, suit));
						}
					}
				}
			}
		}

		#endregion

		#region Hash Code

		/// <summary>
		/// Hash code.
		/// </summary>
		public override int GetHashCode()
		{
			return _rank.GetHashCode() ^ _suit.GetHashCode();
		}

		#endregion

	} // End class.
} // End namespace.