using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class LeafElementBase : BlockElement
    {
        protected readonly List<string> warnings;
        public override IReadOnlyList<string> Warnings => warnings.AsReadOnly();

        internal LeafElementBase()
        {
            warnings = new List<string>();
        }
    }
}
