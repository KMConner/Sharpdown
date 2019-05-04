using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;


namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class LinkReferenceDefinitionsTest
    {
        private readonly MarkdownParser parser;

        public LinkReferenceDefinitionsTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_161()
        {
            const string html = "[foo]: /url \"title\"\r\n\r\n[foo]";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual("foo", doc.LinkDefinition["foo"].Label);
            Assert.AreEqual("/url", doc.LinkDefinition["foo"].Destination);
            Assert.AreEqual("title", doc.LinkDefinition["foo"].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("/url", (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual("title", (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_162()
        {
            const string html = "   [foo]: \r\n      /url  \r\n           'the title'  \r\n\r\n[foo]";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual("foo", doc.LinkDefinition["foo"].Label);
            Assert.AreEqual("/url", doc.LinkDefinition["foo"].Destination);
            Assert.AreEqual("the title", doc.LinkDefinition["foo"].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("/url", (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual("the title", (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_163()
        {
            const string html = "[Foo*bar\\]]:my_(url) 'title (with parens)'\r\n\r\n[Foo*bar\\]]";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual("Foo*bar\\]", doc.LinkDefinition["Foo*bar\\]"].Label);
            Assert.AreEqual("my_(url)", doc.LinkDefinition["Foo*bar\\]"].Destination);
            Assert.AreEqual("title (with parens)", doc.LinkDefinition["Foo*bar\\]"].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "Foo*bar]"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("my_(url)", (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual("title (with parens)", (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_164()
        {
            const string html = "[Foo bar]:\r\n<my%20url>\r\n'title'\r\n\r\n[Foo bar]";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual("Foo bar", doc.LinkDefinition["Foo bar"].Label);
            Assert.AreEqual("my%20url", doc.LinkDefinition["Foo bar"].Destination);
            Assert.AreEqual("title", doc.LinkDefinition["Foo bar"].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "Foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("my%20url", (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual("title", (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_165()
        {
            const string html = "[foo]: /url '\r\ntitle\r\nline1\r\nline2\r\n'\r\n\r\n[foo]";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "/url";
            var title = "\ntitle\nline1\nline2\n";
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(title, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, label));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual(title, (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_166()
        {
            const string html = "[foo]: /url 'title\r\n\r\nwith blank line'\r\n\r\n[foo]";
            var doc = parser.Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "[foo]: /url 'title");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "with blank line'");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "[foo]");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetInlines());
            inline2.AssertEqual(doc.Elements[2].GetInlines());
        }

        [TestMethod]
        public void TestCase_167()
        {
            const string html = "[foo]:\r\n/url\r\n\r\n[foo]";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "/url";
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, label));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual(null, (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_168()
        {
            const string html = "[foo]:\n\n[foo]";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "[foo]:");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "[foo]");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_169()
        {
            const string html = "[foo]: <>\n\n[foo]";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("", doc.LinkDefinition["foo"].Destination);
        }

        [TestMethod]
        public void TestCase_170()
        {
            const string html = "[foo]: <bar>(baz)\n\n[foo]";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline0 = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo]: "),
                new InlineStructure(InlineElementType.InlineHtml, "<bar>"),
                new InlineStructure(InlineElementType.InlineText, "(baz)"));
            var inline1 = new InlineStructure(InlineElementType.InlineText, "[foo]");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_171()
        {
            const string html = "[foo]: /url\\bar\\*baz \"foo\\\"bar\\baz\"\r\n\r\n[foo]";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "/url%5cbar*baz";
            var title = "foo\"bar\\baz";
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination, true);
            Assert.AreEqual(title, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, label));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[0].GetInline(0) as Link).Destination, true);
            Assert.AreEqual(title, (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_172()
        {
            const string html = "[foo]\r\n\r\n[foo]: url";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "url";
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, label));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual(null, (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_173()
        {
            const string html = "[foo]\r\n\r\n[foo]: first\r\n[foo]: second";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "first";
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, label));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual(null, (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_174()
        {
            const string html = "[FOO]: /url\r\n\r\n[Foo]";
            var doc = parser.Parse(html);
            var label = "Foo";
            var dest = "/url";
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, label));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual(null, (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_175()
        {
            const string html = "[ΑΓΩ]: /φου\r\n\r\n[αγω]";
            var doc = parser.Parse(html);
            var label = "αγω";
            var dest = "/%CF%86%CE%BF%CF%85";
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination, true);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, label));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[0].GetInline(0) as Link).Destination, true);
            Assert.AreEqual(null, (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_176()
        {
            const string html = "[foo]: /url";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "/url";
            Assert.AreEqual(0, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
        }

        [TestMethod]
        public void TestCase_177()
        {
            const string html = "[\r\nfoo\r\n]: /url\r\nbar";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "/url";
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "bar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_178()
        {
            const string html = "[foo]: /url \"title\" ok";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "[foo]: /url \"title\" ok");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_179()
        {
            const string html = "[foo]: /url\r\n\"title\" ok";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "/url";
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "\"title\" ok");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_180()
        {
            const string html = "    [foo]: /url \"title\"\r\n\r\n[foo]";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("[foo]: /url \"title\"", (doc.Elements[0] as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "[foo]");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_181()
        {
            const string html = "```\r\n[foo]: /url\r\n```\r\n\r\n[foo]";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("[foo]: /url", (doc.Elements[0] as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "[foo]");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_182()
        {
            const string html = "Foo\r\n[bar]: /baz\r\n\r\n[bar]";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "[bar]: /baz"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            var inline1 = new InlineStructure(InlineElementType.InlineText, "[bar]");
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_183()
        {
            const string html = "# [Foo]\r\n[foo]: /url\r\n> bar";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "/url";
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.BlockQuote, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].GetChild(0).Type);
            var inline0 = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "Foo"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual(null, (doc.Elements[0].GetInline(0) as Link).Title);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(doc.Elements[1].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_184()
        {
            const string html = "[foo]: /url\nbar\n===\n[foo]\n";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "/url";
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[1].GetInline(0) as Link).Destination);
            Assert.AreEqual(null, (doc.Elements[1].GetInline(0) as Link).Title);
            var inline1 = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_185()
        {
            const string html = "[foo]: /url\n===\n[foo]";
            var doc = parser.Parse(html);
            var label = "foo";
            var dest = "/url";
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline0 = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "==="),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[0].GetInline(2) as Link).Destination);
            Assert.AreEqual(null, (doc.Elements[0].GetInline(2) as Link).Title);
        }

        [TestMethod]
        public void TestCase_186()
        {
            const string html = "[foo]: /foo-url \"foo\"\r\n[bar]: /bar-url\r\n  \"bar\"\r\n" +
                                "[baz]: /baz-url\r\n\r\n[foo],\r\n[bar],\r\n[baz]";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(3, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual("foo", doc.LinkDefinition["foo"].Label);
            Assert.AreEqual("/foo-url", doc.LinkDefinition["foo"].Destination);
            Assert.AreEqual("foo", doc.LinkDefinition["foo"].Title);
            Assert.AreEqual("bar", doc.LinkDefinition["bar"].Label);
            Assert.AreEqual("/bar-url", doc.LinkDefinition["bar"].Destination);
            Assert.AreEqual("bar", doc.LinkDefinition["bar"].Title);
            Assert.AreEqual("baz", doc.LinkDefinition["baz"].Label);
            Assert.AreEqual("/baz-url", doc.LinkDefinition["baz"].Destination);
            Assert.AreEqual(null, doc.LinkDefinition["baz"].Title);

            var inline = new InlineStructure(InlineElementType.CodeSpan,
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, ","),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, ","),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "baz")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_187()
        {
            const string html = "[foo]\r\n\r\n> [foo]: /url";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.BlockQuote, doc.Elements[1].Type);
            Assert.AreEqual("foo", doc.LinkDefinition["foo"].Label);
            Assert.AreEqual("/url", doc.LinkDefinition["foo"].Destination);
            Assert.AreEqual(null, doc.LinkDefinition["foo"].Title);
            var inline0 = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(0, doc.Elements[1].GetChildren().Count);
        }

        [TestMethod]
        public void TestCase_188()
        {
            const string html = "[foo]: /url";
            var doc = parser.Parse(html);
            Assert.AreEqual(0, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual("foo", doc.LinkDefinition["foo"].Label);
            Assert.AreEqual("/url", doc.LinkDefinition["foo"].Destination);
            Assert.AreEqual(null, doc.LinkDefinition["foo"].Title);
        }
    }
}
