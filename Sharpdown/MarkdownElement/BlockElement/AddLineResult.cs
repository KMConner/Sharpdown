using System;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents the result of adding a single line to <see cref="BlockElement"/>.
    /// </summary>
    [Flags]
    internal enum AddLineResult
    {
        /// <summary>
        /// This <see cref="BlockElement"/> need to be closed (More lines must not be added.).
        /// </summary>
        NeedClose = 1,

        /// <summary>
        /// The specified line is successfully added to the <see cref="BlockElement"/>.
        /// </summary>
        Consumed = 2,
    };
}
