using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class LinkTest
    {
        private readonly MarkdownParser parser;

        public LinkTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_493()
        {
            const string code = "[link](/uri \"title\")";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_494()
        {
            const string code = "[link](/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_495()
        {
            const string code = "[link]()";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_496()
        {
            const string code = "[link](<>)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_497()
        {
            const string code = "[link](/my uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_498()
        {
            const string code = "[link](</my uri>)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_499()
        {
            const string code = "[link](foo\nbar)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[link](foo"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "bar)"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_500()
        {
            const string code = "[link](<foo\nbar>)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[link]("),
                new InlineStructure(InlineElementType.InlineHtml, "<foo\r\nbar>"),
                new InlineStructure(InlineElementType.InlineText, ")"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_501()
        {
            const string code = "[a](<b)c>)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "a"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("b)c", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_502()
        {
            const string code = "[link](<foo\\>)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[link](<foo>)");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_503()
        {
            const string code = "[a](<b)c\n[a](<b)c>\n[a](<b>c)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[a](<b)c"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "[a](<b)c>"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "[a]("),
                new InlineStructure(InlineElementType.InlineHtml, "<b>"),
                new InlineStructure(InlineElementType.InlineText, "c)"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_504()
        {
            const string code = "[link](\\(foo\\))";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("(foo)", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_505()
        {
            const string code = "[link](foo(and(bar)))";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("foo(and(bar))", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_506()
        {
            const string code = "[link](foo\\(and\\(bar\\))";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("foo(and(bar)", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_507()
        {
            const string code = "[link](<foo(and(bar)>)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("foo(and(bar)", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_508()
        {
            const string code = "[link](foo\\)\\:)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("foo):", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_509()
        {
            const string code =
                "[link](#fragment)\n\n[link](http://example.com#fragment)\n\n[link](http://example.com?foo=3#frag)";
            var doc = parser.Parse(code);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            inline.AssertEqual(doc.Elements[1].GetInlines());
            inline.AssertEqual(doc.Elements[2].GetInlines());
            var link0 = doc.Elements[0].GetInline(0) as Link;
            var link1 = doc.Elements[1].GetInline(0) as Link;
            var link2 = doc.Elements[2].GetInline(0) as Link;
            Assert.AreEqual("#fragment", link0.Destination);
            Assert.AreEqual(null, link0.Title);
            Assert.AreEqual("http://example.com#fragment", link1.Destination);
            Assert.AreEqual(null, link1.Title);
            Assert.AreEqual("http://example.com?foo=3#frag", link2.Destination);
            Assert.AreEqual(null, link2.Title);
        }

        [TestMethod]
        public void TestCase_510()
        {
            const string code = "[link](foo\\bar)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("foo%5Cbar", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_511()
        {
            const string code = "[link](foo%20b&auml;)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("foo%20b%C3%A4", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_512()
        {
            const string code = "[link](\"title\")";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("%22title%22", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_513()
        {
            const string code = "[link](/url \"title\")\n[link](/url 'title')\n[link](/url (title))";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "link")),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "link")),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "link")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link0 = doc.Elements[0].GetInline(0) as Link;
            var link1 = doc.Elements[0].GetInline(2) as Link;
            var link2 = doc.Elements[0].GetInline(4) as Link;
            Assert.AreEqual("/url", link0.Destination);
            Assert.AreEqual("title", link0.Title);
            Assert.AreEqual("/url", link1.Destination);
            Assert.AreEqual("title", link1.Title);
            Assert.AreEqual("/url", link2.Destination);
            Assert.AreEqual("title", link2.Title);
        }

        [TestMethod]
        public void TestCase_514()
        {
            const string code = "[link](/url \"title \\\"&quot;\")";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url", link.Destination, true);
            Assert.AreEqual("title \"\"", link.Title);
        }

        [TestMethod]
        public void TestCase_515()
        {
            const string code = "[link](/url\xA0\"title\")";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url%C2%A0%22title%22", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_516()
        {
            const string code = "[link](/url \"title \"and\" title\")";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_517()
        {
            const string code = "[link](/url 'title \"and\" title')";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url", link.Destination, true);
            Assert.AreEqual("title \"and\" title", link.Title);
        }

        [TestMethod]
        public void TestCase_518()
        {
            const string code = "[link](   /uri\n  \"title\"  )";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/uri", link.Destination, true);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_519()
        {
            const string code = "[link] (/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_520()
        {
            const string code = "[link [foo [bar]]](/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link [foo [bar]]"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/uri", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_521()
        {
            const string code = "[link] bar](/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_522()
        {
            const string code = "[link [bar](/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[link "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(1) as Link;
            Assert.AreEqual("/uri", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_523()
        {
            const string code = "[link \\[bar](/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link [bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/uri", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_524()
        {
            const string code = "[link *foo **bar** `#`*](/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo "),
                    new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "bar")),
                    new InlineStructure(InlineElementType.InlineText, " "),
                    new InlineStructure(InlineElementType.CodeSpan, "#")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/uri", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_525()
        {
            const string code = "[![moon](moon.jpg)](/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.Image,
                    new InlineStructure(InlineElementType.InlineText, "moon")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/uri", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_526()
        {
            const string code = "[foo [bar](/uri)](/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, "](/uri)"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(1) as Link;
            Assert.AreEqual("/uri", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_527()
        {
            const string code = "[foo *[bar [baz](/uri)](/uri)*](/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "[bar "),
                    new InlineStructure(InlineElementType.Link,
                        new InlineStructure(InlineElementType.InlineText, "baz")),
                    new InlineStructure(InlineElementType.InlineText, "](/uri)")),
                new InlineStructure(InlineElementType.InlineText, "](/uri)"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = (doc.Elements[0].GetInline(1) as Emphasis).Children[1] as Link;

            Assert.AreEqual("/uri", link.Destination, true);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_528()
        {
            const string code = "![[[foo](uri1)](uri2)](uri3)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "["),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "](uri2)"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;

            Assert.AreEqual("uri3", image.Source);
            Assert.AreEqual("[foo](uri2)", image.Alt);
            Assert.AreEqual(null, image.Title);
        }

        [TestMethod]
        public void TestCase_529()
        {
            const string code = "*[foo*](/uri)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo*")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(1) as Link;

            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_530()
        {
            const string code = "[foo *bar](baz*)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo *bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;

            Assert.AreEqual("baz*", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_531()
        {
            const string code = "*foo [bar* baz]";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo [bar")),
                new InlineStructure(InlineElementType.InlineText, " baz]"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_532()
        {
            const string code = "[foo <bar attr=\"](baz)\">";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<bar attr=\"](baz)\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_533()
        {
            const string code = "[foo`](/uri)`";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo"),
                new InlineStructure(InlineElementType.CodeSpan, "](/uri)"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_534()
        {
            const string code = "[foo<http://example.com/?search=](uri)>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "http://example.com/?search=](uri)")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(1) as Link;

            Assert.AreEqual("http://example.com/?search=%5D(uri)", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_535()
        {
            const string code = "[foo][bar]\n\n[bar]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;

            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_536()
        {
            const string code = "[link [foo [bar]]][ref]\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link [foo [bar]]"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;

            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_537()
        {
            const string code = "[link \\[bar][ref]\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link [bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;

            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_538()
        {
            const string code = "[link *foo **bar** `#`*][ref]\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo "),
                    new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "bar")),
                    new InlineStructure(InlineElementType.InlineText, " "),
                    new InlineStructure(InlineElementType.CodeSpan, "#")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;

            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_539()
        {
            const string code = "[![moon](moon.jpg)][ref]\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.Image,
                    new InlineStructure(InlineElementType.InlineText, "moon")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);

            var image = link.Children[0] as Image;
            Assert.AreEqual("moon.jpg", image.Source);
            Assert.AreEqual(null, image.Title);
        }

        [TestMethod]
        public void TestCase_540()
        {
            const string code = "[foo [bar](/uri)][ref]\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, "]"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "ref")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(1) as Link;
            var link2 = doc.Elements[0].GetInline(3) as Link;

            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);

            Assert.AreEqual("/uri", link2.Destination);
            Assert.AreEqual(null, link2.Title);
        }

        [TestMethod]
        public void TestCase_541()
        {
            const string code = "[foo *bar [baz][ref]*][ref]\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar "),
                    new InlineStructure(InlineElementType.Link,
                        new InlineStructure(InlineElementType.InlineText, "baz"))),
                new InlineStructure(InlineElementType.InlineText, "]"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "ref")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = (doc.Elements[0].GetInline(1) as Emphasis).Children[1] as Link;
            var link2 = doc.Elements[0].GetInline(3) as Link;

            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);

            Assert.AreEqual("/uri", link2.Destination);
            Assert.AreEqual(null, link2.Title);
        }

        [TestMethod]
        public void TestCase_542()
        {
            const string code = "*[foo*][ref]\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo*")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(1) as Link;

            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_543()
        {
            const string code = "[foo *bar][ref]\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo *bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;

            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_544()
        {
            const string code = "[foo <bar attr=\"][ref]\">\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<bar attr=\"][ref]\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_545()
        {
            const string code = "[foo`][ref]`\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo"),
                new InlineStructure(InlineElementType.CodeSpan, "][ref]"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_546()
        {
            const string code = "[foo<http://example.com/?search=][ref]>\n\n[ref]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "http://example.com/?search=][ref]")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(1) as Link;

            Assert.AreEqual("http://example.com/?search=%5D%5Bref%5D", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_547()
        {
            const string code = "[foo][BaR]\n\n[bar]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;

            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_548()
        {
            const string code = "[Толпой][Толпой] is a Russian word.\n\n[ТОЛПОЙ]: /url";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "Толпой")),
                new InlineStructure(InlineElementType.InlineText, " is a Russian word."));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;

            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_549()
        {
            const string code = "[Foo\n  bar]: /url\n\n[Baz][Foo bar]";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "Baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;

            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_550()
        {
            const string code = "[foo] [bar]\n\n[bar]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo] "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(1) as Link;

            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_551()
        {
            const string code = "[foo]\n[bar]\n\n[bar]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo]"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(2) as Link;

            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_552()
        {
            const string code = "[foo]: /url1\n\n[foo]: /url2\n\n[bar][foo]";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;

            Assert.AreEqual("/url1", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_553()
        {
            const string code = "[bar][foo\\!]\n\n[foo!]: /url";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[bar][foo!]");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_554()
        {
            const string code = "[foo][ref[]\n\n[ref[]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[foo][ref[]");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "[ref[]: /uri");
            inline.AssertEqual(doc.Elements[0].GetInlines());
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_555()
        {
            const string code = "[foo][ref[bar]]\n\n[ref[bar]]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[foo][ref[bar]]");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "[ref[bar]]: /uri");
            inline.AssertEqual(doc.Elements[0].GetInlines());
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_556()
        {
            const string code = "[[[foo]]]\n\n[[[foo]]]: /url";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[[[foo]]]");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "[[[foo]]]: /url");
            inline.AssertEqual(doc.Elements[0].GetInlines());
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_557()
        {
            const string code = "[foo][ref\\[]\n\n[ref\\[]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_558()
        {
            const string code = "[bar\\\\]: /uri\n\n[bar\\\\]";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "bar\\"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_559()
        {
            const string code = "[]\n\n[]: /uri";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[]");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "[]: /uri");
            inline.AssertEqual(doc.Elements[0].GetInlines());
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_560()
        {
            const string code = "[\n ]\n\n[\n ]: /uri\n";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "["),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "]"));
            var inline2 = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "["),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "]: /uri"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_561()
        {
            const string code = "[foo][]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_562()
        {
            const string code = "[*foo* bar][]\n\n[*foo* bar]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_563()
        {
            const string code = "[Foo][]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "Foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_564()
        {
            const string code = "[foo] \n[]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, ""),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "[]"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_565()
        {
            const string code = "[foo]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_566()
        {
            const string code = "[*foo* bar]\n\n[*foo* bar]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_567()
        {
            const string code = "[[*foo* bar]]\n\n[*foo* bar]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "["),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo")),
                    new InlineStructure(InlineElementType.InlineText, " bar")),
                new InlineStructure(InlineElementType.InlineText, "]"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(1) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_568()
        {
            const string code = "[[bar [foo]\n\n[foo]: /url";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[[bar "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(1) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_569()
        {
            const string code = "[Foo]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "Foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }

        [TestMethod]
        public void TestCase_570()
        {
            const string code = "[foo] bar\n\n[foo]: /url";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_571()
        {
            const string code = "\\[foo]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[foo]");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_572()
        {
            const string code = "[foo*]: /url\n\n*[foo*]";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo*")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(1) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_573()
        {
            const string code = "[foo][bar]\n\n[foo]: /url1\n[bar]: /url2";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(2, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url2", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_574()
        {
            const string code = "[foo][]\n\n[foo]: /url1";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url1", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_575()
        {
            const string code = "[foo]()\n\n[foo]: /url1";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_576()
        {
            const string code = "[foo](not a link)\n\n[foo]: /url1";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "(not a link)"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/url1", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_577()
        {
            const string code = "[foo][bar][baz]\n\n[baz]: /url";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo]"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(1) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_578()
        {
            const string code = "[foo][bar][baz]\n\n[baz]: /url1\n[bar]: /url2";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(2, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "baz")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            var link2 = doc.Elements[0].GetInline(1) as Link;
            Assert.AreEqual("/url2", link.Destination);
            Assert.AreEqual(null, link.Title);
            Assert.AreEqual("/url1", link2.Destination);
            Assert.AreEqual(null, link2.Title);
        }

        [TestMethod]
        public void TestCase_579()
        {
            const string code = "[foo][bar][baz]\n\n[baz]: /url1\n[foo]: /url2";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(2, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo]"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(1) as Link;
            Assert.AreEqual("/url1", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

    }
}
