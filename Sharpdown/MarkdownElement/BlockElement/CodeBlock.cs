using System.Collections.Generic;
using Sharpdown.MarkdownElement.InlineElement;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class CodeBlock : LeafElement
    {
        public abstract string InfoString { get; }

        public abstract string Code { get; }

        internal CodeBlock(ParserConfig config) : base(config)
        {
        }

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions)
        {
            inlines.Add(new LiteralText(Code, parserConfig));
        }
    }
}
