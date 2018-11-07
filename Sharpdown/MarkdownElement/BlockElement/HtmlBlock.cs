using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class HtmlBlock : LeafElement
    {
        private int blockType;

        private List<string> contents;

        public string Content => string.Join("\r\n", contents);
        private bool isClosed;

        internal HtmlBlock() : base()
        {
            contents = new List<string>();
            isClosed = false;
        }

        private static readonly Regex openTag = new Regex(
            @"\<(?<tag>[a-zA-Z][a-zA-Z0-9-]*)[ \t]*?([ \t][a-zA-Z_\:][a-zA-Z0-9_\.\:\-]*[ \t]*(\=[ \t]*([^ \""\'\=\<\>`\t]+|\""[^\""]*\""|\'[^\']*\'))??)*[ \t]*\/??\>[ \t]*$",
                RegexOptions.Compiled);

        private static readonly Regex closeTag = new Regex(
            @"^\<\/(?<tag>[a-zA-Z][a-zA-Z0-9-]*)[ \t]*>[ \t]*$",
            RegexOptions.Compiled);

        public override BlockElementType Type => BlockElementType.HtmlBlock;

        private static readonly string[] htmlTagNames = new[]
        {
            "address", "article", "aside", "base", "basefont", "blockquote", "body", "caption",
            "center", "col", "colgroup", "dd", "details", "dialog", "dir", "div", "dl", "dt",
            "fieldset", "figcaption", "figure", "footer", "form", "frame", "frameset", "h1", "h2",
            "h3", "h4", "h5", "h6", "head", "header", "hr", "html", "iframe", "legend", "li", "link",
            "main", "menu", "menuitem", "meta", "nav", "noframes", "ol", "optgroup",
            "option", "p", "param", "section", "source", "summary", "table",
            "tbody", "td", "tfoot", "th", "thead", "title", "tr", "track", "ul",
        };

        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() < 0 || line.GetIndentNum() >= 4)
            {
                return false;
            }

            string lineTrimmed = line.TrimStart(whiteSpaceShars);

            return IsType1HtmlBlock(lineTrimmed)
                || IsType2HtmlBlock(lineTrimmed)
                || IsType3HtmlBlock(lineTrimmed)
                || IsType4HtmlBlock(lineTrimmed)
                || IsType5HtmlBlock(lineTrimmed)
                || IsType6HtmlBlock(lineTrimmed)
                || IsType7HtmlBlock(lineTrimmed);
        }

        private static bool IsType1HtmlBlock(string line)
        {
            int whiteSpace;
            if (line.StartsWith("<script", StringComparison.OrdinalIgnoreCase))
            {
                whiteSpace = 7;
            }
            else if (line.StartsWith("<pre", StringComparison.OrdinalIgnoreCase))
            {
                whiteSpace = 4;
            }
            else if (line.StartsWith("<style", StringComparison.OrdinalIgnoreCase))
            {
                whiteSpace = 6;
            }
            else
            {
                return false;
            }

            if (line.Length <= whiteSpace
                || whiteSpaceShars.Contains(line[whiteSpace])
                || line[whiteSpace] == '>')
            {
                return true;
            }
            return false;
        }

        private static bool IsType2HtmlBlock(string line)
        {
            return line.StartsWith("<!--");
        }

        private static bool IsType3HtmlBlock(string line)
        {
            return line.StartsWith("<?");
        }

        private static bool IsType4HtmlBlock(string line)
        {
            if (line.Length < 3)
            {
                return false;
            }

            return line.StartsWith("<!")
                && line[2] >= 0x41
                && line[2] <= 0x5A;
        }

        private static bool IsType5HtmlBlock(string line)
        {
            return line.StartsWith("<![CDATA[");
        }

        private static bool IsType6HtmlBlock(string line)
        {
            if (line.Length < 2)
            {
                return false;
            }

            if (!line.StartsWith("<"))
            {
                return false;
            }

            string trimmed;

            if (line[1] == '/')
            {
                trimmed = line.Substring(2);
            }
            else
            {
                trimmed = line.Substring(1);
            }

            foreach (var tagName in htmlTagNames)
            {
                if (trimmed.StartsWith(tagName, StringComparison.OrdinalIgnoreCase))
                {
                    if (trimmed.Length == tagName.Length)
                    {
                        return true;
                    }

                    var afterTag = trimmed.Substring(tagName.Length);
                    if (afterTag.StartsWith("/>") || afterTag.StartsWith(">") || whiteSpaceShars.Contains(afterTag[0]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsType7HtmlBlock(string line)
        {
            string[] invalidTags = new[] { "script", "style", "pre" };
            if (openTag.IsMatch(line))
            {
                Match match = openTag.Match(line);
                string tagName = match.Groups["tag"].Value;
                return !invalidTags.Contains(tagName, StringComparer.OrdinalIgnoreCase);
            }

            if (closeTag.IsMatch(line))
            {
                Match match = closeTag.Match(line);
                string tagName = match.Groups["tag"].Value;
                return !invalidTags.Contains(tagName, StringComparer.OrdinalIgnoreCase);
            }
            return false;
        }

        internal static bool CanInterruptParagraph(string line)
        {
            string lineTrimmed = line.TrimStart(whiteSpaceShars);

            return IsType1HtmlBlock(lineTrimmed)
            || IsType2HtmlBlock(lineTrimmed)
            || IsType3HtmlBlock(lineTrimmed)
            || IsType4HtmlBlock(lineTrimmed)
            || IsType5HtmlBlock(lineTrimmed)
            || IsType6HtmlBlock(lineTrimmed);
        }

        private static int DetermineType(string lineTrimmed)
        {
            if (IsType1HtmlBlock(lineTrimmed))
            {
                return 1;
            }
            else if (IsType2HtmlBlock(lineTrimmed))
            {
                return 2;
            }
            else if (IsType3HtmlBlock(lineTrimmed))
            {
                return 3;
            }
            else if (IsType4HtmlBlock(lineTrimmed))
            {
                return 4;
            }
            else if (IsType5HtmlBlock(lineTrimmed))
            {
                return 5;
            }
            else if (IsType6HtmlBlock(lineTrimmed))
            {
                return 6;
            }
            else if (IsType7HtmlBlock(lineTrimmed))
            {
                return 7;
            }
            else
            {
                return -1;
            }
        }

        private bool NeedClose(string lineTrimmed)
        {
            switch (blockType)
            {
                case 1:
                    return lineTrimmed.Contains("</script>")
                        || lineTrimmed.Contains("</pre>")
                        || lineTrimmed.Contains("</style>");
                case 2:
                    return lineTrimmed.Contains("-->");
                case 3:
                    return lineTrimmed.Contains("?>");
                case 4:
                    return lineTrimmed.Contains(">");
                case 5:
                    return lineTrimmed.Contains("]]>");
                case 6:
                case 7:
                    return lineTrimmed.TrimStartAscii().Length == 0;
                default:
                    throw new InvalidBlockFormatException(BlockElementType.HtmlBlock);
            }
        }

        internal override AddLineResult AddLine(string line)
        {
            string lineTrimmed = line.TrimStartAscii();
            if (contents.Count == 0 && (blockType = DetermineType(lineTrimmed)) < 1)
            {
                throw new InvalidBlockFormatException(BlockElementType.HtmlBlock);
            }

            bool needClose = NeedClose(lineTrimmed);
            if (!needClose)
            {
                contents.Add(line);
                return AddLineResult.Consumed;
            }
            isClosed = true;
            if (blockType < 6)
            {
                contents.Add(line);
                return AddLineResult.NeedClose | AddLineResult.Consumed;
            }

            return AddLineResult.NeedClose;
        }

        internal override BlockElement Close()
        {
            if (!isClosed)
            {
                if (blockType < 6)
                {
                    warnings.Add("HTML Block is not closed.");
                }

                for (int i = contents.Count - 1; i >= 0; i--)
                {
                    if (contents[i].TrimStartAscii() == string.Empty)
                    {
                        contents.RemoveAt(i);
                    }
                }
            }
            return this;
        }
    }
}
