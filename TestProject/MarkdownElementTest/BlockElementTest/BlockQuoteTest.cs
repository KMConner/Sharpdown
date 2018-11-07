using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.BlockElement;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    [TestClass]
    public class BlockQuoteTest
    {
        [TestMethod]
        public void AddLineTest_01()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> foo"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[0].Type);
            var para = (Paragraph)closed.Children[0];
            Assert.AreEqual("foo", para.Content);
        }

        [TestMethod]
        public void AddLineTest_02()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(">foo"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[0].Type);
            var para = (Paragraph)closed.Children[0];
            Assert.AreEqual("foo", para.Content);
        }

        [TestMethod]
        public void AddLineTest_03()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   >   foo"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[0].Type);
            var para = (Paragraph)closed.Children[0];
            Assert.AreEqual("foo", para.Content);
        }

        [TestMethod]
        public void AddLineTest_04()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Baz"));
            Assert.AreEqual(3, block.Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph, block.Children[0].Type);
            Assert.AreEqual(BlockElementType.BlankLine, block.Children[1].Type);
            Assert.AreEqual(BlockElementType.Unknown, block.Children[2].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[1].Type);
            var para1 = (Paragraph)closed.Children[0];
            var para2 = (Paragraph)closed.Children[1];
            Assert.AreEqual("Foo\r\nBar", para1.Content);
            Assert.AreEqual("Baz", para2.Content);
        }

        [TestMethod]
        public void AddLineTest_05()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> # Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Baz"));
            Assert.AreEqual(2, block.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, block.Children[1].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.AtxHeading, closed.Children[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[1].Type);
            var head = (AtxHeaderElement)closed.Children[0];
            Assert.AreEqual("Foo", head.Content);
            Assert.AreEqual(1, head.HeaderLevel);
            var para = (Paragraph)closed.Children[1];
            Assert.AreEqual("Bar\r\nBaz", para.Content);
        }

        [TestMethod]
        public void AddLineTest_06()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> # Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("Baz"));
            Assert.AreEqual(2, block.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, block.Children[1].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.AtxHeading, closed.Children[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[1].Type);
            var head = (AtxHeaderElement)closed.Children[0];
            Assert.AreEqual("Foo", head.Content);
            Assert.AreEqual(1, head.HeaderLevel);
            var para = (Paragraph)closed.Children[1];
            Assert.AreEqual("Bar\r\nBaz", para.Content);
        }

        [TestMethod]
        public void AddLineTest_08()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("Baz"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[0].Type);
            var para = (Paragraph)closed.Children[0];
            Assert.AreEqual("Foo\r\nBar\r\nBaz", para.Content);
        }

        [TestMethod]
        public void AddLineTest_09()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("Bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Baz"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[0].Type);
            var para = (Paragraph)closed.Children[0];
            Assert.AreEqual("Foo\r\nBar\r\nBaz", para.Content);
        }

        [TestMethod]
        public void AddLineTest_10()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("Bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("Baz"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[0].Type);
            var para = (Paragraph)closed.Children[0];
            Assert.AreEqual("Foo\r\nBar\r\nBaz", para.Content);
        }

        [TestMethod]
        public void AddLineTest_11()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("Bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> ----"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.SetextHeading, closed.Children[0].Type);
            var para = (SetextHeader)closed.Children[0];
            Assert.AreEqual("Foo\r\nBar", para.Content);
        }

        [TestMethod]
        public void AddLineTest_12()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("Bar"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine("----"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.Paragraph, closed.Children[0].Type);
            var para = (Paragraph)closed.Children[0];
            Assert.AreEqual("Foo\r\nBar", para.Content);
        }

        [TestMethod]
        public void AddLineTest_13()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(">     Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(">      Bar"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, closed.Children[0].Type);
            var child1 = (IndentedCodeBlock)closed.Children[0];
            Assert.AreEqual("Foo\r\n Bar", child1.Content);
        }

        [TestMethod]
        public void AddLineTest_14()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(">     Foo"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine("     Bar"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, closed.Children[0].Type);
            var child1 = (IndentedCodeBlock)closed.Children[0];
            Assert.AreEqual("Foo", child1.Content);
        }

        [TestMethod]
        public void AddLineTest_15()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> ```java"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> bar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> ```"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.FencedCodeBlock, closed.Children[0].Type);
            var child1 = (FencedCodeBlock)closed.Children[0];
            Assert.AreEqual("java", child1.InfoString);
            Assert.AreEqual("foo\r\nbar", child1.Content);
        }

        [TestMethod]
        public void AddLineTest_16()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> ```java"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine("foo"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(BlockElementType.FencedCodeBlock, closed.Children[0].Type);
            var child1 = (FencedCodeBlock)closed.Children[0];
            Assert.AreEqual("java", child1.InfoString);
            Assert.AreEqual(string.Empty, child1.Content);
            Assert.AreNotEqual(0, child1.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_17()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(">"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(0, closed.Children.Count);
        }

        [TestMethod]
        public void AddLineTest_18()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(">   "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(">  "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> "));
            Assert.AreEqual(3, block.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, block.Children[0].Type);
            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(0, closed.Children.Count);
        }

        [TestMethod]
        public void AddLineTest_19()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(">   > Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> > > Bar "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> Baz"));
            Assert.AreEqual(1, block.Children.Count);
            Assert.AreEqual(BlockElementType.BlockQuote, block.Children[0].Type);
            Assert.AreEqual(2, ((ContainerElementBase)block.Children[0]).Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph,
                ((ContainerElementBase)block.Children[0]).Children[0].Type);
            Assert.AreEqual(BlockElementType.BlockQuote,
                ((ContainerElementBase)block.Children[0]).Children[1].Type);

            var closed = (ContainerElementBase)block.Close();
            Assert.AreEqual(1, closed.Children.Count);
            Assert.AreEqual(BlockElementType.BlockQuote, closed.Children[0].Type);
            Assert.AreEqual(2, ((ContainerElementBase)closed.Children[0]).Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph,
                ((ContainerElementBase)closed.Children[0]).Children[0].Type);
            Assert.AreEqual("Foo",
                ((Paragraph)((ContainerElementBase)closed.Children[0]).Children[0]).Content);
            Assert.AreEqual(BlockElementType.BlockQuote,
                ((ContainerElementBase)closed.Children[0]).Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_20()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> - foo"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine("- bar"));
            BlockElementStructure elementStructure =
                new BlockElementStructure(BlockElementType.BlockQuote,
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            BlockElementType.Unknown)));
            elementStructure.AssertTypeEqual(block);
        }

        [TestMethod]
        public void AddLineTest_21()
        {
            BlockQuote block = TestUtils.CreateInternal<BlockQuote>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("> - foo"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine("    - bar"));
            BlockElementStructure elementStructure =
                new BlockElementStructure(BlockElementType.BlockQuote,
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            BlockElementType.Unknown)));
            elementStructure.AssertTypeEqual(block);
        }

    }
}
