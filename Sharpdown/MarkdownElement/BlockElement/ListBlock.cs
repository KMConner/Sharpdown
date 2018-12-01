using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents lists int markdown documents.
    /// </summary>
    /// <remarks>
    /// The typical list is following.
    /// 
    /// <![CDATA[
    /// - Foo
    /// 
    /// - Bar
    /// 
    /// 1. Baz
    /// 2. Boo
    /// ]]>
    /// </remarks>
    public class ListBlock : ContainerElement
    {
        /// <summary>
        /// Characters which can be used as bullet list markers.
        /// </summary>
        private static readonly char[] bullets = new[] { '-', '*', '+' };

        /// <summary>
        /// Characters which can be used as ordered list deliminators.
        /// </summary>
        private static readonly char[] deliminators = new[] { '.', ')' };

        /// <summary>
        /// Regular expression which matches the first line of list item which starts with a
        /// blank line.
        /// </summary>
        private static readonly Regex blankItemRegex = new Regex(
            @"^([\-\*\+]|\d{1,9}[\.\)])[ \t\r\n]*$", RegexOptions.Compiled);

        /// <summary>
        /// Regular expression which matches the first line of orderd list items.
        /// </summary>
        private static readonly Regex orderdList = new Regex(
            @"^(?<index>\d{1,9})(?<delim>[\.\)])(?:(?<spaces>[ \t]+)(?<content>.*))??$",
            RegexOptions.Compiled);

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.List;

        /// <summary>
        /// Gets wether is List is tight.
        /// </summary>
        public bool IsTight { get; private set; }

        public override string Content => throw new NotImplementedException();

        /// <summary>
        /// Returns whether the specified line can be a start line of <see cref="ListBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>
        /// The specified line of string must be the first line of orderd or bullet
        /// list item.
        /// </item>
        /// <item>Open fence must be indented with 0-3 speces.</item>
        /// </list>
        /// </remarks>
        /// <param name="line">Single line string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="ListBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                return false;
            }

            string trimmed = line.TrimStart(whiteSpaceShars);
            return IsBulletList(trimmed).isBulletList || IsOrderdList(trimmed).isOrderdList;
        }

        /// <summary>
        /// Retuens wether the specified line can be athe first line of a bullet list item.
        /// </summary>
        /// <param name="line">A line of string. (Indent removal is necessary.)</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of bullet list item.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        private static (bool isBulletList, char mark) IsBulletList(string line)
        {
            if (line.Length == 0)
            {
                return (false, '\0');
            }

            if (!bullets.Contains(line[0]))
            {
                return (false, '\0');
            }

            if (line.Length == 1 || line[1] == ' ')
            {
                return (true, line[0]);
            }
            return (false, '\0');
        }

        /// <summary>
        /// Retuens wether the specified line can be athe first line of a ordered list item.
        /// </summary>
        /// <param name="line">A line of string. (Indent removal is necessary.)</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of ordered list item.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        private static (bool isOrderdList, int index, char deliminator) IsOrderdList(string line)
        {
            if (!orderdList.IsMatch(line))
            {
                return (false, -1, '\0');
            }
            Match match = orderdList.Match(line);
            string indexStr = match.Groups["index"].Value;
            if (!int.TryParse(indexStr, out int index) || index < 0)
            {
                throw new InvalidBlockFormatException(BlockElementType.List);
            }
            return (true, index, match.Groups["delim"].Value[0]);
        }

        /// <summary>
        /// Creates a new <see cref="ListItem"/> from the specified line.
        /// </summary>
        /// <param name="line">A single line of string.</param>
        /// <returns>
        /// <see cref="ListItem"/> which is created from <paramref name="line"/>.
        /// </returns>
        private ListItem CreateItem(string line)
        {
            if (line.Length == 0)
            {
                throw new InvalidBlockFormatException(BlockElementType.List);
            }
            Match orderd = orderdList.Match(line);
            if (orderd.Success && int.TryParse(orderd.Groups["index"].Value, out int index))
            {
                int indent = orderd.Groups["spaces"].Success ? (orderd.Groups["spaces"].Length > 4 ? 2 : orderd.Groups["spaces"].Length) + orderd.Groups["spaces"].Index : 2;
                int delimIndex = orderd.Groups["delim"].Index;

                return new ListItem
                {
                    Index = index,
                    Deliminator = orderd.Groups["delim"].Value[0],
                    contentIndent = indent,
                    MarkIndent = 0,
                };
            }

            if (!bullets.Contains(line[0]))
            {
                throw new InvalidBlockFormatException(BlockElementType.List);
            }

            if (line.Length == 1 || line.Substring(1).TrimStartAscii().Length == 0)
            {
                return new ListItem
                {
                    Deliminator = line[0],
                    contentIndent = 2,
                    MarkIndent = 0,
                };
            }

            if (line[1] != ' ')
            {
                throw new InvalidBlockFormatException(BlockElementType.List);
            }

            for (int i = 1; i < line.Length; i++)
            {
                if (line[i] != ' ' && line[i] != '\t')
                {
                    return new ListItem
                    {
                        Deliminator = line[0],
                        MarkIndent = 0,
                        contentIndent = i,
                    };
                }
            }
            throw new InvalidBlockFormatException(BlockElementType.List);
        }

        /// <summary>
        /// Wether the specified line can interrupt <see cref="Paragraph"/>.
        /// </summary>
        /// <param name="line">A single line of string.</param>
        /// <returns>
        /// If the line is the start line of bullet list which does not starts with blank
        /// line, 
        /// </returns>
        internal static bool CanInterruptParagraph(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                return false;
            }
            string trimmed = line.Trim(whiteSpaceShars);
            var (isOrderd, index, _) = IsOrderdList(trimmed);

            return (IsBulletList(trimmed).isBulletList || (isOrderd && index == 1))
                && !blankItemRegex.IsMatch(trimmed);
        }

        /// <summary>
        /// Adds a line of string to this <see cref="AtxHeaderElement"/>.
        /// </summary>
        /// <param name="line">A single line to add to this element.</param>
        /// <returns>
        /// Returns <c>AddLineResult.Consumed | AddLineResult.NeedClose</c>
        /// except when this block need to be insterrupted.
        /// </returns>
        internal override AddLineResult AddLine(string line, bool lazy)
        {
            int lineIndent = line.GetIndentNum();
            string lineTrimmed = line.TrimStartAscii();
            if (openElement == null)
            {
                if (lineIndent < 0 || lineIndent >= 4)
                {
                    throw new InvalidBlockFormatException(BlockElementType.List);
                }
                var item = CreateItem(lineTrimmed);
                AddChild(item);
                item.contentIndent += lineIndent;
                item.MarkIndent += lineIndent;
                openElement.AddLine(line.Substring(item.contentIndent), false);
                return AddLineResult.Consumed;
            }
            else if (openElement is ListItem item)
            {
                if (lineIndent == -1)
                {
                    item.AddLine(line, lazy);
                    return AddLineResult.Consumed;
                }
                if (lineIndent >= item.contentIndent)
                {
                    item.AddLine(line.Substring(item.contentIndent), false);
                    return AddLineResult.Consumed;
                }

                if (CanStartBlock(lineTrimmed))
                {
                    if (ThemanticBreak.CanStartBlock(line))
                    {
                        return AddLineResult.NeedClose;
                    }
                    var newItem = CreateItem(lineTrimmed);
                    if (newItem.Deliminator != item.Deliminator)
                    {
                        return AddLineResult.NeedClose;
                    }
                    CloseOpenlement();
                    AddChild(newItem);
                    newItem.contentIndent += lineIndent;
                    newItem.MarkIndent += lineIndent;
                    openElement.AddLine(line.Substring(Math.Min(newItem.contentIndent, line.Length)), false);
                    return AddLineResult.Consumed;
                }

                if (!CanLazyContinue())
                {
                    return AddLineResult.Consumed;
                }

                var newBlock = BlockElementUtil.CreateBlockFromLine(line);
                if (newBlock.Type != BlockElementType.Unknown)
                {
                    return AddLineResult.NeedClose;
                }

                return openElement.AddLine(line, true) & AddLineResult.Consumed;
            }
            else
            {
                throw new InvalidBlockFormatException(BlockElementType.List);
            }
        }

        /// <summary>
        /// This method is not used in this class.
        /// 
        /// In some other class which inherites <see cref="ContainerElement"/>
        /// uses this method in <see cref="ContainerElement.AddLine(string)"/>.
        /// However <see cref="ListBlock.AddLine(string)"/> does not use this.
        /// </summary>
        /// <param name="line">Ignored.</param>
        /// <param name="markRemoved">Ignored.</param>
        /// <returns>Always throws <see cref="InvalidOperationException"/>.</returns>
        /// <exception cref="InvalidOperationException">Always.</exception>
        internal override bool HasMark(string line, out string markRemoved)
        {
            throw new InvalidOperationException();
        }

        internal override BlockElement Close()
        {
            ListBlock ret = (ListBlock)base.Close();
            IsTight = ret.Children.Cast<ListItem>().All(i => i.IsTight);
            return ret;
        }
    }
}
