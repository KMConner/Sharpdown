using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class BlankLineTest
    {
        private readonly MarkdownParser parser;

        public BlankLineTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_197()
        {
            const string html = "\r\n\r\n  \r\n\r\naaa\r\n  \r\n\r\n# aaa\r\n\r\n  \r\n\r\n";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "aaa");
            inline.AssertEqual(doc.Elements[0].GetInlines());
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }
    }
}
