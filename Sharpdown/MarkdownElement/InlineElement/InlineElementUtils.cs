using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public class InlineElementUtils
    {
        private static readonly Regex inlineLinkRegex = new Regex(
            @"!??\[(?<label>(?:[^\]]|\\\]){1,999})\]\([ \t]*(?:\r|\r\n|\n)??[ \t]*(?<destination>\<(?:[^ \t\r\n\<\>]|\\\<|\\\>)+\>|[^ \t\r\n]+)([ \t]*(?: |\t|\r|\r\n|\n)[ \t]*(?<title>\""(?:[^\""]|\\\"")*\""|\'(?:[^\']|\\\')*\'|\((?:[^\)]|\\\))*\)))??[ \t]*\)",
            RegexOptions.Compiled);

        private static readonly Regex linkLabelRegex = new Regex(
            @"\[(?<label>(?:[^\]]|\\\]){1,999})\]", RegexOptions.Compiled);

        private static bool IsLeftFlanking(string text, DeliminatorInfo info)
        {
            char prevChar = info.Index == 0 ? ' ' : text[info.Index - 1];
            char nextChar = info.Index + info.DeliminatorLength >= text.Length
                ? ' ' : text[info.Index + info.DeliminatorLength];
            if (nextChar == ' ')
            {
                return false;
            }

            if (!InlineElementBase.asciiPunctuationChars.Contains(nextChar))
            {
                return true;
            }
            return char.IsWhiteSpace(prevChar) || InlineElementBase.asciiPunctuationChars.Contains(prevChar);
        }

        private static bool IsRightFlanking(string text, DeliminatorInfo info)
        {
            char prevChar = info.Index == 0 ? ' ' : text[info.Index - 1];
            char nextChar = info.Index + info.DeliminatorLength >= text.Length
                ? ' ' : text[info.Index + info.DeliminatorLength];
            if (prevChar == ' ')
            {
                return false;
            }

            if (!InlineElementBase.asciiPunctuationChars.Contains(prevChar))
            {
                return true;
            }

            return char.IsWhiteSpace(nextChar) || InlineElementBase.asciiPunctuationChars.Contains(nextChar);
        }

        private static bool IsFollowedByPunctuation(string text, DeliminatorInfo info)
        {
            if (text.Length <= info.Index + info.DeliminatorLength)
            {
                return false;
            }
            return InlineElementBase.asciiPunctuationChars
                .Contains(text[info.Index + info.DeliminatorLength]);
        }

        private static bool IsPrecededByPuncuation(string text, DeliminatorInfo info)
        {
            if (info.Index == 0)
            {
                return false;
            }
            return InlineElementBase.asciiPunctuationChars.Contains(text[info.Index - 1]);
        }

        private static IEnumerable<InlineElementBase> ParseLineBreak(string text)
        {
            string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i < lines.Length - 1)
                {
                    yield return InlineText.CreateFromText(lines[i].TrimEnd(new[] { ' ' }));
                    yield return lines[i].EndsWith("  ") ?
                        (InlineElementBase)new HardLineBreak() : new SoftLineBreak();
                }
                else
                {
                    yield return InlineText.CreateFromText(lines[i]);
                }
            }
            if (text.EndsWith("\r") || text.EndsWith("\n"))
            {
                yield return lines.Length > 0 && lines[lines.Length - 1].EndsWith("  ") ?
                    (InlineElementBase)new HardLineBreak() : new SoftLineBreak();
            }
        }

        private class DeliminatorInfo
        {
            public enum DeliminatorType
            {
                OpenLink,
                OpenImage,
                Star,
                Underbar,
            }

            public int DeliminatorLength { get; set; }

            public DeliminatorType Type { get; set; }

            public bool Active { get; set; } = true;

            public bool CanOpen { get; set; }

            public bool CanClose { get; set; }

            public int Index { get; set; }
        }

        [DebuggerDisplay("Begin = {Begin}, End = {End}, Type = {DeliminatorType}")]
        private class DelimSpan
        {
            public int Begin { get; set; }
            public int End { get; set; }

            private int? parseBegin;
            private int? parseEnd;

            public int ParseBegin
            {
                get { return parseBegin ?? Begin; }
                set { parseBegin = value; }
            }

            public int ParseEnd
            {
                get { return parseEnd ?? End; }
                set { parseEnd = value; }
            }

            public DelimType DeliminatorType { get; set; }
            public enum DelimType
            {
                Link, Image, Emphasis, StrongEmplasis, Root,
            };
            public SortedList<int, DelimSpan> Children { get; private set; }
            public DelimSpan Parent { get; set; }
            public DelimSpan()
            {
                Children = new SortedList<int, DelimSpan>();
            }
        }

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

        private static IEnumerable<InlineElementBase> GetInlineTree(
            string text, SortedList<int, DelimSpan> span)
        {
            InlineElementBase[] ToInlines(DelimSpan delim)
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
                    newChildren.AddRange(ToInlines(delimSpan));
                    lastEnd = delimSpan.End;
                }
                if (lastEnd < delim.ParseEnd)
                {
                    newChildren.AddRange(ParseLineBreak(text.Substring(lastEnd, delim.ParseEnd - lastEnd)));
                }

                switch (delim.DeliminatorType)
                {
                    case DelimSpan.DelimType.Link:
                        return new InlineElementBase[] { new Link(newChildren.ToArray()) };
                    case DelimSpan.DelimType.Image:
                        return new InlineElementBase[] { new Image(newChildren.ToArray()) };
                    case DelimSpan.DelimType.Emphasis:
                        return new InlineElementBase[] { new Emphasis(newChildren.ToArray(), false) };
                    case DelimSpan.DelimType.StrongEmplasis:
                        return new InlineElementBase[] { new Emphasis(newChildren.ToArray(), true) };
                    case DelimSpan.DelimType.Root:
                        return newChildren.ToArray();
                    default:
                        throw new Exception();
                }
            }

            var root = new DelimSpan()
            {
                Begin = 0,
                End = text.Length,
                DeliminatorType = DelimSpan.DelimType.Root,
            };
            DelimSpan currentSpan = root;
            foreach (var item in span)
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
            return ToInlines(root);
        }

        public static IEnumerable<InlineElementBase> ParseLinkEmphasis(string text, IEnumerable<string> linkReferences)
        {
            var deliminators = new LinkedList<DeliminatorInfo>();
            var delimSpans = new SortedList<int, DelimSpan>();

            void ParseEmphasis(DeliminatorInfo stackBottom)
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
                        ParseEmphasis(firstClose?.Value);
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
                        DeliminatorType = delimLength > 1 ? DelimSpan.DelimType.StrongEmplasis : DelimSpan.DelimType.Emphasis,
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
                    ParseEmphasis(firstClose.Next?.Value);
                }
                else
                {
                    ParseEmphasis(firstClose.Value);
                }
            }

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (text[i] == '*')
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
                    continue;
                }
                else if (text[i] == '_')
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
                        || IsPrecededByPuncuation(text, delimInfo));

                    delimInfo.CanClose = IsRightFlanking(text, delimInfo)
                        && (!IsLeftFlanking(text, delimInfo)
                        || IsFollowedByPunctuation(text, delimInfo));
                    deliminators.AddLast(delimInfo);

                    i += length - 1;
                    continue;
                }
                else if (text[i] == '[')
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
                else if (text.Length > i + 1 && text[i] == '!' && text[i + 1] == '[')
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
                else if (i > 0 && text[i] == ']' && text[i - 1] != '\\')
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
                        Match inlineLinkMatch = inlineLinkRegex.Match(text, openInfo.Index);
                        Match linkLabelMatch = linkLabelRegex.Match(text, Math.Min(text.Length - 1, i + 1));
                        // Inline Link/Image
                        if (text[Math.Min(i + 1, text.Length - 1)] == '(' && inlineLinkMatch.Index == openInfo.Index && inlineLinkMatch.Success)
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
                                DeliminatorType = openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ?
                                    DelimSpan.DelimType.Link : DelimSpan.DelimType.Image,
                                ParseBegin = openInfo.Index
                                    + (openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ? 1 : 2),
                                ParseEnd = i,
                            });
                            ParseEmphasis(openInfo);
                            deliminators.Remove(openInfo);
                            i = openInfo.Index + inlineLinkMatch.Length - 1;
                        }
                        // Collapsed Reference Link
                        else if (i < text.Length - 1
                            && text[Math.Min(i + 1, text.Length - 1)] == '['
                            && text[Math.Min(i + 2, text.Length - 1)] == ']'
                            && linkReferences.Contains(text.Substring(
                                openInfo.Index + openInfo.DeliminatorLength,
                                i - openInfo.Index - openInfo.DeliminatorLength)))
                        {
                            delimSpans.Add(openInfo.Index, new DelimSpan()
                            {
                                Begin = openInfo.Index,
                                End = i + 3,
                                DeliminatorType = openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ?
                                    DelimSpan.DelimType.Link : DelimSpan.DelimType.Image,
                                ParseBegin = openInfo.Index
                                    + (openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ? 1 : 2),
                                ParseEnd = i,
                            });
                            ParseEmphasis(openInfo);
                            i += 2;
                            continue;
                        }
                        // Full Reference Link
                        else if (text[Math.Min(i + 1, text.Length - 1)] == '[' && linkLabelMatch.Success
                            && linkReferences.Contains(linkLabelMatch.Groups["label"].Value))
                        {
                            delimSpans.Add(openInfo.Index, new DelimSpan()
                            {
                                Begin = openInfo.Index,
                                End = i + linkLabelMatch.Length + 1,
                                DeliminatorType = openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ?
                                    DelimSpan.DelimType.Link : DelimSpan.DelimType.Image,
                                ParseBegin = openInfo.Index
                                    + (openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ? 1 : 2),
                                ParseEnd = i,
                            });
                            ParseEmphasis(openInfo);
                            i += linkLabelMatch.Length;
                            continue;
                        }
                        // shortcut link
                        else if (linkReferences.Contains(text.Substring(openInfo.Index + 1, i - openInfo.Index - 1)))
                        {
                            delimSpans.Add(openInfo.Index, new DelimSpan()
                            {
                                Begin = openInfo.Index,
                                End = i + 1,
                                DeliminatorType = openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ?
                                    DelimSpan.DelimType.Link : DelimSpan.DelimType.Image,
                                ParseBegin = openInfo.Index
                                    + (openInfo.Type == DeliminatorInfo.DeliminatorType.OpenLink ? 1 : 2),
                                ParseEnd = i,
                            });
                            ParseEmphasis(openInfo);
                        }
                        else
                        {
                            deliminators.Remove(openInfo);
                        }
                    }
                }
            }

            ParseEmphasis(null);

            return GetInlineTree(text, delimSpans);
        }
    }
}
