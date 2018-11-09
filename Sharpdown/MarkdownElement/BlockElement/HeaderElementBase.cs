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
        public string Content { get; protected set; }
    }
}
