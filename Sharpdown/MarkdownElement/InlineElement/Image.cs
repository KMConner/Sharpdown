using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public class Image : ContainerInlineElement
    {
        public override InlineElementType Type => InlineElementType.Image;

        public Image(InlineElementBase[] text)
        {
            Children = text;
        }
    }
}
