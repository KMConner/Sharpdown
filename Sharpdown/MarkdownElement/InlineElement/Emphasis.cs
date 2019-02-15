namespace Sharpdown.MarkdownElement.InlineElement
{
    public class Emphasis : ContainerInlineElement
    {
        public bool IsStrong { get; }

        public override InlineElementType Type =>
            IsStrong ? InlineElementType.StrongEmphasis : InlineElementType.Emphasis;

        internal Emphasis(InlineElement[] children, bool isStrong, ParserConfig config) : base(config)
        {
            Children = children;
            IsStrong = isStrong;
        }
    }
}
