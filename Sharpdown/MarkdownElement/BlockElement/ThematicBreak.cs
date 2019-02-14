using System.Collections.Generic;
using System.Linq;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents Thematic breaks in markdown documents.
    /// </summary>
    /// <remarks>
    /// The typical thematic break is following.
    /// 
    /// <![CDATA[
    /// ****
    /// 
    /// ----
    /// 
    /// ____
    /// ]]>
    /// </remarks>
    class ThematicBreak : LeafElement
    {
        /// <summary>
        /// The characters which can be used in thematic breaks.
        /// </summary>
        private static readonly char[] ThematicBreakChars = {'-', '_', '*'};

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.ThematicBreak;

        public override string Content => string.Empty;

        /// <summary>
        /// Returns whether the specified line can be a start line of <see cref="FencedCodeBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>
        /// Must start with three or more *, - or _ characters.
        /// (Each optionally can be followed by any number of spaces.)
        /// </item>
        /// <item>
        /// The characters introduced above must be indented with 0-3 spaces.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="line">Single line string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="FencedCodeBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        public static bool CanStartBlock(string line, int currentIndent)
        {
            if (line.GetIndentNum(currentIndent) >= 4)
            {
                return false;
            }

            var shortenLine = line.Remove(" ").Remove("\t").Remove("\x000B").Remove("\x000C");
            return shortenLine.Length >= 3
                   && ThematicBreakChars.Contains(shortenLine[0])
                   && shortenLine.All(c => c == shortenLine[0]);
        }

        /// <summary>
        /// Adds a line of string to this <see cref="ThematicBreak"/>.
        /// </summary>
        /// <param name="line">A single line to add to this element.</param>
        /// <returns>
        /// Always returns <c>AddLineResult.Consumed | AddLineResult.NeedClose</c>.
        /// </returns>
        internal override AddLineResult AddLine(string line, bool lazy, int currentIndent)
        {
            if (!CanStartBlock(line, currentIndent) || lazy)
            {
                throw new InvalidBlockFormatException(BlockElementType.ThematicBreak);
            }

            if (lazy)
            {
                throw new InvalidBlockFormatException(
                    BlockElementType.ThematicBreak, "Must Not be Lazy.");
            }

            return AddLineResult.Consumed | AddLineResult.NeedClose;
        }

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions)
        {
        }
    }
}
