namespace Sharpdown
{
    /// <summary>
    /// Provides features to build <see cref="ParserConfig"/>.
    /// </summary>
    public class ParserConfigBuilder
    {
        /// <summary>
        /// Gets the configuration of CommonMark.
        /// </summary>
        public static ParserConfigBuilder CommonMark => new ParserConfigBuilder();

        public bool IsTableExtensionEnabled { get; set; }

        public bool IsTaskListExtensionEnabled { get; set; }

        public bool IsStrikethroughExtensionEnabled { get; set; }

        public bool IsAutoLinkExtensionEnabled { get; set; }

        public bool IsDisallowedRawHtmlExtensionEnabled { get; set; }

        /// <summary>
        /// Returns a equivalent <see cref="ParserConfig"/> to the current builder.
        /// </summary>
        /// <returns></returns>
        public ParserConfig ToParserConfig()
        {
            return new ParserConfig
            {
                IsTableExtensionEnabled = IsTableExtensionEnabled,
                IsAutoLinkExtensionEnabled = IsAutoLinkExtensionEnabled,
                IsDisallowedRawHtmlExtensionEnabled = IsDisallowedRawHtmlExtensionEnabled,
                IsStrikethroughExtensionEnabled = IsStrikethroughExtensionEnabled,
                IsTaskListExtensionEnabled = IsTaskListExtensionEnabled,
            };
        }
    }
}
