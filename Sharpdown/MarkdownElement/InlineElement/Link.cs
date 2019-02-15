using System;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public class Link : ContainerInlineElement
    {
        public override InlineElementType Type => InlineElementType.Link;

        public string Destination { get; }
        public string Title { get; }

        public Link(InlineElement[] linkText, string destination, string title, ParserConfig config) : base(config)
        {
            Children = linkText;
            Destination =
                InlineElementUtils.UrlEncode(InlineText.HandleEscapeAndHtmlEntity(RemoveAngleBrackets(destination)));
            Title = title == null ? null : InlineText.HandleEscapeAndHtmlEntity(RemoveQuotes(title));
        }

        internal Link(string destination, ParserConfig config, bool mailto = false) : base(config)
        {
            Children = new InlineElement[] {InlineText.CreateFromText(destination, config, false)};
            Destination = (mailto ? "mailto:" : string.Empty) + InlineElementUtils.UrlEncode(destination);
        }

        private string RemoveQuotes(string text)
        {
            if (text.StartsWith("\"", StringComparison.Ordinal) && text.EndsWith("\"", StringComparison.Ordinal))
            {
                return text.Substring(1, text.Length - 2);
            }

            if (text.StartsWith("'", StringComparison.Ordinal) && text.EndsWith("'", StringComparison.Ordinal))
            {
                return text.Substring(1, text.Length - 2);
            }

            if (text.StartsWith("(", StringComparison.Ordinal) && text.EndsWith(")", StringComparison.Ordinal))
            {
                return text.Substring(1, text.Length - 2);
            }

            return text;
        }

        private string RemoveAngleBrackets(string text)
        {
            if (text.StartsWith("<", StringComparison.Ordinal) && text.EndsWith(">", StringComparison.Ordinal))
            {
                return text.Substring(1, text.Length - 2);
            }

            return text;
        }
    }
}
