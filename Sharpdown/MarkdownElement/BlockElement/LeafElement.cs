using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class LeafElement : BlockElement
    {
        public override IReadOnlyList<string> Warnings => warnings.AsReadOnly();

        internal LeafElement() : base() { }
    }
}
