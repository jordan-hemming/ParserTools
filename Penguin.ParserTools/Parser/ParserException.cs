using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    /// <summary>
    /// Base class for parser exceptions.
    /// </summary>
    public class ParserException : Exception
    {
        /// <summary>
        /// Creates the exception with the specified message.
        /// </summary>
        /// <param name="msg">The message.</param>
        public ParserException(string msg)
            : base(msg)
        {

        }
    }

    /// <summary>
    /// Exception raised when invalid token is encountered in input stream.
    /// </summary>
    public class InvalidTokenException : ParserException
    {
        /// <summary>
        /// Creates the exception with the specified line and column numbers.
        /// </summary>
        /// <param name="line">The line number.</param>
        /// <param name="col">The column number.</param>
        public InvalidTokenException(int line, int col)
            : base("Invalid token @ line: " + line + " col: " + col)
        {

        }
    }

    /// <summary>
    /// Generic base exception raised by BaseParser.
    /// </summary>
    /// <typeparam name="TToken">Type representing tokens.</typeparam>
    /// <typeparam name="TTokenType">Type representing variaties of token.</typeparam>
    public class ParserException<TToken, TTokenType> : ParserException
        where TToken: Token<TTokenType>
        where TTokenType: IEquatable<TTokenType>
    {
        /// <summary>
        /// Creates the exception with the specified message.
        /// </summary>
        /// <param name="msg">The message.</param>
        public ParserException(string msg)
            : base(msg)
        {

        }
    }

    /// <summary>
    /// Exception raised when unexpected EOF encountered by BaseParser.
    /// </summary>
    /// <typeparam name="TToken">Type representing tokens.</typeparam>
    /// <typeparam name="TTokenType">Type representing variaties of token.</typeparam>
    public class UnexpectedEndOfFileException<TToken, TTokenType> : ParserException<TToken, TTokenType>
        where TToken : Token<TTokenType>
        where TTokenType : IEquatable<TTokenType>
    {
        /// <summary>
        /// Creates the exception with the specified message.
        /// </summary>
        /// <param name="msg">The message.</param>
        public UnexpectedEndOfFileException(string msg)
            : base(msg)
        {

        }

        /// <summary>
        /// Creates the exception with the specified token type.
        /// </summary>
        /// <param name="type">The token type that was expected.</param>
        public UnexpectedEndOfFileException(TTokenType type)
            : base("Expected " + type + " but got EOF instead.")
        {

        }
    }

    /// <summary>
    /// Exception raised when unexpected token encountered by BaseParser.
    /// </summary>
    /// <typeparam name="TToken">Type representing tokens.</typeparam>
    /// <typeparam name="TTokenType">Type representing variaties of token.</typeparam>
    public class UnexpectedTokenException<TToken, TTokenType> : ParserException<TToken, TTokenType>
        where TToken : Token<TTokenType>
        where TTokenType : IEquatable<TTokenType>
    {
        /// <summary>
        /// Create the exception with the specified message.
        /// </summary>
        /// <param name="msg">The message.</param>
        public UnexpectedTokenException(string msg)
            : base(msg)
        {

        }

        /// <summary>
        /// Create the exception with the specified token and the token type.
        /// </summary>
        /// <param name="token">The token that was received.</param>
        /// <param name="expected">The token type that was expected.</param>
        public UnexpectedTokenException(TToken token, TTokenType expected)
            : base("Expected " + expected + " but got " + token.Type + " instead @ line: " + token.LineNumber + " col: " + token.ColumnNumber)
        {

        }

        /// <summary>
        /// Create the exception with the specified token.
        /// </summary>
        /// <param name="token">The token that was received instead of EOF.</param>
        public UnexpectedTokenException(TToken token)
            : base("Expected EOF but got " + token.Type + " instead @ line: " + token.LineNumber + " col: " + token.ColumnNumber)
        {

        }
    }
}
