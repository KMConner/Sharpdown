using System;
using System.Collections.Generic;
using Sharpdown.MarkdownElement.InlineElement;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents paragraph blocks in markdown documents.
    /// </summary>
    /// <remarks>
    /// The typical paragraph is following.
    /// 
    /// <![CDATA[
    /// Foobar
    /// baz
    /// ]]>
    /// </remarks>
    public class Paragraph : LeafElement
    {
        /// <summary>
        /// The lines in the current paragraph.
        /// </summary>
        private readonly List<string> contents;

        /// <summary>
        /// Gets the content of this paragraph.
        /// </summary>
        public override string Content => string.Join("\r\n", contents);

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.Paragraph;

        /// <summary>
        /// Initializes a new instance of <see cref="Paragraph"/>.
        /// </summary>
        /// <param name="element">
        /// The <see cref="UnknownElement"/> object to create this object from.
        /// </param>
        internal Paragraph(UnknownElement element) : base()
        {
            contents = element.content;
            warnings.AddRange(element.Warnings);
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
        internal override AddLineResult AddLine(string line, bool lazy)
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
        internal override BlockElement Close()
        {
            throw new InvalidOperationException();
        }

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions)
        {
            inlines.AddRange(InlineElementUtils.ParseInlineElements(Content, linkDefinitions));
        }
    }
}
