using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class BlockElementUtil
    {
        public static BlockElementType DetermineNewBlockType(string line)
        {
            if (IndentedCodeBlock.CanStartBlock(line))
            {
                return BlockElementType.IndentedCodeBlock;
            }

            if (ThemanticBreak.CanStartBlock(line))
            {
                return BlockElementType.ThemanticBreak;
            }

            if (AtxHeaderElement.CanStartBlock(line))
            {
                return BlockElementType.AtxHeading;
            }

            if (FencedCodeBlock.CanStartBlock(line))
            {
                return BlockElementType.FencedCodeBlock;
            }

            if (HtmlBlock.CanStartBlock(line))
            {
                return BlockElementType.HtmlBlock;
            }

            if (BlockQuote.CanStartBlock(line))
            {
                return BlockElementType.BlockQuote;
            }

            if (ListBlock.CanStartBlock(line))
            {
                return BlockElementType.List;
            }

            if (BlankLine.CanStartBlock(line))
            {
                return BlockElementType.BlankLine;
            }

            return BlockElementType.Unknown;
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
    }
}
