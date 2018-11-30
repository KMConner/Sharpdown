using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents Indented code block in markdown documents.
    /// </summary>
    /// <remarks>
    /// The specific indented code block is following.
    /// 
    /// <![CDATA[
    ///     Foo
    ///     Bar
    /// ]]>
    /// </remarks>
    public class IndentedCodeBlock : CodeBlockBase
    {
        /// <summary>
        /// The lines in this block.
        /// </summary>
        private readonly List<string> contents;

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.IndentedCodeBlock;

        /// <summary>
        /// Gets the info string of this block.
        /// Always <see cref="string.Empty"/>.
        /// </summary>
        public override string InfoString => string.Empty;

        /// <summary>
        /// Gets the content of this block.
        /// </summary>
        public override string Content => string.Join("\r\n", contents);

        /// <summary>
        /// Initializes a new instance of <see cref="IndentedCodeBlock"/>.
        /// </summary>
        internal IndentedCodeBlock() : base()
        {
            contents = new List<string>();
        }

        /// <summary>
        /// Returns whether the specified line can be a start line of <see cref="IndentedCodeBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>The line must be indented with 4 or more spaces.</item>
        /// </list>
        /// </remarks>
        /// <param name="line">Single line string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="FencedCodeBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        public static bool CanStartBlock(string line)
        {
            return line.GetIndentNum() >= 4;
        }

        /// <summary>
        /// Adds a line of string to this <see cref="IndentedCodeBlock"/>.
        /// </summary>
        /// <param name="line">A single line to add to this element.</param>
        /// <returns>
        /// Returns <c>AddLineResult.Consumed</c> except when <paramref name="line"/>
        /// is indented less than 4 spaces.
        /// </returns>
        internal override AddLineResult AddLine(string line, bool lazy)
        {
            if (lazy)
            {
                throw new InvalidBlockFormatException(BlockElementType.IndentedCodeBlock);
            }
            int indent = line.GetIndentNum();
            if (indent >= 0 && indent < 4)
            {
                for (int i = contents.Count - 1; i >= 0; i--)
                {
                    if (contents[i] == string.Empty)
                    {
                        contents.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                }
                return AddLineResult.NeedClose;
            }

            string lineTrimmed = RemoveIndent(line);

            contents.Add(lineTrimmed);
            return AddLineResult.Consumed;
        }

        /// <summary>
        /// Removes indent from the specied string up to four spaces or one tab character.
        /// </summary>
        /// <param name="line"><see cref="string"/> to remove indent.</param>
        /// <returns>String which removed indent.</returns>
        private string RemoveIndent(string line)
        {
            if (line.Length == 0)
            {
                return line;
            }
            if (line[0] == '\t')
            {
                return line.Substring(1);
            }

            int trimLength = 4;
            for (int i = 0; i < 4; i++)
            {
                if (line.Length < i + 1 || line[i] != ' ')
                {
                    trimLength = i;
                    break;
                }
            }
            return line.Substring(trimLength);
        }

        internal override BlockElement Close()
        {
            for (int i = contents.Count - 1; i >= 0; i--)
            {
                if (contents[i] == string.Empty)
                {
                    contents.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
            return base.Close();
        }
    }
}
