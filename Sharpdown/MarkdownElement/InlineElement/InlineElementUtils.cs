using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sharpdown.MarkdownElement.BlockElement;
using System.Text;

namespace Sharpdown.MarkdownElement.InlineElement
{
    /// <summary>
    /// Provides some methods for Inline parsing.
    /// </summary>
    public static class InlineElementUtils
    {
        /// <summary>
        /// Regular expression which matches InlineLink.
        /// </summary>
        private static readonly Regex InlineLinkRegex = new Regex(
            @"!??\[(?<label>(?:[^\]]|\\\]){1,999})\]\([ \t]*(?:\r|\r\n|\n)??[ \t]*"
            + @"(?<destination>\<(?:[^ \t\r\n\<\>]|\\\<|\\\>)+\>|[^ \t\r\n]+)([ \t]*(?: |\t|\r|\r\n|\n)[ \t]*"
            + @"(?<title>\""(?:[^\""]|\\\"")*\""|\'(?:[^\']|\\\')*\'|\((?:[^\)]|\\\))*\)))??[ \t]*\)",
            RegexOptions.Compiled);

        /// <summary>
        /// Regular expression which matches link labels.
        /// </summary>
        private static readonly Regex LinkLabelRegex = new Regex(
            @"\[(?<label>(?:[^\]]|\\\]){1,999})\]", RegexOptions.Compiled);

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
            @"\<[a-zA-Z][0-9a-z-A-Z\-]*(?:[ \t\r\n]+[a-zA-Z_\:][a-zA-Z0-9_\.\:\-]*[ \t\r\n]*(?:=[ \t\r\n]*(?:[^ \r\n\t\""\'=\<\>`]*|'[^ ']*?' |\""[^\""]*\""))??)*[ \t\r\n]*\/??\>"
            + @"|\<\/[a-zA-Z][0-9a-z-A-Z\-]*[ \t\r\n]*\>"
            + @"|<\?.*?\?>|<![A-Z]+(?:[ \t\r\n]+[^>]*)??>"
            + @"|<!\[CDATA\[(?s:.*?)\]\]>"
            + @"|<!--(?!>)(?!-\>)(?s:.(.(?<!--))*)(?<!-)-->",
            RegexOptions.Compiled);

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

            if (!InlineElementBase.asciiPunctuationChars.Contains(nextChar))
            {
                return true;
            }

            return char.IsWhiteSpace(prevChar) || InlineElementBase.asciiPunctuationChars.Contains(prevChar);
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

            if (!InlineElementBase.asciiPunctuationChars.Contains(prevChar))
            {
                return true;
            }

            return char.IsWhiteSpace(nextChar) || InlineElementBase.asciiPunctuationChars.Contains(nextChar);
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

            return InlineElementBase.asciiPunctuationChars
                .Contains(text[info.Index + info.DeliminatorLength]);
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
            return info.Index != 0 && InlineElementBase.asciiPunctuationChars.Contains(text[info.Index - 1]);
        }

        /// <summary>
        /// Returns <see cref="InlineElementBase[]"/> 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static IEnumerable<InlineElementBase> ParseLineBreak(string text)
        {
            string[] lines = text.Replace("\r\n", "\n").Replace("\r", "\n").Split(new[] { '\n' });
            for (int i = 0; i < lines.Length; i++)
            {

                bool isHardBreak = lines[i].EndsWith("  ")
                    || (lines[i].EndsWith("\\") && !IsEscaped(lines[i], lines.Length - 1));
                if (i < lines.Length - 1)
                {
                    if (lines[i] != string.Empty)
                    {
                        if (isHardBreak && lines[i].EndsWith("\\"))
                        {
                            yield return InlineText.CreateFromText(lines[i].Substring(0, lines[i].Length - 1));
                        }
                        else
                        {
                            yield return InlineText.CreateFromText(lines[i].TrimEnd(new[] { ' ' }));
                        }
                    }
                    yield return isHardBreak
                        ? (InlineElementBase)new HardLineBreak()
                        : new SoftLineBreak();
                }
                else
                {
                    if (lines[i] != string.Empty)
                    {
                        yield return InlineText.CreateFromText(lines[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Represents the nodes of deliminator stack.
        /// </summary>
        /// <remarks>
        /// See https://spec.commonmark.org/0.28/#delimiter-stack for more information about deliminator stack.
        /// </remarks>
        private class DeliminatorInfo
        {
            /// <summary>
            /// The types of deliminators.
            /// </summary>
            public enum DeliminatorType
            {
                /// <summary>
                /// Open Link ([).
                /// </summary>
                OpenLink,

                /// <summary>
                /// Opem Image (![).
                /// </summary>
                OpenImage,

                /// <summary>
                /// Star (*).
                /// </summary>
                Star,

                /// <summary>
                /// Underbar (_).
                /// </summary>
                Underbar,
            }

            /// <summary>
            /// The length of the deliminator.
            /// </summary>
            public int DeliminatorLength { get; set; }

            /// <summary>
            /// The type pf the deliminator.
            /// </summary>
            public DeliminatorType Type { get; set; }

            /// <summary>
            /// Whether the deliminator is active.
            /// </summary>
            public bool Active { get; set; } = true;

            /// <summary>
            /// Whether the deliminator is a potential opener.
            /// </summary>
            public bool CanOpen { get; set; }

            /// <summary>
            /// Whether the deliminator is potential closer.
            /// </summary>
            public bool CanClose { get; set; }

            /// <summary>
            /// The index of the first character of the current deliminator.
            /// </summary>
            public int Index { get; set; }
        }

        /// <summary>
        /// Represents the open and close pair of <see cref="DeliminatorInfo"/>.
        /// </summary>
        private class DelimSpan
        {
            public int Begin { get; set; }
            public int End { get; set; }
            public string Destination { get; set; }
            public string Title { get; set; }

            private int? parseBegin;
            private int? parseEnd;

            public int ParseBegin
            {
                get => parseBegin ?? Begin;
                set => parseBegin = value;
            }

            public int ParseEnd
            {
                get => parseEnd ?? End;
                set => parseEnd = value;
            }

            public DelimType DeliminatorType { get; set; }

            public enum DelimType
            {
                Link,
                Image,
                Emphasis,
                StrongEmplasis,
                CodeSpan,
                AutoLink,
                InlineHtml,
                Root,
            };

            public SortedList<int, DelimSpan> Children { get; private set; }
            public DelimSpan Parent { get; set; }

            public InlineElementBase DelimElem { get; set; }

            public DelimSpan()
            {
                Children = new SortedList<int, DelimSpan>();
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

        /// <summary>
        /// Converts the specified string and <see cref="DelimSpan"/> tree to inline elements.
        /// </summary>
        /// <param name="text">The string object.</param>
        /// <param name="delim">The <see cref="DelimSpan"/> tree.</param>
        /// <returns>The inline elemets which is equivalent to <paramref name="delim"/>.</returns>
        private static InlineElementBase[] ToInlines(string text, DelimSpan delim)
        {
            int lastEnd = delim.ParseBegin;
            List<InlineElementBase> newChildren = new List<InlineElementBase>();
            foreach (var item in delim.Children)
            {
                DelimSpan delimSpan = item.Value;
                if (lastEnd < delimSpan.Begin)
                {
                    newChildren.AddRange(ParseLineBreak(text.Substring(lastEnd, delimSpan.Begin - lastEnd)));
                }

                newChildren.AddRange(ToInlines(text, delimSpan));
                lastEnd = delimSpan.End;
            }

            if (lastEnd < delim.ParseEnd)
            {
                newChildren.AddRange(ParseLineBreak(text.Substring(lastEnd, delim.ParseEnd - lastEnd)));
            }

            switch (delim.DeliminatorType)
            {
                case DelimSpan.DelimType.Link:
                    return new InlineElementBase[] { new Link(newChildren.ToArray(), delim.Destination, delim.Title) };
                case DelimSpan.DelimType.Image:
                    return new InlineElementBase[] { new Image(newChildren.ToArray()) };
                case DelimSpan.DelimType.Emphasis:
                    return new InlineElementBase[] { new Emphasis(newChildren.ToArray(), false) };
                case DelimSpan.DelimType.StrongEmplasis:
                    return new InlineElementBase[] { new Emphasis(newChildren.ToArray(), true) };
                case DelimSpan.DelimType.Root:
                    return newChildren.ToArray();
                default:
                    return new InlineElementBase[] { delim.DelimElem };
            }
        }

        /// <summary>
        /// Creates a tree of <see cref="DelimSpan"/> from its array.
        /// </summary>
        /// <param name="spans">The array of <see cref="DelimSpan"/>.</param>
        /// <param name="rootLength">The length of the whole text.</param>
        /// <returns>The tree of <see cref="DelimSpan"/> which is equivalent to <paramref name="spans"/>.</returns>
        private static DelimSpan GetInlineTree(SortedList<int, DelimSpan> spans, int rootLength)
        {
            var root = new DelimSpan()
            {
                Begin = 0,
                End = rootLength,
                DeliminatorType = DelimSpan.DelimType.Root,
            };
            DelimSpan currentSpan = root;
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
        /// Parses the structure and add the <see cref="DelimSpan"/> to <paramref name="delimSpans"/>.
        /// See https://spec.commonmark.org/0.28/#%2Dprocess%2Demphasis%2D  for the details of this algorithm. 
        /// </summary>
        /// <param name="deliminators">The list of <see cref="DeliminatorInfo"/>.</param>
        /// <param name="delimSpans">The list of <see cref="DelimSpan"/>.</param>
        /// <param name="stackBottom">The stack bottom.</param>
        private static void ParseEmphasis(LinkedList<DeliminatorInfo> deliminators,
            SortedList<int, DelimSpan> delimSpans, DeliminatorInfo stackBottom)
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
                        || !firstClose.Value.CanOpen))
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
                else
                {
                    ParseEmphasis(deliminators, delimSpans, firstClose?.Value);
                    return;
                }
            }
            else
            {
                DeliminatorInfo openInfo = startDelimNode.Value;
                DeliminatorInfo closeInfo = firstClose.Value;
                int delimLength = openInfo.DeliminatorLength > 1 && closeInfo.DeliminatorLength > 1 ? 2 : 1;
                var delimSpan = new DelimSpan()
                {
                    Begin = openInfo.Index + openInfo.DeliminatorLength - delimLength,
                    End = closeInfo.Index + delimLength,
                    DeliminatorType =
                        delimLength > 1 ? DelimSpan.DelimType.StrongEmplasis : DelimSpan.DelimType.Emphasis,
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
            }

            if (deliminators.Find(firstClose.Value) == null)
            {
                ParseEmphasis(deliminators, delimSpans, firstClose.Next?.Value);
            }
            else
            {
                ParseEmphasis(deliminators, delimSpans, firstClose.Value);
            }
        }

        /// <summary>
        /// Parses inline elements to Links, Images Emphasis and Line breaks.
        /// </summary>
        /// <param name="text">The string object to @arse.</param>
        /// <param name="linkReferences">Link reference definitions.</param>
        /// <returns>The parse result.</returns>
        private static IEnumerable<InlineElementBase> ParseLinkEmphasis(string text, Dictionary<string, LinkReferenceDefinition> linkReferences, List<DelimSpan> higherDelims)
        {
            bool IsDelim(int index)
            {
                return !higherDelims.Any(d => d.Begin <= index && d.End > index);
            }

            var deliminators = new LinkedList<DeliminatorInfo>();
            var delimSpans = new SortedList<int, DelimSpan>();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '*' && !IsEscaped(text, i) && IsDelim(i))
                {
                    int length = CountSameChars(text, i);
                    var delimInfo = new DeliminatorInfo()
                    {
                        Type = DeliminatorInfo.DeliminatorType.Star,
                        DeliminatorLength = length,
                        CanOpen = true,
                        CanClose = true,
                        Index = i
                    };
                    delimInfo.CanOpen = IsLeftFlanking(text, delimInfo);
                    delimInfo.CanClose = IsRightFlanking(text, delimInfo);

                    deliminators.AddLast(delimInfo);
                    i += length - 1;
                }
                else if (text[i] == '_' && !IsEscaped(text, i) && IsDelim(i))
                {
                    int length = CountSameChars(text, i);
                    var delimInfo = new DeliminatorInfo()
                    {
                        Type = DeliminatorInfo.DeliminatorType.Underbar,
                        DeliminatorLength = length,
                        CanOpen = true,
                        CanClose = true,
                        Index = i
                    };
                    delimInfo.CanOpen = IsLeftFlanking(text, delimInfo)
                                        && (!IsRightFlanking(text, delimInfo)
                                            || IsPrecededByPunctuation(text, delimInfo));

                    delimInfo.CanClose = IsRightFlanking(text, delimInfo)
                                         && (!IsLeftFlanking(text, delimInfo)
                                             || IsFollowedByPunctuation(text, delimInfo));
                    deliminators.AddLast(delimInfo);

                    i += length - 1;
                }
                else if (text[i] == '[' && !IsEscaped(text, i) && IsDelim(i))
                {
                    deliminators.AddLast(new DeliminatorInfo()
                    {
                        Type = DeliminatorInfo.DeliminatorType.OpenLink,
                        DeliminatorLength = 1,
                        CanOpen = true,
                        CanClose = false,
                        Index = i
                    });
                }
                else if (text.Length > i + 1 && text[i] == '!' && text[i + 1] == '[' && !IsEscaped(text, i) && IsDelim(i))
                {
                    deliminators.AddLast(new DeliminatorInfo()
                    {
                        Type = DeliminatorInfo.DeliminatorType.OpenImage,
                        DeliminatorLength = 2,
                        CanOpen = true,
                        CanClose = false,
                        Index = i
                    });
                    i++;
                }
                else if (i > 0 && text[i] == ']' && !IsEscaped(text, i) && IsDelim(i))
                {
                    DeliminatorInfo openInfo = deliminators
                        .LastOrDefault(info => info.Type == DeliminatorInfo.DeliminatorType.OpenImage
                                               || info.Type == DeliminatorInfo.DeliminatorType.OpenLink);
                    if (openInfo == null)
                    {
                        continue;
                    }

                    if (!openInfo.Active)
                    {
                        deliminators.Remove(openInfo);
                    }
                    else
                    {
                        Match inlineLinkMatch = InlineLinkRegex.Match(text, openInfo.Index);
                        Match linkLabelMatch = LinkLabelRegex.Match(text, Math.Min(text.Length - 1, i + 1));
                        // Inline Link/Image
                        if (text[Math.Min(i + 1, text.Length - 1)] == '(' && inlineLinkMatch.Index == openInfo.Index &&
                            inlineLinkMatch.Success)
                        {
                            if (openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink)
                            {
                                foreach (var item in deliminators.TakeWhile(c => c != openInfo))
                                {
                                    if (item.Type == DeliminatorInfo.DeliminatorType.OpenLink)
                                    {
                                        item.Active = false;
                                    }
                                }
                            }

                            delimSpans.Add(openInfo.Index, new DelimSpan()
                            {
                                Begin = openInfo.Index,
                                End = openInfo.Index + inlineLinkMatch.Length,
                                DeliminatorType = openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink
                                    ? DelimSpan.DelimType.Link
                                    : DelimSpan.DelimType.Image,
                                ParseBegin = openInfo.Index
                                             + (openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ? 1 : 2),
                                ParseEnd = i,
                                Title = inlineLinkMatch.Groups["title"].Value,
                                Destination = inlineLinkMatch.Groups["destination"].Value,
                            });
                            ParseEmphasis(deliminators, delimSpans, openInfo);
                            deliminators.Remove(openInfo);
                            i = openInfo.Index + inlineLinkMatch.Length - 1;
                        }
                        // Collapsed Reference Link
                        else if (i < text.Length - 1
                                 && text[Math.Min(i + 1, text.Length - 1)] == '['
                                 && text[Math.Min(i + 2, text.Length - 1)] == ']'
                                 && linkReferences.TryGetValue(text.Substring(
                                     openInfo.Index + openInfo.DeliminatorLength,
                                     i - openInfo.Index - openInfo.DeliminatorLength), out var definition))
                        {
                            delimSpans.Add(openInfo.Index, new DelimSpan()
                            {
                                Begin = openInfo.Index,
                                End = i + 3,
                                DeliminatorType = openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink
                                    ? DelimSpan.DelimType.Link
                                    : DelimSpan.DelimType.Image,
                                ParseBegin = openInfo.Index
                                             + (openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ? 1 : 2),
                                ParseEnd = i,
                                Destination = definition.Destination,
                                Title = definition.Title,
                            });
                            ParseEmphasis(deliminators, delimSpans, openInfo);
                            i += 2;
                        }
                        // Full Reference Link
                        else if (text[Math.Min(i + 1, text.Length - 1)] == '[' && linkLabelMatch.Success
                                                                               && linkReferences.TryGetValue(linkLabelMatch
                                                                                   .Groups["label"].Value, out definition))
                        {
                            delimSpans.Add(openInfo.Index, new DelimSpan()
                            {
                                Begin = openInfo.Index,
                                End = i + linkLabelMatch.Length + 1,
                                DeliminatorType = openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink
                                    ? DelimSpan.DelimType.Link
                                    : DelimSpan.DelimType.Image,
                                ParseBegin = openInfo.Index
                                             + (openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ? 1 : 2),
                                ParseEnd = i,
                                Destination = definition.Destination,
                                Title = definition.Title,
                            });
                            ParseEmphasis(deliminators, delimSpans, openInfo);
                            i += linkLabelMatch.Length;
                        }
                        // shortcut link
                        else if (linkReferences.TryGetValue(text.Substring(openInfo.Index + 1, i - openInfo.Index - 1),
                            out definition))
                        {
                            delimSpans.Add(openInfo.Index, new DelimSpan()
                            {
                                Begin = openInfo.Index,
                                End = i + 1,
                                DeliminatorType = openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink
                                    ? DelimSpan.DelimType.Link
                                    : DelimSpan.DelimType.Image,
                                ParseBegin = openInfo.Index
                                             + (openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ? 1 : 2),
                                ParseEnd = i,
                                Destination = definition.Destination,
                                Title = definition.Title,
                            });
                            ParseEmphasis(deliminators, delimSpans, openInfo);
                        }
                        else
                        {
                            deliminators.Remove(openInfo);
                        }
                    }
                }
            }

            ParseEmphasis(deliminators, delimSpans, null);

            foreach (var item in higherDelims)
            {
                delimSpans.Add(item.Begin, item);
            }

            var tree = GetInlineTree(delimSpans, text.Length);

            return ToInlines(text, tree);
        }

        /// <summary>
        /// Returns a code span which starts the specified index.
        /// </summary>
        /// <param name="text">The string which contains the code span.</param>
        /// <param name="index">The index of the first character of the span.</param>
        /// <param name="currentIndex">Updates this value to the index of the next character of the span end.</param>
        /// <returns>The code span which starts with the specified index.</returns>
        private static CodeSpan GetCodeSpan(string text, int index, ref int currentIndex)
        {
            int openLength = CountSameChars(text, index);
            int closeIndex = index + openLength;

            do
            {
                closeIndex = text.IndexOf(new string('`', openLength), closeIndex);
                if (closeIndex >= 0)
                {
                    int closeLength = CountSameChars(text, closeIndex);
                    if (closeLength == openLength)
                    {
                        currentIndex = closeIndex + closeLength;
                        return new CodeSpan(
                            text.Substring(index + openLength, closeIndex - index - openLength));
                    }
                    else
                    {
                        closeIndex += closeLength;
                    }
                }
            } while (closeIndex >= 0 && closeIndex < text.Length);

            return null;
        }

        /// <summary>
        /// Parses raw html or autolinks which starts with the specified index.
        /// </summary>
        /// <param name="text">String object which contains autolinks or raw html.</param>
        /// <param name="index">The index where the element starts.</param>
        /// <param name="currentIndex">Updates this value to the index of the next character of the span end.</param>
        /// <returns>The autolinks or raw html if one is found, otherwise <c>null</c>.</returns>
        private static InlineElementBase GetInlineHtmlOrLink(string text, int index, ref int currentIndex)
        {
            // Auto link (URL)
            Match urlMatch = UrlRegex.Match(text, index);
            if (urlMatch.Success
                && urlMatch.Index == index
                && urlMatch.Value.All(c => !char.IsControl(c)))
            {
                currentIndex += urlMatch.Length;
                return new Link(new[] { InlineText.CreateFromText(urlMatch.Groups["url"].Value, false) },
                    urlMatch.Groups["url"].Value, null);
            }

            // Auto link (E-Mail)
            Match emailMatch = MailAddressRegex.Match(text, index);
            if (emailMatch.Success && emailMatch.Index == index)
            {
                currentIndex += emailMatch.Length;
                var target = "mailto:" + emailMatch.Groups["addr"].Value;
                return new Link(new[] { InlineText.CreateFromText(target, false) }, target, null);
            }

            // Inline html
            Match htmlTagMatch = HtmlTagRegex.Match(text, index);
            if (htmlTagMatch.Success && htmlTagMatch.Index == index)
            {
                currentIndex += htmlTagMatch.Length;
                return new InlineHtml(htmlTagMatch.Value);
            }

            return null;
        }

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
        /// Parses inline elements and returns them.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="linkRefernces">Reference definitions in this document.</param>
        /// <returns>Inline elements in <paramref name="text"/>.</returns>
        public static IEnumerable<InlineElementBase> ParseInlineElements(string text,
            Dictionary<string, LinkReferenceDefinition> linkRefernces)
        {
            var highPriorityDelims = new List<DelimSpan>();
            int currentIndex = 0;
            int nextBacktick = GetNextUnescaped(text, '`', 0);
            int nextLessThan = GetNextUnescaped(text, '<', 0);
            while (currentIndex < text.Length)
            {
                var newIndex = currentIndex;
                int nextElemIndex;
                InlineElementBase newInline = null;

                // Find `
                if (nextBacktick >= 0 && (nextLessThan < 0 || nextBacktick < nextLessThan))
                {
                    nextElemIndex = nextBacktick;
                    newIndex = nextElemIndex;
                    newInline = GetCodeSpan(text, nextBacktick, ref newIndex);
                }
                // Find <
                else if (nextLessThan >= 0 && (nextBacktick < 0 || nextLessThan < nextBacktick))
                {
                    nextElemIndex = nextLessThan;
                    newIndex = nextElemIndex;
                    newInline = GetInlineHtmlOrLink(text, nextLessThan, ref newIndex);
                }
                else // Find neigher ` nor <
                {
                    break;
                }

                if (newInline != null)
                {
                    var span = new DelimSpan()
                    {
                        Begin = nextElemIndex,
                        End = newIndex,
                        DeliminatorType = ToDelimType(newInline.Type),
                        DelimElem = newInline,
                    };
                    highPriorityDelims.Add(span);
                    //foreach (var element in
                    //    ParseLinkEmphasis(text.Substring(currentIndex, nextElemIndex - currentIndex),
                    //        linkRefernces))
                    //{
                    //    yield return element;
                    //}

                    currentIndex = newIndex;
                    nextBacktick = GetNextUnescaped(text, '`', currentIndex);
                    nextLessThan = GetNextUnescaped(text, '<', currentIndex);
                    //yield return newInline;
                }
                else
                {
                    nextElemIndex += CountSameChars(text, nextElemIndex);
                    nextBacktick = GetNextUnescaped(text, '`', nextElemIndex);
                    nextLessThan = GetNextUnescaped(text, '<', nextElemIndex);
                }
            }

            return ParseLinkEmphasis(text, linkRefernces, highPriorityDelims);

            //if (currentIndex < text.Length)
            //{
            //    foreach (var element in
            //        ParseLinkEmphasis(text.Substring(currentIndex, text.Length - currentIndex),
            //            linkRefernces))
            //    {
            //        yield return element;
            //    }
            //}
        }


        private static DelimSpan.DelimType ToDelimType(InlineElementType elemType)
        {
            switch (elemType)
            {
                case InlineElementType.CodeSpan:
                    return DelimSpan.DelimType.CodeSpan;
                case InlineElementType.Link:
                    return DelimSpan.DelimType.AutoLink;
                case InlineElementType.InlineHtml:
                    return DelimSpan.DelimType.InlineHtml;
                default:
                    throw new ArgumentException();
            }
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

        public static string HandleEscape(string text)
        {
            var builder = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                if (i < text.Length - 1 && text[i] == '\\'
                    && MarkdownElementBase.asciiPunctuationChars.Contains(text[i + 1]))
                {
                    builder.Append(text[i + 1]);
                    i++;
                }
                else
                {
                    builder.Append(text[i]);
                }
            }
            return builder.ToString();
        }

    }
}