using System.Collections.Generic;
using Sharpdown.MarkdownElement.InlineElement;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class CodeBlockBase : LeafElement
    {
        public abstract string InfoString { get; }

        internal CodeBlockBase()
        {
        }

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions)
        {
            inlines.Add(new LiteralText(Content));
        }
    }
}
