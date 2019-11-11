using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{
    [TestClass]
    public class DisallowedRawHtmlTest
    {
        private readonly MarkdownParser parser;

        public DisallowedRawHtmlTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_653()
        {
            const string code = "<strong> <title> <style> <em>\n\n<blockquote>\n  <xmp> is disallowed.  <XMP> is also disallowed.\n</blockquote>";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[1].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineHtml, "<strong>"),
                new InlineStructure(InlineElementType.InlineText, " <title> <style> "),
                new InlineStructure(InlineElementType.InlineHtml, "<em>"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            Assert.AreEqual(
                "<blockquote>\r\n  &lt;xmp> is disallowed.  &lt;XMP> is also disallowed.\r\n</blockquote>",
                (doc.Elements[1] as HtmlBlock).Code);
        }
    }
}
