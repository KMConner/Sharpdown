using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    abstract class HeaderElementBase:BlockElement
    {
        public virtual int HeaderLevel { get; }
    }
}
