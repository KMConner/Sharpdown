﻿namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Provides methods which commonly used in <see cref="BlockElement"/> and its inheritances.
    /// </summary>
    public class BlockElementUtil
    {
        /// <summary>
        /// Creates a new instance of <see cref="BlockElement"/>.
        /// Its type will be determined with the specified first line.
        /// </summary>
        /// <param name="line">The line which is the first line of the block.</param>
        /// <param name="createListItem">
        /// Creates a <see cref="ListItem"/> instead of <see cref="ListBlock"/> when <c>true</c> is specified.
        /// </param>
        /// <returns>The new element that the type corresponds to <paramref name="line"/>.</returns>
        public static BlockElement CreateBlockFromLine(string line, bool createListItem = false)
        {
            if (IndentedCodeBlock.CanStartBlock(line))
            {
                return new IndentedCodeBlock();
            }

            if (ThemanticBreak.CanStartBlock(line))
            {
                return new ThemanticBreak();
            }

            if (AtxHeaderElement.CanStartBlock(line))
            {
                return new AtxHeaderElement();
            }

            if (FencedCodeBlock.CanStartBlock(line))
            {
                return new FencedCodeBlock();
            }

            if (HtmlBlock.CanStartBlock(line))
            {
                return new HtmlBlock();
            }

            if (BlockQuote.CanStartBlock(line))
            {
                return new BlockQuote();
            }

            if (ListBlock.CanStartBlock(line))
            {
                return createListItem ? (BlockElement)new ListItem() : new ListBlock();
            }

            if (BlankLine.CanStartBlock(line))
            {
                return new BlankLine();
            }

            return new UnknownElement();
        }
    }

}
