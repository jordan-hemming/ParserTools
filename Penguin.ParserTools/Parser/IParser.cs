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
        TResult Parse();
    }
}
