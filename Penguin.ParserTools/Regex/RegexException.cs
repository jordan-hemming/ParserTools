using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex
{
    /// <summary>
    /// Base exception raised when tokenizing/parsing Regex expressions.
    /// </summary>
    class RegexException : Exception
    {
        public RegexException(string msg)
            : base(msg)
        {

        }
    }

    /// <summary>
    /// Exception raised when parsing invalid character ranges in Regex expressions (i.e. [z-a]).
    /// </summary>
    class InvalidCharacterRangeException : RegexException
    {
        public InvalidCharacterRangeException(string msg)
            : base(msg)
        {
            
        }
    }
}
