using System;

namespace Poker
{
	public enum BestHand { HighCard = 1, Pair = 2, TwoPair = 3, ThreeOfAKind = 4, Straight = 5, Flush = 6, FullHouse = 7, FourOfAKind = 8, StraightFlush = 9, RoyalFlush = 10 }
	public enum CardTypes { Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13, Ace = 14 }
	public enum CardSuits { Hearts, Diamonds, Spades, Clubs }

	public static class CardConstants { }
}
