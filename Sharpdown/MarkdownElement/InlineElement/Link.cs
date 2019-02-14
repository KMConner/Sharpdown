using System;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public class Link : ContainerInlineElement
    {
        public override InlineElementType Type => InlineElementType.Link;

        public string Destination { get; private set; }
        public string Title { get; private set; }

        public Link(InlineElementBase[] linkText, string destination, string title)
        {
            Children = linkText;
            Destination =
                InlineElementUtils.UrlEncode(InlineText.HandleEscapeAndHtmlEntity(RemoveAngleBrackets(destination)));
            Title = title == null ? null : InlineText.HandleEscapeAndHtmlEntity(RemoveQuotes(title));
        }

        public Link(string destination, bool mailto = false)
        {
            Children = new[] {InlineText.CreateFromText(destination, false)};
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
