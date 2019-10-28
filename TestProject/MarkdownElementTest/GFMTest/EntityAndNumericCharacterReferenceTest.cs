using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class EntityAndNumericCharacterReferenceTest
    {
        private readonly MarkdownParser parser;

        public EntityAndNumericCharacterReferenceTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_321()
        {
            const string code = "&nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace;" +
                                " &DifferentialD;\n&ClockwiseContourIntegral; &ngE;";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "\x00A0 & © Æ Ď"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "¾ \x210B \x2146"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "∲ ≧̸"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_322()
        {
            const string code = "&#35; &#1234; &#992; &#0;";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "# Ӓ Ϡ \xFFFD");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_323()
        {
            const string code = "&#X22; &#XD06; &#xcab;";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "\" ആ ಫ");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_324()
        {
            const string code = "&nbsp &x; &#; &#x;\n&#987654321;\n&#abcdef0;\n&ThisIsNotDefined; &hi?;";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "&nbsp &x; &#; &#x;"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "&#987654321;"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "&#abcdef0;"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "&ThisIsNotDefined; &hi?;"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_325()
        {
            const string code = "&copy";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "&copy");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_326()
        {
            const string code = "&MadeUpEntity;";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "&MadeUpEntity;");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_327()
        {
            const string code = "<a href=\"&ouml;&ouml;.html\">";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<a href=\"&ouml;&ouml;.html\">", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_328()
        {
            const string code = "[foo](/f&ouml;&ouml; \"f&ouml;&ouml;\")";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("föö", (doc.Elements[0].GetInline(0) as Link).Title);
            Assert.AreEqual("/f%C3%B6%C3%B6", (doc.Elements[0].GetInline(0) as Link).Destination, true);
        }

        [TestMethod]
        public void TestCase_329()
        {
            const string code = "[foo]\n\n[foo]: /f&ouml;&ouml; \"f&ouml;&ouml;\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("föö", (doc.Elements[0].GetInline(0) as Link).Title);
            Assert.AreEqual("/f%C3%B6%C3%B6", (doc.Elements[0].GetInline(0) as Link).Destination, true);
        }

        [TestMethod]
        public void TestCase_330()
        {
            const string code = "``` f&ouml;&ouml;\nfoo\n```";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo", (doc.Elements[0] as CodeBlock).Code);
            Assert.AreEqual("föö", (doc.Elements[0] as CodeBlock).InfoString);
        }

        [TestMethod]
        public void TestCase_331()
        {
            const string code = "`f&ouml;&ouml;`";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "f&ouml;&ouml;");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_332()
        {
            const string code = "    f&ouml;f&ouml;";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("f&ouml;f&ouml;", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_333()
        {
            const string code = "&#42;foo&#42;\n*foo*";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*foo*"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_334()
        {
            const string code = "&#42; foo\n\n* foo";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[1]);
            var inline = new InlineStructure(InlineElementType.InlineText, "* foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var inline2 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline2.AssertEqual(doc.Elements[1].GetChild(0).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_335()
        {
            const string code = "foo&#10;&#10;bar";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "foo\n\nbar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_336()
        {
            const string code = "&#9;foo";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "\tfoo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_337()
        {
            const string code = "[a](url &quot;tit&quot;)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "[a](url \"tit\")");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }
    }
}
