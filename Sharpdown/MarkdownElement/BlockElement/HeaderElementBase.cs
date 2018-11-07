namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class HeaderElementBase : LeafElement
    {
        public int HeaderLevel { get; protected set; }

        public string Content { get; protected set; }
    }
}
