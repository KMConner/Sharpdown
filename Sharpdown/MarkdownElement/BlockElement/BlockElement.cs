using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents a block elements in markdown documents.
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
        /// <param name="lazy">Whether <paramref name="line"/> is lazy continuation.</param>
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <returns></returns>
        internal abstract AddLineResult AddLine(string line, bool lazy, int currentIndent);

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
        /// <param name="maxRemoveCount">The string after indent removal.</param>
        /// <param name="currentIndent">The indent count of <paramref name="str"/>.</param>
        /// <returns></returns>
        protected string RemoveIndent(string str, int maxRemoveCount, int currentIndent)
        {
            if (maxRemoveCount == 0 || str.Length == 0)
            {
                return str;
            }

            if (str[0] == ' ')
            {
                return RemoveIndent(str.Substring(1), maxRemoveCount - 1, currentIndent + 1);
            }

            if (str[0] == '\t')
            {
                int tabWidth = 4 - (currentIndent % 4);
                return RemoveIndent(new string(' ', tabWidth - 1) + str.Substring(1), maxRemoveCount - 1,
                    currentIndent + 1);
            }

            return str;
        }

        internal abstract void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions);
    }
}
