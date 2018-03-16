using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    public abstract class BaseParser<TToken, TType>
        where TToken: Token<TType> 
        where TType: IEquatable<TType>
    {
        private List<TToken> _tokens;
        private int _tokenIndex;

        public BaseParser(IEnumerable<TToken> tokens)
        {
            _tokens = new List<TToken>(tokens);
            _tokenIndex = 0;
        }

        protected bool EndOfFile()
        {
            return _tokenIndex >= _tokens.Count;
        }

        protected TToken Next()
        {
            if (EndOfFile())
                return null;
            else
                return _tokens[_tokenIndex++];
        }

        protected TToken Prev()
        {
            if (_tokenIndex > 0)
                return _tokens[_tokenIndex - 1];
            else
                return null;
        }

        protected TToken Peek(int n = 1)
        {
            if (_tokenIndex + n >= _tokens.Count)
                return null;
            else
                return _tokens[_tokenIndex + n];
        }

        protected void ExpectEOF()
        {
            if (!EndOfFile())
                throw new UnexpectedTokenException<TToken, TType>(_tokens[_tokenIndex]);
        }

        protected void Expect(TType type, out TToken token)
        {
            token = Next();
            if (token == null)
                throw new UnexpectedEndOfFileException<TToken, TType>(type);
            else if (!token.Type.Equals(type))
                throw new UnexpectedTokenException<TToken, TType>(token, type);
        }

        protected void Expect(TType type)
        {
            Expect(type, out var token);
        }

        protected bool Accept(TType type, out TToken token)
        {
            token = Peek();
            if (token == null || !token.Type.Equals(type))
                return false;
            Next();
            return true;
        }

        protected bool Accept(TType type)
        {
            return Accept(type, out var token);
        }
    }
}
