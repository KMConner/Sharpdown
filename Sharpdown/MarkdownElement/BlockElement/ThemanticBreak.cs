﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sharpdown.MarkdownElement.BlockElement
{
    class ThemanticBreak : LeafElementBase
    {
        private static readonly char[] ThemanticBreakChars = new[] { '-', '_', '*' };

        public override BlockElementType Type => BlockElementType.ThemanticBreak;

        internal ThemanticBreak() : base() { }

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

        internal override AddLineResult AddLine(string line)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        internal override BlockElement Close()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}
