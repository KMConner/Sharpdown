using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;


namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class TabsTest
    {
        private readonly MarkdownParser parser;

        public TabsTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_001()
        {
            var doc = parser.Parse("\tfoo\tbaz\t\tbim");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(1, ((CodeBlock)doc.Elements[0]).Inlines.Count);
            var lit = (LiteralText)((CodeBlock)doc.Elements[0]).Inlines[0];
            Assert.AreEqual(InlineElementType.LiteralText, lit.Type);
            Assert.AreEqual("foo\tbaz\t\tbim", lit.Content);
        }

        [TestMethod]
        public void TestCase_002()
        {
            var doc = parser.Parse("  \tfoo\tbaz\t\tbim");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(1, ((CodeBlock)doc.Elements[0]).Inlines.Count);
            var lit = (LiteralText)((CodeBlock)doc.Elements[0]).Inlines[0];
            Assert.AreEqual(InlineElementType.LiteralText, lit.Type);
            Assert.AreEqual("foo\tbaz\t\tbim", lit.Content);
        }

        [TestMethod]
        public void TestCase_003()
        {
            var doc = parser.Parse("    a\ta\n    ὐ\ta");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(1, ((CodeBlock)doc.Elements[0]).Inlines.Count);
            var lit = (LiteralText)((CodeBlock)doc.Elements[0]).Inlines[0];
            Assert.AreEqual(InlineElementType.LiteralText, lit.Type);
            Assert.AreEqual("a\ta\r\nὐ\ta", lit.Content);
        }

        [TestMethod]
        public void TestCase_004()
        {
            var doc = parser.Parse("  - foo\n\n\tbar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var listItem = doc.Elements[0].GetChild(0) as ListItem;
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            inline1.AssertEqual(listItem.GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_005()
        {
            var doc = parser.Parse("- foo\n\n\t\tbar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var listItem = doc.Elements[0].GetChild(0) as ListItem;
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("  bar", (listItem.GetChild(1) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_006()
        {
            var doc = parser.Parse(">\t\tfoo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.CodeBlock));
            blocks.AssertTypeEqual(doc.Elements[0]);

            Assert.AreEqual("  foo", (doc.Elements[0].GetChild(0) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_007()
        {
            var doc = parser.Parse("-\t\tfoo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.CodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0) as ListItem;
            Assert.AreEqual("  foo", (listItem.GetChild(0) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_008()
        {
            var doc = parser.Parse("    foo\n\tbar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo\r\nbar", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_009()
        {
            var doc = parser.Parse(" - foo\n   - bar\n\t - baz");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph),
                            new BlockElementStructure(BlockElementType.List,
                                new BlockElementStructure(BlockElementType.ListItem,
                                    new BlockElementStructure(BlockElementType.Paragraph)))))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem0 = doc.Elements[0].GetChild(0) as ListItem;
            var listItem1 = listItem0.GetChild(1).GetChild(0) as ListItem;
            var listItem2 = listItem1.GetChild(1).GetChild(0) as ListItem;
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline0.AssertEqual(listItem0.GetChild(0).GetInlines());
            inline1.AssertEqual(listItem1.GetChild(0).GetInlines());
            inline2.AssertEqual(listItem2.GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_010()
        {
            var doc = parser.Parse("#\tFoo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);

            Assert.AreEqual("Foo", (doc.Elements[0].GetInline(0) as InlineText).Content);
        }

        [TestMethod]
        public void TestCase_011()
        {
            var doc = parser.Parse("*\t*\t*\t");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
        }
    }
}
