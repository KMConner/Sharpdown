namespace Sharpdown.MarkdownElement.BlockElement
{
    public class BlockElementUtil
    {
        public static BlockElement CreateBlockFromLine(string line)
        {
            if (IndentedCodeBlock.CanStartBlock(line))
            {
                return new IndentedCodeBlock();
            }

            if (ThemanticBreak.CanStartBlock(line))
            {
                return new ThemanticBreak();
            }

            if (AtxHeaderElement.CanStartBlock(line))
            {
                return new AtxHeaderElement();
            }

            if (FencedCodeBlock.CanStartBlock(line))
            {
                return new FencedCodeBlock();
            }

            if (HtmlBlock.CanStartBlock(line))
            {
                return new HtmlBlock();
            }

            if (BlockQuote.CanStartBlock(line))
            {
                return new BlockQuote();
            }

            if (ListBlock.CanStartBlock(line))
            {
                return new ListBlock();
            }

            if (BlankLine.CanStartBlock(line))
            {
                return new BlankLine();
            }

            return new UnknownElement();
        }
    }

    public static class ExtendMethods
    {
        internal static string Remove(this string str, string remove)
        {
            return str.Replace(remove, string.Empty);
        }

        internal static bool IsAsciiWhiteSpace(this char ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\x000B' || ch == '\x000C';
        }

        internal static int GetIndentNum(this string line)
        {
            if (line.TrimStart(MarkdownElementBase.whiteSpaceShars).Length == 0)
            {
                return -1;
            }
            int ret = 0;
            foreach (var item in line)
            {
                switch (item)
                {
                    case ' ':
                        ret++;
                        break;
                    case '\t':
                        ret += 4;
                        break;
                    default:
                        return ret;
                }
            }
            return ret;
        }

        internal static string TrimStartAscii(this string str)
        {
            return str.TrimStart(MarkdownElementBase.whiteSpaceShars);
        }


    }
}
