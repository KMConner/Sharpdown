﻿using System.Text;

namespace Sharpdown.MarkdownElement.InlineElement
{
    /// <summary>
    /// Represents inline images in markdown documents.
    /// </summary>
    public class Image : ContainerInlineElement
    {
        /// <summary>
        /// Gets the type of this element.
        /// </summary>
        public override InlineElementType Type => InlineElementType.Image;

        /// <summary>
        /// Gets the description of this image.
        /// </summary>
        public string Alt { get; }

        /// <summary>
        /// Gets the image source.
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Gets the title of this image.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sharpdown.MarkdownElement.InlineElement.Image"/> class.
        /// </summary>
        /// <param name="text">The description.</param>
        /// <param name="src">The source of this image.</param>
        /// <param name="title">The title of this image.</param>
        /// <param name="config">Configuration of the parser.</param>
        public Image(InlineElement[] text, string src, string title, ParserConfig config) : base(config)
        {
            Alt = TextFromInlines(text);
            Children = text;
            Source = src;
            Title = title;
        }

        /// <summary>
        /// Get text from inline elements
        /// </summary>
        /// <param name="inlines">The inline elements to convert from.</param>
        /// <returns></returns>
        private string TextFromInlines(InlineElement[] inlines)
        {
            var builder = new StringBuilder();
            TextFromInlines(inlines, builder);
            return builder.ToString();
        }

        /// <summary>
        /// Get text from inline elements
        /// </summary>
        /// <param name="inlines">Inlines.</param>
        /// <param name="builder">The <see cref="StringBuilder"/>.</param>
        private void TextFromInlines(InlineElement[] inlines, StringBuilder builder)
        {
            if (inlines == null)
            {
                return;
            }

            foreach (var item in inlines)
            {
                switch (item.Type)
                {
                    case InlineElementType.InlineText:
                        builder.Append(((InlineText)item).Content);
                        break;
                    case InlineElementType.LiteralText:
                        builder.Append(((LiteralText)item).Content);
                        break;
                    case InlineElementType.CodeSpan:
                        builder.Append(((CodeSpan)item).Code);
                        break;
                    case InlineElementType.Image:
                        builder.Append(((Image)item).Alt);
                        break;
                    case InlineElementType.Link:
                    case InlineElementType.Emphasis:
                    case InlineElementType.StrongEmphasis:
                        TextFromInlines(((ContainerInlineElement)item).Children, builder);
                        break;
                }
            }
        }
    }
}
