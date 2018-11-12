using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.InlineElement
{
    public class InlineElementBase : MarkdownElementBase
    {
        protected static readonly char[] asciiPunctuationChars = new[]
        {
            '!', '"', '#', '$', '%', '&','\'',
            '(', ')', '*', '+', ',', '-', '.',
            '/', ':', ';', '<', '=', '>', '?',
            '@', '[', '\\', ']', '^', '_', '`',
            '{', '|', '}', '~',
        };

    }
}
