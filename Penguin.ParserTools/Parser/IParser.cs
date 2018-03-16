using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    public interface IParser<TResult>
    {
        TResult Parse();
    }
}
