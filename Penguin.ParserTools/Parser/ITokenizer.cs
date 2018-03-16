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
        /// <summary>
        /// Tokenizes the input.
        /// </summary>
        /// <param name="input">The input to tokenize.</param>
        /// <returns>The list of tokens tokenized.</returns>
        IReadOnlyList<TToken> Tokenize(string input);
    }
}
