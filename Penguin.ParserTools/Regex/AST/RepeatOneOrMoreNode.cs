using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class RepeatOneOrMoreNode : RegexNode
    {
        public RegexNode Node { get; }

        public RepeatOneOrMoreNode(RegexNode node)
        {
            Node = node;
        }
    }
}
