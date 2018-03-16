using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex
{
    class RegexException : Exception
    {
        public RegexException(string msg)
            : base(msg)
        {

        }
    }

    class InvalidCharacterRangeException : RegexException
    {
        public InvalidCharacterRangeException(string msg)
            : base(msg)
        {
            
        }
    }
}
