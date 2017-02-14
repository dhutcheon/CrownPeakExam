using System;
using System.Linq;
using Numbers2Words;
using Poker;

namespace ConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			int opt = 0;
			bool invalidOpt = false;

			do
			{
				Console.WriteLine();
				Console.WriteLine("Select an exam option:");
				Console.WriteLine("  1. Numbers To Words");
				Console.WriteLine("  2. Cards");
				Console.WriteLine();

				invalidOpt = false;
				opt = GetUserInput("Option? (1 or 2)", 0, true);
				Console.WriteLine();

				switch (opt)
				{
					case 1:
						//numbers to words
						Random rnd = new Random();
						double rndNum = rnd.Next();
						double number = GetUserInput("Enter your number (or press enter to try " + rndNum + ")", rndNum, false);
						string word = Number2Word.Parse(number);
						Console.WriteLine();
						Console.WriteLine(number + ": " + word);
						break;

					case 2:
						//poker
						bool anotherRound = true;
						do
						{
							invalidOpt = false;

							int numHands = GetUserInput("How many hands do you want to deal (max 9)", 3, true);
							if (numHands < 1 || numHands > 9)
							{
								Console.WriteLine("Enter a number from 1-9");
								invalidOpt = true;
								continue;
							}

							int numCards = GetUserInput("How many cards per hand (max 9)", 5, true);
							if (numCards < 1 || numCards > 9)
							{
								Console.WriteLine("Enter a number from 1-9");
								invalidOpt = true;
								continue;
							}

							if (numCards * numHands > 52)
							{
								Console.WriteLine("Settle down.....I can't deal more than 52 cards.");
								invalidOpt = true;
								continue;
							}


							Console.WriteLine();

							Deck d = new Deck();
							d.Shuffle();
							Hand[] hands = d.Deal(numHands, numCards);
							BestHand bestOfTheBest = hands.Max(h => h.Result.BestHand);
							var winners = hands.Where(h => h.Result.BestHand == bestOfTheBest);
							int maxPts = 0;
							foreach (Hand h in winners)
							{
								if (h.Result.Points > maxPts)
									maxPts = h.Result.Points;
							}

							for (int i = 0; i < hands.Length; i++)
							{
								Hand h = hands[i];
								Result r = h.Result;
								bool winningHand = h.Result.BestHand == bestOfTheBest && h.Result.Points == maxPts;
								string winner = (winningHand) ? " WINNER!" : "";
								Console.WriteLine("POKER HAND " + (i + 1) + winner);
								Console.WriteLine("===============================");
								Console.WriteLine(h);
								Console.WriteLine(r);
							}

							anotherRound = GetUserInput<string>("Another Round? (y/n)", null, true).ToUpper() == "Y";
						}
						while (invalidOpt || anotherRound);
						break;

					default:
						Console.WriteLine("Not a valid option");
						invalidOpt = true;
						break;
				}

				Console.WriteLine();    

			} while (invalidOpt || GetUserInput<string>("Would you like to quit the exam? (y/n)", null, true).ToUpper() != "Y");

			Console.WriteLine();
			Console.WriteLine("Press <ENTER> to continue.");
			Console.ReadLine();
		}

		static string GetUserInput(string prompt, string defaultVal) { return GetUserInput<string>(prompt, defaultVal, false); }
		static T GetUserInput<T>(string prompt, T defaultVal, bool singleKey)
		{
			while (true)
			{
				//prompt = (defaultVal == null) ? prompt : prompt + " (" + defaultVal + ")";
				Console.Write(prompt + " ");
				string input = (singleKey) ? Console.ReadKey().KeyChar.ToString() : Console.ReadLine();
				Object retVal = null;
				if (defaultVal != null && string.IsNullOrEmpty(input))
					retVal = defaultVal;
				else if (!string.IsNullOrWhiteSpace(input))
					retVal = input.Trim();
				if (retVal != null)
				{
					try
					{
						Console.WriteLine();
						return (T)Convert.ChangeType(retVal, typeof(T));
					}
					catch (Exception ex) 
					{
						//Console.WriteLine(ex);
					}
				}

				Console.WriteLine("Invalid input, try again");
			}
		}
	}
}
