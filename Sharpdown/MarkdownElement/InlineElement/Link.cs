using Sharpdown.MarkdownElement.BlockElement;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public class Link : ContainerInlineElement
    {
        public override InlineElementType Type => InlineElementType.Link;

        public string Destination { get; private set; }
        public string Title { get; private set; }

        public Link(InlineElementBase[] linkText, LinkReferenceDefinition definition)
        {
            Children = linkText;
            Destination = definition.Destination;
            Title = definition.Title;
        }

        public Link(InlineElementBase[] linkText, string destination, string title)
        {
            Children = linkText;
            Destination = destination;
            Title = title;
        }
    }
}
