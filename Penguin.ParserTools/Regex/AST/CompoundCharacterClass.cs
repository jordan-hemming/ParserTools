using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class CompoundCharacterClass : CharacterClass
    {
        private List<CharacterClass> _subClasses = new List<CharacterClass>();

        public bool IsInverted { get; set; }

        public void Add(CharacterClass subClass)
        {
            _subClasses.Add(subClass);
        }

        public override bool Match(char c)
        {
            if (IsInverted)
            {
                foreach (var subClass in _subClasses)
                {
                    if (subClass.Match(c))
                        return false;
                }
                return true;
            }
            else
            {
                foreach (var subClass in _subClasses)
                {
                    if (subClass.Match(c))
                        return true;
                }
                return false;
            }
        }
    }
}
