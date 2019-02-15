namespace Sharpdown.MarkdownElement.InlineElement.InlineParserObjects
{
    /// <summary>
    /// Represents the type of <see cref="InlineSpan"/>.
    /// </summary>
    internal enum InlineSpanType
    {
        /// <summary>
        /// Inline link,
        /// </summary>
        Link,
        
        /// <summary>
        /// Inline image.
        /// </summary>
        Image,
        
        /// <summary>
        /// Emphasis.
        /// </summary>
        Emphasis,
        
        /// <summary>
        /// Strong emphasis.
        /// </summary>
        StrongEmphasis,
        
        /// <summary>
        /// Code span.
        /// </summary>
        CodeSpan,
        
        /// <summary>
        /// Auto link.
        /// </summary>
        AutoLink,
        
        /// <summary>
        /// Inline html.
        /// </summary>
        InlineHtml,
        
        /// <summary>
        /// Root.
        /// </summary>
        Root,
    }
}
