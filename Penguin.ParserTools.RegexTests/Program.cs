using System;
using System.Linq;
using System.Text;
using Penguin.ParserTools.Regex;

namespace Penguin.ParserTools.RegexTests
{
    class Program
    {
        static void Main(string[] args)
        {
            string intPattern = "[0-9]+";

            var tokenizer = new RegexTokenizer();
            var tokens = tokenizer.Tokenize(intPattern);

            foreach (var token in tokens)
            {
                Console.WriteLine(token.ToString(true));
            }
            
            Console.ReadLine();
        }
    }
}
