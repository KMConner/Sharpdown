using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sharpdown.MarkdownElement.InlineElement;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents HTML blocks in markdown documents.
    /// </summary>
    /// <remarks>
    /// The typical html block is following.
    /// 
    /// <![CDATA[
    /// <script>
    /// alert("Foo");
    /// </sctipt>
    /// 
    /// <div>Bar</div>
    /// ]]>
    /// </remarks>
    public class HtmlBlock : LeafElement
    {
        /// <summary>
        /// Regular expression which matches open tags.
        /// </summary>
        private static readonly Regex openTag = new Regex(
            @"\<(?<tag>[a-zA-Z][a-zA-Z0-9-]*)[ \t]*?([ \t][a-zA-Z_\:][a-zA-Z0-9_\.\:\-]*[ \t]*(\=[ \t]*([^ \""\'\=\<\>`\t]+|\""[^\""]*\""|\'[^\']*\'))??)*[ \t]*\/??\>[ \t]*$",
                RegexOptions.Compiled);

        /// <summary>
        /// Regular expression which matches close tags.
        /// </summary>
        private static readonly Regex closeTag = new Regex(
            @"^\<\/(?<tag>[a-zA-Z][a-zA-Z0-9-]*)[ \t]*>[ \t]*$",
            RegexOptions.Compiled);

        /// <summary>
        /// HTML tag names which constitutes type 6 HTML block.
        /// </summary>
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

        /// <summary>
        /// The type of this html block. (1-7)
        /// </summary>
        private int blockType;

        /// <summary>
        /// The contents of this block.
        /// </summary>
        private List<string> contents;

        /// <summary>
        /// Wether a line which satisfies the close condition of each type has appeard.
        /// </summary>
        private bool isClosed;

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.HtmlBlock;

        /// <summary>
        /// Gets the contents of this block.
        /// </summary>
        public string Content => string.Join("\r\n", contents);

        internal HtmlBlock() : base()
        {
            contents = new List<string>();
            isClosed = false;
        }

        /// <summary>
        /// Returns whether the specified line can be a start line of <see cref="HtmlBlock"/>.
        /// </summary>
        /// <remarks>
        /// To be the start line, one of 1-6 condition must be satisfied.
        /// </remarks>
        /// <param name="line">Single line string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="HtmlBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() < 0 || line.GetIndentNum() >= 4)
            {
                return false;
            }

            string lineTrimmed = line.TrimStart(whiteSpaceShars);

            return DetermineType(lineTrimmed) > 0;
        }

        /// <summary>
        /// Returns wether the specified line can be the first line of type 1 <see cref="HtmlBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>
        /// Begins with &lt;script , &lt;pre or &lt;style (case insensitive),
        /// followed by whitespace, the string &gt; or the end of the line.
        /// </item>
        /// <item>
        /// The open tag must be indented with 0-3 spaces.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="line">A line of string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of type 1 <see cref="HtmlBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Returns wether the specified line can be the first line of type 2 <see cref="HtmlBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>
        /// Begins with &lt;!--
        /// </item>
        /// <item>
        /// The comment must be indented with 0-3 spaces.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="line">A line of string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of type 2 <see cref="HtmlBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        private static bool IsType2HtmlBlock(string line)
        {
            return line.StartsWith("<!--");
        }

        /// <summary>
        /// Returns wether the specified line can be the first line of type 3 <see cref="HtmlBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>
        /// Begins with &lt;?
        /// </item>
        /// <item>
        /// The processing instructions must be indented with 0-3 spaces.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="line">A line of string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of type 3 <see cref="HtmlBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        private static bool IsType3HtmlBlock(string line)
        {
            return line.StartsWith("<?");
        }

        /// <summary>
        /// Returns wether the specified line can be the first line of type 4 <see cref="HtmlBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>
        /// Begins with &lt;! fllowed by a upper case ascii letter.
        /// </item>
        /// <item>
        /// The doctype declaration must be indented with 0-3 spaces.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="line">A line of string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of type 4 <see cref="HtmlBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Returns wether the specified line can be the first line of type 5 <see cref="HtmlBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>
        /// Begins with &lt;[!CDATA[.
        /// </item>
        /// <item>
        /// The CDATA tag must be indented with 0-3 spaces.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="line">A line of string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of type 5 <see cref="HtmlBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        private static bool IsType5HtmlBlock(string line)
        {
            return line.StartsWith("<![CDATA[");
        }

        /// <summary>
        /// Returns wether the specified line can be the first line of type 6 <see cref="HtmlBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>
        /// Begins with &lt;or &lt;/ followed by one of the strings in <see cref="htmlTagNames"/>
        /// followed by whitespace, the end of the line, \gt; or /\gt;.
        /// </item>
        /// <item>
        /// The html tags must be indented with 0-3 spaces.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="line">A line of string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of type 6 <see cref="HtmlBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Returns wether the specified line can be the first line of type 7 <see cref="HtmlBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>
        /// Line must begin with complete open tag or closing tag.
        /// </item>
        /// <item>
        /// The processing instructions must be indented with 0-3 spaces.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="line">A line of string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of type 7 <see cref="HtmlBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Wether the specified line can interrupt <see cref="Paragraph"/>.
        /// </summary>
        /// <param name="line">A single line of string.</param>
        /// <returns>
        /// If the line is the start line of type 1-6 <see cref="HtmlBlock"/>,
        /// returns <c>true</c>, otherwise <c>false.</c></returns>
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

        /// <summary>
        /// Determines which type the specified line is.
        /// </summary>
        /// <param name="lineTrimmed">The single line of string.</param>
        /// <returns>
        /// The type of htnl block,
        /// if no srequirements of any types are satisfied, return -1.
        /// </returns>
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

        /// <summary>
        /// Returns wether the specified line satisfies the end condition of the current block.
        /// </summary>
        /// <param name="lineTrimmed">A line of string. (After remove indent.)</param>
        /// <returns>
        /// Return <c>true</c> if the specified line satisfies the end condition
        /// of the current block, otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Adds a line of string to the current block.
        /// </summary>
        /// <param name="line">A single line to add to this element.</param>
        /// <returns>
        /// Returns <c>AddLineResult.NeedClose | AddLineResult.Consumed</c> when
        /// <see cref="NeedClose(string)"/> returns <c>true</c>,
        /// <c>AddLineResult.Consumed</c> otherwise.
        /// </returns>
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

        /// <summary>
        /// Closes this <see cref="BlockElement"/> after closes all children
        /// and removing all <see cref="BlankLine"/> elements.
        /// 
        /// If <see cref="Type"/> is 1-5 and no line which satisfies the end condition
        /// has appeared, a warning message is added.
        /// </summary>
        /// <returns>
        /// Ths current block after close process.
        /// </returns>
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

        internal override void ParseInline(IEnumerable<string> linkDefinitions)
        {
            inlines.Add(new LiteralText(Content));
        }
    }
}
