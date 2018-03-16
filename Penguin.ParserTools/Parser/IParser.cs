using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    public interface IParser<TResult, TToken, TTokenType>
        where TToken: Token<TTokenType>
        where TTokenType: IEquatable<TTokenType>
    {
        TResult Parse(IReadOnlyList<TToken> tokens);
    }
}
