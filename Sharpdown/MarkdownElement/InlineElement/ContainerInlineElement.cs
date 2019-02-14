namespace Sharpdown.MarkdownElement.InlineElement
{
    public abstract class ContainerInlineElement : InlineElementBase
    {
        public InlineElementBase[] Children { get; protected set; }
    }
}
