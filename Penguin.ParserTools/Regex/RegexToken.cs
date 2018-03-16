using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Penguin.ParserTools.Parser;
using Penguin.ParserTools.Regex.AST;

namespace Penguin.ParserTools.Regex
{
    public class RegexToken : Token<RegexTokenType>
    {
        public char? NormalCharacter { get; }
        public char? EscapeCharacter { get; }
        public CharacterClass SpecialClass { get; }
        public CharacterClass EscapeClass { get; }

        public RegexToken(char c, int line, int col, char? normalCharacter = null, char? escapeCharacter = null, CharacterClass specialClass = null, CharacterClass escapeClass = null, params RegexTokenSubType[] otherSubTypes)
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

    public enum RegexTokenSubType
    {
        NormalCharacter = 0x0001,
        EscapeCharacter = 0x0002,
        SpecialClass = 0x0004,
        EscapeClass = 0x0008,
        Not = 0x0010,
        OpenBracket = 0x0020,
        CloseBracket = 0x0040,
        OpenParan = 0x0080,
        CloseParan = 0x0100,
        Range = 0x0200,
        Backslash = 0x0400,
        OneOrMore = 0x0800,
        ZeroOrMore = 0x1000,
        ZeroOrOne = 0x2000,
        HexIndicator = 0x4000,
        UnicodeIndicator = 0x8000,
        HexDigit = 0x10000,
        Choice = 0x20000
    }

    public class RegexTokenType : IEquatable<RegexTokenType>
    {
        private HashSet<RegexTokenSubType> _subtypes;
        
        public RegexTokenType(params RegexTokenSubType[] subtypes)
        {
            _subtypes = new HashSet<RegexTokenSubType>(subtypes);
        }

        public RegexTokenType(IEnumerable<RegexTokenSubType> subTypes)
        {
            _subtypes = new HashSet<RegexTokenSubType>(subTypes);
        }

        public bool Equals(RegexTokenType other)
        {
            return _subtypes.Intersect(other._subtypes).Count() > 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is RegexTokenType)
                return Equals(obj as RegexTokenType);
            return false;
        }

        public override int GetHashCode()
        {
            int i = 0;
            foreach (var subType in _subtypes)
                i |= (int)subType;
            return i;
        }

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

        public static implicit operator RegexTokenType(RegexTokenSubType subType)
        {
            return new RegexTokenType(subType);
        }
    }
}
