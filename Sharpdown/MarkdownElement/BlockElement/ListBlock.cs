using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class ListBlock : ContainerElementBase
    {
        private static readonly char[] bullets = new[] { '-', '*', '+' };
        private static readonly char[] deliminators = new[] { '.', ')' };
        private static readonly Regex blankItemRegex = new Regex(@"^([\-\*\+]|\d{1,9}[\.\)])[ \t\r\n]*$", RegexOptions.Compiled);
        private static readonly Regex orderdList = new Regex(@"^(?<index>\d{1,9})[\.\)]([ \t].*)??$", RegexOptions.Compiled);

        public override BlockElementType Type => BlockElementType.List;

        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                return false;
            }

            string trimmed = line.TrimStart(whiteSpaceShars);
            return IsBulletList(trimmed) || IsOrderdList(trimmed).isOrderdList;
        }

        private static bool IsBulletList(string line)
        {
            if (line.Length == 0)
            {
                return false;
            }

            if (!bullets.Contains(line[0]))
            {
                return false;
            }

            if (line.Length == 1 || line[1] == ' ')
            {
                return true;
            }
            return false;
        }

        private static (bool isOrderdList, int index) IsOrderdList(string line)
        {
            if (!orderdList.IsMatch(line))
            {
                return (false, -1);
            }
            Match match = orderdList.Match(line);
            string indexStr = match.Groups["index"].Value;
            if (!int.TryParse(indexStr, out int index) || index < 0)
            {
                throw new InvalidBlockFormatException(BlockElementType.List);
            }
            return (true, index);
        }

        internal static bool CanInterruptParagraph(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                return false;
            }
            string trimmed = line.Trim(whiteSpaceShars);
            var (isOrderd, index) = IsOrderdList(trimmed);

            return (IsBulletList(trimmed) || (isOrderd && index == 1)) && !blankItemRegex.IsMatch(trimmed);
        }

        internal override AddLineResult AddLine(string line)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}
