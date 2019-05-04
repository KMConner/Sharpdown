using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class ListItemTest
    {
        private readonly MarkdownParser parser;

        public ListItemTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_231()
        {
            const string html = "A paragraph\r\nwith two lines.\r\n\r\n    indented code\r\n\r\n> A block quote.";
            var doc = parser.Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[1].Type);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[2]);


            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("indented code", (doc.Elements[1] as CodeBlock).Code);
            var inline2 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline2.AssertEqual(doc.Elements[2].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_232()
        {
            const string html = "1.  A paragraph\n    with two lines.\n\n        indented code\n\n    > A block quote.";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("indented code", (listItem.GetChild(1) as CodeBlock).Code);
            var inline2 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline2.AssertEqual(listItem.GetChild(2).GetChild(0).GetInlines());
        }


        [TestMethod]
        public void TestCase_233()
        {
            const string html = "- one\r\n\r\n two";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "one");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());
            var inline2 = new InlineStructure(InlineElementType.InlineText, "two");
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_234()
        {
            const string html = "- one\r\n\r\n  two";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "one");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());
            var inline2 = new InlineStructure(InlineElementType.InlineText, "two");
            inline2.AssertEqual(doc.Elements[0].GetChild(0).GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_235()
        {
            const string html = " -    one\r\n\r\n     two";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[1].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "one");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());
            Assert.AreEqual(" two", (doc.Elements[1] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_236()
        {
            const string html = " -    one\r\n\r\n      two";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "one");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());
            var inline2 = new InlineStructure(InlineElementType.InlineText, "two");
            inline2.AssertEqual(doc.Elements[0].GetChild(0).GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_237()
        {
            const string html = "   > > 1.  one\r\n>>\r\n>>     two";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.BlockQuote,
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph),
                            new BlockElementStructure(BlockElementType.Paragraph)))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0).GetChild(0).GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "one");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            var inline2 = new InlineStructure(InlineElementType.InlineText, "two");
            inline2.AssertEqual(listItem.GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_238()
        {
            const string html = ">>- one\r\n>>\r\n  >  > two";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.BlockQuote,
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph))),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0).GetChild(0).GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "one");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            var inline2 = new InlineStructure(InlineElementType.InlineText, "two");
            inline2.AssertEqual(doc.Elements[0].GetChild(0).GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_239()
        {
            const string html = "-one\r\n\r\n2.two";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "-one");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            var inline2 = new InlineStructure(InlineElementType.InlineText, "2.two");
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_240()
        {
            const string html = "- foo\r\n\r\n\r\n  bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            var inline2 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline2.AssertEqual(listItem.GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_241()
        {
            const string html = "1.  foo\r\n\r\n    ```\r\n    bar\r\n    ```\r\n\r\n" +
                                "    baz\r\n\r\n    > bam";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock),
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual("bar", (listItem.GetChild(1) as CodeBlock).Code);
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline2.AssertEqual(listItem.GetChild(2).GetInlines());
            var inline3 = new InlineStructure(InlineElementType.InlineText, "bam");
            inline3.AssertEqual(listItem.GetChild(3).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_242()
        {
            const string html = "- Foo\n\n      bar\n\n\n      baz";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual("bar\r\n\r\n\r\nbaz", (listItem.GetChild(1) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_243()
        {
            const string html = "123456789. ok";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = (ListItem)doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "ok");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual(123456789, (doc.Elements[0] as ListBlock).StartIndex);
        }

        [TestMethod]
        public void TestCase_244()
        {
            const string html = "1234567890. not ok";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, html);
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_245()
        {
            const string html = "0. ok";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = (ListItem)doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "ok");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual(0, (doc.Elements[0] as ListBlock).StartIndex);
        }

        [TestMethod]
        public void TestCase_246()
        {
            const string html = "003. ok";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = (ListItem)doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "ok");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual(3, (doc.Elements[0] as ListBlock).StartIndex);
        }

        [TestMethod]
        public void TestCase_247()
        {
            const string html = "-1. not ok";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, html);
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_248()
        {
            const string html = "- foo\n\n      bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual("bar", (listItem.GetChild(1) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_249()
        {
            const string html = "  10.  foo\n\n           bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual("bar", (listItem.GetChild(1) as CodeBlock).Code);
            Assert.AreEqual(10, (doc.Elements[0] as ListBlock).StartIndex);
        }

        [TestMethod]
        public void TestCase_250()
        {
            const string code = "    indented code\n\nparagraph\n\n    more code";
            var doc = parser.Parse(code);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[2].Type);

            Assert.AreEqual("indented code", (doc.Elements[0] as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "paragraph");
            inline.AssertEqual(doc.Elements[1].GetInlines());
            Assert.AreEqual("more code", (doc.Elements[2] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_251()
        {
            const string code = "1.     indented code\n\n   paragraph\n\n       more code";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.CodeBlock),
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            Assert.AreEqual("indented code", (listItem.GetChild(0) as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "paragraph");
            inline.AssertEqual(listItem.GetChild(1).GetInlines());
            Assert.AreEqual("more code", (listItem.GetChild(2) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_252()
        {
            const string code = "1.      indented code\n\n   paragraph\n\n       more code";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.CodeBlock),
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            Assert.AreEqual(" indented code", (listItem.GetChild(0) as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "paragraph");
            inline.AssertEqual(listItem.GetChild(1).GetInlines());
            Assert.AreEqual("more code", (listItem.GetChild(2) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_253()
        {
            const string code = "   foo\n\nbar";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_254()
        {
            const string code = "-    foo\n\n  bar";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var listItem = doc.Elements[0].GetChild(0);
            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(listItem.GetChild(0).GetInlines());
            var inline2 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_255()
        {
            const string code = "-  foo\n\n   bar";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(listItem.GetChild(0).GetInlines());
            var inline2 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline2.AssertEqual(listItem.GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_256()
        {
            const string code = "-\n  foo\n-\n  ```\n  bar\n  ```\n-\n      baz";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.CodeBlock)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.CodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());
            Assert.AreEqual("bar", (doc.Elements[0].GetChild(1).GetChild(0) as CodeBlock).Code);
            Assert.AreEqual("baz", (doc.Elements[0].GetChild(2).GetChild(0) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_257()
        {
            const string code = "-   \n  foo\n";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_258()
        {
            const string code = "-\n\n  foo\n";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_259()
        {
            const string code = "- foo\n-\n- bar";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());

            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(doc.Elements[0].GetChild(2).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_260()
        {
            const string code = "- foo\n-   \n- bar";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());

            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(doc.Elements[0].GetChild(2).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_261()
        {
            const string code = "1. foo\n2.\n3. bar";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());

            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(doc.Elements[0].GetChild(2).GetChild(0).GetInlines());
        }


        [TestMethod]
        public void TestCase_262()
        {
            const string code = "*";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem));
            blocks.AssertTypeEqual(doc.Elements[0]);
        }

        [TestMethod]
        public void TestCase_263()
        {
            const string code = "\n\nfoo\n*\n\nfoo\n1.\n\n";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "*"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());

            var inline1 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "1."));
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_264()
        {
            const string code =
                " 1.  A paragraph\n     with two lines.\n\n         indented code\n\n     > A block quote.";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("indented code", (listItem.GetChild(1) as CodeBlock).Code);

            var inline1 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline1.AssertEqual(listItem.GetChild(2).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_265()
        {
            const string code =
                "  1.  A paragraph\n      with two lines.\n\n          indented code\n\n      > A block quote.";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("indented code", (listItem.GetChild(1) as CodeBlock).Code);

            var inline1 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline1.AssertEqual(listItem.GetChild(2).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_266()
        {
            const string code =
                "   1.  A paragraph\n       with two lines.\n\n           indented code\n\n       > A block quote.";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("indented code", (listItem.GetChild(1) as CodeBlock).Code);

            var inline1 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline1.AssertEqual(listItem.GetChild(2).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_267()
        {
            const string code =
                "    1.  A paragraph\n        with two lines.\n\n            indented code\n\n        > A block quote.";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);

            Assert.AreEqual(
                "1.  A paragraph\r\n    with two lines.\r\n\r\n        indented code\r\n\r\n    > A block quote.",
                (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_268()
        {
            const string code =
                "  1.  A paragraph\nwith two lines.\n\n          indented code\n\n      > A block quote.";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("indented code", (listItem.GetChild(1) as CodeBlock).Code);

            var inline1 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline1.AssertEqual(listItem.GetChild(2).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_269()
        {
            const string code = "  1.  A paragraph\n    with two lines.";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_270()
        {
            const string code = "> 1. > Blockquote\ncontinued here.";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem,
                        new BlockElementStructure(BlockElementType.BlockQuote,
                            new BlockElementStructure(BlockElementType.Paragraph)))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0).GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Blockquote"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "continued here."));
            inline0.AssertEqual(listItem.GetChild(0).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_271()
        {
            const string code = "> 1. > Blockquote\n> continued here.";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem,
                        new BlockElementStructure(BlockElementType.BlockQuote,
                            new BlockElementStructure(BlockElementType.Paragraph)))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0).GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Blockquote"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "continued here."));
            inline0.AssertEqual(listItem.GetChild(0).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_272()
        {
            const string code = "- foo\n  - bar\n    - baz\n      - boo\n";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph),
                            new BlockElementStructure(BlockElementType.List,
                                new BlockElementStructure(BlockElementType.ListItem,
                                    new BlockElementStructure(BlockElementType.Paragraph),
                                    new BlockElementStructure(BlockElementType.List,
                                        new BlockElementStructure(BlockElementType.ListItem,
                                            new BlockElementStructure(BlockElementType.Paragraph)))))))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var para1 = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(0);
            var para2 = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(0);
            var para3 = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(1)
                .GetChild(0).GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            var inline3 = new InlineStructure(InlineElementType.InlineText, "boo");


            inline0.AssertEqual(para0.GetInlines());
            inline1.AssertEqual(para1.GetInlines());
            inline2.AssertEqual(para2.GetInlines());
            inline3.AssertEqual(para3.GetInlines());
        }

        [TestMethod]
        public void TestCase_273()
        {
            const string code = "- foo\n - bar\n  - baz\n   - boo";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var para1 = doc.Elements[0].GetChild(1).GetChild(0);
            var para2 = doc.Elements[0].GetChild(2).GetChild(0);
            var para3 = doc.Elements[0].GetChild(3).GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            var inline3 = new InlineStructure(InlineElementType.InlineText, "boo");


            inline0.AssertEqual(para0.GetInlines());
            inline1.AssertEqual(para1.GetInlines());
            inline2.AssertEqual(para2.GetInlines());
            inline3.AssertEqual(para3.GetInlines());
        }

        [TestMethod]
        public void TestCase_274()
        {
            const string code = "10) foo\n    - bar";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph)))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            Assert.AreEqual(10, (doc.Elements[0] as ListBlock).StartIndex);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var para1 = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");

            inline0.AssertEqual(para0.GetInlines());
            inline1.AssertEqual(para1.GetInlines());
        }

        [TestMethod]
        public void TestCase_275()
        {
            const string code = "10) foo\n   - bar";
            var doc = parser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            blocks.AssertTypeEqual(doc.Elements[1]);

            Assert.AreEqual(10, (doc.Elements[0] as ListBlock).StartIndex);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var para1 = doc.Elements[1].GetChild(0).GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");

            inline0.AssertEqual(para0.GetInlines());
            inline1.AssertEqual(para1.GetInlines());
        }

        [TestMethod]
        public void TestCase_276()
        {
            const string code = "- - foo";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph)))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0).GetChild(0).GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(para0.GetInlines());
        }

        [TestMethod]
        public void TestCase_277()
        {
            const string code = "1. - 2. foo";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.List,
                                new BlockElementStructure(BlockElementType.ListItem,
                                    new BlockElementStructure(BlockElementType.Paragraph)))))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(para0.GetInlines());
        }

        [TestMethod]
        public void TestCase_278()
        {
            const string code = "- # Foo\n- Bar\n  ---\n  baz";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Heading)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Heading),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var head0 = doc.Elements[0].GetChild(0).GetChild(0) as Heading;
            Assert.AreEqual(1, head0.HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline0.AssertEqual(head0.GetInlines());

            var head1 = doc.Elements[0].GetChild(1).GetChild(0) as Heading;
            Assert.AreEqual(2, head1.HeaderLevel);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "Bar");
            inline1.AssertEqual(head1.GetInlines());

            var para0 = doc.Elements[0].GetChild(1).GetChild(1);
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline2.AssertEqual(para0.GetInlines());
        }
    }
}
