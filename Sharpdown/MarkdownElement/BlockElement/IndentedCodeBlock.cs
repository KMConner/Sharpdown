using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class IndentedCodeBlock : CodeBlockBase
    {
        public override BlockElementType Type => BlockElementType.IndentedCodeBlock;

        public override string InfoString => string.Empty;

        public override string Content => string.Join("\r\n", contents);

        private List<string> contents;

        internal IndentedCodeBlock()
        {
            contents = new List<string>();
        }

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
