using System;
using System.Collections.Generic;
using System.Text;
using Sharpdown.MarkdownElement.BlockElement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    [TestClass]
    public class ListBlockTest
    {
        [TestMethod]
        public void AddLineTest_01()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- bar"));
            BlockElementStructure structure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph));
            structure.AssertTypeEqual(block.Close());
        }

        [TestMethod]
        public void AddLineTest_02()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("1. foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("2. bar"));
            BlockElementStructure structure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph));
            structure.AssertTypeEqual(block.Close());
        }

        [TestMethod]
        public void AddLineTest_03()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- foo"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine("+ bar"));
            BlockElementStructure structure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph));
        }

        [TestMethod]
        public void AddLineTest_04()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("1. foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   1.  bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("       1.  baz"));
            BlockElementStructure structure =
                new BlockElementStructure(BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem,
                        BlockElementType.Paragraph,
                        new BlockElementStructure(BlockElementType.List,
                            new BlockElementStructure(BlockElementType.ListItem,
                                BlockElementType.Paragraph,
                                new BlockElementStructure(BlockElementType.List,
                                    new BlockElementStructure(BlockElementType.ListItem,
                                        BlockElementType.Unknown))))));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_05()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("1. foo"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine("3) bar"));
            BlockElementStructure structure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Unknown));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_06()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  - foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   - bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    - baz"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   - boo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  - foobar"));
            BlockElementStructure structure =
                new BlockElementStructure(BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Unknown));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_07()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("     - bar"));
            Assert.AreEqual(1, block.Children.Count);
            BlockElementStructure structure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    BlockElementType.Paragraph,
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Unknown))));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_08()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  * bar"));
            BlockElementStructure structure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    BlockElementType.Paragraph,
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Unknown))));
        }

        [TestMethod]
        public void AddLineTest_09()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  * bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    + baz"));
            BlockElementStructure structure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    BlockElementType.Paragraph,
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            BlockElementType.Paragraph,
                            new BlockElementStructure(BlockElementType.List,
                                new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Unknown))))));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_10()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  * bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   + baz"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(3, ((ContainerElementBase)block.Children[0]).Children.Count);
        }

        [TestMethod]
        public void AddLineTest_11()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("1. foo"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine(" - bar"));
            Assert.AreEqual(1, block.Children.Count);
        }

        [TestMethod]
        public void AddLineTest_12()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- - foo"));
            BlockElementStructure structure = new BlockElementStructure(
                BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Unknown))));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_13()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("1. - 2. foo"));
            BlockElementStructure structure = new BlockElementStructure(
                BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem,
                        new BlockElementStructure(BlockElementType.List,
                            new BlockElementStructure(BlockElementType.ListItem,
                                new BlockElementStructure(BlockElementType.List,
                                    new BlockElementStructure(BlockElementType.ListItem,
                                        BlockElementType.Unknown))))));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_14()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- # Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- Bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  ---"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  baz"));
            BlockElementStructure structure = new BlockElementStructure(
                BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem,
                        BlockElementType.AtxHeading),
                    new BlockElementStructure(BlockElementType.ListItem,
                        BlockElementType.SetextHeading,
                        BlockElementType.Unknown
                   ));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_15()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("-"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("- "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("-  baz"));
            BlockElementStructure structure = new BlockElementStructure(
                BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem,
                        BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem),
                    new BlockElementStructure(BlockElementType.ListItem),
                    new BlockElementStructure(BlockElementType.ListItem,
                        BlockElementType.Unknown
                   ));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_16()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("1. foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("2."));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("1. "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("334.  baz"));
            BlockElementStructure structure = new BlockElementStructure(
                BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem,
                        BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem),
                    new BlockElementStructure(BlockElementType.ListItem),
                    new BlockElementStructure(BlockElementType.ListItem,
                        BlockElementType.Unknown
                   ));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_17()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  1. foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    2. bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("      3. baz"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   4. boo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  5. foobar"));
            BlockElementStructure structure =
                new BlockElementStructure(BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Unknown));
            structure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_18()
        {
            ListBlock block = TestUtils.CreateInternal<ListBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  1234. foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    2. bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("       - baz"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   4. boo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  5. foobar"));
            BlockElementStructure structure =
                new BlockElementStructure(BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem,
                        BlockElementType.Paragraph,
                        new BlockElementStructure(BlockElementType.List,
                            new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph))),
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.ListItem, BlockElementType.Unknown));
            structure.AssertTypeEqual(block);
        }


    }
}
