using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class NonDigitCharacterClass : CharacterClass
    {
        public override bool Match(char c)
        {
            return !char.IsDigit(c);
        }
    }
}
