using System;

namespace Poker
{
	public class Card
	{
		public CardTypes Type { get; private set; }
		public CardSuits Suit { get; private set; }

		public Card(CardTypes type, CardSuits suit)
		{
			Type = type;
			Suit = suit;
		}

		public override bool Equals(object obj)
		{
			if (obj is Card)
			{
				Card c = obj as Card;
				return c.Suit == this.Suit && c.Type == this.Type;
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", Type, Suit);
		}
	}
}
