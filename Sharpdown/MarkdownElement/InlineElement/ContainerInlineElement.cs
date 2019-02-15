namespace Sharpdown.MarkdownElement.InlineElement
{
    public abstract class ContainerInlineElement : InlineElement
    {
        public InlineElement[] Children { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ContainerInlineElement"/>.
        /// </summary>
        /// <param name="config">Configuration of the parser.</param>
        protected ContainerInlineElement(ParserConfig config) : base(config)
        {
        }
    }
}
