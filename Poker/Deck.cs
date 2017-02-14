using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker
{
	public class Deck
	{
		private List<Card> _cards = new List<Card>();

		public Card[] Cards
		{
			get
			{
				return _cards.ToArray();
			}
		}

		public Deck()
		{
			//create a stacked deck
			foreach (CardSuits suit in Enum.GetValues(typeof(CardSuits)))
			{
				foreach (CardTypes type in Enum.GetValues(typeof(CardTypes)))
					_cards.Add(new Card(type, suit));
			}
		}

		public void Shuffle()
		{
			Random rnd = new Random();
			int shuffleCnt = 0;
			while (shuffleCnt < 25) //shuffle the deck a bunch
			{
				List<Card> temp = new List<Card>(_cards);
				_cards.Clear();

				while (temp.Count > 0)
				{
					int idx = rnd.Next(0, temp.Count - 1);
					Card c = temp[idx];
					temp.RemoveAt(idx);
					_cards.Add(c);
				}
				shuffleCnt++;
			}
		}

		public Hand[] Deal(int numHands, int numCards)
		{
			
			if (numHands == 0 || numCards == 0)
				throw new ArgumentException("Num Hands and Num Cards must be > 0");

			Hand[] hands = new Hand[numHands];

			for (int x = 0; x < numCards; x++)
			{
				for (int y = 0; y < numHands; y++)
				{
					if (hands[y] == null)
						hands[y] = new Hand();
					Card c = TakeCards(1).FirstOrDefault();
					if (c == null)
						throw new ArgumentOutOfRangeException("The deck is out of cards!");
					hands[y].AddCard(c);
				}
			}

			return hands;
		}

		public Card[] TakeCards(int numCards)
		{
			Card[] cards = _cards.Take(numCards).ToArray();
			_cards.RemoveRange(0, numCards);
			return cards;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (Card c in _cards)
				sb.AppendLine(c.ToString());
			return sb.ToString();
		}
	}
}
