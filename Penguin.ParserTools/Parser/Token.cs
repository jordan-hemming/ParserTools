using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    public abstract class Token<TTokenType> 
        where TTokenType : IEquatable<TTokenType>
    {
        public int LineNumber { get; protected set; }
        public int ColumnNumber { get; protected set; }
        public string Text { get; protected set; }
        public TTokenType Type { get; protected set; }
        public bool IsIgnored { get; protected set; }

        public Token(string text, int line, int col, bool ignore = false)
        {

        }
    }
}
