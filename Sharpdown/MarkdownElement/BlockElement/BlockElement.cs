using System.Collections.Generic;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class BlockElement : MarkdownElementBase
    {
        internal abstract AddLineResult AddLine(string line);

        public abstract BlockElementType Type { get; }

        public abstract IReadOnlyList<string> Warnings { get; }

        protected readonly List<string> warnings;

        internal virtual BlockElement Close()
        {
            return this;
        }

        internal BlockElement()
        {
            warnings = new List<string>();
        }

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
