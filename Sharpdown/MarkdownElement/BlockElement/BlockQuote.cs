namespace Sharpdown.MarkdownElement.BlockElement
{
    public class BlockQuote : ContainerElementBase
    {
        public override BlockElementType Type => BlockElementType.BlockQuote;

        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                return false;
            }

            if (line.TrimStart(whiteSpaceShars).StartsWith(">"))
            {
                return true;
            }

            return false;
        }

        internal override bool HasMark(string line, out string markRemoved)
        {
            markRemoved = null;
            if (line.GetIndentNum() > 4)
            {
                return false;
            }
            string trimmed = line.TrimStartAscii();

            if (!trimmed.StartsWith(">"))
            {
                return false;
            }

            if (trimmed.StartsWith("> "))
            {
                markRemoved = trimmed.Substring(2);
                return true;
            }
            else
            {
                markRemoved = trimmed.Substring(1);
                return true;
            }
        }
    }
}
