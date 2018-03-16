using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class ChoiceNode
    {
        private List<RegexNode> _nodes;

        public ChoiceNode()
        {
            _nodes = new List<RegexNode>();
        }

        public ChoiceNode(params RegexNode[] nodes)
        {
            _nodes = new List<RegexNode>(nodes);
        }

        public void Add(RegexNode node)
        {
            _nodes.Add(node);
        }
    }
}
