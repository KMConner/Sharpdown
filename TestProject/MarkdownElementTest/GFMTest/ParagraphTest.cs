using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;


namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class ParagraphTest
    {
        private readonly MarkdownParser parser;

        public ParagraphTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_189()
        {
            const string html = "aaa\r\n\r\nbbb";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "aaa");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_190()
        {
            const string html = "aaa\r\nbbb\r\n\r\nccc\r\nddd";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "aaa"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bbb"));
            var inline1 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "ccc"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "ddd"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_191()
        {
            const string html = "aaa\r\n\r\n\r\nbbb";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "aaa");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_192()
        {
            const string html = "  aaa\r\n bbb";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "aaa"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bbb"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_193()
        {
            const string html = "aaa\r\n             bbb\r\n                                       ccc";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "aaa"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bbb"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "ccc"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_194()
        {
            const string html = "   aaa\r\nbbb";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "aaa"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bbb"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_195()
        {
            const string html = "    aaa\r\nbbb";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("aaa", (doc.Elements[0] as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_196()
        {
            const string html = "aaa     \r\nbbb     ";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "aaa"),
                new InlineStructure(InlineElementType.HardLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bbb"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }
    }
}
