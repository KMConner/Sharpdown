using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class BlockQuoteTest
    {
        private readonly MarkdownParser parser;

        public BlockQuoteTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_206()
        {
            const string html = "> # Foo\r\n> bar\r\n> baz";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Heading),
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
            inline1.AssertEqual(doc.Elements[0].GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_207()
        {
            const string html = "># Foo\r\n>bar\r\n> baz";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Heading),
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
            inline1.AssertEqual(doc.Elements[0].GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_208()
        {
            const string html = "   > # Foo\r\n   > bar\r\n > baz";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Heading),
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
            inline1.AssertEqual(doc.Elements[0].GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_209()
        {
            const string html = "    > # Foo\r\n    > bar\r\n    > baz";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("> # Foo\r\n> bar\r\n> baz", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_210()
        {
            const string html = "> # Foo\r\n> bar\r\nbaz";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Heading),
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
            inline1.AssertEqual(doc.Elements[0].GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_211()
        {
            const string html = "> bar\r\nbaz\r\n> foo";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "baz"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_212()
        {
            const string html = "> foo\r\n---";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_213()
        {
            const string html = "> - foo\r\n- bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block1 = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.List,
                    new BlockElementStructure(BlockElementType.ListItem,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            block1.AssertTypeEqual(doc.Elements[0]);
            var block2 = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            block2.AssertTypeEqual(doc.Elements[1]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetChild(0).GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetChild(0).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_214()
        {
            const string html = ">     foo\r\n    bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block1 = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.CodeBlock));
            block1.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[1].Type);
            Assert.AreEqual("foo", (doc.Elements[0].GetChild(0) as CodeBlock).Code);
            Assert.AreEqual("bar", (doc.Elements[1] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_215()
        {
            const string html = "> ```\r\nfoo\r\n```";
            var doc = parser.Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block1 = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.CodeBlock));
            block1.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[2].Type);
            Assert.AreEqual("", (doc.Elements[0].GetChild(0) as CodeBlock).Code);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[1].GetInlines());

            Assert.AreEqual("", (doc.Elements[2] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_216()
        {
            const string html = "> foo\r\n    - bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block1 = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            block1.AssertTypeEqual(doc.Elements[0]);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "- bar"));
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_217()
        {
            const string html = ">";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.BlockQuote, doc.Elements[0].Type);
            Assert.AreEqual(0, doc.Elements[0].GetChildren().Count);
        }

        [TestMethod]
        public void TestCase_218()
        {
            const string html = ">\r\n>  \r\n> ";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.BlockQuote, doc.Elements[0].Type);
            Assert.AreEqual(0, doc.Elements[0].GetChildren().Count);
        }

        [TestMethod]
        public void TestCase_219()
        {
            const string html = ">\r\n> foo\r\n>  ";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_220()
        {
            const string html = "> foo\r\n\r\n> bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            blocks.AssertTypeEqual(doc.Elements[1]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_221()
        {
            const string html = "> foo\r\n> bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_222()
        {
            const string html = "> foo\r\n>\r\n> bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph),
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
            inline1.AssertEqual(doc.Elements[0].GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_223()
        {
            const string html = "foo\r\n> bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[1]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_224()
        {
            const string html = "> aaa\r\n***\r\n> bbb";
            var doc = parser.Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            blocks.AssertTypeEqual(doc.Elements[2]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "aaa");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
            inline1.AssertEqual(doc.Elements[2].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_225()
        {
            const string html = "> bar\r\nbaz";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_226()
        {
            const string html = "> bar\r\n\r\nbaz";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
            var inline1 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_227()
        {
            const string html = "> bar\r\n>\r\nbaz";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
            var inline1 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_228()
        {
            const string html = "> > > foo\r\nbar";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.BlockQuote,
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_229()
        {
            const string html = ">>> foo\r\n> bar\r\n>>baz";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.BlockQuote,
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_230()
        {
            const string html = ">     code\r\n\r\n>    not code";
            var doc = parser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks0 = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.CodeBlock));
            blocks0.AssertTypeEqual(doc.Elements[0]);
            var blocks1 = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks1.AssertTypeEqual(doc.Elements[1]);

            Assert.AreEqual("code", (doc.Elements[0].GetChild(0) as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "not code");
            inline.AssertEqual(doc.Elements[1].GetChild(0).GetInlines());
        }
    }
}
