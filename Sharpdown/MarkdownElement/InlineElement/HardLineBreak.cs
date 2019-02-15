namespace Sharpdown.MarkdownElement.InlineElement
{
    class HardLineBreak : InlineElement
    {
        public override InlineElementType Type => InlineElementType.HardLineBreak;

        public HardLineBreak(ParserConfig config) : base(config)
        {
        }
    }
}
