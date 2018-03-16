using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Penguin.ParserTools.Regex;

namespace Penguin.ParserTools.Parser
{
    /// <summary>
    /// Base class for Regex based tokenizers.
    /// </summary>
    /// <typeparam name="TToken">Type representing tokens in the token stream.</typeparam>
    /// <typeparam name="TType">Type representing variaties of token.</typeparam>
    public abstract class BaseTokenizer<TToken, TType> : ITokenizer<TToken, TType>
        where TToken : Token<TType>
        where TType : IEquatable<TType>
    {
        private List<TokenDefintion<TType>> _tokenDefintions = new List<TokenDefintion<TType>>();

        public void DefineIgnore(string pattern)
        {
            _tokenDefintions.Add(new TokenDefintion<TType>(pattern));
        }

        public void DefineToken(TType type, string pattern)
        {
            _tokenDefintions.Add(new TokenDefintion<TType>(type, pattern, _tokenDefintions.Count));
        }

        protected abstract TToken CreateToken(TType type, string text, int line, int col);

        protected TToken TokenizeNext(string input, ref int index, ref int line, ref int col)
        {
            int startAt = index;
            int startLine = line;
            int startCol = col;

            //Match first character
            char c = input[index++];
            foreach (var tokenDef in _tokenDefintions)
            {
                tokenDef.Regex.Reset();
                tokenDef.Regex.Match(c);
            }
            var lastValidDefs = _tokenDefintions.Where(x => x.Regex.LastResult == RegexResult.Matched).ToList();
            if (!_tokenDefintions.Any(x => x.Regex.LastResult != RegexResult.NotMatched))
                throw new InvalidTokenException(line, col);

            //Increament line/col
            int lastLine = startLine;
            int lastCol = startCol;
            col += 1;
            if (c == '\n')
            {
                line += 1;
                col = 1;
            }

            //Match addition characters
			while (_tokenDefintions.Any(x => x.Regex.LastResult != RegexResult.NotMatched) && index < input.Length)
            {
                //Match
                lastValidDefs = _tokenDefintions.Where(x => x.Regex.LastResult == RegexResult.Matched).ToList();
                c = input[index++];
                foreach (var tokenDef in _tokenDefintions)
                    tokenDef.Regex.Match(c);

                //Update line/col
                lastLine = line;
                lastCol = col;
                col += 1;
                if (c == '\n')
                {
                    line += 1;
                    col = 1;
                }
            }

            //Backtrack
            if (index < input.Length)
            {
                index -= 1;
                line = lastLine;
                col = lastCol;
            }

            //Get result token (or null for ignored tokens)
            var resultTokenDef = lastValidDefs.OrderByDescending(x => x.Priority).First();
            if (resultTokenDef.Ignore)
                return null;
            var text = input.Substring(startAt, index - startAt);
            return CreateToken(resultTokenDef.Type, text, startLine, startCol);
        }

        public IReadOnlyList<TToken> Tokenize(string input)
        {
            List<TToken> result = new List<TToken>();
            int index = 0;
            int line = 1;
            int col = 1;
            while (index < input.Length)
            {
                var token = TokenizeNext(input, ref index, ref line, ref col);
                if (token != null)
                    result.Add(token);
            }
            return result;
        }
    }

    public class TokenDefintion<TType>
    {
        public TType Type { get; }
        public RegexFSM Regex { get; }
        public int Priority { get; }
        public bool Ignore { get; }

        public TokenDefintion(TType type, string pattern, int priority)
        {
            Type = type;
            Priority = priority;
            Ignore = false;

            var tokenizer = new RegexTokenizer();
            var tokens = tokenizer.Tokenize(pattern);
            var compiler = new RegexParser(tokens);
            Regex = compiler.Parse().BuildFSM();
        }

        public TokenDefintion(string pattern)
        {
            Priority = -1;
            Ignore = true;

            var tokenizer = new RegexTokenizer();
            var tokens = tokenizer.Tokenize(pattern);
            var compiler = new RegexParser(tokens);
            Regex = compiler.Parse().BuildFSM();
        }
    }
}