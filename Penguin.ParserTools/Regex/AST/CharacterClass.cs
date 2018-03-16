using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools.Regex.AST
{
    /// <summary>
    /// Base class representing Regex character classes.
    /// </summary>
    public abstract class CharacterClass
    {
        /// <summary>
        /// Attempt to match the specified character to the character class.
        /// </summary>
        /// <param name="c">The character to match.</param>
        /// <returns>True if the match is successful.</returns>
        public abstract bool Match(char c);
    }
}
