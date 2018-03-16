using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Penguin.ParserTools.Regex;

namespace Penguin.ParserTools.Parser
{
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
            //Match first character
            int startAt = index;
            char c = input[index++];
            foreach (var tokenDef in _tokenDefintions)
            {
                tokenDef.Regex.Reset();
                tokenDef.Regex.Match(c);
            }
            var lastValidDefs = _tokenDefintions.Where(x => x.Regex.LastResult == RegexResult.Matched).ToList();
            if (!_tokenDefintions.Any(x => x.Regex.LastResult != RegexResult.NotMatched))
                throw new InvalidTokenException(line, col);
            col += 1;
            if (c == '\n')
            {
                line += 1;
                col = 1;
            }
			while (_tokenDefintions.Any(x => x.Regex.LastResult != RegexResult.NotMatched) && index < input.Length)
            {
                lastValidDefs = _tokenDefintions.Where(x => x.Regex.LastResult == RegexResult.Matched).ToList();
                c = input[index++];
                foreach (var tokenDef in _tokenDefintions)
                    tokenDef.Regex.Match(c);
                col += 1;
                if (c == '\n')
                {
                    line += 1;
                    col = 1;
                }
            }
            var resultTokenDef = lastValidDefs.OrderByDescending(x => x.Priority).First();
            if (resultTokenDef.Ignore)
                return null;
            else
                return CreateToken(resultTokenDef.Type, input.Substring(startAt, index - startAt - (index < input.Length ? 1 : 0)), line, col);
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