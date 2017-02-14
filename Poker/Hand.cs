using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;

namespace Poker
{
	public class Hand
	{
		List<Card> _cards = new List<Card>();

		public Card[] Cards
		{
			get
			{
				return _cards.ToArray();
			}
		}

		Result _result = null;
		public Result Result
		{
			get { return _result ?? (_result = GetResult()); }
		}


		public Hand() { }

		public void AddCard(Card c)
		{
			_cards.Add(c);
		}

		public Card RemoveCard(int idx)
		{
			Card c = _cards[idx];
			_cards.RemoveAt(idx);
			return c;
		}

		public void Sort()
		{
			
			var sorted = _cards.AsParallel()
			                   .OrderByDescending(c => (int)c.Type)
			                   .ThenByDescending(c => (int)c.Suit)
			                   .GroupBy(c => c.Type)
							   .SelectMany(c => c);
			_cards = sorted.ToList();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (Card c in _cards)
				sb.AppendLine(c.ToString());
			return sb.ToString();
		}
		
		Result GetResult()
		{
			this.Sort();

			Result result = CheckStraightAndFlush();
			if (result != null)
				return result;
			    
			return CheckCardGroups();
		}

		Result CheckStraightAndFlush()
		{
			BestHand bestHand = BestHand.HighCard;
			List<Card> bestCards = new List<Card>();

			var flushGroups = _cards.GroupBy(c => c.Suit)
									.Select(grp => Tuple.Create(grp.ToList(), grp.Count()))
									.Where(grp => grp.Item2 >= 5)
									.Select((arg1, arg2) => arg1.Item1).ToList();
			bool hasFlush = flushGroups.Count() > 0;

			//not sure on a LINQ query that checks ordered sequences, checking "old school" way
			List<Card> straight = new List<Card>();
			Card next = null;
			for (int i = 0; i < _cards.Count - 1; i++)
			{
				Card c = _cards[i];
				next = _cards[i + 1];
				if (c.Type - 1 == next.Type)
				{
					straight.Add(c);
				}
				else
				{
					straight.Clear();
					next = null;
				}
			}
			if (next != null)
				straight.Add(next); //add the last item if we are bailing out of for loop
			bool hasStraight = straight.Count >= 5;

			if (!hasFlush && !hasStraight)
				return null; //bail and check card groups

			if (hasFlush && hasStraight) //straight flush, royal flush
			{
				bestCards.AddRange(straight.Take(5));

				//first card should be an ace on royal flush, otherwise straight flush
				if (straight[0].Type == CardTypes.Ace)
				{
					bestHand = BestHand.RoyalFlush;
				}
				else
				{
					bestHand = BestHand.StraightFlush;
				}
			}
			else if (hasFlush) //flush
			{
				bestCards.AddRange(flushGroups.First().Take(5));
				bestHand = BestHand.Flush;
			}
			else //straight
			{
				bestCards.AddRange(straight.Take(5));
				bestHand = BestHand.Straight;
			}

			return new Result(bestHand, bestCards);
		}

		Result CheckCardGroups()
		{
			BestHand bestHand = BestHand.HighCard;
			List<Card> bestCards = new List<Card>();

			var typeGroups = _cards.GroupBy(c => c.Type)
									   .Select(grp => Tuple.Create(grp.ToList(), grp.Count()));
			int maxGrpSize = typeGroups.Max(grp => grp.Item2);
			if (maxGrpSize == 1) //high card
			{
				bestCards.Add(_cards.First()); //cards are sorted so take the first card
			}
			if (maxGrpSize == 2) //pairs and two pairs
			{
				var bestGrp = GetBestGroups(typeGroups, 2, 2);
				//check for two pair here
				if (bestGrp.Count > 1)
				{
					bestCards.AddRange(bestGrp.SelectMany((List<Card> arg) => arg));
					bestHand = BestHand.TwoPair;
				}
				else
				{
					bestCards.AddRange(bestGrp.First());
					bestHand = BestHand.Pair;
				}
			}
			if (maxGrpSize == 3) //3 of a kind and full house
			{
				var bestGrp = GetBestGroups(typeGroups, 3, 1);
				//check for FH here
				if (typeGroups.Any((arg) => arg.Item2 == 2))
				{
					bestCards.AddRange(bestGrp.First());
					bestGrp = GetBestGroups(typeGroups, 2, 1);
					bestCards.AddRange(bestGrp.First());
					bestHand = BestHand.FullHouse;
				}
				else
				{
					bestCards.AddRange(bestGrp.First());
					bestHand = BestHand.ThreeOfAKind;
				}
			}
			if (maxGrpSize == 4) //4 of a kind
			{
				var bestGrp = GetBestGroups(typeGroups, 4, 1);
				//check for FH here

				bestCards.AddRange(bestGrp.First());
				bestHand = BestHand.FourOfAKind;
			}

			return new Result(bestHand, bestCards);
		}

		//find groups by size and take n number of highest ranking group(s)
		List<List<Card>> GetBestGroups(IEnumerable<Tuple<List<Card>, int>> typeGroups, int grpSize, int take)
		{
			var bestGrp = typeGroups.Where(grp => grp.Item2 == grpSize);
			return bestGrp.Select((arg1, arg2) => arg1.Item1).Take(take).ToList();
		}
	}
}
