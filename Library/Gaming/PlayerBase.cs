using System;

namespace DigitalProduction.Gaming
{
	/// <summary>
	/// A Blackjack player.  The player is responsible for decision making for the hand.  It also
	/// contains the money the player has and a holder for the stats of the player.
	/// </summary>
	abstract public class PlayerBase<T> where T : struct
	{
		#region Members / Variables

		/// <summary>The player's score.</summary>
		protected T				_score;
		private string			_name;

		#endregion

		#region Construction / Destruction.

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="score">Amount of money the player starts with.</param>
		/// <param name="name">Name of player.</param>
		public PlayerBase(T score, string name)
		{
			_score		= score;
			_name		= name;
		}

		/// <summary>
		/// Constructor, name is set to "Anonymouns."
		/// </summary>
		/// <param name="score">Amount of money the player starts with.</param>
		public PlayerBase(T score)
		{
			_score		= score;
			_name		= "Anonymous";
		}

		/// <summary>
		/// Constructor, name is set to "Anonymous" and money is set to 10,000.
		/// </summary>
		public PlayerBase()
		{
			_score		= default(T);
			_name		= "Anonymous";
		}

		#endregion

		#region Properties.

		/// <value>
		/// Name of the player.  Read/write.
		/// </value>
		public string Name
		{
			get
			{
				return new string(_name.ToCharArray());
			}
			set
			{
				_name = value;
			}
		}

		/// <value>
		/// Player's score.  Read/write.
		/// </value>
		public T Score
		{
			get
			{
				return _score;
			}
			set
			{
				_score = value;
			}
		}

		#endregion

		#region Functions.
		#endregion

	} // End class.
} // End name space.