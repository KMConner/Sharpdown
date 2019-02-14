namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents the types of <see cref="BlockElement"/>.
    /// </summary>
    public enum BlockElementType
    {
        /// <summary>
        /// Thematic Break.
        /// </summary>
        ThematicBreak,

        /// <summary>
        /// Heading.
        /// </summary>
        Heading,

        /// <summary>
        /// Indented/Fenced Code Block.
        /// </summary>
        CodeBlock,

        /// <summary>
        /// HTML Block.
        /// </summary>
        HtmlBlock,

        /// <summary>
        /// Link Reference Definition.
        /// </summary>
        LinkReferenceDefinition,

        /// <summary>
        /// Paragraph.
        /// </summary>
        Paragraph,

        /// <summary>
        /// BlankLine.
        /// </summary>
        BlankLine,

        /// <summary>
        /// Block Quote.
        /// </summary>
        BlockQuote,

        /// <summary>
        /// List Block.
        /// </summary>
        List,

        /// <summary>
        /// List Item.
        /// </summary>
        ListItem,

        /// <summary>
        /// Unknown Element.
        /// </summary>
        Unknown,
    }
}
