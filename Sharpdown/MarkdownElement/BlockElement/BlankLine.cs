using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents blank lines in markdown documents.
    /// </summary>
    public class BlankLine : LeafElement
    {
        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.BlankLine;

        public override string Content => string.Empty;

        /// <summary>
        /// Initializes a new instance of <see cref="BlankLine"/>
        /// </summary>
        internal BlankLine() : base() { }

        /// <summary>
        /// Returns wether the specified line can be a start line of <see cref="BlankLine"/>.
        /// </summary>
        /// <param name="line">Single line string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="BlankLine"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        internal static bool CanStartBlock(string line)
        {
            return line.TrimStart(whiteSpaceShars).Length == 0;
        }

        /// <summary>
        /// Adds a line of string to this <see cref="BlankLine"/>.
        /// </summary>
        /// <param name="line">The line to add.</param>
        /// <returns>
        /// Always returns <c>AddLineResult.Consumed | AddLineResult.NeedClose</c>.
        /// </returns>
        internal override AddLineResult AddLine(string line, bool lazy)
        {
            if (!CanStartBlock(line) || lazy)
            {
                throw new InvalidBlockFormatException(BlockElementType.BlankLine);
            }
            return AddLineResult.Consumed | AddLineResult.NeedClose;
        }

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions) { }
    }
}
