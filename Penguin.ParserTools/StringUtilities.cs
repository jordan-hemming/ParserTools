using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools
{
    /// <summary>
    /// Various utility functions for strings.
    /// </summary>
    public static class StringUtilities
    {
        public static string Escape(this string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            var sb = new StringBuilder();
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\a':
                        sb.Append("\\a");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\v':
                        sb.Append("\\v");
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    default:
                        if (char.IsControl(c))
                        {
                            if (c >= 0 && c <= 0xff)
                            {
                                sb.Append("\\x");
                                sb.Append(((byte)c).ToString("X2"));
                            }
                            else
                            {
                                sb.Append("\\u");
                                sb.Append(((int)c).ToString("X4"));
                            }
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            return sb.ToString();
        }

        public static string EscapeAndQuote(this string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            var sb = new StringBuilder();
            sb.Append("\"");
            sb.Append(s.Escape());
            sb.Append("\"");
            return sb.ToString();
        }

        public static string Unescape(this string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            var sb = new StringBuilder();
            var state = UnescapeState.Normal;
            int hexDigitsRemaining = 0;
            int hexValue = 0;
            foreach (char c in s)
            {
                if (state == UnescapeState.Normal)
                {
                    if (c == '\\')
                        state = UnescapeState.Escape;
                    else
                        sb.Append(c);
                }
                else if (state == UnescapeState.Escape)
                {
                    switch (c)
                    {
                        case 'x':
                            state = UnescapeState.Hex;
                            hexValue = 0;
                            hexDigitsRemaining = 2;
                            break;
                        case 'u':
                            state = UnescapeState.Hex;
                            hexValue = 0;
                            hexDigitsRemaining = 4;
                            break;
                        case 'a':
                            sb.Append('\a');
                            state = UnescapeState.Normal;
                            break;
                        case 'b':
                            sb.Append('\b');
                            state = UnescapeState.Normal;
                            break;
                        case 'f':
                            sb.Append('\f');
                            state = UnescapeState.Normal;
                            break;
                        case 'n':
                            sb.Append('\n');
                            state = UnescapeState.Normal;
                            break;
                        case 'r':
                            sb.Append('\r');
                            state = UnescapeState.Normal;
                            break;
                        case 't':
                            sb.Append('\t');
                            state = UnescapeState.Normal;
                            break;
                        case 'v':
                            sb.Append('\v');
                            state = UnescapeState.Normal;
                            break;
                        case '\\':
                            sb.Append('\\');
                            state = UnescapeState.Normal;
                            break;
                        case '"':
                            sb.Append('"');
                            state = UnescapeState.Normal;
                            break;
                        default:
                            throw new FormatException("Invalid escape sequence '\\" + c + "'.");
                    }
                }
                else if (state == UnescapeState.Hex)
                {
                    if (!CharUtilities.IsHexDigit(c, out int val))
                        throw new FormatException("Invalid hex digit '" + c + "'.");
                    hexValue <<= 4;
                    hexValue |= val;
                    hexDigitsRemaining -= 1;
                    if (hexDigitsRemaining == 0)
                        state = UnescapeState.Normal;
                }
            }
            return sb.ToString();
        }

        public static string UnescapeAndUnquote(this string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (s.Length < 2)
                throw new FormatException("Not a valid quoted string.");
            if (s[0] != '"' || s[s.Length - 1] != '"')
                throw new FormatException("Not a valid quoted string.");

            s = s.Substring(1, s.Length - 2);
            return s.Unescape();
        }

        enum UnescapeState
        {
            Normal,
            Escape,
            Hex
        }
    }
}
