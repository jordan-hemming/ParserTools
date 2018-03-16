using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class CompoundCharacterClass : CharacterClass
    {
        private List<CharacterClass> _subClasses;
        
        public CompoundCharacterClass()
        {
            _subClasses = new List<CharacterClass>();
        }

        public CompoundCharacterClass(CharacterClass charClass, CharacterClass other)
        {
            if (charClass is CompoundCharacterClass)
            {
                _subClasses = (charClass as CompoundCharacterClass)._subClasses;
                _subClasses.Add(other);
            }
            else if (charClass is NullCharacterClass)
            {
                _subClasses = new List<CharacterClass>() { other };
            }
            else
            {
                _subClasses = new List<CharacterClass>() { charClass, other };
            }
        }

        public CompoundCharacterClass(params CharacterClass[] subClasses)
        {
            _subClasses = new List<CharacterClass>(subClasses);
        }

        public void Add(CharacterClass subClass)
        {
            if (subClass is CompoundCharacterClass)
            {
                _subClasses.AddRange((subClass as CompoundCharacterClass)._subClasses);
            }
            else if (!(subClass is NullCharacterClass))
            {
                _subClasses.Add(subClass);
            }
        }

        public override bool Match(char c)
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
