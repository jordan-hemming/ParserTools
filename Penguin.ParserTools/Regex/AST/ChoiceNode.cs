using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class ChoiceNode : RegexNode
    {
        private List<RegexNode> _nodes;

        public ChoiceNode()
        {
            _nodes = new List<RegexNode>();
        }

        public ChoiceNode(RegexNode node, RegexNode other)
        {
            if (node is ChoiceNode)
            {

                _nodes = (node as ChoiceNode)._nodes;
                _nodes.Add(other);
            }
            else
            {
                _nodes = new List<RegexNode>() { node, other };
            }
        }

        public ChoiceNode(params RegexNode[] nodes)
        {
            _nodes = new List<RegexNode>(nodes);
        }

        public void Add(RegexNode node)
        {
            if (node is ChoiceNode)
            {
                _nodes.AddRange((node as ChoiceNode)._nodes);
            }
            else
            {
                _nodes.Add(node);
            }
        }

        public override void BuildTransitions(HashSet<RegexState> states, RegexState startState, RegexState endState)
        {
            foreach (var node in _nodes)
                node.BuildTransitions(states, startState, endState);
        }
    }
}
