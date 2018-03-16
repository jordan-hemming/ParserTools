using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex
{
    /// <summary>
    /// Class representing complete Regex non-deterministic finite state machine.
    /// </summary>
    public class RegexFSM
    {
        private HashSet<RegexState> _states;

        /// <summary>
        /// The starting state of the FSM. Set to active on reset.
        /// </summary>
        public RegexState StartState { get; }
        /// <summary>
        /// The ending (completed) state of the FSM. Set to active on successful match.
        /// </summary>
        public RegexState EndState { get; }

        /// <summary>
        /// The result of the last Match() call.
        /// </summary>
        public RegexResult LastResult { get; private set; }

        /// <summary>
        /// Creates a RegexFSM object.
        /// </summary>
        /// <param name="states">Collection of all required states.</param>
        /// <param name="startState">The starting state.</param>
        /// <param name="endState">The end (completed) state.</param>
        public RegexFSM(IEnumerable<RegexState> states, RegexState startState, RegexState endState)
        {
            _states = new HashSet<RegexState>(states);
            _states.Add(startState);
            _states.Add(endState);
            StartState = startState;
            EndState = endState;
            Reset();
        }

        /// <summary>
        /// Resets the FSM, clearing the IsActive flag of all but the starting state. Also sets LastResult to 'Matching'.
        /// </summary>
        public void Reset()
        {
            foreach (var state in _states)
                state.IsActive = false;
            StartState.IsActive = true;
            LastResult = RegexResult.Matching;
        }

        /// <summary>
        /// Resets the IsActive flag of all states.
        /// </summary>
        protected void Clear()
        {
            foreach (var state in _states)
                state.IsActive = false;
        }

        /// <summary>
        /// Attempts the match the specified character.
        /// </summary>
        /// <param name="c">The character tp match.</param>
        /// <returns>The result of the match attempt.</returns>
        public RegexResult Match(char c)
        {
            var activeStates = _states.Where(x => x.IsActive).ToList();
            Clear();
            foreach (var state in activeStates)
                state.Match(c);

            if (EndState.IsActive)
                LastResult = RegexResult.Matched;
            else if (_states.Any(x => x.IsActive))
                LastResult = RegexResult.Matching;
            else
                LastResult = RegexResult.NotMatched;

            return LastResult;
        }
    }

    /// <summary>
    /// Possible results of a RegexFSM.Match() call.
    /// </summary>
    public enum RegexResult
    {
        /// <summary>
        /// The FSM is still active (at least one active state) but not yet complete.
        /// </summary>
        Matching,
        /// <summary>
        /// The FSM is complete (i.e. fully matched).
        /// </summary>
        Matched,
        /// <summary>
        /// The FSM has failed to match. No active states remain.
        /// </summary>
        NotMatched
    }
}
