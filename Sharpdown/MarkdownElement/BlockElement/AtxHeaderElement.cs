using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class AtxHeaderElement : HeaderElementBase
    {
        public override BlockElementType Type => BlockElementType.AtxHeading;

        private static readonly Regex headerRegex = new Regex(
            @"^[ ]{0,3}(?<level>\#{1,6})(?:[ \t]+(?<content>.*?))??(?:[ ]+\#*)??$", RegexOptions.Compiled);

        public int Level { get; private set; }

        public string Content { get; private set; }

        internal AtxHeaderElement() : base() { }

        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() > 4)
            {
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

        internal override AddLineResult AddLine(string line)
        {
            if (!headerRegex.IsMatch(line))
            {
                throw new InvalidBlockFormatException(BlockElementType.AtxHeading);
            }

            Match match = headerRegex.Match(line);
            Level = match.Groups["level"].Value.Length;
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
