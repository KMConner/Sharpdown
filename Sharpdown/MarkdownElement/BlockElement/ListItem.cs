﻿using System.Linq;
using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents list items in markdown documents.
    /// </summary>
    public class ListItem : ContainerElement
    {
        /// <summary>
        /// The index of the first letter in list block.
        /// </summary>
        internal int contentIndent;

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.ListItem;

        /// <summary>
        /// Gets or sets the indent size of the list marker.
        /// </summary>
        internal int MarkIndent { get; set; }

        /// <summary>
        /// Gets or sets the bullet marker of bullet list item or
        /// deliminator character of ordered list items.
        /// </summary>
        internal char Deliminator { get; set; }

        /// <summary>
        /// Gets or sets the index of current list item.
        /// If the current list item is a bullet list item, this value is 0 by default.
        /// </summary>
        internal int Index { get; set; }

        internal ListType ItemType => ListBlock.bullets.Contains(Deliminator)
            ? ListType.Bullet
            : (ListBlock.orderedDelims.Contains(Deliminator) ? ListType.Ordered : ListType.Unknown);

        internal bool IsTight { get; private set; }

        internal bool IsLastBlank { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ListItem"/>
        /// </summary>
        /// <param name="config">Configuration of the parser.</param>
        internal ListItem(ParserConfig config) : base(config)
        {
        }

        /// <summary>
        /// Returns whether the specified line satisfied proper conditions to 
        /// continue (without lazy continuation).
        /// </summary>
        /// <param name="line">The line to continue.</param>
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <param name="markRemoved">
        /// This value is always equivalent to <paramref name="line"/>.
        /// </param>
        /// <param name="markLength">
        /// The length of the mark of this block is set when this method returns.
        /// </param>
        /// <returns>
        /// Always <c>true</c>.
        /// </returns>
        protected override bool HasMark(string line, int currentIndent, out string markRemoved, out int markLength)
        {
            markRemoved = line;
            markLength = 0;
            return true;
        }

        internal override BlockElement Close()
        {
            if (openElement != null)
            {
                children[children.Count - 1] = openElement.Close();
                openElement = null;
            }

            IsLastBlank = children.LastOrDefault(c => c.Type != BlockElementType.LinkReferenceDefinition)?.Type ==
                          BlockElementType.BlankLine
                          || (children.LastOrDefault() as ListBlock)?.IsLastBlank == true;
            IsTight = ((IEnumerable<BlockElement>)children).Reverse()
                      .SkipWhile(c => c.Type == BlockElementType.BlankLine)
                      .All(c => c.Type != BlockElementType.BlankLine)
                      && children.Where(c => c.Type == BlockElementType.List).Cast<ListBlock>()
                          .All(c => !c.IsLastBlank);
            return base.Close();
        }

        internal override AddLineResult AddLine(string line, bool lazy, int currentIndent)
        {
            if (children.Count > 1 && children.All(c => c.Type == BlockElementType.BlankLine))
            {
                return AddLineResult.NeedClose;
            }

            return base.AddLine(line, lazy, currentIndent);
        }
    }
}
