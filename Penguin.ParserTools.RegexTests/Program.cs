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
            string floatPattern = "[0-9]+\\.[0-9]+([eE][-+]?[0-9]+)?";

            var tokenizer = new RegexTokenizer();
            var tokens = tokenizer.Tokenize(floatPattern);

            foreach (var token in tokens)
                Console.WriteLine(token.ToString(true));

            var parser = new RegexParser(tokens);
            var regex = parser.Parse();
            
            Console.ReadLine();
        }
    }
}
