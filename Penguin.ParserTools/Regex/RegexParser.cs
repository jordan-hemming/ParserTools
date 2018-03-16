using System;
using System.Collections.Generic;
using System.Text;
using Penguin.ParserTools.Parser;
using Penguin.ParserTools.Regex.AST;

namespace Penguin.ParserTools.Regex
{
    /// <summary>
    /// Parser for Regex expressions.
    /// </summary>
    public class RegexParser : BaseParser<RegexToken, RegexTokenType>, IParser<RegexFSM>, IParser<RegexNode>
    {
        /// <summary>
        /// Create a RegexParser using the specified token list as input.
        /// </summary>
        /// <param name="tokens">The token list to use as input.</param>
        public RegexParser(IEnumerable<RegexToken> tokens)
            : base(tokens)
        {
            
        }
        
        private char ParseBoxCharacter()
        {
            if (Accept(RegexTokenSubType.Backslash))
            {
                Expect(RegexTokenSubType.EscapeCharacter, out var token);
                return token.EscapeCharacter.Value;
            }
            else
            {
                Expect(RegexTokenSubType.NormalCharacter, out var token);
                return token.NormalCharacter.Value;
            }
        }

        private CharacterClass ParseBoxRange()
        {
            var from = ParseBoxCharacter();
            if (Accept(RegexTokenSubType.Range))
            {
                var to = ParseBoxCharacter();
                return new RangeCharacterClass(from, to);
            }
            else
            {
                return new SingleCharacterClass(from);
            }
        }

        private CharacterClass ParseBoxClass()
        {
            bool invert = false;
            CharacterClass result = new NullCharacterClass();
            if (Accept(RegexTokenSubType.Not))
                invert = true;
            if (Accept(RegexTokenSubType.Range))
                result = new SingleCharacterClass('-');
            while (!Peek(RegexTokenSubType.CloseBracket))
                result = new CompoundCharacterClass(result, ParseBoxRange());
            if (invert)
                return new InvertCharacterClass(result);
            else
                return result;
        }

        private CharacterClass ParseCharacterClass()
        {
            if (Accept(RegexTokenSubType.SpecialClass, out var specialToken))
            {
                return specialToken.SpecialClass;
            }
            else if (Accept(RegexTokenSubType.OpenBracket))
            {
                var boxClass = ParseBoxClass();
                Expect(RegexTokenSubType.CloseBracket);
                return boxClass;
            }
            else if (Accept(RegexTokenSubType.Backslash))
            {
                if (Accept(RegexTokenSubType.EscapeClass, out var escapeClassToken))
                    return escapeClassToken.EscapeClass;
                Expect(RegexTokenSubType.EscapeCharacter, out var escapeCharToken);
                return new SingleCharacterClass(escapeCharToken.EscapeCharacter.Value);
            }
            else
            {
                Expect(RegexTokenSubType.NormalCharacter, out var normalToken);
                return new SingleCharacterClass(normalToken.NormalCharacter.Value);
            }
        }

        private RegexNode ParseAtom()
        {
            if (Accept(RegexTokenSubType.OpenParan))
            {
                var result = ParseRegex();
                Expect(RegexTokenSubType.CloseParan);
                return result;
            }
            else
            {
                return new ClassNode(ParseCharacterClass());
            }
        }

        private RegexNode ParseArityNode()
        {
            var result = ParseAtom();
            if (Accept(RegexTokenSubType.ZeroOrMore))
                result = new RepeatZeroOrMoreNode(result);
            else if (Accept(RegexTokenSubType.ZeroOrOne))
                result = new OptionalNode(result);
            else if (Accept(RegexTokenSubType.OneOrMore))
                result = new RepeatOneOrMoreNode(result);
            return result;
        }

        private RegexNode ParseSequence()
        {
            var result = ParseArityNode();
            while (!EndOfFile() && !Peek(RegexTokenSubType.Choice) && !Peek(RegexTokenSubType.CloseParan))
                result = new SequenceNode(result, ParseArityNode());
            return result;
        }

        private RegexNode ParseRegex()
        {
            var result = ParseSequence();
            while (Accept(RegexTokenSubType.Choice))
                result = new ChoiceNode(result, ParseSequence());
            return result;
        }

        /// <summary>
        /// Parse the input and return a RegexFSM.
        /// </summary>
        /// <returns>The RegexFSM represented by the input.</returns>
        public RegexFSM Parse()
        {
            return ParseRegex().BuildFSM();
        }
        
        /// <summary>
        /// Parse the input and return a RegexNode.
        /// </summary>
        /// <returns>The RegexNode represented by the input.</returns>
        RegexNode IParser<RegexNode>.Parse()
        {
            return ParseRegex();
        }
    }
}
