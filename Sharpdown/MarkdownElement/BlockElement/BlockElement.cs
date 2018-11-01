using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class BlockElement : MarkdownElementBase
    {
        //public abstract bool CanContinueBlock(string line);

        internal abstract AddLineResult AddLine(string line);

        public abstract BlockElementType Type { get; }
    }
}
