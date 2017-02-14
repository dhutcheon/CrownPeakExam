using NUnit.Framework;
using System;
using Numbers2Words;
using Poker;
using System.Linq;
using System.Collections.Generic;

namespace Tests
{
	[TestFixture()]
	public class PokerTest
	{
		readonly IEnumerable<CardTypes> CARD_TYPES = Enum.GetValues(typeof(CardTypes)).Cast<CardTypes>();

		[Test]
		public void CheckCardConstructor()
		{
			Card card = new Card(CardTypes.Ace, CardSuits.Spades);
			Assert.AreEqual(CardTypes.Ace, card.Type);
			Assert.AreEqual(CardSuits.Spades, card.Suit);
		}

		[Test]
		public void CheckDeckConstructor()
		{
			Deck deck = new Deck();

			Assert.AreEqual(52, deck.Cards.Length);

			//check for duplicate cards
			var dupes = deck.Cards.GroupBy(c => new { c.Suit, c.Type })
									  .Where(c => c.Skip(1).Any()).Any();
			Assert.IsFalse(dupes);
		}

		[Test]
		public void Shuffle()
		{
			Deck deck1 = new Deck();
			deck1.Shuffle();

			Deck deck2 = new Deck();

			bool shuffled = !deck1.Cards.SequenceEqual(deck2.Cards);
			Console.WriteLine(deck1);
			Console.WriteLine(deck2);

			Assert.AreEqual(52, deck1.Cards.Length);
			Assert.AreEqual(52, deck2.Cards.Length);
			Assert.IsTrue(shuffled);
		}

		[Test]
		public void Deal()
		{
			Deck deck = new Deck();
			deck.Shuffle();

			Hand[] hands = deck.Deal(2, 5);
			Assert.IsTrue(hands.Length == 2);
			Assert.IsTrue(hands.First().Cards.Length == 5);
			Assert.IsTrue(deck.Cards.Length == 42);
		}

		[Test]
		public void SortHand()
		{
			Card c1 = new Card(CardTypes.Seven, CardSuits.Clubs);
			Card c2 = new Card(CardTypes.Ace, CardSuits.Spades);
			Card c3 = new Card(CardTypes.Five, CardSuits.Clubs);
			Card c4 = new Card(CardTypes.Seven, CardSuits.Hearts);
			Card c5 = new Card(CardTypes.Seven, CardSuits.Diamonds);
			Card c6 = new Card(CardTypes.Two, CardSuits.Clubs);
			Card c7 = new Card(CardTypes.King, CardSuits.Diamonds);

			Hand hand = new Hand();
			hand.AddCard(c1);
			hand.AddCard(c2);
			hand.AddCard(c3);
			hand.AddCard(c4);
			hand.AddCard(c5);
			hand.AddCard(c6);
			hand.AddCard(c7);

			Card[] sorted = new Card[7];
			sorted[6] = c6;
			sorted[5] = c3;
			sorted[4] = c4;
			sorted[3] = c5;
			sorted[2] = c1;
			sorted[1] = c7;
			sorted[0] = c2;

			hand.Sort();
			Console.WriteLine(hand);
			for (int i = 0; i < 7; i++)
			{
				Card expected = sorted[i];
				Card actual = hand.Cards[i];
				Console.WriteLine(actual + " => " + expected);
				Assert.IsTrue(expected.Equals(actual));
			}
		}

		[Test]
		public void AceHigh()
		{
			RunBestHandTest(BestHand.HighCard, CardTypes.Ace, 1, 5);
		}

		[Test]
		public void Pair()
		{
			RunBestHandTest(BestHand.Pair, CardTypes.Five, 2, 5);
		}

		[Test]
		public void TwoPair()
		{
			Hand h = BuildTestHand(CardTypes.Four, 2, 5);
			//get a type that isn't in the hand to add to it
			CardTypes type = CARD_TYPES.SkipWhile((CardTypes t) => h.Cards.Any(c => c.Type == t)).First();
			h.AddCard(new Card(type, CardSuits.Clubs));
			h.AddCard(new Card(type, CardSuits.Diamonds));

			Result r = h.Result;

			Console.WriteLine(h);
			Console.WriteLine(r);

			Assert.AreEqual(BestHand.TwoPair, r.BestHand);
			Assert.IsTrue(r.BestCards.Count() == 4);
		}


		[Test]
		public void ThreeOfAKind()
		{
			RunBestHandTest(BestHand.ThreeOfAKind, CardTypes.Queen, 3, 5);
		}

		[Test]
		public void FullHouse()
		{
			Hand h = BuildTestHand(CardTypes.Jack, 3, 5);
			//get a type that isn't in the hand to add to it
			CardTypes type = CARD_TYPES.SkipWhile((CardTypes t) => h.Cards.Any(c => c.Type == t)).First();
			h.AddCard(new Card(type, CardSuits.Clubs));
			h.AddCard(new Card(type, CardSuits.Diamonds));

			Result r = h.Result;

			Console.WriteLine(h);
			Console.WriteLine(r);

			Assert.AreEqual(BestHand.FullHouse, r.BestHand);
			Assert.IsTrue(r.BestCards.Count() == 5);
		}

		[Test]
		public void FourOfAKind()
		{
			RunBestHandTest(BestHand.FourOfAKind, CardTypes.Two, 4, 7);
		}

		[Test]
		public void Flush()
		{
			Hand h = BuildTestHand(CardTypes.Ace, 1, 1);
			CardSuits suit = h.Cards[0].Suit;
			while (h.Cards.Count() < 5)
			{
				//get a type that isn't in the hand to add to it
				CardTypes type = CARD_TYPES.SkipWhile((CardTypes t) => h.Cards.Any(c => c.Type == t)).First();
				h.AddCard(new Card(type, suit));
			}
			RunStraightFlushAssertions(h, BestHand.Flush, false, true);
		}

		[Test]
		public void StraightFlush()
		{
			Hand h = new Hand();
			while (h.Cards.Count() < 5)
			{
				//get a type that isn't in the hand to add to it
				CardTypes type = CARD_TYPES.SkipWhile((CardTypes t) => h.Cards.Any(c => c.Type == t)).First();
				h.AddCard(new Card(type, CardSuits.Spades));
			}
			RunStraightFlushAssertions(h, BestHand.StraightFlush, true, true);
		}

		[Test]
		public void RoyalFlush()
		{
			Hand h = BuildTestHand(CardTypes.Ace, 1, 1);
			CardSuits suit = h.Cards[0].Suit;
			while (h.Cards.Count() < 5)
			{
				//get a type that isn't in the hand to add to it
				CardTypes type = CARD_TYPES.Reverse().SkipWhile((CardTypes t) => h.Cards.Any(c => c.Type == t)).First();
				h.AddCard(new Card(type, suit));
			}
			RunStraightFlushAssertions(h, BestHand.RoyalFlush, true, true);
		}

		[Test]
		public void Straight()
		{
			Hand h = BuildTestHand(CardTypes.Two, 1, 1);
			while (h.Cards.Count() < 7)
			{
				//get a type that isn't in the hand to add to it
				CardTypes type = CARD_TYPES.SkipWhile((CardTypes t) => h.Cards.Any(c => c.Type == t)).First();
				CardSuits suit = ((int)type % 2 == 0) ? CardSuits.Clubs : CardSuits.Diamonds;
				h.AddCard(new Card(type, suit));
			}
			RunStraightFlushAssertions(h, BestHand.Straight, true, false);
		}

		void RunStraightFlushAssertions(Hand h, BestHand bestHand, bool straight, bool flush)
		{
			Result r = h.Result;

			Console.WriteLine(h);
			Console.WriteLine(r);

			Assert.AreEqual(bestHand, r.BestHand);
			Assert.IsTrue(r.BestCards.Count() == 5);

			if (straight)
			{
				Card prev = null;
				foreach (Card c in r.BestCards)
				{
					if (prev != null)
						Assert.AreEqual(prev.Type - 1, c.Type);
					prev = c;
				}
			}

			if (flush)
				Assert.IsTrue(r.BestCards.GroupBy(c => c.Suit).Distinct().Count() == 1);
		}


		void RunBestHandTest(BestHand bestHand, CardTypes bestType, int qty, int handSize)
		{
			Hand h = BuildTestHand(bestType, qty, handSize);
			Result r = h.Result;

			Console.WriteLine(h);
			Console.WriteLine(r);

			Assert.AreEqual(bestHand, r.BestHand);
			Assert.IsTrue(r.BestCards.Count() == qty);
			Assert.IsTrue(r.BestCards.All(c => c.Type == bestType));

		}


		Hand BuildTestHand(CardTypes bestType, int qty, int handSize)
		{
			Hand h = new Hand();
			var suits = Enum.GetValues(typeof(CardSuits)).Cast<CardSuits>();
			for (int i = 0; i < qty; i++)
			{
				CardSuits suit = suits.ElementAt(i);
				Card c = new Card(bestType, suit);
				h.AddCard(c);
			}

			Deck d = new Deck();
			d.Shuffle();
			while (h.Cards.Length < handSize)
			{
				Card c = d.TakeCards(1).First();
				if (h.Cards.Contains(c))
					continue;
				//if (c.Type >= bestType && qty < 4)
				//	continue;
				if (qty < 4 && h.Cards.Where(crd => crd.Type == c.Type).Any()) //don't add the same card of a different suit
					continue;
				h.AddCard(c);
			}

			return h;
		}

		void AssertEquals(string actual, string expected)
		{
			Console.WriteLine(actual + " => " + expected);
			Assert.AreEqual(expected, actual);
		}
	}
}
