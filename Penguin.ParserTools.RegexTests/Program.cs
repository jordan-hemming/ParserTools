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
            string pattern = "[0-9]+\\.[0-9]+([eE][-+]?[0-9]+)?";

            var tokenizer = new RegexTokenizer();
            var tokens = tokenizer.Tokenize(pattern);

            foreach (var token in tokens)
                Console.WriteLine(token.ToString(true));

            var parser = new RegexParser(tokens);
            var regex = parser.Parse();

            var fsm = regex.BuildFSM();
            var testString = "123.456e-890";
            foreach (char c in testString)
            {
                Console.WriteLine("{0}: {1}", c, fsm.Match(c));
            }

            
            Console.ReadLine();
        }
    }
}
