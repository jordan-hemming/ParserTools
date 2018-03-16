using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    /// <summary>
    /// Generic interface for parsers.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the parse.</typeparam>
    public interface IParser<TResult>
    {
        /// <summary>
        /// Parses the token list.
        /// </summary>
        /// <returns></returns>
        TResult Parse();
    }
}
