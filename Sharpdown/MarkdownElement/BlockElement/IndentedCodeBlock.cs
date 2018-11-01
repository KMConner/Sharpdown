using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class IndentedCodeBlock : CodeBlockBase
    {
        public override BlockElementType Type => BlockElementType.IndentedCodeBlock;

        public static bool CanStartBlock(string line)
        {
            return line.GetIndentNum() >= 4;
        }

        internal override AddLineResult AddLine(string line)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}
