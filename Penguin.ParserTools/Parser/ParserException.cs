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
        public UnexpectedEndOfFileException(string msg)
            : base(msg)
        {

        }

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
        public UnexpectedTokenException(string msg)
            : base(msg)
        {

        }

        public UnexpectedTokenException(TToken token, TTokenType expected)
            : base("Expected " + expected + " but got " + token.Type + " instead @ line: " + token.LineNumber + " col: " + token.ColumnNumber)
        {

        }

        public UnexpectedTokenException(TToken token)
            : base("Expected EOF but got " + token.Type + " instead @ line: " + token.LineNumber + " col: " + token.ColumnNumber)
        {

        }
    }
}
