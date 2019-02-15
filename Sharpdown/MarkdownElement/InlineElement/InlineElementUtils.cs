using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement.InlineParserObjects;

namespace Sharpdown.MarkdownElement.InlineElement
{
    /// <summary>
    /// Provides some methods for Inline parsing.
    /// </summary>
    public static class InlineElementUtils
    {
        private static readonly char[] unreservedChars =
        {
            'A', 'B', 'C', 'D', 'E',
            'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O',
            'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y',
            'Z',
            'a', 'b', 'c', 'd', 'e',
            'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n', 'o',
            'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y',
            'z',
            '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9',
            ';', '/', '?', ':', '@',
            '&', '=', '+', '$', ',',
            '-', '_', '.', '!', '~',
            '*', '\'', '(', ')', '#',
        };

        /// <summary>
        /// Returns <see cref="InlineElement"/> 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="config">Configuration of the parser.</param>
        /// <returns></returns>
        private static IEnumerable<InlineElement> ParseLineBreak(string text, ParserConfig config)
        {
            string[] lines = text.Replace("\r\n", "\n").Replace("\r", "\n").Split(new[] {'\n'});
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
                            yield return InlineText.CreateFromText(lines[i].Substring(0, lines[i].Length - 1), config);
                        }
                        else
                        {
                            yield return InlineText.CreateFromText(lines[i].TrimEnd(new[] {' '}), config);
                        }
                    }

                    yield return isHardBreak
                        ? (InlineElement)new HardLineBreak(config)
                        : new SoftLineBreak(config);
                }
                else
                {
                    if (lines[i] != string.Empty)
                    {
                        yield return InlineText.CreateFromText(lines[i], config);
                    }
                }
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
        internal static int CountSameChars(string str, int index)
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
        /// Converts the specified string and <see cref="InlineSpan"/> tree to inline elements.
        /// </summary>
        /// <param name="text">The string object.</param>
        /// <param name="delim">The <see cref="InlineSpan"/> tree.</param>
        /// <param name="config">Configuration of the parser.</param>
        /// <returns>The inline elements which is equivalent to <paramref name="delim"/>.</returns>
        internal static InlineElement[] ToInlines(string text, InlineSpan delim, ParserConfig config)
        {
            int lastEnd = delim.ParseBegin;
            List<InlineElement> newChildren = new List<InlineElement>();
            foreach (var item in delim.Children.Where(d => d.Value.End <= delim.ParseEnd))
            {
                InlineSpan delimSpan = item.Value;
                if (lastEnd < delimSpan.Begin)
                {
                    newChildren.AddRange(ParseLineBreak(text.Substring(lastEnd, delimSpan.Begin - lastEnd), config));
                }

                newChildren.AddRange(ToInlines(text, delimSpan, config));
                lastEnd = delimSpan.End;
            }

            if (lastEnd < delim.ParseEnd)
            {
                newChildren.AddRange(ParseLineBreak(text.Substring(lastEnd, delim.ParseEnd - lastEnd), config));
            }

            switch (delim.SpanType)
            {
                case InlineSpanType.Link:
                    return new InlineElement[]
                        {new Link(newChildren.ToArray(), delim.Destination, delim.Title, config)};
                case InlineSpanType.Image:
                    return new InlineElement[]
                        {new Image(newChildren.ToArray(), delim.Destination, delim.Title, config)};
                case InlineSpanType.Emphasis:
                    return new InlineElement[] {new Emphasis(newChildren.ToArray(), false, config)};
                case InlineSpanType.StrongEmphasis:
                    return new InlineElement[] {new Emphasis(newChildren.ToArray(), true, config)};
                case InlineSpanType.Root:
                    return newChildren.ToArray();
                default:
                    return new[] {delim.DelimElem};
            }
        }

        /// <summary>
        /// Creates a tree of <see cref="InlineSpan"/> from its array.
        /// </summary>
        /// <param name="spans">The array of <see cref="InlineSpan"/>.</param>
        /// <param name="rootLength">The length of the whole text.</param>
        /// <returns>The tree of <see cref="InlineSpan"/> which is equivalent to <paramref name="spans"/>.</returns>
        internal static InlineSpan GetInlineTree(SortedList<int, InlineSpan> spans, int rootLength)
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
        /// Parses the structure and add the <see cref="InlineSpan"/> to <paramref name="delimSpans"/>.
        /// </summary>
        /// <param name="deliminators">The list of <see cref="DeliminatorInfo"/>.</param>
        /// <param name="delimSpans">The list of <see cref="InlineSpan"/>.</param>
        /// <param name="stackBottom">The stack bottom.</param>
        internal static void ParseEmphasis(LinkedList<DeliminatorInfo> deliminators,
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

            if (deliminators.Find(firstClose.Value) == null)
            {
                ParseEmphasis(deliminators, delimSpans, firstClose.Next?.Value);
            }
            else
            {
                ParseEmphasis(deliminators, delimSpans, firstClose.Value);
            }
        }

        internal static bool AreBracketsBalanced(string text)
        {
            int depth = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '[' && !IsEscaped(text, i))
                {
                    depth++;
                }

                if (text[i] == ']' && !IsEscaped(text, i))
                {
                    depth--;
                }

                if (depth < 0)
                {
                    return false;
                }
            }

            return depth == 0;
        }

        internal static int GetNextUnescaped(string str, char ch, int index)
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

        internal static InlineSpanType ToDelimType(InlineElementType elemType)
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

        internal static bool IsEscaped(string text, int index)
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

        private static bool IsPercentEncoded(string text, int index)
        {
            bool IsHexChar(char ch)
            {
                if (ch >= 0x30 && ch <= 0x39)
                {
                    return true;
                }

                if (ch >= 0x41 && ch <= 0x5A)
                {
                    return true;
                }

                if (ch >= 0x61 && ch <= 0x7A)
                {
                    return true;
                }

                return false;
            }

            if (index >= text.Length - 2)
            {
                return false;
            }

            return text[index] == '%'
                   && IsHexChar(text[index + 1])
                   && IsHexChar(text[index + 2]);
        }

        /// <summary>
        /// Do percent encode to the specific string.
        /// </summary>
        /// <returns>The text to encode.</returns>
        /// <param name="text">The encoded text.</param>
        /// <remarks>
        /// Ascii alphabets, digits and some other ascii characters (see <see cref="unreservedChars"/>)
        /// will not be encoded which is different from the specification of url.
        /// % characters will be escaped only when it is not the beginning of percent encoded letters.
        /// </remarks>
        public static string UrlEncode(string text)
        {
            var builder = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                if (unreservedChars.Contains(text[i]))
                {
                    builder.Append(text[i]);
                }
                else if (text[i] == '%' && IsPercentEncoded(text, i))
                {
                    builder.Append(text[i]);
                }
                else
                {
                    var bytes = Encoding.UTF8.GetBytes(new[] {text[i]});
                    foreach (var b in bytes)
                    {
                        builder.Append("%");
                        builder.Append(b.ToString("X2"));
                    }
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the start index of link label.
        /// </summary>
        /// <param name="wholeText">Whole text.</param>
        /// <param name="begin">Begin of the seatch area.</param>
        /// <param name="end">End of the link label.</param>
        /// <param name="higherDelims">Higher delims.</param>
        /// <returns>The start index of link label.</returns>
        internal static int GetStartIndexOfLinkLabel(string wholeText, int begin, int end, List<InlineSpan> higherDelims)
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

        internal static int GetEndIndexOfLinkLabel(string wholeText, int begin, List<InlineSpan> higherDelims)
        {
            bool IsDelim(int index)
            {
                return !higherDelims.Any(d => d.Begin <= index && d.End > index);
            }

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

                if (!IsDelim(i))
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
        internal static int GetLinkBodyEndIndex(string wholeText, int begin, out string destination, out string title)
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
                    if (MarkdownElementBase.whiteSpaceChars.Contains(ch))
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

        internal static bool TryGetReference(Dictionary<string, LinkReferenceDefinition> references,
            string refName, out LinkReferenceDefinition referenceDefinition)
        {
            var name = LinkReferenceDefinition.GetSimpleName(refName);
            return references.TryGetValue(name, out referenceDefinition);
        }
    }
}
