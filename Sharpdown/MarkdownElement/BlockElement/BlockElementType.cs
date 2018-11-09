namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents the types of <see cref="BlockElement"/>.
    /// </summary>
    public enum BlockElementType
    {
        /// <summary>
        /// Themantic Break.
        /// </summary>
        ThemanticBreak,

        /// <summary>
        /// ATX Header.
        /// </summary>
        AtxHeading,

        /// <summary>
        /// Setext Header.
        /// </summary>
        SetextHeading,

        /// <summary>
        /// Indented Code Block.
        /// </summary>
        IndentedCodeBlock,

        /// <summary>
        /// Fenced Code Block.
        /// </summary>
        FencedCodeBlock,

        /// <summary>
        /// HTML Block.
        /// </summary>
        HtmlBlock,

        /// <summary>
        /// Link Refernce Definition.
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
