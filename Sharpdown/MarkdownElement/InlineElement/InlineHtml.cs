﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.InlineElement
{
    class InlineHtml : ContainerInlineElement
    {
        public override InlineElementType Type => InlineElementType.InlineHtml;
    }
}
