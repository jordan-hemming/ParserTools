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
        /// <summary>
        /// Build the necessary transitions between the start and end states of the RegexNode.
        /// </summary>
        /// <param name="states">The list of requied states.</param>
        /// <param name="startState">The starting state.</param>
        /// <param name="endState">The end (completed) state.</param>
        public abstract void BuildTransitions(HashSet<RegexState> states, RegexState startState, RegexState endState);

        /// <summary>
        /// Builds a finite state machine from the RegexNode.
        /// </summary>
        /// <returns></returns>
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
