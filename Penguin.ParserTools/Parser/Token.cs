using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Parser
{
    /// <summary>
    /// Generic base class for Tokens.
    /// </summary>
    /// <typeparam name="TTokenType">Type representing variaties of token.</typeparam>
    public abstract class Token<TTokenType> 
        where TTokenType : IEquatable<TTokenType>
    {
        public int LineNumber { get; protected set; }
        public int ColumnNumber { get; protected set; }
        public string Text { get; protected set; }
        public TTokenType Type { get; protected set; }
        
        public Token(string text, TTokenType type, int line, int col)
        {
            Text = text;
            Type = type;
            LineNumber = line;
            ColumnNumber = col;
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
