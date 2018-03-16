using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools
{
    /// <summary>
    /// Various utility functions for characters.
    /// </summary>
    public static class CharUtilities
    {
        /// <summary>
        /// Checks if character is a hex digit.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <param name="val">The numerical representation of the hex digit.</param>
        /// <returns>True if the character is a hex digit.</returns>
        public static bool IsHexDigit(char c, out int val)
        {
            if (c >= '0' && c <= '9')
            {
                val = c - '0';
                return true;
            }
            else if (c >= 'a' && c <= 'f')
            {
                val = c - 'a' + 10;
                return true;
            }
            else if (c >= 'A' && c <= 'F')
            {
                val = c - 'A' + 10;
                return true;
            }
            else
            {
                val = 0;
                return false;
            }
        }

        /// <summary>
        /// Checks if character is a hex digit.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>True if the charcter is a hex digit.</returns>
        public static bool IsHexDigit(char c)
        {
            return IsHexDigit(c, out int val);
        }
    }
}
