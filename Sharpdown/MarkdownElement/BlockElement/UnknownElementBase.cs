using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class UnknownElement : LeafElementBase
    {
        private static readonly Regex linkLabelRegex = new Regex(
            @"^\[(?:[^\]]|\\\]){1,999}\]\:", RegexOptions.Compiled);

        private static readonly Regex linkDefinitionRegex = new Regex(
            @"^\[(?<label>(?:[^\]]|\\\]){1,999})\]\:[ \t]*(?:\r|\r\n|\n)??[ \t]*(?<destination>\<(?:[^ \t\r\n\<\>]|\\\<|\\\>)+\>|[^ \t\r\n]+)([ \t]*(?: |\t|\r|\r\n|\n)[ \t]*(?<title>\""(?:[^\""]|\\\"")*\""|\'(?:[^\']|\\\')*\'|\((?:[^\)]|\\\))*\)))??[ \t]*$",
            RegexOptions.Compiled);

        private List<string> content;
        private bool mayBeLinkReferenceDefinition;
        internal UnknownElement() : base()
        {
            content = new List<string>();
            actualType = BlockElementType.Unknown;
        }
        private BlockElementType actualType;

        public override BlockElementType Type => BlockElementType.Unknown;

        internal override AddLineResult AddLine(string line)
        {
            var trimmed = line.TrimStartAscii();
            if (content.Count == 0)
            {
                if (line.GetIndentNum() >= 4 || line.GetIndentNum() < 0)
                {
                    throw new InvalidBlockFormatException(BlockElementType.Unknown);
                }

                mayBeLinkReferenceDefinition = linkLabelRegex.IsMatch(trimmed);
            }
            else if (line.GetIndentNum() < 4)
            {
                if (ListBlock.CanInterruptParagraph(line))
                {
                    actualType = BlockElementType.Paragraph;
                    return AddLineResult.NeedClose;
                }

                string removed = line.Trim(whiteSpaceShars);
                if (removed.Length > 0
                    && (removed[0] == '-' || removed[0] == '=')
                    && removed.All(c => removed[0] == c))
                {
                    actualType = BlockElementType.SetextHeading;
                    return AddLineResult.Consumed | AddLineResult.NeedClose;
                }
            }

            if (Interrupted(line))
            {
                actualType = linkDefinitionRegex.IsMatch(string.Join("\n", content))
                    ? BlockElementType.LinkReferenceDefinition : BlockElementType.Paragraph;
                return AddLineResult.NeedClose;
            }

            content.Add(trimmed);

            if (mayBeLinkReferenceDefinition)
            {
                string joined = string.Join("\n", content);
                if (linkDefinitionRegex.IsMatch(joined))
                {
                    Match match = linkDefinitionRegex.Match(joined);
                    if (AreParenthesesBalanced(match.Groups["destination"].Value))
                    {
                        if (match.Groups["title"].Success)
                        {
                            actualType = BlockElementType.LinkReferenceDefinition;
                            return AddLineResult.Consumed | AddLineResult.NeedClose;
                        }
                        else if (linkDefinitionRegex.IsMatch(string.Join("\n", content.GetRange(0, content.Count - 1))))
                        {
                            content.RemoveAt(content.Count - 1);
                            actualType = BlockElementType.LinkReferenceDefinition;
                            return AddLineResult.NeedClose;
                        }

                    }
                    else
                    {
                        mayBeLinkReferenceDefinition = false;
                    }
                }
                else
                {
                    if (linkDefinitionRegex.IsMatch(string.Join("\n", content.GetRange(0, content.Count - 1))))
                    {
                        content.RemoveAt(content.Count - 1);
                        actualType = BlockElementType.LinkReferenceDefinition;
                        return AddLineResult.NeedClose;
                    }
                }
            }


            return AddLineResult.Consumed;
        }

        private bool Interrupted(string line)
        {
            if (BlockQuote.CanStartBlock(line)
                || ThemanticBreak.CanStartBlock(line)
                || AtxHeaderElement.CanStartBlock(line)
                || FencedCodeBlock.CanStartBlock(line)
                || HtmlBlock.CanInterruptParagraph(line)
                || BlankLine.CanStartBlock(line))
            {
                return true;
            }
            return false;
        }

        private bool AreParenthesesBalanced(string str)
        {
            int depth = 0;
            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '(':
                        depth++;
                        break;
                    case ')':
                        depth--;
                        if (depth < 0)
                        {
                            return false;
                        }
                        break;

                    case '\\':
                        i++;
                        break;
                    default:
                        break;
                }
            }
            return depth == 0;
        }

        internal override BlockElement Close()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}
