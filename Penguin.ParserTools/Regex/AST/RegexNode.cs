using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    /// <summary>
    /// Base class representing complete Regex expression.
    /// </summary>
    public abstract class RegexNode
    {
        public abstract void BuildTransitions(HashSet<RegexState> states, RegexState startState, RegexState endState);

        public RegexFSM BuildFSM()
        {
            var states = new HashSet<RegexState>();
            var startState = new RegexState();
            var endState = new RegexState();
            BuildTransitions(states, startState, endState);
            return new RegexFSM(states, startState, endState);
        }
    }
}
