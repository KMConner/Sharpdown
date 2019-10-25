namespace Sharpdown.MarkdownElement
{
    /// <summary>
    /// Represents markdown elements.
    /// </summary>
    public abstract class MarkdownElementBase
    {
        /// <summary>
        /// Characters which are treated as white space in markdown documents.
        /// </summary>
        internal static readonly char[] whiteSpaceChars =
        {
            ' ', '\t', '\x000B', '\x000C', '\r', '\n'
        };

        /// <summary>
        /// Characters which are treated as line breaks in markdown documents.
        /// </summary>
        internal static readonly char[] lineBreaks = { '\r', '\n' };

        protected internal static readonly char[] asciiPunctuationChars =
        {
            '!', '"', '#', '$', '%', '&', '\'',
            '(', ')', '*', '+', ',', '-', '.',
            '/', ':', ';', '<', '=', '>', '?',
            '@', '[', '\\', ']', '^', '_', '`',
            '{', '|', '}', '~',
        };
    }
}
