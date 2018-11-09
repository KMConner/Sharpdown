using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents a block elements in makdown documents.
    /// </summary>
    public abstract class BlockElement : MarkdownElementBase
    {
        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public abstract BlockElementType Type { get; }

        /// <summary>
        /// Gets the warnings raised while parsing.
        /// </summary>
        public abstract IReadOnlyList<string> Warnings { get; }

        /// <summary>
        /// Warning raised while parsing.
        /// </summary>
        protected readonly List<string> warnings;

        /// <summary>
        /// Add a line to this block.
        /// </summary>
        /// <param name="line">A line to add.</param>
        /// <returns></returns>
        internal abstract AddLineResult AddLine(string line);

        /// <summary>
        /// Initializes a new instance of <see cref="BlockElement"/>.
        /// </summary>
        protected BlockElement()
        {
            warnings = new List<string>();
        }

        /// <summary>
        /// Closes this <see cref="BlockElement"/>.
        /// 
        /// Some block types cannot be determined with only its first line.
        /// Then, the actual type is determined after appending the last line of this block.
        /// </summary>
        /// <returns>
        /// The closed block.
        /// The <see cref="Type"/> may be different between the two.
        /// </returns>
        internal virtual BlockElement Close()
        {
            return this;
        }

        /// <summary>
        /// Removes spaces at the beginning of <paramref name="str"/>.
        /// The number of removed spaces is up to <paramref name="maxRemoveCount"/>.
        /// </summary>
        /// <param name="str">The string to remove indent.</param>
        /// <param name="maxRemoveCount">The string after unindented.</param>
        /// <returns></returns>
        protected string RemoveIndent(string str, int maxRemoveCount)
        {
            if (maxRemoveCount == 0 || str.Length == 0 || str[0] != ' ')
            {
                return str;
            }
            return RemoveIndent(str.Substring(1), maxRemoveCount - 1);
        }
    }
}
