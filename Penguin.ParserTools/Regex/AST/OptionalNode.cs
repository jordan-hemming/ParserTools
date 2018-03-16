using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class OptionalNode : RegexNode
    {
        public RegexNode Node { get; }

        public OptionalNode(RegexNode node)
        {
            Node = node;
        }
    }
}
