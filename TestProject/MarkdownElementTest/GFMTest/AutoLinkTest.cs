using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class AutoLinkTest
    {
        private readonly MarkdownParser parser;

        public AutoLinkTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_602()
        {
            const string code = "<http://foo.bar.baz>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://foo.bar.baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("http://foo.bar.baz", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_603()
        {
            const string code = "<http://foo.bar.baz/test?q=hello&id=22&boolean>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://foo.bar.baz/test?q=hello&id=22&boolean"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("http://foo.bar.baz/test?q=hello&id=22&boolean", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_604()
        {
            const string code = "<irc://foo.bar:2233/baz>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "irc://foo.bar:2233/baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("irc://foo.bar:2233/baz", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_605()
        {
            const string code = "<MAILTO:FOO@BAR.BAZ>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "MAILTO:FOO@BAR.BAZ"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("MAILTO:FOO@BAR.BAZ", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_606()
        {
            const string code = "<a+b+c:d>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "a+b+c:d"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("a+b+c:d", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_607()
        {
            const string code = "<made-up-scheme://foo,bar>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "made-up-scheme://foo,bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("made-up-scheme://foo,bar", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_608()
        {
            const string code = "<http://../>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://../"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("http://../", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_609()
        {
            const string code = "<localhost:5001/foo>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "localhost:5001/foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("localhost:5001/foo", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_610()
        {
            const string code = "<http://foo.bar/baz bim>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_611()
        {
            const string code = "<http://example.com/\\[\\>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://example.com/\\[\\"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("http://example.com/%5C%5B%5C", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_612()
        {
            const string code = "<foo@bar.example.com>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo@bar.example.com"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("mailto:foo@bar.example.com", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_613()
        {
            const string code = "<foo+special@Bar.baz-bar0.com>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo+special@Bar.baz-bar0.com"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("mailto:foo+special@Bar.baz-bar0.com", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_614()
        {
            const string code = "<foo\\+@bar.example.com>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "<foo+@bar.example.com>");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_615()
        {
            const string code = "<>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_616()
        {
            const string code = "< http://foo.bar >";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_617()
        {
            const string code = "<m:abc>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_618()
        {
            const string code = "<foo.bar.baz>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_619()
        {
            const string code = "http://example.com";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_620()
        {
            const string code = "foo@bar.example.com";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }
    }
}
