using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class SingleCharacterClass : CharacterClass
    {
        public char Character { get; }

        public SingleCharacterClass(char c)
        {
            Character = c;
        }

        public override bool Match(char c)
        {
            return Character == c;
        }
    }
}
