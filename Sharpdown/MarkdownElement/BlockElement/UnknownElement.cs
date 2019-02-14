using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents markdown elements that the type cannot be determined
    /// from only the first line.
    /// </summary>
    /// <seealso cref="Paragraph"/>
    /// <seealso cref="LinkReferenceDefinition"/>
    /// <seealso cref="SetextHeader"/>
    public class UnknownElement : LeafElement
    {
        /// <summary>
        /// A regular expression which matches link reference definition.
        /// </summary>
        private static readonly Regex linkDefinitionRegex = new Regex(
            @"^\[(?<label>(?:[^\]\[]|\\\]|\\\[){1,999})\]\:[ \t]*(?:\r|\r\n|\n)??[ \t]*(?<destination>\<(?:[^ \t\r\n\<\>]|\\\<|\\\>)+\>|[^ \t\r\n]+)([ \t]*(?: |\t|\r|\r\n|\n)[ \t]*(?<title>\""(?:[^\""]|\\\"")*\""|\'(?:[^\']|\\\')*\'|\((?:[^\)]|\\\))*\)))??[ \t]*$",
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
        /// Whether this block can be a link reference definition.
        /// </summary>
        private bool mayBeLinkReferenceDefinition;

        public override BlockElementType Type => BlockElementType.Unknown;

        public override string Content => string.Join("\n", content);

        internal UnknownElement()
        {
            content = new List<string>();
            actualType = BlockElementType.Unknown;
        }

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        internal override AddLineResult AddLine(string line, bool lazy, int currentIndent)
        {
            var trimmed = line.TrimStartAscii();
            if (content.Count == 0)
            {
                if (line.GetIndentNum(currentIndent) >= 4 || line.GetIndentNum(currentIndent) < 0)
                {
                    throw new InvalidBlockFormatException(BlockElementType.Unknown);
                }

                mayBeLinkReferenceDefinition = trimmed.StartsWith("[", StringComparison.Ordinal);
            }
            else if (line.GetIndentNum(currentIndent) < 4)
            {
                if (ListBlock.CanInterruptParagraph(line, currentIndent))
                {
                    actualType = BlockElementType.Paragraph;
                    return AddLineResult.NeedClose;
                }

                string removed = line.Trim(whiteSpaceChars);
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

            if (Interrupted(line, currentIndent))
            {
                var match = linkDefinitionRegex.Match(string.Join("\n", content));
                actualType = match.Success && !IsBlank(match.Groups["label"].Value)
                    ? BlockElementType.LinkReferenceDefinition
                    : BlockElementType.Paragraph;
                return AddLineResult.NeedClose;
            }

            content.Add(trimmed);

            if (mayBeLinkReferenceDefinition)
            {
                string joined = string.Join("\n", content);
                var match0 = linkDefinitionRegex.Match(joined);
                if (match0.Success && !IsBlank(match0.Groups["label"].Value))
                {
                    Match match = linkDefinitionRegex.Match(joined);
                    if (AreParenthesesBalanced(match.Groups["destination"].Value))
                    {
                        if (match.Groups["title"].Success)
                        {
                            actualType = BlockElementType.LinkReferenceDefinition;
                            return AddLineResult.Consumed | AddLineResult.NeedClose;
                        }

                        if (linkDefinitionRegex.IsMatch(string.Join("\n", content.GetRange(0, content.Count - 1))))
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
                    var match1 = linkDefinitionRegex.Match(string.Join("\n", content.GetRange(0, content.Count - 1)));
                    if (match1.Success && !IsBlank(match1.Groups["label"].Value))
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
        /// Returns whether the paragraph needs to be interrupted.
        /// </summary>
        /// <param name="line">The single line of string.</param>
        /// <returns>
        /// <c>true</c> if the paragraph needs to be interrupted,
        /// otherwise, <c>false</c>.
        /// </returns>
        private bool Interrupted(string line, int currentIndent)
        {
            if (BlockQuote.CanStartBlock(line, currentIndent)
                || ThematicBreak.CanStartBlock(line, currentIndent)
                || AtxHeaderElement.CanStartBlock(line, currentIndent)
                || FencedCodeBlock.CanStartBlock(line, currentIndent)
                || HtmlBlock.CanInterruptParagraph(line)
                || BlankLine.CanStartBlock(line))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns whether parentheses in the specified string is balanced.
        /// </summary>
        /// <param name="str">
        /// String object to determine whether the parentheses are balanced.
        /// </param>
        /// <returns>
        /// <c>true</c> if  parentheses in the specified string is balanced,
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
                    if (!match.Success || IsBlank(match.Groups["label"].Value))
                    {
                        throw new InvalidBlockFormatException(BlockElementType.LinkReferenceDefinition);
                    }

                    return new LinkReferenceDefinition(match.Groups["label"].Value,
                        ExtractDestination(match.Groups["destination"].Value),
                        match.Groups["title"].Success ? ExtractTitle(match.Groups["title"].Value) : null, this);
                }

                case BlockElementType.Paragraph:
                    return new Paragraph(this);

                case BlockElementType.Unknown:
                {
                    if (mayBeLinkReferenceDefinition)
                    {
                        string joined = string.Join("\n", content);
                        Match match = linkDefinitionRegex.Match(joined);

                        if (match.Success && !IsBlank(match.Groups["label"].Value))
                        {
                            if (AreParenthesesBalanced(match.Groups["destination"].Value))
                            {
                                return new LinkReferenceDefinition(match.Groups["label"].Value,
                                    ExtractDestination(match.Groups["destination"].Value),
                                    match.Groups["title"].Success ? ExtractTitle(match.Groups["title"].Value) : null,
                                    this);
                            }
                        }
                    }

                    return new Paragraph(this);
                }
                default:
                    throw new InvalidBlockFormatException(BlockElementType.Unknown);
            }
        }

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions)
        {
            throw new InvalidOperationException();
        }

        private bool IsBlank(string str)
        {
            foreach (var ch in str)
            {
                if (!whiteSpaceChars.Contains(ch))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
