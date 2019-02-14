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
        private static readonly char[] bullets = {'-', '*', '+'};

        /// <summary>
        /// Regular expression which matches the first line of list item which starts with a
        /// blank line.
        /// </summary>
        private static readonly Regex blankItemRegex = new Regex(
            @"^([\-\*\+]|\d{1,9}[\.\)])[ \t\r\n]*$", RegexOptions.Compiled);

        /// <summary>
        /// Regular expression which matches the first line of ordered list items.
        /// </summary>
        private static readonly Regex orderedList = new Regex(
            @"^(?<index>\d{1,9})(?<delim>[\.\)])(?:(?<spaces>[ \t]+)(?<content>.*))??$",
            RegexOptions.Compiled);

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.List;

        /// <summary>
        /// Gets whether is List is tight.
        /// </summary>
        public bool IsTight { get; private set; }

        internal bool IsLastBlank { get; private set; }

        public int StartIndex => (children.FirstOrDefault() as ListItem)?.Index ?? 0;

        /// <summary>
        /// Returns whether the specified line can be a start line of <see cref="ListBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>
        /// The specified line of string must be the first line of ordered or bullet
        /// list item.
        /// </item>
        /// <item>Open fence must be indented with 0-3 spaces.</item>
        /// </list>
        /// </remarks>
        /// <param name="line">Single line string.</param>
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="ListBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        public static bool CanStartBlock(string line, int currentIndent)
        {
            if (line.GetIndentNum(currentIndent) >= 4)
            {
                return false;
            }

            string trimmed = line.TrimStart(whiteSpaceChars);
            return IsBulletList(trimmed).isBulletList || IsOrderedList(trimmed).isOrderdList;
        }

        /// <summary>
        /// Returns whether the specified line can be the first line of a bullet list item.
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

            if (line.Length == 1 || line[1] == ' ' || line[1] == '\t')
            {
                return (true, line[0]);
            }

            return (false, '\0');
        }

        /// <summary>
        /// Returns whether the specified line can be the first line of a ordered list item.
        /// </summary>
        /// <param name="line">A line of string. (Indent removal is necessary.)</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of ordered list item.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
        private static (bool isOrderdList, int index, char deliminator) IsOrderedList(string line)
        {
            if (!orderedList.IsMatch(line))
            {
                return (false, -1, '\0');
            }

            Match match = orderedList.Match(line);
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
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <returns>
        /// <see cref="ListItem"/> which is created from <paramref name="line"/>.
        /// </returns>
        private ListItem CreateItem(string line, int currentIndent)
        {
            if (line.Length == 0)
            {
                throw new InvalidBlockFormatException(BlockElementType.List);
            }

            Match ordered = orderedList.Match(line);
            if (ordered.Success && int.TryParse(ordered.Groups["index"].Value, out int index))
            {
                int indent = ordered.Groups["spaces"].Success
                    ? ((ordered.Groups["spaces"].Length > 4 ? 1 : ordered.Groups["spaces"].Length) +
                       ordered.Groups["spaces"].Index)
                    : 2;

                return new ListItem
                {
                    Index = index,
                    Deliminator = ordered.Groups["delim"].Value[0],
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

            if (line[1] != ' ' && line[1] != '\t')
            {
                throw new InvalidBlockFormatException(BlockElementType.List);
            }

            var conIn = line.Substring(1).GetIndentNum(currentIndent + 1) + 1;
            if (conIn > 5)
            {
                conIn = 2;
            }

            return new ListItem
            {
                Deliminator = line[0],
                MarkIndent = 0,
                contentIndent = conIn
            };
        }

        /// <summary>
        /// Whether the specified line can interrupt <see cref="Paragraph"/>.
        /// </summary>
        /// <param name="line">A single line of string.</param>
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <returns>
        /// If the line is the start line of bullet list which does not starts with blank
        /// line, 
        /// </returns>
        internal static bool CanInterruptParagraph(string line, int currentIndent)
        {
            if (line.GetIndentNum(currentIndent) >= 4)
            {
                return false;
            }

            string trimmed = line.Trim(whiteSpaceChars);
            var (isOrdered, index, _) = IsOrderedList(trimmed);

            return (IsBulletList(trimmed).isBulletList || (isOrdered && index == 1))
                   && !blankItemRegex.IsMatch(trimmed);
        }

        /// <summary>
        /// Adds a line of string to this <see cref="AtxHeading"/>.
        /// </summary>
        /// <param name="line">A single line to add to this element.</param>
        /// <param name="lazy">Whether <paramref name="line"/> is lazy continuation.</param>
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <returns>
        /// Returns <c>AddLineResult.Consumed | AddLineResult.NeedClose</c>
        /// except when this block need to be interrupted.
        /// </returns>
        internal override AddLineResult AddLine(string line, bool lazy, int currentIndent)
        {
            int lineIndent = line.GetIndentNum(currentIndent);
            string lineTrimmed = line.TrimStartAscii();
            int trimmedIndent = lineIndent + currentIndent;
            if (openElement == null)
            {
                if (lineIndent < 0 || lineIndent >= 4)
                {
                    throw new InvalidBlockFormatException(BlockElementType.List);
                }

                var item = CreateItem(lineTrimmed, trimmedIndent);
                AddChild(item);
                item.contentIndent += lineIndent;
                item.MarkIndent += lineIndent;
                openElement.AddLine(
                    item.contentIndent > line.Length
                        ? string.Empty
                        : SubStringExpandingTabs(line, item.contentIndent, currentIndent), false,
                    currentIndent + item.contentIndent);
                return AddLineResult.Consumed;
            }

            if (!(openElement is ListItem listItem))
            {
                throw new InvalidBlockFormatException(BlockElementType.List);
            }

            if (lineIndent == -1)
            {
                return listItem.AddLine(line, true, currentIndent);
            }

            if (lazy)
            {
                return listItem.AddLine(line, true, currentIndent);
            }

            if (lineIndent >= listItem.contentIndent)
            {
                return listItem.AddLine(SubStringExpandingTabs(line, listItem.contentIndent, currentIndent), false,
                    currentIndent + listItem.contentIndent);
            }

            if (CanStartBlock(lineTrimmed, currentIndent))
            {
                if (ThematicBreak.CanStartBlock(line, currentIndent))
                {
                    return AddLineResult.NeedClose;
                }

                var newItem = CreateItem(lineTrimmed, trimmedIndent);
                if (newItem.Deliminator != listItem.Deliminator)
                {
                    return AddLineResult.NeedClose;
                }

                CloseOpenElement();
                AddChild(newItem);
                newItem.contentIndent += lineIndent;
                newItem.MarkIndent += lineIndent;
                openElement.AddLine(
                    SubStringExpandingTabs(line, Math.Min(newItem.contentIndent, line.Length), currentIndent), false,
                    currentIndent + Math.Min(newItem.contentIndent, line.Length));
                return AddLineResult.Consumed;
            }

            if (!CanLazyContinue())
            {
                return AddLineResult.NeedClose;
            }

            var indentRemoved = RemoveIndent(line, listItem.contentIndent, currentIndent);
            var newBlock = BlockElementUtil.CreateBlockFromLine(indentRemoved,
                currentIndent + Math.Min(listItem.contentIndent, lineIndent));
            if (newBlock.Type != BlockElementType.Unknown)
            {
                return AddLineResult.NeedClose;
            }

            return openElement.AddLine(line, true, currentIndent) & AddLineResult.Consumed;
        }

        /// <summary>
        /// This method is not used in this class.
        /// 
        /// In some other class which inherits <see cref="ContainerElement"/>
        /// uses this method in <see cref="ContainerElement.AddLine"/>.
        /// However <see cref="AddLine"/> does not use this.
        /// </summary>
        /// <param name="line">Ignored.</param>
        /// <param name="currentIndent">The indent count of <paramref name="line"/>.</param>
        /// <param name="markRemoved">Ignored.</param>
        /// <param name="markLength">
        /// The length of the mark of this block is set when this method returns.
        /// </param>
        /// <returns>Always throws <see cref="InvalidOperationException"/>.</returns>
        /// <exception cref="InvalidOperationException">Always.</exception>
        internal override bool HasMark(string line, int currentIndent, out string markRemoved, out int markLength)
        {
            throw new InvalidOperationException();
        }

        internal override BlockElement Close()
        {
            ListBlock ret = (ListBlock)base.Close();
            IsLastBlank = (ret.children.LastOrDefault() as ListItem)?.IsLastBlank == true;
            IsTight = ret.Children.Cast<ListItem>().All(i => i.IsTight)
                      && ret.Children.Reverse().Skip(1).Cast<ListItem>().All(c => !c.IsLastBlank);
            return ret;
        }
    }
}
