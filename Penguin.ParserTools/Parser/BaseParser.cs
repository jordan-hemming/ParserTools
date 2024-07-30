using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    /// <summary>
    /// Base class for parsers.
    /// </summary>
    /// <typeparam name="TToken">Type representing tokens in the token stream.</typeparam>
    /// <typeparam name="TType">Type representing variaties of token.</typeparam>
    public abstract class BaseParser<TToken, TType>
        where TToken: Token<TType> 
        where TType: IEquatable<TType>
    {
        private List<TToken> _tokens;
        private int _tokenIndex;

        /// <summary>
        /// Construct the parser using the specified token list as a source.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        public BaseParser(IEnumerable<TToken> tokens)
        {
            _tokens = new List<TToken>(tokens);
            _tokenIndex = 0;
        }

        /// <summary>
        /// Checks if the end of the token list is reached.
        /// </summary>
        /// <returns>True if the end of the token list is reached.</returns>
        protected bool EndOfFile()
        {
            return _tokenIndex >= _tokens.Count;
        }

        /// <summary>
        /// Returns the next token in the token list and advances by one token.
        /// </summary>
        /// <returns>The next token in the token list.</returns>
        protected TToken Next()
        {
            if (EndOfFile())
                return null;
            else
                return _tokens[_tokenIndex++];
        }

        /// <summary>
        /// Returns the previous token in the token list.
        /// </summary>
        /// <returns>The previous token in the token list.</returns>
        protected TToken Prev()
        {
            if (_tokenIndex > 0)
                return _tokens[_tokenIndex - 1];
            else
                return null;
        }

        /// <summary>
        /// Return the nth next token in the token list.
        /// </summary>
        /// <param name="n">How far forward in the token list to look (default = 0).</param>
        /// <returns>The nth token in the token list.</returns>
        protected TToken Peek(int n = 0)
        {
            if (_tokenIndex + n >= _tokens.Count)
                return null;
            else
                return _tokens[_tokenIndex + n];
        }

        /// <summary>
        /// Checks if the nth token in the token list is of the specified type.
        /// </summary>
        /// <param name="type">The type to check the token against.</param>
        /// <param name="n">The nth token in the token list.</param>
        /// <returns>True if the token is of the specified type.</returns>
        protected bool Peek(TType type, int n = 0)
        {
            var token = Peek(n);
            return token != null && token.Type.Equals(type);
        }

        /// <summary>
        /// Checks if the nth token in the token list if one of the specified types.
        /// </summary>
        /// <param name="types">The types to check the token against.</param>
        /// <param name="n">The nth token in the token list.</param>
        /// <returns>True if the token is one of the specified type.</returns>
        protected bool Peek(IEnumerable<TType> types, int n = 0)
        {
            var token = Peek(n);
            return token != null && types.Contains(token.Type);
        }

        /// <summary>
        /// Raises an exception if the end of the token list has not been reached.
        /// </summary>
        protected void ExpectEOF()
        {
            if (!EndOfFile())
                throw new UnexpectedTokenException<TToken, TType>(_tokens[_tokenIndex]);
        }

        /// <summary>
        /// Raises an exception if the next token is not of the specified type.
        /// </summary>
        /// <param name="type">The token type to check against.</param>
        /// <param name="token">The token that was matched.</param>
        protected void Expect(TType type, out TToken token)
        {
            token = Next();
            if (token == null)
                throw new UnexpectedEndOfFileException<TToken, TType>(type);
            else if (!token.Type.Equals(type))
                throw new UnexpectedTokenException<TToken, TType>(token, type);
        }

        /// <summary>
        /// Raises an exception if the next token is not the specified type.
        /// </summary>
        /// <param name="type">The token type to check against.</param>
        protected void Expect(TType type)
        {
            Expect(type, out var token);
        }

        /// <summary>
        /// Checks if the next token is of the specified type, and if it is advances to the next token.
        /// </summary>
        /// <param name="type">The token type to check against.</param>
        /// <param name="token">The token that was matched.</param>
        /// <returns>True if the token was matched.</returns>
        protected bool Accept(TType type, out TToken token)
        {
            token = Peek();
            if (token == null || !token.Type.Equals(type))
                return false;
            Next();
            return true;
        }

        /// <summary>
        /// Checks if the next token is of the specified type, and if it is advances to the next token.
        /// </summary>
        /// <param name="type">The token type to check against.</param>
        /// <returns>True if the token was matched.</returns>
        protected bool Accept(TType type)
        {
            return Accept(type, out var token);
        }
    }
}
