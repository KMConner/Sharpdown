namespace Sharpdown.MarkdownElement.InlineElement
{
    public abstract class ContainerInlineElement : InlineElement
    {
        public InlineElement[] Children { get; protected set; }
    }
}
