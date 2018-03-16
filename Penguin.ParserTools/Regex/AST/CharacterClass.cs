using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    public abstract class CharacterClass
    {
        public abstract bool Match(char c);
    }
}
