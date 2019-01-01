using Sharpdown.MarkdownElement.BlockElement;
using System.Text;
using System.Linq;
using System;
using System.Web;

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
            Destination = HttpUtility.UrlPathEncode(InlineText.HandleEscapeAndHtmlEntity(destination));
            Title = InlineText.HandleEscapeAndHtmlEntity(RemoveQuotes(title ?? string.Empty));
        }

        private string RemoveQuotes(string text)
        {
            if (text.StartsWith("\"") && text.EndsWith("\""))
            {
                return text.Substring(1, text.Length - 2);
            }

            if (text.StartsWith("'") && text.EndsWith("'"))
            {
                return text.Substring(1, text.Length - 2);
            }

            return text;
        }
    }
}
