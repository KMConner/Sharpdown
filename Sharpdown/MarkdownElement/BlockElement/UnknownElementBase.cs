using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class UnknownElementBase : LeafElementBase
    {
        private List<string> content;
        private bool mayBeLinkReferenceDefinition;
        protected UnknownElementBase()
        {
            content = new List<string>();
        }

        public override BlockElementType Type => BlockElementType.Unknown;

    }
}
