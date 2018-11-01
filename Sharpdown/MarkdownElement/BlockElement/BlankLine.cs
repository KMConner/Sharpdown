using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class BlankLine : LeafElementBase
    {
        public override BlockElementType Type => BlockElementType.BlankLine;

        {
            return line.TrimStart(whiteSpaceShars).Length == 0;
        }
    }
}
