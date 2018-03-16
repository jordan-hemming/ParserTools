using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    public interface ITokenizer<TToken, TType>
        where TToken: Token<TType>
        where TType: IEquatable<TType>
    {
        IReadOnlyList<TToken> Tokenize(string input);
    }
}
