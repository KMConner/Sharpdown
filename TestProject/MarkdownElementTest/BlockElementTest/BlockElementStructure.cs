using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.BlockElement;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    class BlockElementStructure
    {
        public BlockElementType Content { get; set; }

        public BlockElementStructure[] Children { get; set; }

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
            if (element is ContainerElementBase container)
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
