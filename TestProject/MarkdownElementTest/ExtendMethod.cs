using System;
using System.Collections.Generic;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest
{
    static class ExtendMethod
    {
        public static IReadOnlyList<InlineElementBase> GetInlines(this BlockElement element)
        {
            if (!(element is LeafElement leaf))
            {
                throw new Exception("This block is a container block.");
            }

            return leaf.Inlines;
        }

        public static InlineElementBase GetInline(this BlockElement element, int index)
        {
            return element.GetInlines()[index];
        }

        public static IReadOnlyList<BlockElement> GetChildren(this BlockElement element)
        {
            if (!(element is ContainerElement container))
            {
                throw new Exception("This block is not a container block.");
            }

            return container.Children;
        }

        public static BlockElement GetChild(this BlockElement element, int index)
        {
            return element.GetChildren()[index];
        }
    }
}
