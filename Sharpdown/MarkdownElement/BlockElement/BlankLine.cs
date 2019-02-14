using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents blank lines in markdown documents.
    /// </summary>
    internal class BlankLine : LeafElement
    {
        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.BlankLine;

        public override string Content => string.Empty;

        /// <summary>
        /// Initializes a new instance of <see cref="BlankLine"/>
        /// </summary>
        internal BlankLine()
        {
        }

        /// <summary>
        /// Returns whether the specified line can be a start line of <see cref="BlankLine"/>.
        /// </summary>
        /// <param name="line">Single line string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="BlankLine"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        internal static bool CanStartBlock(string line)
        {
            return line.TrimStart(whiteSpaceChars).Length == 0;
        }

        /// <summary>
        /// Adds a line of string to this <see cref="BlankLine"/>.
        /// </summary>
        /// <param name="line">The line to add.</param>
        /// <param name="lazy">Whether <paramref name="line"/> is lazy continuation.</param>
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <returns>
        /// Always returns <c>AddLineResult.Consumed | AddLineResult.NeedClose</c>.
        /// </returns>
        internal override AddLineResult AddLine(string line, bool lazy, int currentIndent)
        {
            if (!CanStartBlock(line))
            {
                throw new InvalidBlockFormatException(BlockElementType.BlankLine);
            }

            return AddLineResult.Consumed | AddLineResult.NeedClose;
        }

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions)
        {
        }
    }
}
