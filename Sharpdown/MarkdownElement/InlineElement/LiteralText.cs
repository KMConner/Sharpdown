﻿namespace Sharpdown.MarkdownElement.InlineElement
{
    public class LiteralText : InlineElementBase
    {
        public string Content { get; private set; }

        internal LiteralText(string text)
        {
            Content = text;
        }
    }
}
