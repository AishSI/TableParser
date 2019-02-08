using System.Collections.Generic;
using System.Linq;

namespace TableParser
{
	public class FieldsParserTask
	{
		public static List<string> ParseLine(string line)
		{
			int lastFoundedQuotesIndex = 0;
			List<string> fields = new List<string>();
			SeekQuotes(line, ref lastFoundedQuotesIndex, fields);
			return fields;
		}

		private static void SeekQuotes(string line, ref int lastFoundedQuotesIndex, List<string> fields)
		{
			for (var i = 0; i < line.Length; i++)
			{
				if (line[i] == '\'' || line[i] == '"')
				{
					SeekToken(line, i, line[i], ref lastFoundedQuotesIndex, fields);
					i = lastFoundedQuotesIndex;
				}
			}
			LastToken(line, lastFoundedQuotesIndex, fields);
		}

		public static void SeekToken(string line, int i, char quotes, ref int lastFoundedQuotesIndex, List<string> fields)
		{
			TokenSimpleField(line, i, lastFoundedQuotesIndex, fields);
			TokenInQuotes(line, i, ref lastFoundedQuotesIndex, fields, quotes);
		}

		private static void TokenSimpleField(string line, int i, int lastFoundedQuotesIndex, List<string> fields)
		{
			var token = ReadField(line.Substring(lastFoundedQuotesIndex, i - lastFoundedQuotesIndex), lastFoundedQuotesIndex);
			fields.AddRange(
				token.Value
				.Split(new char[] { ' ' })
				.Where(x => x != "" && x != "\"" && x != "'")
				.ToList());
		}

		public static void TokenInQuotes(string line, int i, ref int lastFoundedQuotesIndex, List<string> fields, char quotes)
		{
			var token = FindSecondQuotes(line, i, quotes);
			fields.Add(token.Value.Replace("\\" + quotes, quotes.ToString()).Replace("\\\\", "\\"));
			lastFoundedQuotesIndex = token.GetIndexNextToToken();
		}

		private static void LastToken(string line, int lastFoundedQuotesIndex, List<string> fields)
		{
			if (lastFoundedQuotesIndex == 0)
				fields.AddRange(line.Split(new char[] { ' ' }).Where(x => x != ""));
			else if (lastFoundedQuotesIndex < line.Length - 1)
			{
				var lastToken = line.Substring(lastFoundedQuotesIndex + 1, line.Length - lastFoundedQuotesIndex - 1).TrimStart();
				if (lastToken != "")
					fields.Add(lastToken);
			}
		}

		public static Token FindSecondQuotes(string line, int i, char quotes)
		{
			var startFieldInQuotes = i + 1;
			for (var j = startFieldInQuotes; j < line.Length; j++)
			{
				if (line[j] == '\\')
					j++;
				else if (line[j] == quotes)
				{
					return ReadField(line.Substring(startFieldInQuotes, j - i - 1), startFieldInQuotes);
				}
			}
			return ReadField(line.Substring(startFieldInQuotes, line.Length - i - 1), startFieldInQuotes);
		}

		private static Token ReadField(string line, int startIndex)
		{
			return new Token(line, startIndex, line.Length);
		}
	}
}