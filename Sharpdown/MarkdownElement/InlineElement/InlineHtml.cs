namespace Sharpdown.MarkdownElement.InlineElement
{
    public class InlineHtml : ContainerInlineElement
    {
        public override InlineElementType Type => InlineElementType.InlineHtml;

        public string Content { get; }

        public InlineHtml(string html, ParserConfig config) : base(config)
        {
            Content = html;
        }
    }
}
