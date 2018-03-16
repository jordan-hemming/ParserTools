using System;
using System.Linq;
using System.Text;
using Penguin.ParserTools.Parser;

namespace Penguin.ParserTools.RegexTests
{
    class TestToken : Token<string>
    {
        public TestToken(string text, string type, int line, int col)
            : base(text, type, line, col)
        {
        }
    }


    class TestTokenizer : BaseTokenizer<TestToken, string>
    {
        public TestTokenizer()
        {
            DefineIgnore("\\s*");
            DefineToken("ident", "[a-zA-Z_][a-zA-Z0-9_]*");
            DefineToken("int", "[0-9]+");
            DefineToken("float", "[0-9]+\\.[0-9]+([eE][-+]?[0-9]+)?");
        }

        protected override TestToken CreateToken(string type, string text, int line, int col)
        {
            return new TestToken(text, type, line, col);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            string input = "someIdent 8367 7384.24 hsudjs sajad";
            var tokenizer = new TestTokenizer();
            var tokens = tokenizer.Tokenize(input);
            foreach (var token in tokens)
                Console.WriteLine(token.ToString(true));
            Console.ReadLine();
        }
    }
}
