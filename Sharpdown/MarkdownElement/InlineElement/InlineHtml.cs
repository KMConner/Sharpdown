using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public class InlineHtml : ContainerInlineElement
    {
        /// <summary>
        /// HTML tag names which are disallowed in GFM.
        /// </summary>
        internal static readonly string[] GfmDisallowedTags =
        {
            "title", "textarea", "style", "xmp", "iframe", "noembed", "noframes", "script", "plaintext",
        };

        public override InlineElementType Type => InlineElementType.InlineHtml;

        public string Content { get; }

        public InlineHtml(string html, ParserConfig config) : base(config)
        {
            Content = html;
        }
    }
}
