using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;


namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class SetextHeadingTest
    {
        private readonly MarkdownParser parser;

        public SetextHeadingTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_050()
        {
            var doc = parser.Parse("Foo *bar*\n=========\n\nFoo *bar*\n---------");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(1, ((Heading)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(2, ((Heading)doc.Elements[1]).HeaderLevel);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(((Heading)doc.Elements[0]).Inlines);
            inline.AssertEqual(((Heading)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_051()
        {
            var doc = parser.Parse("Foo *bar\nbaz*\n====");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(1, ((Heading)doc.Elements[0]).HeaderLevel);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar"),
                    new InlineStructure(InlineElementType.SoftLineBreak, ""),
                    new InlineStructure(InlineElementType.InlineText, "baz")));
            inline.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_052()
        {
            var doc = parser.Parse("  Foo *bar\nbaz*\t\n====");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(1, ((Heading)doc.Elements[0]).HeaderLevel);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar"),
                    new InlineStructure(InlineElementType.SoftLineBreak, ""),
                    new InlineStructure(InlineElementType.InlineText, "baz")));
            inline.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_053()
        {
            var doc = parser.Parse("Foo\n-------------------------\n\nFoo\n=");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(1, ((Heading)doc.Elements[1]).HeaderLevel);
            var inline = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline.AssertEqual(((Heading)doc.Elements[0]).Inlines);
            inline.AssertEqual(((Heading)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_054()
        {
            var doc = parser.Parse("   Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[2].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(2, ((Heading)doc.Elements[1]).HeaderLevel);
            Assert.AreEqual(1, ((Heading)doc.Elements[2]).HeaderLevel);
            var inline = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline.AssertEqual(((Heading)doc.Elements[0]).Inlines);
            inline.AssertEqual(((Heading)doc.Elements[1]).Inlines);
            inline.AssertEqual(((Heading)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_055()
        {
            var doc = parser.Parse("    Foo\n    ---\n\n    Foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual("Foo\r\n---\r\n\r\nFoo", ((CodeBlock)doc.Elements[0]).Code);
        }

        [TestMethod]
        public void TestCase_056()
        {
            var doc = parser.Parse("Foo\n   ----      ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_057()
        {
            var doc = parser.Parse("Foo\n    ---");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inlines = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "---"));
            inlines.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_058()
        {
            var doc = parser.Parse("Foo\n= =\n\nFoo\n--- -");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[2].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "= ="));
            var inline1 = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
            inline1.AssertEqual(((Paragraph)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_059()
        {
            var doc = parser.Parse("Foo  \n-----");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_060()
        {
            var doc = parser.Parse("Foo\\\n-----");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo\\");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_061()
        {
            var doc = parser.Parse("`Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>");
            Assert.AreEqual(4, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[3].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(2, ((Heading)doc.Elements[2]).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "`Foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "`");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "<a title=\"a lot");
            var inline3 = new InlineStructure(InlineElementType.InlineText, "of dashes\"/>");
            inline0.AssertEqual(((Heading)doc.Elements[0]).Inlines);
            inline1.AssertEqual(((Paragraph)doc.Elements[1]).Inlines);
            inline2.AssertEqual(((Heading)doc.Elements[2]).Inlines);
            inline3.AssertEqual(((Paragraph)doc.Elements[3]).Inlines);
        }

        [TestMethod]
        public void TestCase_062()
        {
            var doc = parser.Parse("> Foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            block.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);

            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo");
            inlines.AssertEqual(((Paragraph)((BlockQuote)doc.Elements[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_063()
        {
            var doc = parser.Parse("> foo\nbar\n===");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            block.AssertTypeEqual(doc.Elements[0]);

            var inlines = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "==="));
            inlines.AssertEqual(((Paragraph)((BlockQuote)doc.Elements[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_064()
        {
            var doc = parser.Parse("- Foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            block.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);

            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo");
            inlines.AssertEqual(((Paragraph)((ListItem)((ListBlock)doc.Elements[0]).Children[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_065()
        {
            var doc = parser.Parse("Foo\nBar\n---");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);

            var inlines = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "Bar"));
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_066()
        {
            var doc = parser.Parse("---\nFoo\n---\nBar\n---\nBaz");
            Assert.AreEqual(4, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[3].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[1]).HeaderLevel);
            Assert.AreEqual(2, ((Heading)doc.Elements[2]).HeaderLevel);

            var inline1 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "Bar");
            var inline3 = new InlineStructure(InlineElementType.InlineText, "Baz");
            inline1.AssertEqual(((Heading)doc.Elements[1]).Inlines);
            inline2.AssertEqual(((Heading)doc.Elements[2]).Inlines);
            inline3.AssertEqual(((Paragraph)doc.Elements[3]).Inlines);
        }

        [TestMethod]
        public void TestCase_067()
        {
            var doc = parser.Parse("\n====");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "====");
            inline.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_068()
        {
            var doc = parser.Parse("---\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
        }

        [TestMethod]
        public void TestCase_069()
        {
            var doc = parser.Parse("- foo\n-----");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blockStructure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blockStructure.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo");
            inlines.AssertEqual(((Paragraph)((ListItem)((ListBlock)doc.Elements[0]).Children[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_070()
        {
            var doc = parser.Parse("    foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual("foo", ((CodeBlock)doc.Elements[0]).Code);
        }

        [TestMethod]
        public void TestCase_071()
        {
            var doc = parser.Parse("> foo\n-----");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blockStructure = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blockStructure.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo");
            inlines.AssertEqual(((Paragraph)((BlockQuote)doc.Elements[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_072()
        {
            var doc = parser.Parse("\\> foo\n------");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(2, (doc.Elements[0] as Heading).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "> foo");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_073()
        {
            var doc = parser.Parse("Foo\n\nbar\n---\nbaz");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);
            Assert.AreEqual(2, (doc.Elements[1] as Heading).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline0.AssertEqual(((LeafElement)doc.Elements[0]).Inlines);
            inline1.AssertEqual(((LeafElement)doc.Elements[1]).Inlines);
            inline2.AssertEqual(((LeafElement)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_074()
        {
            var doc = parser.Parse("Foo\nbar\n\n---\n\nbaz");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline0.AssertEqual(((LeafElement)doc.Elements[0]).Inlines);
            inline2.AssertEqual(((LeafElement)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_075()
        {
            var doc = parser.Parse("Foo\nbar\n* * *\nbaz");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline0.AssertEqual(((LeafElement)doc.Elements[0]).Inlines);
            inline2.AssertEqual(((LeafElement)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_076()
        {
            var doc = parser.Parse("Foo\nbar\n\\---\nbaz");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "---"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline0.AssertEqual(((LeafElement)doc.Elements[0]).Inlines);
        }
    }
}
