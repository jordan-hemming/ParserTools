using System;
using System.Collections.Generic;
using System.Text;
using Penguin.ParserTools.Regex.AST;

namespace Penguin.ParserTools.Regex
{
    public class RegexState
    {
        private List<RegexStateTransition> _classTransitions = new List<RegexStateTransition>();
        private List<RegexState> _emptyTransitions = new List<RegexState>();

        private bool _isActive = false;
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

        public void Match(char c)
        {
            foreach (var trans in _classTransitions)
            {
                if (trans.CharacterClass.Match(c))
                    trans.Target.IsActive = true;
            }
        }

        public void Add(RegexState state, CharacterClass charClass)
        {
            _classTransitions.Add(new RegexStateTransition(state, charClass));
        }

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
