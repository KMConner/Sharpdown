using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;


namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class IndentedCodeBlockTest
    {
        private readonly MarkdownParser parser;

        public IndentedCodeBlockTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_077()
        {
            var doc = parser.Parse("    a simple\n      indented code block");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("a simple\r\n  indented code block",
                (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_078()
        {
            var doc = parser.Parse("  - foo\n\n    bar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blockStructure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blockStructure.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline0.AssertEqual(
                (((doc.Elements[0] as ListBlock).Children[0] as ListItem).Children[0] as Paragraph).Inlines);
            inline1.AssertEqual(
                (((doc.Elements[0] as ListBlock).Children[0] as ListItem).Children[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_079()
        {
            var doc = parser.Parse("1.  foo\n\n    - bar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blockStructure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph)))));
            blockStructure.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(
                (((doc.Elements[0] as ListBlock).Children[0] as ListItem).Children[0] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_080()
        {
            var doc = parser.Parse("    <a/>\n    *hi*\n\n    - one");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("<a/>\r\n*hi*\r\n\r\n- one", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_081()
        {
            var doc = parser.Parse("    chunk1\n\n    chunk2\n  \n \n \n    chunk3");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("chunk1\r\n\r\nchunk2\r\n\r\n\r\n\r\nchunk3",
                (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_082()
        {
            var doc = parser.Parse("    chunk1\n      \n      chunk2");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("chunk1\r\n  \r\n  chunk2", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_083()
        {
            var doc = parser.Parse("Foo\n    bar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual((doc.Elements[0] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_084()
        {
            var doc = parser.Parse("    foo\nbar");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("foo", (doc.Elements[0] as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "bar");
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_085()
        {
            var doc = parser.Parse("# Heading\n    foo\nHeading\n------\n    foo\n----");
            Assert.AreEqual(5, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[3].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[4].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "Heading");
            inline.AssertEqual((doc.Elements[0] as Heading).Inlines);
            Assert.AreEqual("foo", (doc.Elements[1] as CodeBlock).Code);
            inline.AssertEqual((doc.Elements[2] as Heading).Inlines);
            Assert.AreEqual("foo", (doc.Elements[3] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_086()
        {
            var doc = parser.Parse("        foo\n    bar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("    foo\r\nbar", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_087()
        {
            var doc = parser.Parse("\n    \n    foo\n    ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_088()
        {
            var doc = parser.Parse("    foo  ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo  ", (doc.Elements[0] as CodeBlock).Code);
        }
    }
}
