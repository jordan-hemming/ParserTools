using System;
using System.Collections.Generic;
using System.Text;
using Penguin.ParserTools.Parser;
using Penguin.ParserTools.Regex.AST;

namespace Penguin.ParserTools.Regex
{
    public class RegexParser : BaseParser<RegexToken, RegexTokenType>, IParser<RegexNode>
    {
        public RegexParser(IEnumerable<RegexToken> tokens)
            : base(tokens)
        {
            
        }

        protected char ParseBoxCharacter()
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

        protected CharacterClass ParseBoxRange()
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
        
        protected CharacterClass ParseBoxClass()
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

        protected CharacterClass ParseCharacterClass()
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

        protected RegexNode ParseAtom()
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

        protected RegexNode ParseArityNode()
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

        protected RegexNode ParseSequence()
        {
            var result = ParseArityNode();
            while (!EndOfFile() && !Peek(RegexTokenSubType.Choice) && !Peek(RegexTokenSubType.CloseParan))
                result = new SequenceNode(result, ParseArityNode());
            return result;
        }

        protected RegexNode ParseRegex()
        {
            var result = ParseSequence();
            while (Accept(RegexTokenSubType.Choice))
                result = new ChoiceNode(result, ParseSequence());
            return result;
        }

        public RegexNode Parse()
        {
            return ParseRegex();
        }
    }
}
