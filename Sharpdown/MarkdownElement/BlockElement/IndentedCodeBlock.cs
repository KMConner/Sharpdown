using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class IndentedCodeBlock : CodeBlockBase
    {
        public static bool CanStartBlock(string line)
        {
            return line.GetIndentNum() >= 4;
        }
    }
}
