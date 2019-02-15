namespace Sharpdown.MarkdownElement.InlineElement.InlineParserObjects
{
    /// <summary>
    /// Represents the nodes of deliminator stack.
    /// </summary>
    /// <remarks>
    /// See https://spec.commonmark.org/0.28/#delimiter-stack for more information about deliminator stack.
    /// </remarks>
    internal class DeliminatorInfo
    {
        /// <summary>
        /// The length of the deliminator.
        /// </summary>
        public int DeliminatorLength { get; set; }

        /// <summary>
        /// The type pf the deliminator.
        /// </summary>
        public DeliminatorType Type { get; set; }

        /// <summary>
        /// Whether the deliminator is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Whether the deliminator is a potential opener.
        /// </summary>
        public bool CanOpen { get; set; }

        /// <summary>
        /// Whether the deliminator is potential closer.
        /// </summary>
        public bool CanClose { get; set; }

        /// <summary>
        /// The index of the first character of the current deliminator.
        /// </summary>
        public int Index { get; set; }
    }
}
