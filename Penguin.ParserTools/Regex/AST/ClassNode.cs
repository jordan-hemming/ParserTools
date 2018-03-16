using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class ClassNode : RegexNode
    {
        public CharacterClass CharacterClass { get; }

        public ClassNode(CharacterClass charClass)
        {
            CharacterClass = charClass;
        }

        public override void BuildTransitions(HashSet<RegexState> states, RegexState startState, RegexState endState)
        {
            startState.Add(endState, CharacterClass);
        }
    }
}
