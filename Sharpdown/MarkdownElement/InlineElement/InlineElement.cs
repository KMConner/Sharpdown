namespace Sharpdown.MarkdownElement.InlineElement
{
    public abstract class InlineElement : MarkdownElementBase
    {
        public abstract InlineElementType Type { get; }

        protected readonly ParserConfig parserConfig;

        internal InlineElement(ParserConfig config)
        {
            parserConfig = config;
        }
    }
}
