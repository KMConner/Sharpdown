namespace Sharpdown.MarkdownElement.InlineElement
{
    public class Emphasis : ContainerInlineElement
    {
        public bool IsStrong { get; }

        public override InlineElementType Type =>
            IsStrong ? InlineElementType.StrongEmphasis : InlineElementType.Emphasis;

        internal Emphasis(InlineElement[] children, bool isStrong)
        {
            Children = children;
            IsStrong = isStrong;
        }
    }
}
