using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class ImageTest
    {
        private readonly MarkdownParser parser;

        public ImageTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_580()
        {
            const string code = "![foo](/url \"title\")";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("foo", image.Alt);
            Assert.AreEqual("title", image.Title);
        }

        [TestMethod]
        public void TestCase_581()
        {
            const string code = "![foo *bar*]\n\n[foo *bar*]: train.jpg \"train & tracks\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("train.jpg", image.Source);
            Assert.AreEqual("foo bar", image.Alt);
            Assert.AreEqual("train & tracks", image.Title);
        }

        [TestMethod]
        public void TestCase_582()
        {
            const string code = "![foo ![bar](/url)](/url2)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Image,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url2", image.Source);
            Assert.AreEqual("foo bar", image.Alt);
            Assert.AreEqual(null, image.Title);
        }

        [TestMethod]
        public void TestCase_583()
        {
            const string code = "![foo [bar](/url)](/url2)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url2", image.Source);
            Assert.AreEqual("foo bar", image.Alt);
            Assert.AreEqual(null, image.Title);
        }

        [TestMethod]
        public void TestCase_584()
        {
            const string code = "![foo *bar*][]\n\n[foo *bar*]: train.jpg \"train & tracks\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("train.jpg", image.Source);
            Assert.AreEqual("foo bar", image.Alt);
            Assert.AreEqual("train & tracks", image.Title);
        }

        [TestMethod]
        public void TestCase_585()
        {
            const string code = "![foo *bar*][foobar]\n\n[FOOBAR]: train.jpg \"train & tracks\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("train.jpg", image.Source);
            Assert.AreEqual("foo bar", image.Alt);
            Assert.AreEqual("train & tracks", image.Title);
        }

        [TestMethod]
        public void TestCase_586()
        {
            const string code = "![foo](train.jpg)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("train.jpg", image.Source);
            Assert.AreEqual("foo", image.Alt);
            Assert.AreEqual(null, image.Title);
        }

        [TestMethod]
        public void TestCase_587()
        {
            const string code = "My ![foo bar](/path/to/train.jpg  \"title\"   )";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "My "),
                new InlineStructure(InlineElementType.Image,
                    new InlineStructure(InlineElementType.InlineText, "foo bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(1) as Image;
            Assert.AreEqual("/path/to/train.jpg", image.Source);
            Assert.AreEqual("foo bar", image.Alt);
            Assert.AreEqual("title", image.Title);
        }

        [TestMethod]
        public void TestCase_588()
        {
            const string code = "![foo](<url>)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("url", image.Source);
            Assert.AreEqual("foo", image.Alt);
            Assert.AreEqual(null, image.Title);
        }

        [TestMethod]
        public void TestCase_589()
        {
            const string code = "![](/url)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("", image.Alt);
            Assert.AreEqual(null, image.Title);
            Assert.AreEqual(0, image.Children.Length);
        }

        [TestMethod]
        public void TestCase_590()
        {
            const string code = "![foo][bar]\n\n[bar]: /url";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("foo", image.Alt);
            Assert.AreEqual(null, image.Title);
        }

        [TestMethod]
        public void TestCase_591()
        {
            const string code = "![foo][bar]\n\n[BAR]: /url";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("foo", image.Alt);
            Assert.AreEqual(null, image.Title);
        }

        [TestMethod]
        public void TestCase_592()
        {
            const string code = "![foo][]\n\n[foo]: /url \"title\"\n";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("foo", image.Alt);
            Assert.AreEqual("title", image.Title);
        }

        [TestMethod]
        public void TestCase_593()
        {
            const string code = "![*foo* bar][]\n\n[*foo* bar]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("foo bar", image.Alt);
            Assert.AreEqual("title", image.Title);
        }

        [TestMethod]
        public void TestCase_594()
        {
            const string code = "![Foo][]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "Foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("Foo", image.Alt);
            Assert.AreEqual("title", image.Title);
        }

        [TestMethod]
        public void TestCase_595()
        {
            const string code = "![foo] \n[]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Image,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, ""),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "[]"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("foo", image.Alt);
            Assert.AreEqual("title", image.Title);
        }

        [TestMethod]
        public void TestCase_596()
        {
            const string code = "![foo]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("foo", image.Alt);
            Assert.AreEqual("title", image.Title);
        }

        [TestMethod]
        public void TestCase_597()
        {
            const string code = "![*foo* bar]\n\n[*foo* bar]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("foo bar", image.Alt);
            Assert.AreEqual("title", image.Title);
        }

        [TestMethod]
        public void TestCase_598()
        {
            const string code = "![[foo]]\n\n[[foo]]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "![[foo]]");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "[[foo]]: /url \"title\"");
            inline.AssertEqual(doc.Elements[0].GetInlines());
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_599()
        {
            const string code = "![Foo]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "Foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;
            Assert.AreEqual("/url", image.Source);
            Assert.AreEqual("Foo", image.Alt);
            Assert.AreEqual("title", image.Title);
        }

        [TestMethod]
        public void TestCase_600()
        {
            const string code = "!\\[foo]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "![foo]");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_601()
        {
            const string code = "\\![foo]\n\n[foo]: /url \"title\"";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "!"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(1) as Link;
            Assert.AreEqual("/url", link.Destination);
            Assert.AreEqual("title", link.Title);
        }
    }
}
