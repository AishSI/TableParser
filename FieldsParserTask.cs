using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TableParser
{
	public class FieldsParserTask
	{
		public static List<string> ParseLine(string line)
		{
			int lastFoundedQuoteIndex = 0;
			List<string> fields = new List<string>();
			SeekQuotes(line, ref lastFoundedQuoteIndex, fields);
			if (lastFoundedQuoteIndex == 0)
				fields.AddRange(line.Split(new char[] { ' ' }).Where(x => x != ""));
			else if (lastFoundedQuoteIndex < line.Length - 1)
			{
				var lastToken = line.Substring(lastFoundedQuoteIndex + 1, line.Length - lastFoundedQuoteIndex - 1);
				if (lastToken != "")
					fields.Add(lastToken);
			}
			return fields;
		}

		private static void SeekQuotes(string line, ref int lastFoundedQuoteIndex, List<string> fields)
		{
			for (var i = 0; i < line.Length; i++)
			{
				if (line[i] == '\'' || line[i] == '"')
				{
					var token = FieldInQuote(i, line, fields, lastFoundedQuoteIndex, line[i]);
					i = token.GetIndexNextToToken();
					lastFoundedQuoteIndex = token.GetIndexNextToToken();
				}
			}
		}

		public static Token FieldInQuote(int i, string line, List<string> fields, int lastFoundedQuoteIndex, char quote)
		{
			var token = ReadField(line.Substring(lastFoundedQuoteIndex, i - lastFoundedQuoteIndex), lastFoundedQuoteIndex);
			foreach (var field in token.Value.Split())
				if (field != "" && field != "\"" && field != "'")
					fields.Add(field);
			token = FindingSecondQuote(i, line, quote);
			fields.Add(token.Value.Replace("\\" + quote, quote.ToString()).Replace("\\\\", "\\").Trim());
			return token;
		}

		public static Token FindingSecondQuote(int i, string line, char quote)
		{
			for (var j = i + 1; j < line.Length; j++)
			{
				if (line[j] == '\\')
					j++;
				else if (line[j] == quote)
				{
					return ReadField(line.Substring(i + 1, j - i - 1), i + 1);
				}
			}
			return ReadField(line.Substring(i + 1, line.Length - i - 1), i + 1);
		}

		private static Token ReadField(string line, int startIndex)
		{
			return new Token(line, startIndex, line.Length);
		}
	}
}