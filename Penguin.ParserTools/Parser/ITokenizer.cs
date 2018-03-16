using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    /// <summary>
    /// Generic interface representing tokenizers.
    /// </summary>
    /// <typeparam name="TToken">Type representing tokens in the token stream.</typeparam>
    /// <typeparam name="TType">Type representing variaties of token.</typeparam>
    public interface ITokenizer<TToken, TType>
        where TToken: Token<TType>
        where TType: IEquatable<TType>
    {
        IReadOnlyList<TToken> Tokenize(string input);
    }
}
