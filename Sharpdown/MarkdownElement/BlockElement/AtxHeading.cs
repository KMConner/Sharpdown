using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents ATX Header element in markdown documents.
    /// </summary>
    /// <remarks>
    /// Typical example is following.
    /// <![CDATA[
    /// # Header1
    /// 
    /// ## Header2
    /// ]]>
    /// </remarks>
    internal class AtxHeading : Heading
    {
        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.Heading;

        /// <summary>
        /// Regular expression which matches lines which contains ATX Header.
        /// </summary>
        private static readonly Regex headerRegex = new Regex(
            @"^[ ]{0,3}(?<level>\#{1,6})(?:[ \t]+(?<content>.*?))??(?:[ ]+\#*[ \t]*)??$", RegexOptions.Compiled);

        /// <summary>
        /// Initializes a new instance of <see cref="AtxHeading"/>.
        /// </summary>
        /// <param name="config">Configuration of the parser.</param>
        internal AtxHeading(ParserConfig config) : base(config)
        {
        }

        /// <summary>
        /// Returns whether the specified line can be a start line of <see cref="AtxHeading"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>Starts with 1-6 # characters.</item>
        /// <item>The opening # characters must be indented with 0-3 spaces.</item>
        /// <item>The opening # characters must be followed by one or more spaces. (Unless the text is empty.)</item>
        /// </list>
        /// </remarks>
        /// <param name="line">Single line string.</param>
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="AtxHeading"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        public static bool CanStartBlock(string line, int currentIndent)
        {
            if (line.GetIndentNum(currentIndent) >= 4)
            {
                // Indented too many spaces
                return false;
            }

            var trimmed = line.TrimStart(whiteSpaceChars);
            int level = trimmed.Length;
            for (int i = 0; i < trimmed.Length; i++)
            {
                if (trimmed[i] != '#')
                {
                    level = i;
                    break;
                }
            }

            if (level == 0 || level > 6)
            {
                return false;
            }

            return trimmed.Length == level || trimmed[level].IsAsciiWhiteSpace();
        }

        /// <summary>
        /// Adds a line of string to this <see cref="AtxHeading"/>.
        /// </summary>
        /// <param name="line">A single line to add to this element.</param>
        /// <param name="lazy">Whether <paramref name="line"/> is lazy continuation.</param>
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <returns>Always returns <c> AddLineResult.Consumed | AddLineResult.NeedClose</c></returns>
        internal override AddLineResult AddLine(string line, bool lazy, int currentIndent)
        {
            if (!headerRegex.IsMatch(line) || lazy)
            {
                throw new InvalidBlockFormatException(BlockElementType.Heading);
            }

            Match match = headerRegex.Match(line);
            HeaderLevel = match.Groups["level"].Value.Length;
            if (match.Groups["content"].Success)
            {
                content = match.Groups["content"].Value;
            }
            else
            {
                content = string.Empty;
            }

            return AddLineResult.Consumed | AddLineResult.NeedClose;
        }
    }
}
