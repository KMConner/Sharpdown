﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents markdown elements that the type cannnot be determined
    /// from only the first line.
    /// </summary>
    /// <seealso cref="Paragraph"/>
    /// <seealso cref="LinkReferenceDefinition"/>
    /// <seealso cref="SetextHeader"/>
    public class UnknownElement : LeafElement
    {
        /// <summary>
        /// A regular expression which matches link label.
        /// </summary>
        private static readonly Regex linkLabelRegex = new Regex(
            @"^\[(?:[^\]]|\\\]){1,999}\]\:", RegexOptions.Compiled);


        /// <summary>
        /// A regular expression which matches link reference definition.
        /// </summary>
        private static readonly Regex linkDefinitionRegex = new Regex(
            @"^\[(?<label>(?:[^\]]|\\\]){1,999})\]\:[ \t]*(?:\r|\r\n|\n)??[ \t]*(?<destination>\<(?:[^ \t\r\n\<\>]|\\\<|\\\>)+\>|[^ \t\r\n]+)([ \t]*(?: |\t|\r|\r\n|\n)[ \t]*(?<title>\""(?:[^\""]|\\\"")*\""|\'(?:[^\']|\\\')*\'|\((?:[^\)]|\\\))*\)))??[ \t]*$",
            RegexOptions.Compiled);

        /// <summary>
        /// The actual type of the current object.
        /// </summary>
        private BlockElementType actualType;

        /// <summary>
        /// The content in this object.
        /// </summary>
        internal List<string> content;

        /// <summary>
        /// The header level.(Used when this object is SetextHeading.)
        /// </summary>
        private int headerLevel;

        /// <summary>
        /// Wether this block can be a link reference definition.
        /// </summary>
        private bool mayBeLinkReferenceDefinition;


        public override BlockElementType Type => BlockElementType.Unknown;

        internal UnknownElement() : base()
        {
            content = new List<string>();
            actualType = BlockElementType.Unknown;
        }

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        internal override AddLineResult AddLine(string line, bool lazy)
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
                    && !lazy
                    && (removed[0] == '-' || removed[0] == '=')
                    && removed.All(c => removed[0] == c))
                {
                    actualType = BlockElementType.SetextHeading;
                    headerLevel = removed[0] == '=' ? 1 : 2;
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

        /// <summary>
        /// Returns wether the paragraph needs to be interrupted.
        /// </summary>
        /// <param name="line">The single line of string.</param>
        /// <returns>
        /// <c>true</c> if the paragraph needs to be interrupted,
        /// otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Returns wether parentheses in the specified string is balanecd.
        /// </summary>
        /// <param name="str">
        /// String object to determine wether the parentheses are balanecd.
        /// </param>
        /// <returns>
        /// <c>true</c> if  parentheses in the specified string is balanecd,
        /// otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Removes ", or ' from link title.
        /// </summary>
        /// <param name="titleString">Link title.</param>
        /// <returns>
        /// Title string without quote characters at the begging or the end.
        /// </returns>
        private string ExtractTitle(string titleString)
        {
            if (string.IsNullOrEmpty(titleString))
            {
                return titleString;
            }

            if ((titleString[0] == '"' && titleString[titleString.Length - 1] == '"')
                || (titleString[0] == '\'' && titleString[titleString.Length - 1] == '\''))
            {
                return titleString.Substring(1, titleString.Length - 2);
            }
            return titleString;
        }

        /// <summary>
        /// Removes &lt;, &gt; from link destination.
        /// </summary>
        /// <param name="destString">Link destination.</param>
        /// <returns>
        /// Link destination string without &lt; or &gt; character at the beginning or the end.
        /// </returns>
        private string ExtractDestination(string destString)
        {
            if (string.IsNullOrEmpty(destString))
            {
                return destString;
            }
            if (destString[0] == '<' && destString[destString.Length - 1] == '>')
            {
                return destString.Substring(1, destString.Length - 2);
            }
            return destString;
        }

        /// <summary>
        /// Closes this <see cref="UnknownElement"/>.
        /// </summary>
        /// <returns>
        /// Ths closed block.
        /// The <see cref="Type"/> is determined in this phase.
        /// 
        /// <para>
        /// If <see cref="actualType"/> is one of <see cref="BlockElementType.Paragraph"/>,
        /// <see cref="BlockElementType.SetextHeading"/> or
        /// <see cref="BlockElementType.LinkReferenceDefinition"/>, the type of returned block
        /// will be the same type of <see cref="actualType"/>.
        /// Otherwise (<see cref="actualType"/> is <see cref="BlockElementType.Unknown"/>),
        /// if <see cref="content"/> matches <see cref="linkDefinitionRegex"/>, the type will
        /// be <see cref="BlockElementType.LinkReferenceDefinition"/>, if not matched,
        /// the type will be <see cref="BlockElementType.Paragraph"/>.
        /// </para>
        /// </returns>
        internal override BlockElement Close()
        {
            switch (actualType)
            {
                case BlockElementType.SetextHeading:
                    return new SetextHeader(this, headerLevel);

                case BlockElementType.LinkReferenceDefinition:
                    {

                        Match match = linkDefinitionRegex.Match(string.Join("\n", content));
                        if (!match.Success)
                        {
                            throw new InvalidBlockFormatException(BlockElementType.LinkReferenceDefinition);
                        }
                        return new LinkReferenceDefinition(match.Groups["label"].Value, ExtractDestination(match.Groups["destination"].Value), ExtractTitle(match.Groups["title"].Value), this);
                    }

                case BlockElementType.Paragraph:
                    return new Paragraph(this);

                case BlockElementType.Unknown:
                    {
                        if (mayBeLinkReferenceDefinition)
                        {
                            string joined = string.Join("\n", content);
                            if (linkDefinitionRegex.IsMatch(joined))
                            {
                                Match match = linkDefinitionRegex.Match(joined);
                                if (AreParenthesesBalanced(match.Groups["destination"].Value))
                                {
                                    return new LinkReferenceDefinition(match.Groups["label"].Value, ExtractDestination(match.Groups["destination"].Value), ExtractTitle(match.Groups["title"].Value), this);
                                }
                            }
                        }
                        return new Paragraph(this);
                    }
                default:
                    throw new InvalidBlockFormatException(BlockElementType.Unknown);
            }
        }

        internal override void ParseInline(IEnumerable<string> linkDefinitions)
        {
            throw new InvalidOperationException();
        }
    }
}
