using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class BlockQuote : ContainerElementBase
    {
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
    }
}
