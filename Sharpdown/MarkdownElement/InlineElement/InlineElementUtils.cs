﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                                       !InlineParser.IsEscaped(lines[i], lines.Length - 1));
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
    }
}
