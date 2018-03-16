using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class ClassNode
    {
        public CharacterClass CharacterClass { get; }

        public ClassNode(CharacterClass charClass)
        {
            CharacterClass = charClass;
        }
    }
}
