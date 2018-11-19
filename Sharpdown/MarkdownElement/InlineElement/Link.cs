using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public class Link : ContainerInlineElement
    {
        public override InlineElementType Type => InlineElementType.Link;
        public Link(InlineElementBase[] linkText)
        {
            Children = linkText;
        }
    }
}
