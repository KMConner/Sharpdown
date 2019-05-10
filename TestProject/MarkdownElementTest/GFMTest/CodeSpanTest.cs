using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class CodeSpanTest
    {
        private readonly MarkdownParser parser;

        public CodeSpanTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_338()
        {
            const string code = "`foo`";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_339()
        {
            const string code = "`` foo ` bar  ``";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo ` bar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_340()
        {
            const string code = "` `` `";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "``");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        // TODO: uncomment after support Common mark 0.29
        //[TestMethod]
        //public void TestCase_341()
        //{
        //    const string code = "`  ``  `";
        //    var doc = parser.Parse(code);
        //    Assert.AreEqual(1, doc.Elements.Count);
        //    Assert.AreEqual(0, doc.LinkDefinition.Count);
        //    Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

        //    var inline = new InlineStructure(InlineElementType.CodeSpan, " `` ");
        //    inline.AssertEqual(doc.Elements[0].GetInlines());
        //}

        //[TestMethod]
        //public void TestCase_342()
        //{
        //    const string code = "` a`";
        //    var doc = parser.Parse(code);
        //    Assert.AreEqual(1, doc.Elements.Count);
        //    Assert.AreEqual(0, doc.LinkDefinition.Count);
        //    Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

        //    var inline = new InlineStructure(InlineElementType.CodeSpan, " a");
        //    inline.AssertEqual(doc.Elements[0].GetInlines());
        //}

        //[TestMethod]
        //public void TestCase_343()
        //{
        //    const string code = "` b `";
        //    var doc = parser.Parse(code);
        //    Assert.AreEqual(1, doc.Elements.Count);
        //    Assert.AreEqual(0, doc.LinkDefinition.Count);
        //    Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

        //    var inline = new InlineStructure(InlineElementType.CodeSpan, "` b `");
        //    inline.AssertEqual(doc.Elements[0].GetInlines());
        //}

        // TODO: Add test case 344, 345, 346, 347

        [TestMethod]
        public void TestCase_348()
        {
            const string code = "`foo\\`bar`";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, "foo\\"),
                new InlineStructure(InlineElementType.InlineText, "bar`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_349()
        {
            const string code = "``foo`bar``";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo`bar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_350()
        {
            const string code = "` foo `` bar `";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo `` bar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_351()
        {
            const string code = "*foo`*`";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*foo"),
                new InlineStructure(InlineElementType.CodeSpan, "*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_352()
        {
            const string code = "[not a `link](/foo`)\n";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[not a "),
                new InlineStructure(InlineElementType.CodeSpan, "link](/foo"),
                new InlineStructure(InlineElementType.InlineText, ")"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_353()
        {
            const string code = "`<a href=\"`\">`";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, "<a href=\""),
                new InlineStructure(InlineElementType.InlineText, "\">`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_354()
        {
            const string code = "<a href=\"`\">`";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"`\">"),
                new InlineStructure(InlineElementType.InlineText, "`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_355()
        {
            const string code = "`<http://foo.bar.`baz>`";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, "<http://foo.bar."),
                new InlineStructure(InlineElementType.InlineText, "baz>`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_356()
        {
            const string code = "<http://foo.bar.`baz>`";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "http://foo.bar.`baz")),
                new InlineStructure(InlineElementType.InlineText, "`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_357()
        {
            const string code = "```foo``";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_358()
        {
            const string code = "`foo";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_359()
        {
            const string code = "`foo``bar``";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "`foo"),
                new InlineStructure(InlineElementType.CodeSpan, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }
    }
}
