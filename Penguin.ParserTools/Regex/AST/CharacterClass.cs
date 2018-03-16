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
        public abstract bool Match(char c);
    }
}
