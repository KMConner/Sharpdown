using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class BlankLine:LeafElementBase
    {
        public static bool CanStartBlock(string line)
        {
            return line.TrimStart(whiteSpaceShars).Length == 0;
        }
    }
}
