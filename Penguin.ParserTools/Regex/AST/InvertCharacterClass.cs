using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class InvertCharacterClass : CharacterClass
    {
        public CharacterClass CharacterClass { get; }

        public InvertCharacterClass(CharacterClass characterClass)
        {
            CharacterClass = characterClass;
        }

        public override bool Match(char c)
        {
            return !CharacterClass.Match(c);
        }
    }
}
