using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class RepeatOneOrMoreNode : RegexNode
    {
        public RegexNode Node { get; }

        public RepeatOneOrMoreNode(RegexNode node)
        {
            Node = node;
        }

        public override void BuildTransitions(HashSet<RegexState> states, RegexState startState, RegexState endState)
        {
            Node.BuildTransitions(states, startState, endState);
            Node.BuildTransitions(states, endState, endState);
        }
    }
}
