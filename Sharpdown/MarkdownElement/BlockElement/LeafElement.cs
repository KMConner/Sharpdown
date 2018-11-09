using System.Collections.Generic;

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

        /// <summary>
        /// Initializes a new instance of <see cref="LeafElement"/>.
        /// </summary>
        internal LeafElement() : base() { }
    }
}
