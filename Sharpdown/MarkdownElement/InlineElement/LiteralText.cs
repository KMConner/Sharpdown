using System;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public class LiteralText : InlineElement
    {
        public string Content { get; private set; }

        public override InlineElementType Type => InlineElementType.LiteralText;

        public LiteralText(string text, ParserConfig config) : base(config)
        {
            Content = text ?? throw new ArgumentNullException(nameof(text));
        }
    }
}
