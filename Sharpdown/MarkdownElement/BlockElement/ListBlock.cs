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
        private static readonly Regex orderdList = new Regex(@"^\d{1,9}[\.\)]([ \t].*)??$", RegexOptions.Compiled);

        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                return false;
            }

            string trimmed = line.TrimStart(whiteSpaceShars);
            return IsBulletList(trimmed) || IsOrderdList(trimmed);
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

        private static bool IsOrderdList(string line)
        {
            return orderdList.IsMatch(line);
        }
    }
}
