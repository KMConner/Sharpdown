using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;


namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class AtxHeadingTest
    {
        private readonly MarkdownParser parser;

        public AtxHeadingTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_032()
        {
            var doc = parser.Parse("# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo");
            Assert.AreEqual(6, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[3].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[4].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[5].Type);

            Assert.AreEqual(1, ((Heading)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(2, ((Heading)doc.Elements[1]).HeaderLevel);
            Assert.AreEqual(3, ((Heading)doc.Elements[2]).HeaderLevel);
            Assert.AreEqual(4, ((Heading)doc.Elements[3]).HeaderLevel);
            Assert.AreEqual(5, ((Heading)doc.Elements[4]).HeaderLevel);
            Assert.AreEqual(6, ((Heading)doc.Elements[5]).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(((Heading)doc.Elements[0]).Inlines);
            inline0.AssertEqual(((Heading)doc.Elements[1]).Inlines);
            inline0.AssertEqual(((Heading)doc.Elements[2]).Inlines);
            inline0.AssertEqual(((Heading)doc.Elements[3]).Inlines);
            inline0.AssertEqual(((Heading)doc.Elements[4]).Inlines);
            inline0.AssertEqual(((Heading)doc.Elements[5]).Inlines);
        }

        [TestMethod]
        public void TestCase_033()
        {
            var doc = parser.Parse("####### foo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "####### foo");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_034()
        {
            var doc = parser.Parse("#5 bolt\n\n#hashtag");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "#5 bolt");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "#hashtag");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
            inline1.AssertEqual(((Paragraph)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_035()
        {
            var doc = parser.Parse("\\## foo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "## foo");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_036()
        {
            var doc = parser.Parse("# foo *bar* \\*baz\\*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, " *baz*"));
            inline0.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_037()
        {
            var doc = parser.Parse("#                  foo                     ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_038()
        {
            var doc = parser.Parse(" ### foo\n  ## foo\n   # foo");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[2].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(((Heading)doc.Elements[0]).Inlines);
            inline0.AssertEqual(((Heading)doc.Elements[1]).Inlines);
            inline0.AssertEqual(((Heading)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_039()
        {
            var doc = parser.Parse("    # foo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("# foo", ((CodeBlock)doc.Elements[0]).Code);
        }

        [TestMethod]
        public void TestCase_040()
        {
            var doc = parser.Parse("foo\n    # bar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var inlines = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "# bar"));
            inlines.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_041()
        {
            var doc = parser.Parse("## foo ##\n  ###   bar    ###");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(3, ((Heading)doc.Elements[1]).HeaderLevel);
            var inlines0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inlines1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inlines0.AssertEqual(((Heading)doc.Elements[0]).Inlines);
            inlines1.AssertEqual(((Heading)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_042()
        {
            var doc = parser.Parse("# foo ##################################\n##### foo ##");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(1, ((Heading)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(5, ((Heading)doc.Elements[1]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
            inlines.AssertEqual(((Heading)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_043()
        {
            var doc = parser.Parse("### foo ###     ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(3, ((Heading)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_044()
        {
            var doc = parser.Parse("### foo ### b");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(3, ((Heading)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo ### b");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_045()
        {
            var doc = parser.Parse("# foo#");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(1, ((Heading)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo#");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_046()
        {
            var doc = parser.Parse("### foo \\###\n## foo #\\##\n# foo \\#");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[2].Type);
            Assert.AreEqual(3, ((Heading)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(2, ((Heading)doc.Elements[1]).HeaderLevel);
            Assert.AreEqual(1, ((Heading)doc.Elements[2]).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo ###");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "foo #");
            inline0.AssertEqual(((Heading)doc.Elements[0]).Inlines);
            inline0.AssertEqual(((Heading)doc.Elements[1]).Inlines);
            inline2.AssertEqual(((Heading)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_047()
        {
            var doc = parser.Parse("****\n## foo\n****");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[2].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[1]).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(((Heading)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_048()
        {
            var doc = parser.Parse("Foo bar\n# baz\nBar foo");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);
            Assert.AreEqual(1, ((Heading)doc.Elements[1]).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo bar");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "baz");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "Bar foo");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
            inline1.AssertEqual(((Heading)doc.Elements[1]).Inlines);
            inline2.AssertEqual(((Paragraph)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_049()
        {
            var doc = parser.Parse("## \n#\n### ###");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[2].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(1, ((Heading)doc.Elements[1]).HeaderLevel);
            Assert.AreEqual(3, ((Heading)doc.Elements[2]).HeaderLevel);
            Assert.AreEqual(0, ((Heading)doc.Elements[0]).Inlines.Count);
            Assert.AreEqual(0, ((Heading)doc.Elements[1]).Inlines.Count);
            Assert.AreEqual(0, ((Heading)doc.Elements[2]).Inlines.Count);
        }
    }
}
