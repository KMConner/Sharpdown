using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class SetextHeader : HeaderElementBase
    {
        public override BlockElementType Type => BlockElementType.SetextHeading;

        internal SetextHeader(UnknownElement elem, int level) : base()
        {
            if (level != 1 && level != 2)
            {
                throw new ArgumentException("level must be 1 or 2.", nameof(level));
            }
            HeaderLevel = level;
            Content = string.Join("\r\n", elem.content);
            warnings.AddRange(elem.Warnings);
        }

        internal override AddLineResult AddLine(string line)
        {
            throw new InvalidOperationException();
        }

        internal override BlockElement Close()
        {
            throw new InvalidOperationException();
        }
    }
}
