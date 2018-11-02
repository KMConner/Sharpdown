using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class BlockElement : MarkdownElementBase
    {
        //public abstract bool CanContinueBlock(string line);

        internal abstract AddLineResult AddLine(string line);

        public abstract BlockElementType Type { get; }

        protected string RemoveIndent(string str, int maxRemoveCount)
        {
            if (maxRemoveCount == 0 || str.Length == 0 || str[0] != ' ')
            {
                return str;
            }
            return RemoveIndent(str.Substring(1), maxRemoveCount - 1);
        }
    }
}
