using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Sharpdown.MarkdownElement.BlockElement;

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
            var highPriorityDelims = new List<InlineElementUtils.DelimSpan>();
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
                    var span = new InlineElementUtils.DelimSpan
                    {
                        Begin = nextElemIndex,
                        End = newIndex,
                        DeliminatorType = InlineElementUtils.ToDelimType(newInline.Type),
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

            return InlineElementUtils.ParseLinkEmphasis(text,
                new Dictionary<string, LinkReferenceDefinition>(linkReferenceDefinitions,
                    StringComparer.OrdinalIgnoreCase), highPriorityDelims,
                parserConfig);
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
    }
}
