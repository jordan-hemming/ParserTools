using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class RepeatNode : RegexNode
    {
        public RegexNode Node { get; }

        public RepeatNode(RegexNode node)
        {
            Node = node;
        }
    }
}
