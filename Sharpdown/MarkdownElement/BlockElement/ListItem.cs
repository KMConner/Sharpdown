using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class ListItem : ContainerElementBase
    {
        public override BlockElementType Type => BlockElementType.ListItem;

        private bool initialized;
        //private static readonly char[] bullets = new[] { '-', '*', '+' };
        internal int contentIndent;

        internal int MarkIndent { get; set; }

        public char Deliminator { get; internal set; }

        public int Index { get; internal set; }

        internal ListItem() : base() { }

        //private static (bool isBulletList, char mark) IsBulletList(string line)
        //{
        //    if (line.Length == 0)
        //    {
        //        return (false, '\0');
        //    }

        //    if (!bullets.Contains(line[0]))
        //    {
        //        return (false, '\0');
        //    }

        //    if (line.Length == 1 || line[1] == ' ')
        //    {
        //        return (true, line[0]);
        //    }
        //    return (false, '\0');
        //}
        //private static (bool isOrderdList, int index, char deliminator) IsOrderdList(string line)
        //{
        //    if (!ListBlock.orderdList.IsMatch(line))
        //    {
        //        return (false, -1, '\0');
        //    }
        //    Match match = ListBlock.orderdList.Match(line);
        //    string indexStr = match.Groups["index"].Value;
        //    if (!int.TryParse(indexStr, out int index) || index < 0)
        //    {
        //        throw new InvalidBlockFormatException(BlockElementType.List);
        //    }
        //    return (true, index, match.Groups["delim"].Value[0]);
        //}

        internal override bool HasMark(string line, out string markRemoved)
        {

            //markRemoved = null;
            //if (!initialized)
            //{
            //    initialized = true;
            //    int indent = line.GetIndentNum();
            //    MarkIndent = indent;
            //    string trimmed = line.TrimStartAscii();
            //    if (indent > 4)
            //    {
            //        return false;
            //    }
            //    var (isBullet, bulletChar) = IsBulletList(trimmed);
            //    if (isBullet)
            //    {
            //        contentIndent = line.Length;
            //        for (int i = line.IndexOf(bulletChar) + 1; i < line.Length; i++)
            //        {
            //            char ch = line[i];
            //            if (ch != ' ')
            //            {
            //                contentIndent = i;
            //                break;
            //            }
            //        }
            //        if (contentIndent - line.IndexOf(bulletChar) > 5)
            //        {
            //            contentIndent = line.IndexOf(bulletChar) + 2;
            //        }
            //        else if (line.Substring(line.IndexOf(bulletChar) + 1).TrimStartAscii().Length == 0)
            //        {
            //            contentIndent = line.IndexOf(bulletChar) + 2;
            //        }
            //        markRemoved = line.Length < contentIndent ? string.Empty : line.Substring(contentIndent);

            //        return true;
            //    }
            //    var (isOrdered, _, delimChar) = IsOrderdList(trimmed);
            //    if (isOrdered)
            //    {
            //        contentIndent = line.Length;
            //        for (int i = line.IndexOf(delimChar) + 1; i < line.Length; i++)
            //        {
            //            char ch = line[i];
            //            if (ch != ' ')
            //            {
            //                contentIndent = i;
            //                break;
            //            }
            //        }
            //        if (contentIndent - line.IndexOf(delimChar) > 5)
            //        {
            //            contentIndent = line.IndexOf(delimChar) + 2;
            //        }
            //        else if (line.Substring(line.IndexOf(delimChar) + 1).TrimStartAscii().Length == 0)
            //        {
            //            contentIndent = line.IndexOf(delimChar) + 2;
            //        }
            //        markRemoved = line.Length < contentIndent ? string.Empty : line.Substring(contentIndent);
            //        return true;
            //    }
            //    return false;
            //}
            //else
            //{
            //    markRemoved = RemoveIndent(line, contentIndent);
            //    return line.GetIndentNum() >= contentIndent || line.GetIndentNum() < 0;
            //}

            markRemoved = line;
            return true;
        }
    }
}
