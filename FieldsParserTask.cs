using System.Collections.Generic;
using System.Linq;

namespace TableParser
{
	public class FieldsParserTask
	{
		public static List<string> ParseLine(string line)
		{
			int lastPositionToken = 0;
			var charQuote = ' ';
			List<string> fields = new List<string>();
			SeekToken(line, ref lastPositionToken, fields, charQuote);
			return fields;
		}

		private static void SeekToken(string line, ref int lastPositionToken, List<string> fields, char charQuote)
		{
			for (var i = 0; i < line.Length; i++)
			{
				if ((charQuote == ' ') && (line[i] == ' ' || line[i] == '\'' || line[i] == '"'))
				{
					TokenSimpleField(line, i, ref lastPositionToken, fields, charQuote);
					charQuote = line[i];
				}
				else if (charQuote == '\'' || charQuote == '"')
				{
					TokenInQuotes(line, i, ref lastPositionToken, fields, charQuote);
					charQuote = ' ';
					i = lastPositionToken;
				}
			}
			if (lastPositionToken != 0 && lastPositionToken < line.Length - 1)
				lastPositionToken += 1;
			TokenSimpleField(line, line.Length, ref lastPositionToken, fields, charQuote);
		}

		private static void TokenSimpleField(string line, int i, ref int lastPositionToken,
			List<string> fields, char charQuote)
		{
			var token = ReadField(line.Substring(lastPositionToken, i - lastPositionToken), lastPositionToken);
			fields.AddRange(token.Value
				.Split(new char[] { ' ' })
				.Where(x => x != "" && x != "\"" && x != "'")
				.ToList());
			lastPositionToken = token.GetIndexNextToToken();
		}

		public static void TokenInQuotes(string line, int startFieldInQuotes, ref int lastPositionToken,
			List<string> fields, char charQuote)
		{
			var endFieldInQuotes = FindSecondQuotes(line, startFieldInQuotes, charQuote);
			var token = ReadField(line.Substring(startFieldInQuotes, endFieldInQuotes - startFieldInQuotes), startFieldInQuotes);
			fields.Add(token.Value.Replace("\\" + charQuote, charQuote.ToString()).Replace("\\\\", "\\"));
			lastPositionToken = token.GetIndexNextToToken();
		}

		public static int FindSecondQuotes(string line, int startFieldInQuotes, char quotes)
		{
			for (var j = startFieldInQuotes; j < line.Length; j++)
			{
				if (line[j] == '\\')
					j++;
				else if (line[j] == quotes)
					return j;
			}
			return line.Length;
		}

		private static Token ReadField(string line, int startIndex)
		{
			return new Token(line, startIndex, line.Length);
		}
	}
}