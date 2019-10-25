using System.Linq;

namespace Sharpdown.MarkdownElement.InlineElement
{
    /// <summary>
    /// Represents code spans in markdown documents.
    /// </summary>
    public class CodeSpan : InlineElement
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
        /// <param name="config">Configuration of the parser.</param>
        internal CodeSpan(string code, ParserConfig config) : base(config)
        {
            Code = CollapseWhiteSpaces(code);
        }

        private static string CollapseWhiteSpaces(string text)
        {
            var ret = text
                .Replace("\r\n", " ")
                .Replace('\r', ' ')
                .Replace('\n', ' ');
            if (ret.Length > 0 && ret[0] == ' ' && ret[ret.Length - 1] == ' ' && ret.Any(c => c != ' '))
            {
                ret = ret.Substring(1, ret.Length - 2);
            }
            return ret;
        }
    }
}
