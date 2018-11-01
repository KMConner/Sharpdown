using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class BlankLine : LeafElementBase
    {
        public override BlockElementType Type => BlockElementType.BlankLine;

        internal static bool CanStartBlock(string line)
        {
            return line.TrimStart(whiteSpaceShars).Length == 0;
        }

        internal override AddLineResult AddLine(string line)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}
