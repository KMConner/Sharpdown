﻿using System;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents a Setext header in markdown documents.
    /// </summary>
    /// <remarks>
    /// The typical setext heading is following.
    /// 
    /// <![CDATA[
    /// Section
    /// Title
    /// -------
    /// 
    /// Chapter
    /// Title
    /// =======
    /// ]]>
    /// </remarks>
    public class SetextHeader : HeaderElementBase
    {
        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.SetextHeading;

        /// <summary>
        /// Initializes a new instance of <see cref="SetextHeader"/>
        /// with header level.
        /// </summary>
        /// <param name="elem">
        /// The <see cref="UnknownElement"/> object to create this object from.
        /// </param>
        /// <param name="level">The header level.</param>
        internal SetextHeader(UnknownElement elem, int level) : base()
        {
            if (level != 1 && level != 2)
            {
                throw new ArgumentException("level must be 1 or 2.", nameof(level));
            }
            HeaderLevel = level;
            Content = string.Join("\r\n", elem.content);
            warnings.AddRange(elem.Warnings);
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
        internal override BlockElement Close()
        {
            throw new InvalidOperationException();
        }
    }
}
