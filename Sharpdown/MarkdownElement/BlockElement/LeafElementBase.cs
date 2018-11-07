using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class LeafElementBase : BlockElement
    {
        public override IReadOnlyList<string> Warnings => warnings.AsReadOnly();

        internal LeafElementBase() : base() { }
    }
}
