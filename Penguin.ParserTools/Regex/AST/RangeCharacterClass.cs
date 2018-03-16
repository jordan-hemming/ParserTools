using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class RangeCharacterClass : CharacterClass
    {
        public char From { get; }
        public char To { get; }

        public RangeCharacterClass(char from, char to)
        {
            if (from > to)
                throw new InvalidCharacterRangeException("Invalid range given. From ('" + from + "') must be lower than To ('" + to + "').");
            From = from;
            To = to;
        }

        public override bool Match(char c)
        {
            return c >= From && c <= To;
        }
    }
}
