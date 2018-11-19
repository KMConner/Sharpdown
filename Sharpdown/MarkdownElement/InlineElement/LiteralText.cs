namespace Sharpdown.MarkdownElement.InlineElement
{
    public class LiteralText : InlineElementBase
    {
        public string Content { get; private set; }

        public override InlineElementType Type => InlineElementType.LiteralText;

        internal LiteralText(string text)
        {
            Content = text;
        }
    }
}
