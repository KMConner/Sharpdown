using System.Collections.Generic;
using Sharpdown.MarkdownElement.InlineElement;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents the Leaf elements in markdown documents.
    /// </summary>
    public abstract class LeafElement : BlockElement
    {
        /// <summary>
        /// Gets warning messages which occuerd in parsing process.
        /// </summary>
        public override IReadOnlyList<string> Warnings => warnings.AsReadOnly();

        public IReadOnlyList<InlineElementBase> Inlines => inlines.AsReadOnly();

        protected List<InlineElementBase> inlines;

        /// <summary>
        /// Initializes a new instance of <see cref="LeafElement"/>.
        /// </summary>
        internal LeafElement()
        {
            inlines = new List<InlineElementBase>();
        }
    }
}
