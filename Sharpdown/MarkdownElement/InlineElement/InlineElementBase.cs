using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public abstract class InlineElementBase : MarkdownElementBase
    {
        public abstract InlineElementType Type { get; }

        protected internal static readonly char[] asciiPunctuationChars = new[]
        {
            '!', '"', '#', '$', '%', '&','\'',
            '(', ')', '*', '+', ',', '-', '.',
            '/', ':', ';', '<', '=', '>', '?',
            '@', '[', '\\', ']', '^', '_', '`',
            '{', '|', '}', '~',
        };

    }
}
