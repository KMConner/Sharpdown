using System;
using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class Paragraph : LeafElement
    {
        public string Content => string.Join("\r\n", contents);
        private readonly List<string> contents;
        public override BlockElementType Type => BlockElementType.Paragraph;

        internal Paragraph(UnknownElement element) : base()
        {
            contents = element.content;
            warnings.AddRange(element.Warnings);
        }


        internal override AddLineResult AddLine(string line)
        {
            throw new InvalidOperationException();
        }
    }
}
