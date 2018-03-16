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

        public SequenceNode(params RegexNode[] nodes)
        {
            _nodes = new List<RegexNode>(nodes);
        }

        public void Add(RegexNode node)
        {
            _nodes.Add(node);
        }
    }
}
