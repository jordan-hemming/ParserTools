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
    }
}
