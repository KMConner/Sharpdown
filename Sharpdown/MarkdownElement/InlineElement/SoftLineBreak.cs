namespace Sharpdown.MarkdownElement.InlineElement
{
    public class SoftLineBreak : InlineElement
    {
        public override InlineElementType Type => InlineElementType.SoftLineBreak;

        public SoftLineBreak(ParserConfig config) : base(config)
        {
        }
    }
}
