using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class NonWordCharacterClass : CharacterClass
    {
        public override bool Match(char c)
        {
            return !char.IsLetterOrDigit(c) && c != '_';
        }
    }
}
