using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sharpdown.MarkdownElement.BlockElement;

namespace Sharpdown.MarkdownElement.InlineElement
{
    /// <summary>
    /// This class provides methods to parse inline structure.
    /// </summary>
    internal class InlineParser
    {
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
                    newInline = InlineElementUtils.GetCodeSpan(text, nextBacktick, ref newIndex, parserConfig);
                }
                // Find <
                // Search raw html and auto links
                else if (nextLessThan >= 0 && (nextBacktick < 0 || nextLessThan < nextBacktick))
                {
                    nextElemIndex = nextLessThan;
                    newIndex = nextElemIndex;
                    newInline = InlineElementUtils.GetInlineHtmlOrLink(text, nextLessThan, ref newIndex, parserConfig);
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
    }
}
