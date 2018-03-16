using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex
{
    public class RegexFSM
    {
        private HashSet<RegexState> _states;

        public RegexState StartState { get; }
        public RegexState EndState { get; }

        public RegexResult LastResult { get; private set; }

        public RegexFSM(IEnumerable<RegexState> states, RegexState startState, RegexState endState)
        {
            _states = new HashSet<RegexState>(states);
            _states.Add(startState);
            _states.Add(endState);
            StartState = startState;
            EndState = endState;
            Reset();
        }

        public void Reset()
        {
            foreach (var state in _states)
                state.IsActive = false;
            StartState.IsActive = true;
            LastResult = RegexResult.Matching;
        }

        protected void Clear()
        {
            foreach (var state in _states)
                state.IsActive = false;
        }

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

    public enum RegexResult
    {
        Matching,
        Matched,
        NotMatched
    }
}
