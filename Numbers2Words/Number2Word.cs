using System;
using System.Text;

namespace Numbers2Words
{
	public static class Number2Word
	{
		public static string Parse(double num)
		{
			if (num > 999999999999999)
				throw new ArgumentException("Chill out, I don't support > 999 quadrillion");
			
			string str = num.ToString();

			bool isNegative = (num < 0);

			//determine number of numbers
			if (isNegative)
				str = str.Remove(0, 1); //strip the negative sign

			//get some dimensions for the number
			int lCnt = 0; 	    //left of decimal
			int rCnt = 0;		//right of decimal
			int decIdx = -1;	//decimal index
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				//check for decimal place
				if (c == '.')
					decIdx = i;
				else if (decIdx > -1)
					rCnt++;
				else
					lCnt++;
			}
			bool isDecimal = decIdx > -1;
			string wholeVal = str.Substring(0, lCnt);

			//if this is a decimal between 0 and 1, drop the zero at the beginning
			StringBuilder word = (wholeVal == "0" && isDecimal) ? new StringBuilder() : BuildNumberFromString(wholeVal);

			if (isNegative)
				word.Insert(0, "negative ");

			if(!isDecimal)
				return word.ToString();

			//handle decimal values
			AppendDecimal(word, str, rCnt, decIdx);
			return word.ToString();
		}

		static void AppendDecimal(StringBuilder word, string str, int rCnt, int decIdx)
		{
			//truncate anything past ten thousandths place for now
			if (rCnt > 4)
				rCnt = 4;

			string decPlace = WordConstants.DECIMALS[rCnt - 1];
			string decVal = str.Substring(decIdx + 1, rCnt);
			//strip any leading zeros
			while (decVal.StartsWith("0"))
				decVal = decVal.Remove(0, 1);

			StringBuilder dec = BuildNumberFromString(decVal);
			if (word.Length > 0)
				word.Append(" and ");
			word.Append(dec + " " + decPlace);
		}

		static StringBuilder BuildNumberFromString(string str)
		{
			StringBuilder word = new StringBuilder();
			string triple = string.Empty;
			int denom = 0;
			for (int i = str.Length - 1; i >= 0; i--)
			{
				char c = str[i];
				triple = c + triple;
				if (triple.Length == 3)
				{
					HandleTriple(word, denom, triple);

					triple = "";
					denom++;
				}
			}

			HandleTriple(word, denom, triple);
			return word;
		}

		static void HandleTriple(StringBuilder word, int denom, string triple)
		{
			if (triple == string.Empty)
				return;
			if (triple == "000") //ignore any triples with all 0's
				return;
			
			int val = Int16.Parse(triple);
			StringBuilder newWord = GetNumber(val);
			if (denom > 0)
			{
				string denomWord = WordConstants.DENOMINATIONS[denom];
				newWord.Append(" " + denomWord);
				if (word.Length > 0) // add a space
					newWord.Append(" ");
			}

			word.Insert(0, newWord);
		}

		static StringBuilder GetNumber(int num)
		{
			StringBuilder word = new StringBuilder();
			if (num > 999 || num < 0)
				throw new ArgumentException("Value " + num + " > 999 or < 0");
			if (num > 99)
				GetThreeDigitNumber(word, num);
			else
				GetOneOrTwoDigitNumber(word, num);
			
			return word;
		}

		static void GetThreeDigitNumber(StringBuilder word, int num)
		{
			int hundredsCol = num / 100;
			GetOneOrTwoDigitNumber(word, hundredsCol);
			word.Append(" " + WordConstants.DENOMINATIONS[0]); //add "hundred"
			int mod = num % 100;
			if (mod != 0)
			{
				word.Append(" ");
				GetOneOrTwoDigitNumber(word, mod);
			}
		}

		static void GetOneOrTwoDigitNumber(StringBuilder word, int num)
		{
			//check for anything that is zero to nineteen
			if (num < 20)
			{
				word.Append(WordConstants.ZERO_TO_19[num]);
			}
			else if (num >= 20 || num < 100)
			{
				word.Append(WordConstants.TENS[num / 10]);
				int mod = num % 10;
				if (mod != 0)
					word.Append(" " + WordConstants.ZERO_TO_19[mod]);
			}
		}
	}
}
