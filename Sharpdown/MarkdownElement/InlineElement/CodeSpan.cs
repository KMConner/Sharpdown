using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.InlineElement
{
    class CodeSpan : ContainerInlineElement
    {
        public override InlineElementType Type => InlineElementType.CodeSpan;
    }
}
