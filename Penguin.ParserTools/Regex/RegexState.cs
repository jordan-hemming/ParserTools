using System;
using System.Collections.Generic;
using System.Text;
using Penguin.ParserTools.Regex.AST;

namespace Penguin.ParserTools.Regex
{
    /// <summary>
    /// Represents an internal state of a RegexFSM.
    /// </summary>
    public class RegexState
    {
        private List<RegexStateTransition> _classTransitions = new List<RegexStateTransition>();
        private List<RegexState> _emptyTransitions = new List<RegexState>();

        private bool _isActive = false;
        /// <summary>
        /// Flag representing if the state is active. Setting to true will also set the active flag of any states connected by
        /// empty transisitions.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                if (_isActive)
                {
                    foreach (var state in _emptyTransitions)
                    {
                        if (!state.IsActive)
                            state.IsActive = true;
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to match the specified character and sets the active flag of any states connected by valid transitions.
        /// </summary>
        /// <param name="c">The character to match.</param>
        public void Match(char c)
        {
            foreach (var trans in _classTransitions)
            {
                if (trans.CharacterClass.Match(c))
                    trans.Target.IsActive = true;
            }
        }

        /// <summary>
        /// Adds a transition to the specified state when a given character class is matched.
        /// </summary>
        /// <param name="state">The state to transition to on match.</param>
        /// <param name="charClass">The charcter class to attempt to match.</param>
        public void Add(RegexState state, CharacterClass charClass)
        {
            _classTransitions.Add(new RegexStateTransition(state, charClass));
        }

        /// <summary>
        /// Adds an empty transition the the specified state. States connected by empty transitions are always set to active when
        /// the current state is set to active.
        /// </summary>
        /// <param name="state">The state to transition to.</param>
        public void Add(RegexState state)
        {
            _emptyTransitions.Add(state);
        }
    }

    class RegexStateTransition
    {
        public RegexState Target { get; }
        public CharacterClass CharacterClass { get; }

        public RegexStateTransition(RegexState target, CharacterClass charClass)
        {
            Target = target;
            CharacterClass = charClass;
        }
    }
}
