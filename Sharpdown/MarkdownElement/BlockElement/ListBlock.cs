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
        internal static readonly Regex orderdList = new Regex(@"^(?<index>\d{1,9})(?<delim>[\.\)])([ \t](?<content>.*))??$", RegexOptions.Compiled);

        private int? startIndex;
        private char? mark;

        private int lastIndent;

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

        internal override bool HasMark(string line, out string markRemoved)
        {
            // TODO: tight ????
            markRemoved = null;
            string trimmed = line.TrimStartAscii();

            if (openElement == null)
            {
                if (line.GetIndentNum() > 4)
                {
                    return false;
                }
                var (isOrderd, index, delim) = IsOrderdList(trimmed);
                if (isOrderd)
                {
                    if (!mark.HasValue)
                    {
                        mark = delim;
                        startIndex = index;
                    }
                    else if (mark != delim)
                    {
                        return false;
                    }
                    markRemoved = line;
                    return true;
                }

                var (isBullet, bullet) = IsBulletList(trimmed);
                if (isBullet)
                {
                    if (!mark.HasValue)
                    {
                        mark = bullet;
                    }
                    else if (mark != bullet)
                    {
                        return false;
                    }
                    markRemoved = line;
                    return true;
                }
                return false;
            }
            else
            {
                if (openElement is ListItem container)
                {
                    var bull = IsBulletList(trimmed);
                    var order = IsOrderdList(trimmed);
                    lastIndent += container.MarkIndent;
                    var trimmedLine = RemoveIndent(line, lastIndent);
                    bool ret = container.HasMark(trimmedLine, out var removed)
                        || (bull.isBulletList && bull.mark == mark)
                        || (order.isOrderdList && order.deliminator == mark);

                    markRemoved = ret ? trimmedLine : removed;
                    return ret;
                }
                else
                {
                    throw new InvalidBlockFormatException(BlockElementType.List);
                }
            }
        }
    }
}
