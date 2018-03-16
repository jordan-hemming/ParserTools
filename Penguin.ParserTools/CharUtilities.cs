using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.ParserTools
{
    public static class CharUtilities
    {
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

        public static bool IsHexDigit(char c)
        {
            return IsHexDigit(c, out int val);
        }
    }
}
