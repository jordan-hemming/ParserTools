using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    public class ParserException : Exception
    {
        public ParserException(string msg)
            : base(msg)
        {

        }
    }

    public class ParserException<TToken, TTokenType> : ParserException
        where TToken: Token<TTokenType>
        where TTokenType: IEquatable<TTokenType>
    {
        public ParserException(string msg)
            : base(msg)
        {

        }
    }

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
