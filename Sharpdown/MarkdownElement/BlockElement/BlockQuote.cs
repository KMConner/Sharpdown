using System;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents Block Quotes in markdown documents.
    /// </summary>
    /// <remarks>
    /// The typical example is following.
    /// <![CDATA[
    /// > Foo
    /// > - bar
    /// > - baz
    /// ]]>
    /// </remarks>
    public class BlockQuote : ContainerElement
    {
        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.BlockQuote;

        public override string Content => throw new InvalidOperationException();

        /// <summary>
        /// Initialzies a new instance of <see cref="BlockQuote"/>.
        /// </summary>
        internal BlockQuote() : base() { }

        /// <summary>
        /// Returns wether the specified line can be a start line of <see cref="BlockQuote"/>.
        /// </summary>
        /// <param name="line">Single line string.</param>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>Starts with 0-3 spaces of initial indent.</item>
        /// <item>
        /// After initial indent, '>' character with or without a following space must appear.
        /// </item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="BlockQuote"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        public static bool CanStartBlock(string line, int currentIndent)
        {
            if (line.GetIndentNum(currentIndent) >= 4)
            { // Too many indent
                return false;
            }

            if (line.TrimStart(whiteSpaceShars).StartsWith(">"))
            {
                return true;
            }

            return false;
        }

        internal override bool HasMark(string line, int currentIndent, out string markRemoved, out int markLength)
        {
            markRemoved = null;
            markLength = 0;
            if (line.GetIndentNum(currentIndent) > 4)
            {
                return false;
            }
            string trimmed = line.TrimStartAscii();

            if (!trimmed.StartsWith(">"))
            {
                return false;
            }

            if (trimmed.StartsWith("> ") || trimmed.StartsWith(">\t"))
            {
                markRemoved = SubStringExpandingTabs(trimmed, 2, currentIndent);
                markLength = 2;
                return true;
            }
            else
            {
                markRemoved = trimmed.Substring(1);
                markLength = 1;
                return true;
            }
        }
    }
}
