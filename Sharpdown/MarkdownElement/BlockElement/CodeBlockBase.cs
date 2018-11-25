using System.Collections.Generic;
using Sharpdown.MarkdownElement.InlineElement;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class CodeBlockBase : LeafElement
    {
        public abstract string InfoString { get; }
        public abstract string Content { get; }
        internal CodeBlockBase() : base() { }
        internal override void ParseInline(IEnumerable<string> linkDefinitions)
        {
            inlines.Add(new LiteralText(Content));
        }
    }
}
