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
    public class AtxHeaderElement : HeaderElementBase
    {
        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.AtxHeading;

        /// <summary>
        /// Regular expression which matches lines which contains ATX Header.
        /// </summary>
        private static readonly Regex headerRegex = new Regex(
            @"^[ ]{0,3}(?<level>\#{1,6})(?:[ \t]+(?<content>.*?))??(?:[ ]+\#*[ \t]*)??$", RegexOptions.Compiled);


        /// <summary>
        /// Initializes a new instance of <see cref="AtxHeaderElement"/>
        /// </summary>
        internal AtxHeaderElement() : base() { }

        /// <summary>
        /// Returns whether the specified line can be a start line of <see cref="AtxHeaderElement"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>Starts with 1-6 # characters.</item>
        /// <item>The opening # characters must be indented with 0-3 speces.</item>
        /// <item>The opening # characters must be followed by one or more spaces. (Unless the text is empty.)</item>
        /// </list>
        /// </remarks>
        /// <param name="line">Single line string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="AtxHeaderElement"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                // Indented too many speces
                return false;
            }
            var trimmed = line.TrimStart(whiteSpaceShars);
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
        /// Adds a line of string to this <see cref="AtxHeaderElement"/>.
        /// </summary>
        /// <param name="line">A single line to add to this element.</param>
        /// <returns>Always returns <c> AddLineResult.Consumed | AddLineResult.NeedClose</c></returns>
        internal override AddLineResult AddLine(string line, bool lazy)
        {
            if (!headerRegex.IsMatch(line) || lazy)
            {
                throw new InvalidBlockFormatException(BlockElementType.AtxHeading);
            }

            Match match = headerRegex.Match(line);
            HeaderLevel = match.Groups["level"].Value.Length;
            if (match.Groups["content"].Success)
            {
                Content = match.Groups["content"].Value;
            }
            else
            {
                Content = string.Empty;
            }
            return AddLineResult.Consumed | AddLineResult.NeedClose;
        }
    }
}
