using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class Paragraph : LeafElementBase
    {
        public string Content => string.Join("\r\n", contents);
        private readonly List<string> contents;
        internal Paragraph(UnknownElement element) : base()
        {
            contents = element.content;
            warnings.AddRange(element.Warnings);
        }

        public override BlockElementType Type => BlockElementType.Paragraph;

        internal override AddLineResult AddLine(string line)
        {
            throw new InvalidOperationException();
        }
    }
}
