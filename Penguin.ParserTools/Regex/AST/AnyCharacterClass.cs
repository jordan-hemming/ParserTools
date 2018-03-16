using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class AnyCharacterClass : CharacterClass
    {
        public override bool Match(char c)
        {
            return c != '\n';
        }
    }
}
