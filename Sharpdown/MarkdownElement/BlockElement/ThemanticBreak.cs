using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class ThemanticBreak
    {
        private static readonly char[] ThemanticBreakChars = new[] { '-', '_', '*' };
        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                return false;
            }
            var shortenLine = line.Remove(" ").Remove("\t").Remove("\x000B").Remove("\x000C");
            return shortenLine.Length >= 3
                && ThemanticBreakChars.Contains(shortenLine[0])
                && shortenLine.All(c => c == shortenLine[0]);
        }
    }
}
