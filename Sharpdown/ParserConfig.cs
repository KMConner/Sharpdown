namespace Sharpdown
{
    /// <summary>
    /// Represents configurations for <see cref="MarkdownParser"/>.
    /// </summary>
    public class ParserConfig
    {
        public bool IsTableExtensionEnabled { get; internal set; }

        public bool IsTaskListExtensionEnabled { get; internal set; }

        public bool IsStrikethroughExtensionEnabled { get; internal set; }

        public bool IsAutoLinkExtensionEnabled { get; internal set; }

        public bool IsDisallowedRawHtmlExtensionEnabled { get; internal set; }

        /// <summary>
        /// Creates a new instance of <see cref="ParserConfig"/>.
        /// </summary>
        internal ParserConfig()
        {
        }
    }
}
