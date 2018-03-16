using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class NullCharacterClass : CharacterClass
    {
        public override bool Match(char c)
        {
            return false;
        }
    }
}
