using System.Collections.Generic;
using Sharpdown.MarkdownElement.InlineElement;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents the Header elements in markdown document.
    /// </summary>
    /// <seealso cref="AtxHeaderElement"/>
    /// <seealso cref="SetextHeader"/>
    public abstract class HeaderElementBase : LeafElement
    {
        /// <summary>
        /// The header level
        /// </summary>
        public int HeaderLevel { get; protected set; }

        /// <summary>
        /// The content of this header.
        /// </summary>
        public override string Content => content;

        protected string content;

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions)
        {
            inlines.AddRange(InlineElementUtils.ParseInlineElements(Content, linkDefinitions));
        }
    }
}
