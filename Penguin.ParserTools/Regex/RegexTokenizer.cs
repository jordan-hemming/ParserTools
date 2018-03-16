using System;
using System.Collections.Generic;
using System.Text;
using Penguin.ParserTools.Parser;

namespace Penguin.ParserTools.Regex
{
    /// <summary>
    /// Tokenizer for Regex expressions.
    /// </summary>
    public class RegexTokenizer : ITokenizer<RegexToken, RegexTokenType>
    {
        public IReadOnlyList<RegexToken> Tokenize(string input)
        {
            List<RegexToken> result = new List<RegexToken>();
            int col = 1;
            int line = 1;
            foreach (char c in input)
            {
                if (c == '\n')
                {
                    line += 1;
                    col = 1;
                }
                var token = RegexToken.FromChar(c, line, col);
                result.Add(token);
                col += 1;
            }
            return result;
        }
    }
}
