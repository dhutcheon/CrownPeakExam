using NUnit.Framework;
using System;
using Numbers2Words;

namespace Tests
{
	[TestFixture()]
	public class Numbers2WordsTest
	{
		[Test()]
		public void IntZeroToNineteen()
		{
			for (int i = 0; i < 20; i++)
			{
				string actual = Number2Word.Parse(i);
				string expected = WordConstants.ZERO_TO_19[i];
				AssertEquals(actual, expected, i, i);
			}
		}

		[Test()]
		public void IntTwentyToNinetyBy10()
		{
			for (int i = 0; i < 10; i++)
			{
				int num = i * 10;
				string actual = Number2Word.Parse(num);
				string expected = WordConstants.TENS[i];

				AssertEquals(actual, expected, num, i);
			}
		}

		[Test()]
		public void IntTwentyToNinetyNine()
		{
			for (int i = 0; i < 100; i++)
			{
				string actual = Number2Word.Parse(i);

				//choose some numbers at random
				switch (i)
				{
					case 5:
						AssertEquals(actual, "five", i, i);
						break;
					case 18:
						AssertEquals(actual, "eighteen", i, i);
						break;
					case 42:
						AssertEquals(actual, "forty two", i, i);
						break;
					case 74:
						AssertEquals(actual, "seventy four", i, i);
						break;
					default:
						Console.WriteLine(i + " : " + actual);// + expected);
						break;
				}
			}
		}

		[Test()]
		public void IntOneHundredToNineHundredNinetyNine()
		{
			for (int i = 100; i < 1000; i++)
			{
				string actual = Number2Word.Parse(i);

				//choose some numbers at random
				switch (i)
				{
					case 500:
						AssertEquals(actual, "five hundred", i, i);
						break;
					case 701:
						AssertEquals(actual, "seven hundred one", i, i);
						break;
					case 999:
						AssertEquals(actual, "nine hundred ninety nine", i, i);
						break;
					default:
						Console.WriteLine(i + " : " + actual);// + expected);
						break;
				}
			}
		}

		[Test()]
		public void Int1000To999000By1000()
		{
			for (int i = 1000; i < 1000000; i = i + 1000)
			{
				string actual = Number2Word.Parse(i);

				//choose some numbers at random
				switch (i)
				{
					case 6000:
						AssertEquals(actual, "six thousand", i, i);
						break;
					case 50000:
						AssertEquals(actual, "fifty thousand", i, i);
						break;
					case 100000:
						AssertEquals(actual, "one hundred thousand", i, i);
						break;
					case 880000:
						AssertEquals(actual, "eight hundred eighty thousand", i, i);
						break;
					default:
						Console.WriteLine(i + " : " + actual);// + expected);
						break;
				}
			}
		}


		[Test()]
		public void Int1000To10000()
		{
			for (int i = 1000; i < 1000000; i++)
			{
				string actual = Number2Word.Parse(i);

				//choose some numbers at random
				switch (i)
				{
					case 6000:
						AssertEquals(actual, "six thousand", i, i);
						break;
					case 4052:
						AssertEquals(actual, "four thousand fifty two", i, i);
						break;
					case 8999:
						AssertEquals(actual, "eight thousand nine hundred ninety nine", i, i);
						break;
					default:
						//Console.WriteLine(i + " : " + actual);// + expected);
						break;
				}
			}
		}

		[Test]
		public void IntMillionPlus()
		{
			string actual = string.Empty;
			string expected = string.Empty;

			actual = Number2Word.Parse(1000000);
			expected = "one million";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(5000000000);
			expected = "five billion";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(8000000000000);
			expected = "eight trillion";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(721537840);
			expected = "seven hundred twenty one million five hundred thirty seven thousand eight hundred forty";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Negatives()
		{
			string actual = Number2Word.Parse(-421);
			string expected = "negative four hundred twenty one";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Tenths()
		{
			string actual = Number2Word.Parse(.4);
			string expected = "four tenths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(11.5);
			expected = "eleven and five tenths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(123456.7);
			expected = "one hundred twenty three thousand four hundred fifty six and seven tenths";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Hundredths()
		{
			string actual = Number2Word.Parse(.40);
			string expected = "four tenths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(.45);
			expected = "forty five hundredths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(11.75);
			expected = "eleven and seventy five hundredths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(123456.78);
			expected = "one hundred twenty three thousand four hundred fifty six and seventy eight hundredths";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Thousandths()
		{
			string actual = Number2Word.Parse(.400);
			string expected = "four tenths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(.456);
			expected = "four hundred fifty six thousandths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(11.075);
			expected = "eleven and seventy five thousandths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(123456.789);
			expected = "one hundred twenty three thousand four hundred fifty six and seven hundred eighty nine thousandths";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TenThousandths()
		{
			string actual = Number2Word.Parse(.4567);
			string expected = "four thousand five hundred sixty seven ten thousandths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(11.0005);
			expected = "eleven and five ten thousandths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(11.00054533);
			expected = "eleven and five ten thousandths";
			Assert.AreEqual(expected, actual);

			actual = Number2Word.Parse(456.7891);
			expected = "four hundred fifty six and seven thousand eight hundred ninety one ten thousandths";
			Assert.AreEqual(expected, actual);
		}

		private void AssertEquals(string actual, string expected, int num, int idx)
		{
			Console.WriteLine(idx + " " + num + ":" + actual + " => " + expected);
			Assert.AreEqual(expected, actual);
		}
	}
}
