using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    abstract class HeaderElementBase:BlockElementBase
    {
        public virtual int HeaderLevel { get; }
    }
}
