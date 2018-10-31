using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class FencedCodeBlock : CodeBlockBase
    {
        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                return false;
            }

            var trimmed = line.TrimStart(whiteSpaceShars);

            if (trimmed.Length < 3 || trimmed[0] != '`' && trimmed[0] != '~')
            {
                return false;
            }

            int fence = trimmed.Length;
            for (int i = 1; i < trimmed.Length; i++)
            {
                if (trimmed[i] != trimmed[0])
                {
                    fence = i;
                    break;
                }
            }

            if (fence < 3)
            {
                return false;
            }

            string infoString = trimmed.Substring(fence);

            return !infoString.Contains(trimmed[0]);
        }
    }
}
