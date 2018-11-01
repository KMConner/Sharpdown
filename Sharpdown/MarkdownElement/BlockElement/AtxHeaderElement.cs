using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class AtxHeaderElement : HeaderElementBase
    {
        public override BlockElementType Type => BlockElementType.AtxHeading;

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
    }
}
