using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker
{
	public class Result
	{
		public BestHand BestHand { get; private set; }

		readonly List<Card> _bestCards = new List<Card>();
		public Card[] BestCards
		{
			get
			{
				return _bestCards.ToArray();
			}
		}

		public int Points
		{
			get
			{
				if (_bestCards.Count == 0)
					return 0;
				return _bestCards.Sum(c => (int)c.Type);
			}
		}
		
		public Result(BestHand bestHand, List<Card> bestCards)
		{
			this.BestHand = bestHand;
			this._bestCards = bestCards;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (Card c in _bestCards)
				sb.AppendLine(c.ToString());
			return string.Format("{0}\r\n{1}", BestHand, sb);
		}
	}
}