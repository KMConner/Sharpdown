using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement.InlineParserObjects;

namespace Sharpdown.MarkdownElement.InlineElement
{
    /// <summary>
    /// This class provides methods to parse inline structure.
    /// </summary>
    internal class InlineParser
    {
        /// <summary>
        /// Regular expression which matches urls in auto links.
        /// </summary>
        private static readonly Regex UrlRegex = new Regex(
            @"\<(?<url>[a-zA-Z][a-zA-Z0-9\+\.\-]{1,31}\:[^\<\> \r\n\t]*)\>",
            RegexOptions.Compiled);

        /// <summary>
        /// Regular expression which matches email address in auto links.
        /// </summary>
        private static readonly Regex MailAddressRegex = new Regex(
            @"\<(?<addr>[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+\@"
            + @"[a-zA-Z0-9](?:[a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])??(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])??)*)\>",
            RegexOptions.Compiled);

        /// <summary>
        /// Regular expression which matches raw html.
        /// </summary>
        private static readonly Regex HtmlTagRegex = new Regex(
            @"\<[a-zA-Z][0-9a-z-A-Z\-]*(?:[ \t\r\n]+[a-zA-Z_\:][a-zA-Z0-9_\.\:\-]*[ \t\r\n]*(?:=[ \t\r\n]*(?:[^ \r\n\t\""\'=\<\>`]*|'[^']*?'|\""[^\""]*\""))??)*[ \t\r\n]*\/??\>"
            + @"|\<\/[a-zA-Z][0-9a-z-A-Z\-]*[ \t\r\n]*\>"
            + @"|<\?.*?\?>|<![A-Z]+(?:[ \t\r\n]+[^>]*)??>"
            + @"|<!\[CDATA\[(?s:.*?)\]\]>"
            + @"|<!--(?!>)(?!-\>)(?s:.(.(?<!--))*)(?<!-)-->",
            RegexOptions.Compiled);

        private readonly ParserConfig parserConfig;
        private readonly ReadOnlyDictionary<string, LinkReferenceDefinition> linkReferenceDefinitions;

        internal InlineParser(ParserConfig config, IDictionary<string, LinkReferenceDefinition> links)
        {
            parserConfig = config;
            linkReferenceDefinitions = new ReadOnlyDictionary<string, LinkReferenceDefinition>(links);
        }

        /// <summary>
        /// Parses inline elements and returns them.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>Inline elements in <paramref name="text"/>.</returns>
        public IEnumerable<InlineElement> ParseInlineElements(string text)
        {
            var highPriorityDelims = new List<InlineSpan>();
            int currentIndex = 0;
            int nextBacktick = InlineElementUtils.GetNextUnescaped(text, '`', 0);
            int nextLessThan = InlineElementUtils.GetNextUnescaped(text, '<', 0);

            // Extract code spans, raw html and auto links.
            while (currentIndex < text.Length)
            {
                int newIndex;
                int nextElemIndex;
                InlineElement newInline;

                // Find `
                // Search code span
                if (nextBacktick >= 0 && (nextLessThan < 0 || nextBacktick < nextLessThan))
                {
                    nextElemIndex = nextBacktick;
                    newIndex = nextElemIndex;
                    newInline = GetCodeSpan(text, nextBacktick, ref newIndex);
                }
                // Find <
                // Search raw html and auto links
                else if (nextLessThan >= 0 && (nextBacktick < 0 || nextLessThan < nextBacktick))
                {
                    nextElemIndex = nextLessThan;
                    newIndex = nextElemIndex;
                    newInline = GetInlineHtmlOrLink(text, nextLessThan, ref newIndex);
                }
                else // Find neither ` nor <
                {
                    // End Searching
                    break;
                }

                if (newInline != null)
                {
                    var span = new InlineSpan
                    {
                        Begin = nextElemIndex,
                        End = newIndex,
                        SpanType = InlineElementUtils.ToDelimType(newInline.Type),
                        DelimElem = newInline,
                    };
                    highPriorityDelims.Add(span);

                    currentIndex = newIndex;
                    nextBacktick = InlineElementUtils.GetNextUnescaped(text, '`', currentIndex);
                    nextLessThan = InlineElementUtils.GetNextUnescaped(text, '<', currentIndex);
                }
                else
                {
                    nextElemIndex += InlineElementUtils.CountSameChars(text, nextElemIndex);
                    nextBacktick = InlineElementUtils.GetNextUnescaped(text, '`', nextElemIndex);
                    nextLessThan = InlineElementUtils.GetNextUnescaped(text, '<', nextElemIndex);
                }
            }

            return ParseLinkEmphasis(text, highPriorityDelims);
        }

        /// <summary>
        /// Parses inline elements to Links, Images Emphasis and Line breaks.
        /// </summary>
        /// <param name="text">The string object to @arse.</param>
        /// <param name="higherDelims">Delim Spans which represents higher priority.</param>
        /// <returns>The parse result.</returns>
        private IEnumerable<InlineElement> ParseLinkEmphasis(string text, List<InlineSpan> higherDelims)
        {
            bool IsDelim(int index)
            {
                return !higherDelims.Any(d => d.Begin <= index && d.End > index);
            }

            var deliminators = new LinkedList<DeliminatorInfo>();
            var inlineSpans = new SortedList<int, InlineSpan>();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '*' && !InlineElementUtils.IsEscaped(text, i) && IsDelim(i))
                {
                    int length = InlineElementUtils.CountSameChars(text, i);
                    var delimInfo = DeliminatorInfo.Create(DeliminatorType.Star, i, length);
                    delimInfo.CanOpen = IsLeftFlanking(text, delimInfo);
                    delimInfo.CanClose = IsRightFlanking(text, delimInfo);

                    deliminators.AddLast(delimInfo);
                    i += length - 1;
                }
                else if (text[i] == '_' && !InlineElementUtils.IsEscaped(text, i) && IsDelim(i))
                {
                    int length = InlineElementUtils.CountSameChars(text, i);
                    var delimInfo = DeliminatorInfo.Create(DeliminatorType.UnderBar, i, length);
                    delimInfo.CanOpen = IsLeftFlanking(text, delimInfo)
                                        && (!IsRightFlanking(text, delimInfo) ||
                                            IsPrecededByPunctuation(text, delimInfo));
                    delimInfo.CanClose = IsRightFlanking(text, delimInfo)
                                         && (!IsLeftFlanking(text, delimInfo)
                                             || IsFollowedByPunctuation(text, delimInfo));
                    deliminators.AddLast(delimInfo);

                    i += length - 1;
                }
                else if (text[i] == '[' && !InlineElementUtils.IsEscaped(text, i) && IsDelim(i))
                {
                    deliminators.AddLast(DeliminatorInfo.Create(DeliminatorType.OpenLink, i, 1));
                }
                else if (BeginWith(text, i, "![") &&
                         !InlineElementUtils.IsEscaped(text, i) &&
                         IsDelim(i))
                {
                    deliminators.AddLast(DeliminatorInfo.Create(DeliminatorType.OpenImage, i, 2));
                    i++;
                }
                else if (i > 0 && text[i] == ']' && !InlineElementUtils.IsEscaped(text, i) && IsDelim(i))
                {
                    DeliminatorInfo openInfo = deliminators
                        .LastOrDefault(info => info.Type == DeliminatorType.OpenImage
                                               || info.Type == DeliminatorType.OpenLink);
                    if (openInfo == null)
                    {
                        continue;
                    }

                    if (!openInfo.Active)
                    {
                        deliminators.Remove(openInfo);
                        continue;
                    }

                    int linkLabel = InlineElementUtils.GetStartIndexOfLinkLabel(text, 0, i + 1, higherDelims);
                    int linkBody = InlineElementUtils.GetLinkBodyEndIndex(text, i + 1, out var dest, out var title);
                    int linkLabel2 = InlineElementUtils.GetEndIndexOfLinkLabel(text, i + 1, higherDelims);

                    // Inline Link/Image
                    if (text[Math.Min(i + 1, text.Length - 1)] == '('
                        && InlineElementUtils.AreBracketsBalanced(text.Substring(openInfo.Index,
                            i - openInfo.Index + 1))
                        && linkLabel >= 0 && linkBody >= 0)
                    {
                        if (openInfo.Type == DeliminatorType.OpenLink)
                        {
                            foreach (var item in deliminators.TakeWhile(c => c != openInfo))
                            {
                                if (item.Type == DeliminatorType.OpenLink)
                                {
                                    item.Active = false;
                                }
                            }
                        }

                        inlineSpans.Add(openInfo.Index, new InlineSpan
                        {
                            Begin = openInfo.Index,
                            End = linkBody,
                            SpanType = openInfo.Type == DeliminatorType.OpenLink
                                ? InlineSpanType.Link
                                : InlineSpanType.Image,
                            ParseBegin = openInfo.Index
                                         + (openInfo.Type == DeliminatorType.OpenLink ? 1 : 2),
                            ParseEnd = i,
                            Title = title,
                            Destination = dest ?? string.Empty,
                        });
                        InlineElementUtils.ParseEmphasis(deliminators, inlineSpans, openInfo);
                        deliminators.Remove(openInfo);
                        i = linkBody - 1;
                        deliminators.Remove(openInfo);
                    }
                    // Collapsed Reference Link
                    else if (BeginWith(text, i + 1, "[]")
                             && TryGetReference(text.Substring(openInfo.Index + openInfo.DeliminatorLength,
                                 i - openInfo.Index - openInfo.DeliminatorLength), out var definition))
                    {
                        if (openInfo.Type == DeliminatorType.OpenLink)
                        {
                            foreach (var item in deliminators.TakeWhile(c => c != openInfo))
                            {
                                if (item.Type == DeliminatorType.OpenLink)
                                {
                                    item.Active = false;
                                }
                            }
                        }

                        inlineSpans.Add(openInfo.Index, new InlineSpan
                        {
                            Begin = openInfo.Index,
                            End = i + 3,
                            SpanType = openInfo.Type == DeliminatorType.OpenLink
                                ? InlineSpanType.Link
                                : InlineSpanType.Image,
                            ParseBegin = openInfo.Index
                                         + (openInfo.Type == DeliminatorType.OpenLink ? 1 : 2),
                            ParseEnd = i,
                            Destination = definition.Destination,
                            Title = definition.Title,
                        });
                        InlineElementUtils.ParseEmphasis(deliminators, inlineSpans, openInfo);
                        i += 2;
                        deliminators.Remove(openInfo);
                    }
                    // Full Reference Link
                    else if (text[Math.Min(i + 1, text.Length - 1)] == '[' && linkLabel2 >= 0
                                                                           && TryGetReference(
                                                                               text.Substring(i + 2,
                                                                                   linkLabel2 - i - 3),
                                                                               out definition))
                    {
                        if (openInfo.Type == DeliminatorType.OpenLink)
                        {
                            foreach (var item in deliminators.TakeWhile(c => c != openInfo))
                            {
                                if (item.Type == DeliminatorType.OpenLink)
                                {
                                    item.Active = false;
                                }
                            }
                        }

                        inlineSpans.Add(openInfo.Index, new InlineSpan
                        {
                            Begin = openInfo.Index,
                            End = linkLabel2,
                            SpanType = openInfo.Type == DeliminatorType.OpenLink
                                ? InlineSpanType.Link
                                : InlineSpanType.Image,
                            ParseBegin = openInfo.Index
                                         + (openInfo.Type == DeliminatorType.OpenLink ? 1 : 2),
                            ParseEnd = i,
                            Destination = definition.Destination,
                            Title = definition.Title,
                        });
                        InlineElementUtils.ParseEmphasis(deliminators, inlineSpans, openInfo);
                        i = linkLabel2 - 1;
                        deliminators.Remove(openInfo);
                    }
                    // shortcut link
                    else if (TryGetReference(text.Substring(openInfo.Index + openInfo.DeliminatorLength,
                                 i - openInfo.Index - openInfo.DeliminatorLength), out definition) &&
                             InlineElementUtils.GetEndIndexOfLinkLabel(text, i + 1, higherDelims) < 0)
                    {
                        if (openInfo.Type == DeliminatorType.OpenLink)
                        {
                            foreach (var item in deliminators.TakeWhile(c => c != openInfo))
                            {
                                if (item.Type == DeliminatorType.OpenLink)
                                {
                                    item.Active = false;
                                }
                            }
                        }

                        inlineSpans.Add(openInfo.Index, new InlineSpan
                        {
                            Begin = openInfo.Index,
                            End = i + 1,
                            SpanType = openInfo.Type == DeliminatorType.OpenLink
                                ? InlineSpanType.Link
                                : InlineSpanType.Image,
                            ParseBegin = openInfo.Index
                                         + (openInfo.Type == DeliminatorType.OpenLink ? 1 : 2),
                            ParseEnd = i,
                            Destination = definition.Destination,
                            Title = definition.Title,
                        });
                        InlineElementUtils.ParseEmphasis(deliminators, inlineSpans, openInfo);
                        deliminators.Remove(openInfo);
                    }
                    else
                    {
                        deliminators.Remove(openInfo);
                    }
                }
            }

            InlineElementUtils.ParseEmphasis(deliminators, inlineSpans, null);

            foreach (var item in higherDelims)
            {
                inlineSpans.Add(item.Begin, item);
            }

            var tree = InlineElementUtils.GetInlineTree(inlineSpans, text.Length);

            return InlineElementUtils.ToInlines(text, tree, parserConfig);
        }


        /// <summary>
        /// Returns a code span which starts the specified index.
        /// </summary>
        /// <param name="text">The string which contains the code span.</param>
        /// <param name="index">The index of the first character of the span.</param>
        /// <param name="currentIndex">Updates this value to the index of the next character of the span end.</param>
        /// <returns>The code span which starts with the specified index.</returns>
        private CodeSpan GetCodeSpan(string text, int index, ref int currentIndex)
        {
            int openLength = InlineElementUtils.CountSameChars(text, index);
            int closeIndex = index + openLength;

            do
            {
                closeIndex = text.IndexOf(new string('`', openLength), closeIndex, StringComparison.Ordinal);
                if (closeIndex >= 0)
                {
                    int closeLength = InlineElementUtils.CountSameChars(text, closeIndex);
                    if (closeLength == openLength)
                    {
                        currentIndex = closeIndex + closeLength;
                        return new CodeSpan(
                            text.Substring(index + openLength, closeIndex - index - openLength), parserConfig);
                    }

                    closeIndex += closeLength;
                }
            } while (closeIndex >= 0 && closeIndex < text.Length);

            return null;
        }

        /// <summary>
        /// Parses raw html or auto links which starts with the specified index.
        /// </summary>
        /// <param name="text">String object which contains auto links or raw html.</param>
        /// <param name="index">The index where the element starts.</param>
        /// <param name="currentIndex">Updates this value to the index of the next character of the span end.</param>
        /// <returns>The auto links or raw html if one is found, otherwise <c>null</c>.</returns>
        private InlineElement GetInlineHtmlOrLink(string text, int index, ref int currentIndex)
        {
            // Auto link (URL)
            var urlMatch = UrlRegex.Match(text, index);
            if (urlMatch.Success
                && urlMatch.Index == index
                && urlMatch.Value.All(c => !char.IsControl(c)))
            {
                currentIndex += urlMatch.Length;
                return new Link(urlMatch.Groups["url"].Value, parserConfig);
            }

            // Auto link (E-Mail)
            var emailMatch = MailAddressRegex.Match(text, index);
            if (emailMatch.Success && emailMatch.Index == index)
            {
                currentIndex += emailMatch.Length;
                var target = emailMatch.Groups["addr"].Value;
                return new Link(target, parserConfig, true);
            }

            // Inline html
            var htmlTagMatch = HtmlTagRegex.Match(text, index);
            if (htmlTagMatch.Success && htmlTagMatch.Index == index)
            {
                currentIndex += htmlTagMatch.Length;
                return new InlineHtml(htmlTagMatch.Value, parserConfig);
            }

            return null;
        }

        private bool TryGetReference(string refName, out LinkReferenceDefinition referenceDefinition)
        {
            var name = LinkReferenceDefinition.GetSimpleName(refName);
            return linkReferenceDefinitions.TryGetValue(name, out referenceDefinition);
        }


        /// <summary>
        /// Returns whether the specified deliminator is left flanking.
        /// See https://spec.commonmark.org/0.28/#left-flanking-delimiter-run for more information.
        /// </summary>
        /// <param name="text">The string object which contains <paramref name="info"/>.</param>
        /// <param name="info">
        /// The <see cref="DeliminatorInfo"/> to determine whether left flanking or not.
        /// </param>
        /// <returns>Whether the specified deliminator is left flanking.</returns>
        private static bool IsLeftFlanking(string text, DeliminatorInfo info)
        {
            var prevChar = info.Index == 0 ? ' ' : text[info.Index - 1];
            var nextChar = info.Index + info.DeliminatorLength >= text.Length
                ? ' '
                : text[info.Index + info.DeliminatorLength];
            if (char.IsWhiteSpace(nextChar))
            {
                return false;
            }

            if (!MarkdownElementBase.asciiPunctuationChars.Contains(nextChar))
            {
                return true;
            }

            return char.IsWhiteSpace(prevChar) || MarkdownElementBase.asciiPunctuationChars.Contains(prevChar);
        }

        /// <summary>
        /// Returns whether the specified deliminator is right flanking.
        /// See https://spec.commonmark.org/0.28/#right-flanking-delimiter-run for more information.
        /// </summary>
        /// <param name="text">The string object which contains <paramref name="info"/>.</param>
        /// <param name="info">
        /// The <see cref="DeliminatorInfo"/> to determine whether right flanking or not.
        /// </param>
        /// <returns>Whether the specified deliminator is right flanking.</returns>
        private static bool IsRightFlanking(string text, DeliminatorInfo info)
        {
            var prevChar = info.Index == 0 ? ' ' : text[info.Index - 1];
            var nextChar = info.Index + info.DeliminatorLength >= text.Length
                ? ' '
                : text[info.Index + info.DeliminatorLength];
            if (char.IsWhiteSpace(prevChar))
            {
                return false;
            }

            if (!MarkdownElementBase.asciiPunctuationChars.Contains(prevChar))
            {
                return true;
            }

            return char.IsWhiteSpace(nextChar) || MarkdownElementBase.asciiPunctuationChars.Contains(nextChar);
        }


        /// <summary>
        /// Returns whether the specified <see cref="DeliminatorInfo"/> is preceded by a punctuation character.
        /// </summary>
        /// <param name="text">The string which contains <paramref name="info"/>.</param>
        /// <param name="info">The <see cref="DeliminatorInfo"/> object.</param>
        /// <returns>
        /// <c>true</c> when the specified <see cref="DeliminatorInfo"/> is preceded by a punctuation character,
        /// otherwise <c>false</c>.
        /// </returns>
        private static bool IsPrecededByPunctuation(string text, DeliminatorInfo info)
        {
            return info.Index != 0 && MarkdownElementBase.asciiPunctuationChars.Contains(text[info.Index - 1]);
        }

        /// <summary>
        /// Returns whether the specified <see cref="DeliminatorInfo"/> is followed by a punctuation character.
        /// </summary>
        /// <param name="text">The string which contains <paramref name="info"/>.</param>
        /// <param name="info">The <see cref="DeliminatorInfo"/> object.</param>
        /// <returns>
        /// <c>true</c> when the specified <see cref="DeliminatorInfo"/> is followed by a punctuation character,
        /// otherwise <c>false</c>.
        /// </returns>
        private static bool IsFollowedByPunctuation(string text, DeliminatorInfo info)
        {
            if (text.Length <= info.Index + info.DeliminatorLength)
            {
                return false;
            }

            return MarkdownElementBase.asciiPunctuationChars
                .Contains(text[info.Index + info.DeliminatorLength]);
        }

        private static bool BeginWith(string str, int index, string other)
        {
            if (str.Length < index + other.Length)
            {
                return false;
            }

            for (int i = 0; i < other.Length; i++)
            {
                if (str[index + i] != other[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
