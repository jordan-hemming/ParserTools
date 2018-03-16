using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    class RepeatZeroOrMoreNode : RegexNode
    {
        public RegexNode Node { get; }

        public RepeatZeroOrMoreNode(RegexNode node)
        {
            Node = node;
        }
    }
}
