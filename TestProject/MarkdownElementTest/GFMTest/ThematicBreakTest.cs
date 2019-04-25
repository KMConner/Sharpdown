using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;


namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class ThematicBreakTest
    {
        private readonly MarkdownParser parser;

        public ThematicBreakTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_013()
        {
            var doc = parser.Parse("***\n---\n___");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[2].Type);
        }

        [TestMethod]
        public void TestCase_014()
        {
            var doc = parser.Parse("+++");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(1, ((Paragraph)doc.Elements[0]).Inlines.Count);
            var text = (InlineText)((Paragraph)doc.Elements[0]).Inlines[0];
            Assert.AreEqual("+++", text.Content);
        }

        [TestMethod]
        public void TestCase_015()
        {
            var doc = parser.Parse("===");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(1, ((Paragraph)doc.Elements[0]).Inlines.Count);
            var text = (InlineText)((Paragraph)doc.Elements[0]).Inlines[0];
            Assert.AreEqual("===", text.Content);
        }

        [TestMethod]
        public void TestCase_016()
        {
            var doc = parser.Parse("--\n**\n__");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(5, ((Paragraph)doc.Elements[0]).Inlines.Count);
            var inline1 = (InlineText)((Paragraph)doc.Elements[0]).Inlines[0];
            var inline2 = (SoftLineBreak)((Paragraph)doc.Elements[0]).Inlines[1];
            var inline3 = (InlineText)((Paragraph)doc.Elements[0]).Inlines[2];
            var inline4 = (SoftLineBreak)((Paragraph)doc.Elements[0]).Inlines[3];
            var inline5 = (InlineText)((Paragraph)doc.Elements[0]).Inlines[4];
            Assert.AreEqual(InlineElementType.InlineText, inline1.Type);
            Assert.AreEqual(InlineElementType.SoftLineBreak, inline2.Type);
            Assert.AreEqual(InlineElementType.InlineText, inline3.Type);
            Assert.AreEqual(InlineElementType.SoftLineBreak, inline4.Type);
            Assert.AreEqual(InlineElementType.InlineText, inline5.Type);
            Assert.AreEqual("--", inline1.Content);
            Assert.AreEqual("**", inline3.Content);
            Assert.AreEqual("__", inline5.Content);
        }

        [TestMethod]
        public void TestCase_017()
        {
            var doc = parser.Parse(" ***\n  ***\n   ***");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[2].Type);
        }

        [TestMethod]
        public void TestCase_018()
        {
            var doc = parser.Parse("    ***");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(1, ((CodeBlock)doc.Elements[0]).Inlines.Count);
            var lit = (LiteralText)((CodeBlock)doc.Elements[0]).Inlines[0];
            Assert.AreEqual(InlineElementType.LiteralText, lit.Type);
            Assert.AreEqual("***", lit.Content);
        }

        [TestMethod]
        public void TestCase_019()
        {
            var doc = parser.Parse("Foo\n    ***");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(3, ((Paragraph)doc.Elements[0]).Inlines.Count);
            var inline1 = (InlineText)((Paragraph)doc.Elements[0]).Inlines[0];
            var inline2 = (SoftLineBreak)((Paragraph)doc.Elements[0]).Inlines[1];
            var inline3 = (InlineText)((Paragraph)doc.Elements[0]).Inlines[2];
            Assert.AreEqual(InlineElementType.InlineText, inline1.Type);
            Assert.AreEqual(InlineElementType.SoftLineBreak, inline2.Type);
            Assert.AreEqual(InlineElementType.InlineText, inline3.Type);
            Assert.AreEqual("Foo", inline1.Content);
            Assert.AreEqual("***", inline3.Content);
        }

        [TestMethod]
        public void TestCase_020()
        {
            var doc = parser.Parse("_____________________________________");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_021()
        {
            var doc = parser.Parse(" - - -");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_022()
        {
            var doc = parser.Parse(" **  * ** * ** * **");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_023()
        {
            var doc = parser.Parse("-     -      -      -");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_024()
        {
            var doc = parser.Parse("- - - -    ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_025()
        {
            var doc = parser.Parse("_ _ _ _ a\n\na------\n\n---a---");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(1, ((Paragraph)doc.Elements[0]).Inlines.Count);
            Assert.AreEqual(InlineElementType.InlineText, ((Paragraph)doc.Elements[0]).Inlines[0].Type);
            Assert.AreEqual("_ _ _ _ a", ((InlineText)((Paragraph)doc.Elements[0]).Inlines[0]).Content);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(1, ((Paragraph)doc.Elements[1]).Inlines.Count);
            Assert.AreEqual(InlineElementType.InlineText, ((Paragraph)doc.Elements[1]).Inlines[0].Type);
            Assert.AreEqual("a------", ((InlineText)((Paragraph)doc.Elements[1]).Inlines[0]).Content);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);
            Assert.AreEqual(1, ((Paragraph)doc.Elements[2]).Inlines.Count);
            Assert.AreEqual(InlineElementType.InlineText, ((Paragraph)doc.Elements[2]).Inlines[0].Type);
            Assert.AreEqual("---a---", ((InlineText)((Paragraph)doc.Elements[2]).Inlines[0]).Content);
        }

        [TestMethod]
        public void TestCase_026()
        {
            var doc = parser.Parse(" *-*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var block = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "-"));
            block.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_027()
        {
            var doc = parser.Parse("- foo\n***\n- bar");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            var block0 = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            block0.AssertTypeEqual(doc.Elements[0]);
            block0.AssertTypeEqual(doc.Elements[2]);
            Assert.IsTrue(((ListBlock)doc.Elements[0]).IsTight);
            Assert.IsTrue(((ListBlock)doc.Elements[2]).IsTight);
        }

        [TestMethod]
        public void TestCase_028()
        {
            var doc = parser.Parse("Foo\n***\nbar");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
            inline2.AssertEqual(((Paragraph)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_029()
        {
            var doc = parser.Parse("Foo\n---\nbar");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(((Heading)doc.Elements[0]).Inlines);
            inline2.AssertEqual(((Paragraph)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_030()
        {
            var doc = parser.Parse("* Foo\n* * *\n* bar");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            var block0 = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            block0.AssertTypeEqual(doc.Elements[0]);
            block0.AssertTypeEqual(doc.Elements[2]);
            Assert.IsTrue(((ListBlock)doc.Elements[0]).IsTight);
            Assert.IsTrue(((ListBlock)doc.Elements[2]).IsTight);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline0.AssertEqual(
                ((Paragraph)((ListItem)((ListBlock)doc.Elements[0]).Children[0]).Children[0]).Inlines);
            inline2.AssertEqual(
                ((Paragraph)((ListItem)((ListBlock)doc.Elements[2]).Children[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_031()
        {
            var doc = parser.Parse("- Foo\n- * * *");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.ThematicBreak)));

            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline0.AssertEqual(
                ((Paragraph)((ListItem)((ListBlock)doc.Elements[0]).Children[0]).Children[0]).Inlines);
        }
    }
}
