using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Penguin.ParserTools.Parser;
using Penguin.ParserTools.Regex.AST;

namespace Penguin.ParserTools.Regex
{
    /// <summary>
    /// Represents a token produced by the RegexTokenizer.
    /// </summary>
    public class RegexToken : Token<RegexTokenType>
    {
        /// <summary>
        /// 'normalCharacter' character represented by the token.
        /// </summary>
        public char? NormalCharacter { get; }
        /// <summary>
        /// 'escapeCharacter' character represented by the token.
        /// </summary>
        public char? EscapeCharacter { get; }
        /// <summary>
        /// 'specialClass' character class represented by the token.
        /// </summary>
        public CharacterClass SpecialClass { get; }
        /// <summary>
        /// 'escapeClass' charcter class represented by the token.
        /// </summary>
        public CharacterClass EscapeClass { get; }

        private RegexToken(char c, int line, int col, char? normalCharacter = null, char? escapeCharacter = null, CharacterClass specialClass = null, CharacterClass escapeClass = null, params RegexTokenSubType[] otherSubTypes)
            : base(c.ToString(), null, line, col)
        {
            var subtypes = new HashSet<RegexTokenSubType>(otherSubTypes);
            if (normalCharacter.HasValue)
            {
                NormalCharacter = normalCharacter;
                subtypes.Add(RegexTokenSubType.NormalCharacter);
            }
            if (escapeCharacter.HasValue)
            {
                EscapeCharacter = escapeCharacter;
                subtypes.Add(RegexTokenSubType.EscapeCharacter);
            }
            if (specialClass != null)
            {
                SpecialClass = specialClass;
                subtypes.Add(RegexTokenSubType.SpecialClass);
            }
            if (escapeClass != null)
            {
                EscapeClass = escapeClass;
                subtypes.Add(RegexTokenSubType.EscapeClass);
            }
            Type = new RegexTokenType(subtypes);
        }

        /// <summary>
        /// Creates a RegexToken from the specified character.
        /// </summary>
        /// <param name="c">The character from which to create the RegexToken.</param>
        /// <param name="line">The line number of the source character.</param>
        /// <param name="col">"The column number of the source character.</param>
        /// <returns></returns>
        public static RegexToken FromChar(char c, int line, int col)
        {
            switch (c)
            {
                case '\\':
                    return new RegexToken(c, line, col, escapeCharacter: '\\', otherSubTypes: RegexTokenSubType.Backslash);
                case '[':
                    return new RegexToken(c, line, col, escapeCharacter: '[', otherSubTypes: RegexTokenSubType.OpenBracket);
                case ']':
                    return new RegexToken(c, line, col, escapeCharacter: ']', otherSubTypes: RegexTokenSubType.CloseBracket);
                case '(':
                    return new RegexToken(c, line, col, escapeCharacter: '(', otherSubTypes: RegexTokenSubType.OpenParan);
                case ')':
                    return new RegexToken(c, line, col, escapeCharacter: ')', otherSubTypes: RegexTokenSubType.CloseParan);
                case '|':
                    return new RegexToken(c, line, col, escapeCharacter: '|', otherSubTypes: RegexTokenSubType.Choice);
                case '-':
                    return new RegexToken(c, line, col, normalCharacter: '-', escapeCharacter: '-', otherSubTypes: RegexTokenSubType.Range);
                case '^':
                    return new RegexToken(c, line, col, normalCharacter: '^', escapeCharacter: '^', otherSubTypes: RegexTokenSubType.Not);
                case '+':
                    return new RegexToken(c, line, col, normalCharacter: '+', escapeCharacter: '+', otherSubTypes: RegexTokenSubType.OneOrMore);
                case '*':
                    return new RegexToken(c, line, col, normalCharacter: '*', escapeCharacter: '*', otherSubTypes: RegexTokenSubType.ZeroOrMore);
                case '?':
                    return new RegexToken(c, line, col, normalCharacter: '?', escapeCharacter: '?', otherSubTypes: RegexTokenSubType.ZeroOrOne);
                case '.':
                    return new RegexToken(c, line, col, escapeCharacter: '.', specialClass: new AnyCharacterClass());
                case 'w':
                    return new RegexToken(c, line, col, normalCharacter: 'w', escapeClass: new WordCharacterClass());
                case 'W':
                    return new RegexToken(c, line, col, normalCharacter: 'W', escapeClass: new NonWordCharacterClass());
                case 's':
                    return new RegexToken(c, line, col, normalCharacter: 's', escapeClass: new WhitespaceCharacterClass());
                case 'S':
                    return new RegexToken(c, line, col, normalCharacter: 'S', escapeClass: new NonWhitespaceCharacterClass());
                case 'd':
                    return new RegexToken(c, line, col, normalCharacter: 'd', escapeClass: new DigitCharacterClass(), otherSubTypes: RegexTokenSubType.HexDigit);
                case 'D':
                    return new RegexToken(c, line, col, normalCharacter: 'D', escapeClass: new NonDigitCharacterClass(), otherSubTypes: RegexTokenSubType.HexDigit);
                case 'a':
                    return new RegexToken(c, line, col, normalCharacter: 'a', escapeCharacter: '\a', otherSubTypes: RegexTokenSubType.HexDigit);
                case 'b':
                    return new RegexToken(c, line, col, normalCharacter: 'b', escapeCharacter: '\b', otherSubTypes: RegexTokenSubType.HexDigit);
                case 't':
                    return new RegexToken(c, line, col, normalCharacter: 't', escapeCharacter: '\t');
                case 'r':
                    return new RegexToken(c, line, col, normalCharacter: 'r', escapeCharacter: '\r');
                case 'v':
                    return new RegexToken(c, line, col, normalCharacter: 'v', escapeCharacter: '\v');
                case 'f':
                    return new RegexToken(c, line, col, normalCharacter: 'f', escapeCharacter: '\f', otherSubTypes: RegexTokenSubType.HexDigit);
                case 'n':
                    return new RegexToken(c, line, col, normalCharacter: 'n', escapeCharacter: '\n');
                case 'e':
                    return new RegexToken(c, line, col, normalCharacter: 'e', escapeCharacter: '\x001B', otherSubTypes: RegexTokenSubType.HexDigit);
                case 'x':
                    return new RegexToken(c, line, col, normalCharacter: 'x', otherSubTypes: RegexTokenSubType.HexIndicator);
                case 'u':
                    return new RegexToken(c, line, col, normalCharacter: 'u', otherSubTypes: RegexTokenSubType.UnicodeIndicator);
                default:
                    if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
                        return new RegexToken(c, line, col, normalCharacter: c, otherSubTypes: RegexTokenSubType.HexDigit);
                    else
                        return new RegexToken(c, line, col, normalCharacter: c);
            }
        }
    }

    /// <summary>
    /// Enum representing all RegexType subtypes.
    /// </summary>
    public enum RegexTokenSubType
    {
        /// <summary>
        /// 'normalCharacter' character
        /// </summary>
        NormalCharacter = 0x0001,
        /// <summary>
        /// 'escapeCharacter' character
        /// </summary>
        EscapeCharacter = 0x0002,
        /// <summary>
        /// 'specialClass' character class.
        /// </summary>
        SpecialClass = 0x0004,
        /// <summary>
        /// 'escapeClass' character class.
        /// </summary>
        EscapeClass = 0x0008,
        /// <summary>
        /// The '^' character.
        /// </summary>
        Not = 0x0010,
        /// <summary>
        /// The '[' character.
        /// </summary>
        OpenBracket = 0x0020,
        /// <summary>
        /// The ']' character.
        /// </summary>
        CloseBracket = 0x0040,
        /// <summary>
        /// The '(' character.
        /// </summary>
        OpenParan = 0x0080,
        /// <summary>
        /// The ')' character.
        /// </summary>
        CloseParan = 0x0100,
        /// <summary>
        /// The '-' character.
        /// </summary>
        Range = 0x0200,
        /// <summary>
        /// The '\\' character.
        /// </summary>
        Backslash = 0x0400,
        /// <summary>
        /// The '+' character.
        /// </summary>
        OneOrMore = 0x0800,
        /// <summary>
        /// The '*' character.
        /// </summary>
        ZeroOrMore = 0x1000,
        /// <summary>
        /// The '?' character.
        /// </summary>
        ZeroOrOne = 0x2000,
        /// <summary>
        /// The 'x' character.
        /// </summary>
        HexIndicator = 0x4000,
        /// <summary>
        /// The 'u' character.
        /// </summary>
        UnicodeIndicator = 0x8000,
        /// <summary>
        /// The characters 0'-'9', 'a'-'f' and 'A'-'F'.
        /// </summary>
        HexDigit = 0x10000,
        /// <summary>
        /// The '|' character.
        /// </summary>
        Choice = 0x20000
    }

    /// <summary>
    /// Class representing a RegexToken type. Can be one or more sub types.
    /// </summary>
    public class RegexTokenType : IEquatable<RegexTokenType>
    {
        private HashSet<RegexTokenSubType> _subtypes;
        
        /// <summary>
        /// Creates the object from a list of subtypes.
        /// </summary>
        /// <param name="subtypes">The list of subtypes.</param>
        public RegexTokenType(params RegexTokenSubType[] subtypes)
        {
            _subtypes = new HashSet<RegexTokenSubType>(subtypes);
        }

        /// <summary>
        /// Creates the object from a collection of subtypes.
        /// </summary>
        /// <param name="subTypes"></param>
        public RegexTokenType(IEnumerable<RegexTokenSubType> subTypes)
        {
            _subtypes = new HashSet<RegexTokenSubType>(subTypes);
        }

        /// <summary>
        /// Checks if another RegexTokenType object has at least one subtype in common.
        /// </summary>
        /// <param name="other">The other regexTokenType object.</param>
        /// <returns>True if one or more subtypes are shared.</returns>
        public bool Equals(RegexTokenType other)
        {
            return _subtypes.Intersect(other._subtypes).Count() > 0;
        }

        /// <summary>
        /// Checks if another object is equal.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RegexTokenType)
                return Equals(obj as RegexTokenType);
            return false;
        }

        /// <summary>
        /// Gets the hash code of the object.
        /// </summary>
        /// <returns>The hash code of the object.</returns>
        public override int GetHashCode()
        {
            int i = 0;
            foreach (var subType in _subtypes)
                i |= (int)subType;
            return i;
        }


        /// <summary>
        /// Gets a string representation of the object.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            if (_subtypes.Count == 1)
                return _subtypes.First().ToString();
            var sb = new StringBuilder();
            sb.Append("(");
            sb.Append(string.Join(" and ", _subtypes));
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Implicitly converts a single subtype into a full RegexTokenType containing only that subtype.
        /// </summary>
        /// <param name="subType">The subtype from which to create the RegexTokenType.</param>
        public static implicit operator RegexTokenType(RegexTokenSubType subType)
        {
            return new RegexTokenType(subType);
        }
    }
}
