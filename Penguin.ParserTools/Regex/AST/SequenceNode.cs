using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class SequenceNode : RegexNode
    {
        private List<RegexNode> _nodes;

        public SequenceNode()
        {
            _nodes = new List<RegexNode>();
        }

        public SequenceNode(RegexNode node, RegexNode other)
        {
            if (node is SequenceNode)
            {
                _nodes = (node as SequenceNode)._nodes;
                _nodes.Add(other);
            }
            else
            {
                _nodes = new List<RegexNode>() { node, other };
            }
        }

        public SequenceNode(params RegexNode[] nodes)
        {
            _nodes = new List<RegexNode>(nodes);
        }

        public void Add(RegexNode node)
        {
            if (node is SequenceNode)
                _nodes.AddRange((node as SequenceNode)._nodes);
            else
                _nodes.Add(node);
        }

        public override void BuildTransitions(HashSet<RegexState> states, RegexState startState, RegexState endState)
        {
            var lastState = startState;
            for (int i = 0; i < _nodes.Count - 1; i++)
            {
                var node = _nodes[i];
                var nextState = new RegexState();
                node.BuildTransitions(states, lastState, nextState);
                states.Add(nextState);
                lastState = nextState;
            }
            _nodes[_nodes.Count - 1].BuildTransitions(states, lastState, endState);
        }
    }
}
