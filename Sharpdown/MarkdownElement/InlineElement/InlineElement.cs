namespace Sharpdown.MarkdownElement.InlineElement
{
    public abstract class InlineElement : MarkdownElementBase
    {
        public abstract InlineElementType Type { get; }
    }
}
