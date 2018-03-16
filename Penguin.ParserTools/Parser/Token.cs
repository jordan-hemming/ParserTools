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

        public Token(string text, TTokenType type, int line, int col, bool ignore = false)
        {
            Text = text;
            Type = type;
            LineNumber = line;
            ColumnNumber = col;
            IsIgnored = ignore;
        }

        public string ToString(bool showPosition)
        {
            var sb = new StringBuilder();
            sb.Append(Text.EscapeAndQuote());
            sb.Append(" : ");
            sb.Append(Type.ToString());
            if (showPosition)
            {
                sb.Append(" @ line: ");
                sb.Append(LineNumber);
                sb.Append(" col: ");
                sb.Append(ColumnNumber);
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString(false);
        }
    }
}
