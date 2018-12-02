using Sharpdown.MarkdownElement.BlockElement;
using System.Text;
using System;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public class Link : ContainerInlineElement
    {
        public override InlineElementType Type => InlineElementType.Link;

        public string Destination { get; private set; }
        public string Title { get; private set; }

        public Link(InlineElementBase[] linkText, string destination, string title)
        {
            Children = linkText;
            Destination = destination;
            Title = title ?? string.Empty;
        }
    }
}
