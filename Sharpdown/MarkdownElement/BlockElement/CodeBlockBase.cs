namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class CodeBlockBase : LeafElement
    {
        public abstract string InfoString { get; }
        public abstract string Content { get; }
        internal CodeBlockBase() : base() { }
    }
}
