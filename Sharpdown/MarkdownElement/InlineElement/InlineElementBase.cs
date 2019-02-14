namespace Sharpdown.MarkdownElement.InlineElement
{
    public abstract class InlineElementBase : MarkdownElementBase
    {
        public abstract InlineElementType Type { get; }
    }
}
