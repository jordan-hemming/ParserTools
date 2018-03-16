using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class OptionalNode : RegexNode
    {
        public RegexNode Node { get; }

        public OptionalNode(RegexNode node)
        {
            Node = node;
        }

        public override void BuildTransitions(HashSet<RegexState> states, RegexState startState, RegexState endState)
        {
            Node.BuildTransitions(states, startState, endState);
            startState.Add(endState);
        }
    }
}
