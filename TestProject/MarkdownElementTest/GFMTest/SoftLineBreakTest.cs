using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class SoftLineBreakTest
    {
        private readonly MarkdownParser parser;

        public SoftLineBreakTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_669()
        {
            const string code = "foo\nbaz";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_670()
        {
            const string code = "foo \n baz";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }
    }
}
