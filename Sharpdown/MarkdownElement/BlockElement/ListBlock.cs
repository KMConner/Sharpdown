using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class ListBlock : ContainerElementBase
    {
        private static readonly char[] bullets = new[] { '-', '*', '+' };
        private static readonly char[] deliminators = new[] { '.', ')' };
        internal static readonly Regex blankItemRegex = new Regex(@"^([\-\*\+]|\d{1,9}[\.\)])[ \t\r\n]*$", RegexOptions.Compiled);
        internal static readonly Regex orderdList = new Regex(@"^(?<index>\d{1,9})(?<delim>[\.\)])(?:(?<spaces>[ \t]+)(?<content>.*))??$", RegexOptions.Compiled);

        //private int? startIndex;
        //private char? mark;

        //private int lastIndent;

        public override BlockElementType Type => BlockElementType.List;

        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                return false;
            }

            string trimmed = line.TrimStart(whiteSpaceShars);
            return IsBulletList(trimmed).isBulletList || IsOrderdList(trimmed).isOrderdList;
        }

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

        private static ListItem CreateItem(string line)
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

        internal override AddLineResult AddLine(string line)
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
                openElement.AddLine(line.Substring(item.contentIndent));
                return AddLineResult.Consumed;
            }
            else if (openElement is ListItem item)
            {
                if (lineIndent == -1 || lineIndent >= item.contentIndent)
                {
                    item.AddLine(line.Substring(item.contentIndent));
                    return AddLineResult.Consumed;
                }

                if (CanStartBlock(lineTrimmed))
                {
                    var newItem = CreateItem(lineTrimmed);
                    if (newItem.Deliminator != item.Deliminator)
                    {
                        return AddLineResult.NeedClose;
                    }
                    CloseOpenlement();
                    AddChild(newItem);
                    newItem.contentIndent += lineIndent;
                    newItem.MarkIndent += lineIndent;
                    openElement.AddLine(line.Substring(Math.Min(newItem.contentIndent, line.Length)));
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

                return openElement.AddLine(line) & AddLineResult.Consumed;
            }
            else
            {
                throw new InvalidBlockFormatException(BlockElementType.List);
            }
        }

        internal override bool HasMark(string line, out string markRemoved)
        {
            throw new InvalidOperationException();
        }
    }
}
