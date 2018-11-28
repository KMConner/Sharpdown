using System.Linq;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents list items in markdown documents.
    /// </summary>
    public class ListItem : ContainerElement
    {
        /// <summary>
        /// The index of the first letter in list block.
        /// </summary>
        internal int contentIndent;

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.ListItem;

        /// <summary>
        /// Gets or sets the indent size of the list marker.
        /// </summary>
        internal int MarkIndent { get; set; }

        /// <summary>
        /// Gets or sets the bullet marker of bullet list item or
        /// deliminator character of ordered list items.
        /// </summary>
        public char Deliminator { get; internal set; }

        /// <summary>
        /// Gets or sets the index of current list item.
        /// If the current list item is a bullet list item, this value is 0 by default.
        /// </summary>
        public int Index { get; internal set; }

        internal bool IsTight { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ListItem"/>
        /// </summary>
        internal ListItem() : base() { }

        /// <summary>
        /// Returns wether the specified line satisfied proper conditions to 
        /// continue (without lazy continuation).
        /// </summary>
        /// <param name="line">The line to continue.</param>
        /// <param name="markRemoved">
        /// This value is always equivalent to <paramref name="line"/>.
        /// </param>
        /// <returns>
        /// Always <c>true</c>.
        /// </returns>
        internal override bool HasMark(string line, out string markRemoved)
        {
            markRemoved = line;
            return true;
        }

        internal override BlockElement Close()
        {
            IsTight = Children.LastOrDefault()?.Type != BlockElementType.BlankLine;
            return base.Close();
        }
    }
}
