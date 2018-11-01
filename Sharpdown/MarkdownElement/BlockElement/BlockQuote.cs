﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class BlockQuote : ContainerElementBase
    {
        public override BlockElementType Type => BlockElementType.BlockQuote;

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

        internal override AddLineResult AddLine(string line)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}
