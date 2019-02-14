using System.Collections.Generic;
using Sharpdown.MarkdownElement.InlineElement;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents the Header elements in markdown document.
    /// </summary>
    /// <seealso cref="AtxHeading"/>
    /// <seealso cref="SetextHeading"/>
    public abstract class Heading : LeafElement
    {
        /// <summary>
        /// The header level
        /// </summary>
        public int HeaderLevel { get; protected set; }

        protected string content;

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions)
        {
            inlines.AddRange(InlineElementUtils.ParseInlineElements(content, linkDefinitions));
        }
    }
}
