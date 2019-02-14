using System;
using System.Collections.Generic;
using System.Text;
using Sharpdown.MarkdownElement.InlineElement;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents link reference definitions in markdown documents.
    /// </summary>
    public class LinkReferenceDefinition : LeafElement
    {
        /// <summary>
        /// Gets the label of the current link.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Gets the link destination of the current link.
        /// </summary>
        public string Destination { get; private set; }

        /// <summary>
        /// Gets the link title of the current link.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.LinkReferenceDefinition;

        public override string Content => Label;

        /// <summary>
        /// Initializes a new instance of <see cref="LinkReferenceDefinition"/>
        /// with link label ,destination, title.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="destination"></param>
        /// <param name="title"></param>
        /// <param name="elem"></param>
        internal LinkReferenceDefinition(string label, string destination, string title, UnknownElement elem)
        {
            Label = GetSimpleName(label?.Trim(whiteSpaceShars) ?? throw new ArgumentNullException(nameof(title)));
            Destination = InlineElementUtils.UrlEncode(InlineText.HandleEscapeAndHtmlEntity(
                destination ?? throw new ArgumentNullException(nameof(destination))));
            Title = title == null ? null : InlineText.HandleEscapeAndHtmlEntity(title);
            warnings.AddRange(elem?.Warnings ?? new List<string>());
        }

        /// <summary>
        /// This method must not be called.
        /// 
        /// This object is created in <see cref="UnknownElement.Close"/> method after
        /// adding all lines to the <see cref="UnknownElement"/>.
        /// Therefore, no lines can be added to this block.
        /// </summary>
        /// <param name="line">A single line to add to this element.</param>
        /// <returns>
        /// Always throws <see cref="InvalidOperationException"/>
        /// </returns>
        /// <exception cref="InvalidOperationException">Always.</exception>
        internal override AddLineResult AddLine(string line, bool lazy, int currentIndent)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// This method must not be called.
        /// 
        /// This object is created in <see cref="UnknownElement.Close"/> method after
        /// adding all lines to the <see cref="UnknownElement"/>.
        /// Therefore, the current object is already closed and calling this method is invalid.
        /// </summary>
        /// <returns>Always throws <see cref="InvalidOperationException"/>.</returns>
        /// <exception cref="InvalidOperationException">Always.</exception>
        internal override BlockElement Close()
        {
            throw new InvalidCastException();
        }

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions)
        {
        }

        private string ProcessBackslashEscape(string unescaped)
        {
            var builder = new StringBuilder(unescaped.Length);
            for (int i = 0; i < unescaped.Length; i++)
            {
                if (i == unescaped.Length - 1)
                {
                    builder.Append(unescaped[i]);
                    continue;
                }

                if (unescaped[i] == '\\' && Array.IndexOf(asciiPunctuationChars, unescaped[i + 1]) >= 0)
                {
                    builder.Append(unescaped[i + 1]);
                    i++;
                    continue;
                }

                builder.Append(unescaped[i]);
            }

            return builder.ToString();
        }

        public static string GetSimpleName(string name)
        {
            var builder = new StringBuilder(name.Length);
            for (int i = 0; i < name.Length; i++)
            {
                if (!char.IsWhiteSpace(name, i))
                {
                    builder.Append(name[i]);
                    continue;
                }

                if (i < name.Length - 1 && !char.IsWhiteSpace(name, i + 1))
                {
                    builder.Append(' ');
                }
            }

            return builder.ToString();
        }
    }
}
