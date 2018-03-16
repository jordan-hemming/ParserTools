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

        /// <summary>
        /// Defines a pattern to ignore.
        /// </summary>
        /// <param name="pattern">The Regex pattern to ignore.</param>
        public void DefineIgnore(string pattern)
        {
            _tokenDefintions.Add(new TokenDefintion<TType>(pattern));
        }

        /// <summary>
        /// Defines a pattern as a token type.
        /// </summary>
        /// <param name="type">The value to represent the type of the token.</param>
        /// <param name="pattern">The Regex pattern to match as the token.</param>
        public void DefineToken(TType type, string pattern)
        {
            _tokenDefintions.Add(new TokenDefintion<TType>(type, pattern, _tokenDefintions.Count));
        }

        /// <summary>
        /// Function to create a token. Overriden in sub-classes.
        /// </summary>
        /// <param name="type">The type representing the variety of token.</param>
        /// <param name="text">The text matching the Regex pattern.</param>
        /// <param name="line">The line number of the start of the token.</param>
        /// <param name="col">The column number of the start of the token.</param>
        /// <returns>The created token object.</returns>
        protected abstract TToken CreateToken(TType type, string text, int line, int col);

        private TToken TokenizeNext(string input, ref int index, ref int line, ref int col)
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

        /// <summary>
        /// Tokenizes the input.
        /// </summary>
        /// <param name="input">The input to tokenize.</param>
        /// <returns>The list of tokens tokenized.</returns>
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
    
    class TokenDefintion<TType>
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
            Regex = compiler.Parse();
        }

        public TokenDefintion(string pattern)
        {
            Priority = -1;
            Ignore = true;

            var tokenizer = new RegexTokenizer();
            var tokens = tokenizer.Tokenize(pattern);
            var compiler = new RegexParser(tokens);
            Regex = compiler.Parse();
        }
    }
}