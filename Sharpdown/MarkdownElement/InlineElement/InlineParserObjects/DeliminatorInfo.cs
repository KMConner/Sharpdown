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

        /// <summary>
        /// Creates a new instance of <see cref="DeliminatorInfo"/> with the specified type, index and length.
        /// </summary>
        /// <remarks>
        /// <see cref="Active"/> and <see cref="CanOpen"/> of the returned object are always <c>true</c>.
        /// <see cref="CanClose"/> of the returned object is <c>true</c> iff <paramref name="type"/> is not OpenLink or OpenImage.
        /// </remarks>
        /// <param name="type">The type of this deliminator.</param>
        /// <param name="index">The index of the beginning of this deliminator</param>
        /// <param name="length">The length of this deliminator.</param>
        /// <returns>DeliminatorInfo object.</returns>
        public static DeliminatorInfo Create(DeliminatorType type, int index, int length)
        {
            return new DeliminatorInfo
            {
                Index = index,
                Active = true,
                CanOpen = true,
                CanClose = type != DeliminatorType.OpenLink && type != DeliminatorType.OpenImage,
                Type = type,
                DeliminatorLength = length,
            };
        }
    }
}
