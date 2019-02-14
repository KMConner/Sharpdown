namespace Sharpdown.MarkdownElement.InlineElement
{
    public class Emphasis : ContainerInlineElement
    {
        public bool IsStrong { get; private set; }

        public override InlineElementType Type => IsStrong ? InlineElementType.StrongEmphasis : InlineElementType.Emphasis;

        public Emphasis(InlineElementBase[] children, bool isStrong)
        {
            Children = children;
            IsStrong = isStrong;
        }
    }
}
