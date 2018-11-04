using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class CodeBlockBase : LeafElementBase
    {
        public abstract string InfoString { get; }
        public abstract string Content { get; }
        internal CodeBlockBase() : base() { }
    }
}
