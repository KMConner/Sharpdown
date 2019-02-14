using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.BlockElement;

namespace TestProject.MarkdownElementTest
{
    internal class BlockElementStructure
    {
        private BlockElementType Content { get; }

        private BlockElementStructure[] Children { get; }

        public BlockElementStructure(BlockElementType type, params BlockElementStructure[] structures)
        {
            Content = type;
            Children = structures ?? throw new ArgumentNullException(nameof(structures));
        }

        public static implicit operator BlockElementStructure(BlockElementType type)
        {
            return new BlockElementStructure(type);
        }

        public void AssertTypeEqual(BlockElement element)
        {
            Assert.AreEqual(Content, element.Type);
            if (element is ContainerElement container)
            {
                Assert.AreEqual(Children.Length, container.Children.Count);
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].AssertTypeEqual(container.Children[i]);
                }
            }
        }
    }
}
