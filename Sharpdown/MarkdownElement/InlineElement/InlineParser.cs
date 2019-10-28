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
        #region Regex

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

        #endregion

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
            text = text.Trim();
            var highPriorityDelims = new List<InlineSpan>();
            int currentIndex = 0;
            int nextBacktick = GetNextUnescaped(text, '`', 0);
            int nextLessThan = GetNextUnescaped(text, '<', 0);

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
                        SpanType = ToDelimType(newInline.Type),
                        DelimElem = newInline,
                    };
                    highPriorityDelims.Add(span);

                    currentIndex = newIndex;
                    nextBacktick = GetNextUnescaped(text, '`', currentIndex);
                    nextLessThan = GetNextUnescaped(text, '<', currentIndex);
                }
                else
                {
                    nextElemIndex += CountSameChars(text, nextElemIndex);
                    nextBacktick = GetNextUnescaped(text, '`', nextElemIndex);
                    nextLessThan = GetNextUnescaped(text, '<', nextElemIndex);
                }
            }

            return ParseLinkEmphasis(text, highPriorityDelims);
        }

        /// <summary>
        /// Parses inline elements to Links, Images Emphasis and Line breaks.
        /// </summary>
        /// <param name="text">The string object to @arse.</param>
        /// <param name="higherSpans">Delim Spans which represents higher priority.</param>
        /// <returns>The parse result.</returns>
        private IEnumerable<InlineElement> ParseLinkEmphasis(string text, List<InlineSpan> higherSpans)
        {
            var deliminators = new LinkedList<DeliminatorInfo>();
            var inlineSpans = new SortedList<int, InlineSpan>();

            for (int i = 0; i < text.Length; i++)
            {
                // If the character is escaped or contained in higherSpans
                if (IsEscaped(text, i) || higherSpans.Any(d => d.Begin <= i && d.End > i))
                {
                    continue;
                }

                if (text[i] == '*')
                {
                    int length = CountSameChars(text, i);
                    var delimInfo = DeliminatorInfo.Create(DeliminatorType.Star, i, length);
                    delimInfo.CanOpen = IsLeftFlanking(text, delimInfo);
                    delimInfo.CanClose = IsRightFlanking(text, delimInfo);

                    deliminators.AddLast(delimInfo);
                    i += length - 1;
                }
                else if (text[i] == '_')
                {
                    int length = CountSameChars(text, i);
                    var delimInfo = DeliminatorInfo.Create(DeliminatorType.UnderBar, i, length);
                    bool leftFranking = IsLeftFlanking(text, delimInfo);
                    bool rightFlanking = IsRightFlanking(text, delimInfo);

                    delimInfo.CanOpen = leftFranking && (!rightFlanking || IsPrecededByPunctuation(text, delimInfo));
                    delimInfo.CanClose = rightFlanking && (!leftFranking || IsFollowedByPunctuation(text, delimInfo));
                    deliminators.AddLast(delimInfo);
                    i += length - 1;
                }
                else if (text[i] == '[')
                {
                    deliminators.AddLast(DeliminatorInfo.Create(DeliminatorType.OpenLink, i, 1));
                }
                else if (BeginWith(text, i, "!["))
                {
                    deliminators.AddLast(DeliminatorInfo.Create(DeliminatorType.OpenImage, i, 2));
                    i++;
                }
                else if (i > 0 && text[i] == ']')
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

                    int linkLabel = GetStartIndexOfLinkLabel(text, 0, i + 1, higherSpans);
                    int linkBody = GetLinkBodyEndIndex(text, i + 1, out var dest, out var title);
                    int linkLabel2 = GetEndIndexOfLinkLabel(text, i + 1, higherSpans);

                    // Inline Link/Image
                    if (text[Math.Min(i + 1, text.Length - 1)] == '(' && linkLabel >= 0 && linkBody >= 0)
                    {
                        inlineSpans.Add(openInfo.Index,
                            InlineSpan.FromDeliminatorInfo(openInfo, linkBody, i, dest ?? string.Empty, title));
                        i = linkBody - 1;
                    }
                    // Collapsed Reference Link
                    else if (BeginWith(text, i + 1, "[]")
                             && TryGetReference(text.Substring(openInfo.Index + openInfo.DeliminatorLength,
                                 i - openInfo.Index - openInfo.DeliminatorLength), out var definition))
                    {
                        inlineSpans.Add(openInfo.Index, InlineSpan.FromDeliminatorInfo(openInfo, i + 3, i, definition));
                        i += 2;
                    }
                    // Full Reference Link
                    else if (linkLabel2 >= 0
                             && TryGetReference(text.Substring(i + 2, linkLabel2 - i - 3), out definition))
                    {
                        inlineSpans.Add(openInfo.Index,
                            InlineSpan.FromDeliminatorInfo(openInfo, linkLabel2, i, definition));
                        i = linkLabel2 - 1;
                    }
                    // shortcut link
                    else if (TryGetReference(text.Substring(openInfo.Index + openInfo.DeliminatorLength,
                                 i - openInfo.Index - openInfo.DeliminatorLength), out definition) &&
                             GetEndIndexOfLinkLabel(text, i + 1, higherSpans) < 0)
                    {
                        inlineSpans.Add(openInfo.Index, InlineSpan.FromDeliminatorInfo(openInfo, i + 1, i, definition));
                    }
                    else
                    {
                        deliminators.Remove(openInfo);
                        continue;
                    }

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

                    ParseEmphasis(deliminators, inlineSpans, openInfo);
                    deliminators.Remove(openInfo);
                }
            }

            ParseEmphasis(deliminators, inlineSpans, null);

            foreach (var item in higherSpans)
            {
                inlineSpans.Add(item.Begin, item);
            }

            var tree = GetInlineTree(inlineSpans, text.Length);

            return ToInlines(text, tree);
        }

        /// <summary>
        /// Parses the structure and add the <see cref="InlineSpan"/> to <paramref name="delimSpans"/>.
        /// </summary>
        /// <param name="deliminators">The list of <see cref="DeliminatorInfo"/>.</param>
        /// <param name="delimSpans">The list of <see cref="InlineSpan"/>.</param>
        /// <param name="stackBottom">The stack bottom.</param>
        private static void ParseEmphasis(LinkedList<DeliminatorInfo> deliminators,
            SortedList<int, InlineSpan> delimSpans, DeliminatorInfo stackBottom)
        {
            if (deliminators.Count == 0)
            {
                return;
            }

            LinkedListNode<DeliminatorInfo> firstClose = null;
            LinkedListNode<DeliminatorInfo> infoNode =
                stackBottom == null ? deliminators.First : deliminators.Find(stackBottom);
            while (infoNode != null)
            {
                if (infoNode.Value.CanClose)
                {
                    firstClose = infoNode;
                    break;
                }

                infoNode = infoNode.Next;
            }

            if (firstClose == null)
            {
                return;
            }

            LinkedListNode<DeliminatorInfo> startDelimNode = null;
            infoNode = firstClose;
            while ((infoNode = infoNode.Previous) != null && infoNode.Value != stackBottom)
            {
                if (infoNode.Value.CanOpen
                    && infoNode.Value.Type == firstClose.Value.Type
                    && ((infoNode.Value.DeliminatorLength + firstClose.Value.DeliminatorLength) % 3 != 0
                        || infoNode.Value.DeliminatorLength % 3 == 0
                        || (!firstClose.Value.CanOpen && !infoNode.Value.CanClose)))
                {
                    startDelimNode = infoNode;
                    break;
                }
            }

            if (startDelimNode == null)
            {
                if (!firstClose.Value.CanOpen)
                {
                    deliminators.Remove(firstClose);
                }

                firstClose = firstClose.Next;
                if (firstClose == null)
                {
                    return;
                }

                ParseEmphasis(deliminators, delimSpans, firstClose.Value);
                return;
            }

            DeliminatorInfo openInfo = startDelimNode.Value;
            DeliminatorInfo closeInfo = firstClose.Value;
            int delimLength = openInfo.DeliminatorLength > 1 && closeInfo.DeliminatorLength > 1 ? 2 : 1;
            var delimSpan = new InlineSpan
            {
                Begin = openInfo.Index + openInfo.DeliminatorLength - delimLength,
                End = closeInfo.Index + delimLength,
                SpanType =
                    delimLength > 1 ? InlineSpanType.StrongEmphasis : InlineSpanType.Emphasis,
            };
            delimSpan.ParseBegin = delimSpan.Begin + delimLength;
            delimSpan.ParseEnd = delimSpan.End - delimLength;
            delimSpans.Add(delimSpan.Begin, delimSpan);
            while ((infoNode = infoNode.Next) != null && infoNode != firstClose)
            {
                deliminators.Remove(infoNode);
            }

            openInfo.DeliminatorLength -= delimLength;
            if (openInfo.DeliminatorLength <= 0)
            {
                deliminators.Remove(openInfo);
            }

            closeInfo.DeliminatorLength -= delimLength;
            closeInfo.Index += delimLength;
            if (closeInfo.DeliminatorLength <= 0)
            {
                deliminators.Remove(closeInfo);
            }

            ParseEmphasis(deliminators, delimSpans,
                deliminators.Find(firstClose.Value) == null ? firstClose.Next?.Value : firstClose.Value);
        }

        /// <summary>
        /// Creates a tree of <see cref="InlineSpan"/> from its array.
        /// </summary>
        /// <param name="spans">The array of <see cref="InlineSpan"/>.</param>
        /// <param name="rootLength">The length of the whole text.</param>
        /// <returns>The tree of <see cref="InlineSpan"/> which is equivalent to <paramref name="spans"/>.</returns>
        private static InlineSpan GetInlineTree(SortedList<int, InlineSpan> spans, int rootLength)
        {
            var root = new InlineSpan
            {
                Begin = 0,
                End = rootLength,
                SpanType = InlineSpanType.Root,
            };
            InlineSpan currentSpan = root;
            foreach (var item in spans)
            {
                var delimSpan = item.Value;
                while (currentSpan.End < delimSpan.End)
                {
                    currentSpan = currentSpan.Parent ?? throw new Exception();
                }

                currentSpan.Children.Add(delimSpan.Begin, delimSpan);
                delimSpan.Parent = currentSpan;
                currentSpan = delimSpan;
            }

            return root;
        }

        /// <summary>
        /// Converts the specified string and <see cref="InlineSpan"/> tree to inline elements.
        /// </summary>
        /// <param name="text">The string object.</param>
        /// <param name="delim">The <see cref="InlineSpan"/> tree.</param>
        /// <returns>The inline elements which is equivalent to <paramref name="delim"/>.</returns>
        private InlineElement[] ToInlines(string text, InlineSpan delim)
        {
            int lastEnd = delim.ParseBegin;
            List<InlineElement> newChildren = new List<InlineElement>();
            foreach (var item in delim.Children.Where(d => d.Value.End <= delim.ParseEnd))
            {
                InlineSpan delimSpan = item.Value;
                if (lastEnd < delimSpan.Begin)
                {
                    newChildren.AddRange(
                        ParseLineBreak(text.Substring(lastEnd, delimSpan.Begin - lastEnd)));
                }

                newChildren.AddRange(ToInlines(text, delimSpan));
                lastEnd = delimSpan.End;
            }

            if (lastEnd < delim.ParseEnd)
            {
                newChildren.AddRange(
                    ParseLineBreak(text.Substring(lastEnd, delim.ParseEnd - lastEnd)));
            }

            switch (delim.SpanType)
            {
                case InlineSpanType.Link:
                    return new InlineElement[]
                        {new Link(newChildren.ToArray(), delim.Destination, delim.Title, parserConfig)};
                case InlineSpanType.Image:
                    return new InlineElement[]
                        {new Image(newChildren.ToArray(), delim.Destination, delim.Title, parserConfig)};
                case InlineSpanType.Emphasis:
                    return new InlineElement[] { new Emphasis(newChildren.ToArray(), false, parserConfig) };
                case InlineSpanType.StrongEmphasis:
                    return new InlineElement[] { new Emphasis(newChildren.ToArray(), true, parserConfig) };
                case InlineSpanType.Root:
                    return newChildren.ToArray();
                default:
                    return new[] { delim.DelimElem };
            }
        }

        /// <summary>
        /// Returns <see cref="InlineElement"/> 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private IEnumerable<InlineElement> ParseLineBreak(string text)
        {
            string[] lines = text.Replace("\r\n", "\n").Replace("\r", "\n").Split(new[] { '\n' });
            for (int i = 0; i < lines.Length; i++)
            {
                bool isHardBreak = lines[i].EndsWith("  ", StringComparison.Ordinal)
                                   || (lines[i].EndsWith("\\", StringComparison.Ordinal) &&
                                       !IsEscaped(lines[i], lines.Length - 1));
                if (i < lines.Length - 1)
                {
                    if (lines[i] != string.Empty)
                    {
                        if (isHardBreak && lines[i].EndsWith("\\", StringComparison.Ordinal))
                        {
                            yield return InlineText.CreateFromText(lines[i].Substring(0, lines[i].Length - 1),
                                parserConfig);
                        }
                        else
                        {
                            yield return InlineText.CreateFromText(lines[i].TrimEnd(new[] { ' ' }), parserConfig);
                        }
                    }

                    yield return isHardBreak
                        ? (InlineElement)new HardLineBreak(parserConfig)
                        : new SoftLineBreak(parserConfig);
                }
                else
                {
                    if (lines[i] != string.Empty)
                    {
                        yield return InlineText.CreateFromText(lines[i], parserConfig);
                    }
                }
            }
        }

        #region Extract auto links, inline html and code span

        /// <summary>
        /// Returns a code span which starts the specified index.
        /// </summary>
        /// <param name="text">The string which contains the code span.</param>
        /// <param name="index">The index of the first character of the span.</param>
        /// <param name="currentIndex">Updates this value to the index of the next character of the span end.</param>
        /// <returns>The code span which starts with the specified index.</returns>
        private CodeSpan GetCodeSpan(string text, int index, ref int currentIndex)
        {
            int openLength = CountSameChars(text, index);
            int closeIndex = index + openLength;

            do
            {
                closeIndex = text.IndexOf(new string('`', openLength), closeIndex, StringComparison.Ordinal);
                if (closeIndex >= 0)
                {
                    int closeLength = CountSameChars(text, closeIndex);
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

        #endregion

        #region Used in ParseLinkEmphasis

        /// <summary>
        /// Tries to get Link definition from <see cref="linkReferenceDefinitions"/>.
        /// </summary>
        /// <param name="refName">The label of the link definition.</param>
        /// <param name="referenceDefinition">
        /// If this method returns <c>true</c>, a <see cref="LinkReferenceDefinition"/> object will set.
        /// </param>
        /// <returns>Whether proper link definition is found.</returns>
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

        #region Parsing Links

        /// <summary>
        /// Gets the start index of link label.
        /// </summary>
        /// <param name="wholeText">Whole text.</param>
        /// <param name="begin">Begin of the seatch area.</param>
        /// <param name="end">End of the link label.</param>
        /// <param name="higherDelims">Higher delims.</param>
        /// <returns>The start index of link label.</returns>
        private static int GetStartIndexOfLinkLabel(string wholeText, int begin, int end, List<InlineSpan> higherDelims)
        {
            bool IsDelim(int index)
            {
                return !higherDelims.Any(d => d.Begin <= index && d.End > index);
            }

            if (wholeText[end - 1] != ']')
            {
                return -1;
            }

            int depth = 0;

            for (int i = end - 1; i >= begin; i--)
            {
                char ch = wholeText[i];
                char chPrev = wholeText[Math.Max(0, i - 1)];
                if (chPrev == '\\')
                {
                    continue;
                }

                if (ch != '[' && ch != ']')
                {
                    continue;
                }

                if (!IsDelim(i))
                {
                    continue;
                }

                if (ch == '[')
                {
                    depth--;
                    if (depth == 0)
                    {
                        if (chPrev == '!')
                        {
                            return i - 1;
                        }

                        return i;
                    }
                }
                else if (ch == ']')
                {
                    depth++;
                }
            }

            return -1;
        }

        private static int GetEndIndexOfLinkLabel(string wholeText, int begin, List<InlineSpan> higherDelims)
        {
            if (wholeText.Length <= begin)
            {
                return -1;
            }

            if (wholeText[begin] != '[')
            {
                return -1;
            }

            int depth = 0;
            for (int i = begin; i < wholeText.Length; i++)
            {
                if (IsEscaped(wholeText, i))
                {
                    continue;
                }

                if (higherDelims.Any(d => d.Begin <= i && d.End > i))
                {
                    continue;
                }

                char ch = wholeText[i];
                if (ch == '[')
                {
                    depth++;
                    continue;
                }

                if (ch == ']')
                {
                    depth--;
                    if (depth == 0)
                    {
                        return i + 1;
                    }

                    if (depth < 0)
                    {
                        return -1;
                    }

                    break;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the index of the of the index of the end of the link body.
        /// </summary>
        /// <param name="wholeText">Whole text.</param>
        /// <param name="begin">The being index of the body.</param>
        /// <param name="destination">Destination is set unless return value is -1.</param>
        /// <param name="title">Title is set unless return value is -1.</param>
        /// <returns>The index at the end of the link body.</returns>
        private static int GetLinkBodyEndIndex(string wholeText, int begin, out string destination, out string title)
        {
            destination = string.Empty;
            title = null;

            if (wholeText.Length <= begin || wholeText[begin] != '(')
            {
                return -1;
            }

            int current = begin + 1;

            // Search begging of the destination.
            for (; current < wholeText.Length; current++)
            {
                if (!MarkdownElementBase.whiteSpaceChars.Contains(wholeText[current]))
                {
                    break;
                }
            }

            int destinationStart = current;

            if (wholeText[destinationStart] == ')')
            {
                return begin + 2;
            }

            // Extract destination
            if (wholeText[destinationStart] == '<')
            {
                current++;
                for (; current < wholeText.Length; current++)
                {
                    char ch = wholeText[current];
                    if (MarkdownElementBase.lineBreaks.Contains(ch))
                    {
                        return -1;
                    }

                    if (ch == '<' && !IsEscaped(wholeText, current))
                    {
                        return -1;
                    }

                    if (ch == '>' && !IsEscaped(wholeText, current))
                    {
                        destination = wholeText.Substring(destinationStart + 1, current - destinationStart - 1);
                        break;
                    }
                }

                if (current >= wholeText.Length)
                {
                    return -1;
                }
            }
            else
            {
                int parenDepth = 0;
                for (; current < wholeText.Length; current++)
                {
                    char ch = wholeText[current];
                    if (MarkdownElementBase.whiteSpaceChars.Contains(ch) || char.IsControl(ch))
                    {
                        if (parenDepth != 0)
                        {
                            return -1;
                        }

                        destination = wholeText.Substring(destinationStart, current - destinationStart);
                        break;
                    }

                    if (ch == '(' && !IsEscaped(wholeText, current))
                    {
                        parenDepth++;
                    }

                    if (ch == ')' && !IsEscaped(wholeText, current))
                    {
                        parenDepth--;
                        if (parenDepth < 0)
                        {
                            current--;
                            destination = wholeText.Substring(destinationStart, current - destinationStart + 1);
                            break;
                        }
                    }
                }

                if (parenDepth > 0)
                {
                    return -1;
                }
            }

            current++;

            // Search begging of the title
            for (; current < wholeText.Length; current++)
            {
                if (!MarkdownElementBase.whiteSpaceChars.Contains(wholeText[current]))
                {
                    break;
                }
            }

            if (current >= wholeText.Length)
            {
                return -1;
            }

            // Without title
            if (wholeText[current] == ')')
            {
                return current + 1;
            }

            int titleBegin = current + 1;

            switch (wholeText[current])
            {
                case '"':
                    current = GetNextUnescaped(wholeText, '"', current + 1);
                    break;
                case '\'':
                    current = GetNextUnescaped(wholeText, '\'', current + 1);
                    break;
                case '(':
                    current = GetNextUnescaped(wholeText, ')', current + 1);
                    break;
                default:
                    return -1;
            }

            title = wholeText.Substring(titleBegin, current - titleBegin);
            current++;

            for (; current < wholeText.Length; current++)
            {
                if (!MarkdownElementBase.whiteSpaceChars.Contains(wholeText[current]))
                {
                    break;
                }
            }

            if (wholeText[current] != ')')
            {
                return -1;
            }

            return current + 1;
        }

        #endregion

        #endregion

        #region Commonly used for text processing

        private static int GetNextUnescaped(string str, char ch, int index)
        {
            int ret = index;
            while (true)
            {
                ret = str.IndexOf(ch, ret);
                if (ret <= 0 || !IsEscaped(str, ret))
                {
                    return ret;
                }

                ret++;
            }
        }

        /// <summary>
        /// Counts how many times the same characters are repeated.
        /// </summary>
        /// <param name="str">The string object.</param>
        /// <param name="index">The character index in <paramref name="str"/>.</param>
        /// <returns>
        /// How many times the same characters are repeated. (1 or more.)
        /// </returns>
        private static int CountSameChars(string str, int index)
        {
            if (index < 0)
            {
                throw new ArgumentException("index must be 0 or more.");
            }

            for (int i = index; i < str.Length; i++)
            {
                if (str[i] != str[index])
                {
                    return i - index;
                }
            }

            return str.Length - index;
        }

        private static bool IsEscaped(string text, int index)
        {
            int count = 0;
            for (int i = index - 1; i >= 0; i--)
            {
                if (text[i] == '\\')
                {
                    count++;
                }
                else
                {
                    return count % 2 != 0;
                }
            }

            return index % 2 != 0;
        }

        #endregion

        private static InlineSpanType ToDelimType(InlineElementType elemType)
        {
            switch (elemType)
            {
                case InlineElementType.CodeSpan:
                    return InlineSpanType.CodeSpan;
                case InlineElementType.Link:
                    return InlineSpanType.AutoLink;
                case InlineElementType.InlineHtml:
                    return InlineSpanType.InlineHtml;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
