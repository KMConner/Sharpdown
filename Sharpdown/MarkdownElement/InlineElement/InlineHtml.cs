namespace Sharpdown.MarkdownElement.InlineElement
{
    public class InlineHtml : ContainerInlineElement
    {
        public override InlineElementType Type => InlineElementType.InlineHtml;

        public string Content { get; private set; }

        public InlineHtml(string html)
        {
            Content = html;
        }
    }
}
