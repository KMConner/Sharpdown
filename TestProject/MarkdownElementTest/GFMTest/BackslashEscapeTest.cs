using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class BackslashEscapeTest
    {
        private readonly MarkdownParser parser;

        public BackslashEscapeTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_308()
        {
            const string code =
                "\\!\\\"\\#\\$\\%\\&\\'\\(\\)\\*\\+\\,\\-\\.\\/\\:\\;\\<\\=\\>\\?\\@\\[\\\\\\]\\^\\_\\`\\{\\|\\}\\~";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_309()
        {
            const string code = "\\\t\\A\\a\\ \\3\\φ\\«";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "\\\t\\A\\a\\ \\3\\φ\\«");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_310()
        {
            const string code = "\\*not emphasized*\n\\<br/> not a tag\n\\[not a link](/foo)\n\\`not code`\n1\\." +
                                " not a list\n\\* not a list\n\\# not a heading\n\\[foo]: /url \"not a reference\"\n\\&ouml; not a character entity";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*not emphasized*"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "<br/> not a tag"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "[not a link](/foo)"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "`not code`"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "1. not a list"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "* not a list"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "# not a heading"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "[foo]: /url \"not a reference\""),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "&ouml; not a character entity"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_311()
        {
            const string code = "\\\\*emphasis*";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "\\"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "emphasis")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_312()
        {
            const string code = "foo\\\nbar\n";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.HardLineBreak),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_313()
        {
            const string code = "`` \\[\\` ``";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "\\[\\`");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_314()
        {
            const string code = "    \\[\\]";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\\[\\]", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_315()
        {
            const string code = "~~~\n\\[\\]\n~~~";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\\[\\]", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_316()
        {
            const string code = "<http://example.com?find=\\*>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://example.com?find=\\*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("http://example.com?find=%5C*", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_317()
        {
            const string code = "<a href=\"/bar\\/)\">";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<a href=\"/bar\\/)\">", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_318()
        {
            const string code = "[foo](/bar\\* \"ti\\*tle\")";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("/bar*", (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual("ti*tle", (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_319()
        {
            const string code = "[foo]\n\n[foo]: /bar\\* \"ti\\*tle\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("/bar*", (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual("ti*tle", (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_320()
        {
            const string code = "``` foo\\+bar\nfoo\n```";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            var block = (CodeBlock)doc.Elements[0];
            Assert.AreEqual("foo", block.Code);
            Assert.AreEqual("foo+bar", block.InfoString);
        }
    }
}
