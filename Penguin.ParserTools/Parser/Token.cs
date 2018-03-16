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
        /// <summary>
        /// The line number of the start of the token.
        /// </summary>
        public int LineNumber { get; protected set; }
        /// <summary>
        /// The column number of the start of the token.
        /// </summary>
        public int ColumnNumber { get; protected set; }
        /// <summary>
        /// The text that was matched.
        /// </summary>
        public string Text { get; protected set; }
        /// <summary>
        /// The type of the token.
        /// </summary>
        public TTokenType Type { get; protected set; }
        
        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="text">The text that was matched.</param>
        /// <param name="type">The type of the token.</param>
        /// <param name="line">The line number of the start of the token.</param>
        /// <param name="col">The column number of the start of the token.</param>
        public Token(string text, TTokenType type, int line, int col)
        {
            Text = text;
            Type = type;
            LineNumber = line;
            ColumnNumber = col;
        }

        /// <summary>
        /// Gets a string representation of the token object.
        /// </summary>
        /// <param name="showPosition">Whether to include line and column numbers in the result.</param>
        /// <returns>The string representation of the token.</returns>
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

        /// <summary>
        /// Gets a string representation of the token object.
        /// </summary>
        /// <returns>The string representation of the token.</returns>
        public override string ToString()
        {
            return ToString(false);
        }
    }
}
