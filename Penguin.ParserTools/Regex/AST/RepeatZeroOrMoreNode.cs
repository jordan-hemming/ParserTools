using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class RepeatZeroOrMoreNode : RegexNode
    {
        public RegexNode Node { get; }

        public RepeatZeroOrMoreNode(RegexNode node)
        {
            Node = node;
        }

        public override void BuildTransitions(HashSet<RegexState> states, RegexState startState, RegexState endState)
        {
            startState.Add(endState);
            Node.BuildTransitions(states, endState, endState);
        }
    }
}
