namespace Sharpdown.MarkdownElement.BlockElement
{
    public class ListItem : ContainerElementBase
    {
        public override BlockElementType Type => BlockElementType.ListItem;

        internal int contentIndent;

        internal int MarkIndent { get; set; }

        public char Deliminator { get; internal set; }

        public int Index { get; internal set; }

        internal ListItem() : base() { }

        internal override bool HasMark(string line, out string markRemoved)
        {
            markRemoved = line;
            return true;
        }
    }
}
