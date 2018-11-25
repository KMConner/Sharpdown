﻿using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Initializes a new instance of <see cref="LinkReferenceDefinition"/>
        /// with link label ,destination, title.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="destination"></param>
        /// <param name="title"></param>
        /// <param name="elem"></param>
        internal LinkReferenceDefinition(string label, string destination, string title, UnknownElement elem) : base()
        {
            Label = label ?? throw new ArgumentNullException(nameof(title));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            Title = title ?? string.Empty;
            warnings.AddRange(elem.Warnings);
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
        internal override AddLineResult AddLine(string line)
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

        internal override void ParseInline(IEnumerable<string> linkDefinitions) { }
    }
}
