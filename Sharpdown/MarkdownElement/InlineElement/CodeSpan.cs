namespace Sharpdown.MarkdownElement.InlineElement
{
    /// <summary>
    /// Represents code spans in markdown documents.
    /// </summary>
    public class CodeSpan : InlineElementBase
    {
        /// <summary>
        /// Gets the type of the current element.
        /// </summary>
        public override InlineElementType Type => InlineElementType.CodeSpan;

        /// <summary>
        /// Gets the code of this code span.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="CodeSpan"/> with the specified code.
        /// </summary>
        /// <param name="code"></param>
        public CodeSpan(string code)
        {
            Code = CollapseWhiteSpaces(code);
        }

        private static string CollapseWhiteSpaces(string text)
        {
            var ret = text.Trim(new[] {'\r', '\n', ' '})
                .Replace('\t', ' ')
                .Replace('\r', ' ')
                .Replace('\n', ' ')
                .Replace("  ", " ");
            while (ret.IndexOf("  ", System.StringComparison.Ordinal) >= 0)
            {
                ret = ret.Replace("  ", " ");
            }

            return ret;
        }
    }
}
