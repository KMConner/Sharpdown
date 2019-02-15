namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Provides methods which commonly used in <see cref="BlockElement"/> and its inheritances.
    /// </summary>
    public static class BlockElementUtil
    {
        /// <summary>
        /// Creates a new instance of <see cref="BlockElement"/>.
        /// Its type will be determined with the specified first line.
        /// </summary>
        /// <param name="line">The line which is the first line of the block.</param>
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <param name="config">Configuration of the parser.</param>
        /// <param name="createListItem">
        /// Creates a <see cref="ListItem"/> instead of <see cref="ListBlock"/> when <c>true</c> is specified.
        /// </param>
        /// <returns>The new element that the type corresponds to <paramref name="line"/>.</returns>
        public static BlockElement CreateBlockFromLine(string line, int currentIndent, ParserConfig config,
            bool createListItem = false)
        {
            if (IndentedCodeBlock.CanStartBlock(line, currentIndent))
            {
                return new IndentedCodeBlock(config);
            }

            if (ThematicBreak.CanStartBlock(line, currentIndent))
            {
                return new ThematicBreak(config);
            }

            if (AtxHeading.CanStartBlock(line, currentIndent))
            {
                return new AtxHeading(config);
            }

            if (FencedCodeBlock.CanStartBlock(line, currentIndent))
            {
                return new FencedCodeBlock(config);
            }

            if (HtmlBlock.CanStartBlock(line, currentIndent))
            {
                return new HtmlBlock(config);
            }

            if (BlockQuote.CanStartBlock(line, currentIndent))
            {
                return new BlockQuote(config);
            }

            if (ListBlock.CanStartBlock(line, currentIndent))
            {
                return createListItem ? (BlockElement)new ListItem(config) : new ListBlock(config);
            }

            if (BlankLine.CanStartBlock(line))
            {
                return new BlankLine(config);
            }

            return new UnknownElement(config);
        }
    }
}
