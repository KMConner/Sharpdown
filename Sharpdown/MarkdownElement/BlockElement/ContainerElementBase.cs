using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public abstract class ContainerElementBase : BlockElement
    {
        internal abstract bool HasMark(string line, out string markRemoved);

        protected BlockElement openElement;

        private readonly List<BlockElement> children;

        public IReadOnlyList<BlockElement> Children => children.AsReadOnly();

        public override IReadOnlyList<string> Warnings =>
            new List<string>(children.Select(c => c.Warnings.AsEnumerable())
                .Aggregate((l1, l2) => l1.Union(l2)).Union(warnings))
            .AsReadOnly();

        protected void CloseOpenlement()
        {
            if (openElement != null)
            {
                children[children.Count - 1] = openElement.Close();
                openElement = null;
            }
        }

        protected void AddChild(BlockElement elem)
        {
            if (openElement != null)
            {
                throw new InvalidOperationException("openElement is not null.");
            }
            openElement = elem;
            children.Add(elem);
        }

        protected bool CanLazyContinue()
        {
            if (children.Count == 0)
            {
                return false;
            }
            BlockElementType lastElementType = children.Last().Type;

            if (children.Last() is ContainerElementBase container)
            {
                return container.CanLazyContinue();
            }

            return lastElementType == BlockElementType.Unknown || lastElementType == BlockElementType.Paragraph;
        }

        internal override BlockElement Close()
        {
            if (openElement != null)
            {
                children[children.Count - 1] = openElement.Close();
            }
            for (int i = children.Count - 1; i >= 0; i--)
            {
                if (children[i].Type == BlockElementType.BlankLine)
                {
                    children.RemoveAt(i);
                }
            }
            return this;
        }

        internal ContainerElementBase() : base()
        {
            children = new List<BlockElement>();
        }

        internal override AddLineResult AddLine(string line)
        {
            if (HasMark(line, out string markRemoved))
            {
                AddLineResult addLineResult;
                do
                {
                    if (openElement == null)
                    {
                        openElement = BlockElementUtil.CreateBlockFromLine(markRemoved, Type == BlockElementType.List);
                        children.Add(openElement);
                    }
                    addLineResult = openElement.AddLine(markRemoved);
                    if ((addLineResult & AddLineResult.NeedClose) != 0)
                    {
                        children[children.Count - 1] = openElement.Close();
                        openElement = null;
                    }
                } while ((addLineResult & AddLineResult.Consumed) == 0);
                return addLineResult & AddLineResult.Consumed;
            }

            line = markRemoved ?? line;

            var newElem = BlockElementUtil.CreateBlockFromLine(line, Type == BlockElementType.List);
            if ((newElem.Type != BlockElementType.Unknown && newElem.Type != BlockElementType.BlankLine)
                || !CanLazyContinue())
            {
                return AddLineResult.NeedClose;
            }

            return openElement?.AddLine(line) ?? throw new Exception();
        }
    }
}
