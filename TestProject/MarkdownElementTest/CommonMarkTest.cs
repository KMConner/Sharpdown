using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;
using TestProject.MarkdownElementTest.BlockElementTest;
using TestProject.MarkdownElementTest.InlineElementTest;

namespace TestProject.MarkdownElementTest
{
    [TestClass]
    public class CommonMarkTest
    {
        #region Tabs

        [TestMethod]
        public void TestCase_001()
        {
            var doc = MarkdownParser.Parse("\tfoo\tbaz\t\tbim");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(1, ((IndentedCodeBlock)doc.Elements[0]).Inlines.Count);
            var lit = (LiteralText)((IndentedCodeBlock)doc.Elements[0]).Inlines[0];
            Assert.AreEqual(InlineElementType.LiteralText, lit.Type);
            Assert.AreEqual("foo\tbaz\t\tbim", lit.Content);
        }

        //[TestMethod]
        //public void TestCase_002()
        //{
        //    var doc = MarkdownParser.Parse("  \tfoo\tbaz\t\tbim");
        //    Assert.AreEqual(1, doc.Elements.Count);
        //    Assert.AreEqual(0, doc.LinkDefinition.Count);
        //    Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
        //    Assert.AreEqual(1, ((IndentedCodeBlock)doc.Elements[0]).Inlines.Count);
        //    var lit = (LiteralText)((IndentedCodeBlock)doc.Elements[0]).Inlines[0];
        //    Assert.AreEqual(InlineElementType.LiteralText, lit.Type);
        //    Assert.AreEqual("foo\tbaz\t\tbim", lit.Content);
        //}

        #endregion
        // TODO: test 02 - 12

        #region Themantic break

        [TestMethod]
        public void TestCase_013()
        {
            var doc = MarkdownParser.Parse("***\n---\n___");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[2].Type);
        }

        [TestMethod]
        public void TestCase_014()
        {
            var doc = MarkdownParser.Parse("+++");
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
            var doc = MarkdownParser.Parse("===");
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
            var doc = MarkdownParser.Parse("--\n**\n__");
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
            var doc = MarkdownParser.Parse(" ***\n  ***\n   ***");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[2].Type);
        }

        [TestMethod]
        public void TestCase_018()
        {
            var doc = MarkdownParser.Parse("    ***");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(1, ((IndentedCodeBlock)doc.Elements[0]).Inlines.Count);
            var lit = (LiteralText)((IndentedCodeBlock)doc.Elements[0]).Inlines[0];
            Assert.AreEqual(InlineElementType.LiteralText, lit.Type);
            Assert.AreEqual("***", lit.Content);
        }

        [TestMethod]
        public void TestCase_019()
        {
            var doc = MarkdownParser.Parse("Foo\n    ***");
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
            var doc = MarkdownParser.Parse("_____________________________________");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_021()
        {
            var doc = MarkdownParser.Parse(" - - -");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_022()
        {
            var doc = MarkdownParser.Parse(" **  * ** * ** * **");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_023()
        {
            var doc = MarkdownParser.Parse("-     -      -      -");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_024()
        {
            var doc = MarkdownParser.Parse("- - - -    ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_025()
        {
            var doc = MarkdownParser.Parse("_ _ _ _ a\n\na------\n\n---a---");
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
            var doc = MarkdownParser.Parse(" *-*");
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
            var doc = MarkdownParser.Parse("- foo\n***\n- bar");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
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
            var doc = MarkdownParser.Parse("Foo\n***\nbar");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
            inline2.AssertEqual(((Paragraph)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_029()
        {
            var doc = MarkdownParser.Parse("Foo\n---\nbar");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(((SetextHeader)doc.Elements[0]).Inlines);
            inline2.AssertEqual(((Paragraph)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_030()
        {
            var doc = MarkdownParser.Parse("* Foo\n* * *\n* bar");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
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
            var doc = MarkdownParser.Parse("- Foo\n- * * *");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.ThemanticBreak)));

            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline0.AssertEqual(
                ((Paragraph)((ListItem)((ListBlock)doc.Elements[0]).Children[0]).Children[0]).Inlines);
        }

        #endregion

        #region ATX heading

        [TestMethod]
        public void TestCase_032()
        {
            var doc = MarkdownParser.Parse("# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo");
            Assert.AreEqual(6, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[3].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[4].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[5].Type);

            Assert.AreEqual(1, ((AtxHeaderElement)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(2, ((AtxHeaderElement)doc.Elements[1]).HeaderLevel);
            Assert.AreEqual(3, ((AtxHeaderElement)doc.Elements[2]).HeaderLevel);
            Assert.AreEqual(4, ((AtxHeaderElement)doc.Elements[3]).HeaderLevel);
            Assert.AreEqual(5, ((AtxHeaderElement)doc.Elements[4]).HeaderLevel);
            Assert.AreEqual(6, ((AtxHeaderElement)doc.Elements[5]).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[0]).Inlines);
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[1]).Inlines);
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[2]).Inlines);
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[3]).Inlines);
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[4]).Inlines);
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[5]).Inlines);
        }

        [TestMethod]
        public void TestCase_033()
        {
            var doc = MarkdownParser.Parse("####### foo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "####### foo");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_034()
        {
            var doc = MarkdownParser.Parse("#5 bolt\n\n#hashtag");
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
            var doc = MarkdownParser.Parse("\\## foo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "## foo");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_036()
        {
            var doc = MarkdownParser.Parse("# foo *bar* \\*baz\\*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, " *baz*"));
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_037()
        {
            var doc = MarkdownParser.Parse("#                  foo                     ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_038()
        {
            var doc = MarkdownParser.Parse(" ### foo\n  ## foo\n   # foo");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[2].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[0]).Inlines);
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[1]).Inlines);
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_039()
        {
            var doc = MarkdownParser.Parse("    # foo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("# foo", ((IndentedCodeBlock)doc.Elements[0]).Content);
        }

        [TestMethod]
        public void TestCase_040()
        {
            var doc = MarkdownParser.Parse("foo\n    # bar");
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
            var doc = MarkdownParser.Parse("## foo ##\n  ###   bar    ###");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[1].Type);
            Assert.AreEqual(2, ((AtxHeaderElement)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(3, ((AtxHeaderElement)doc.Elements[1]).HeaderLevel);
            var inlines0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inlines1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inlines0.AssertEqual(((AtxHeaderElement)doc.Elements[0]).Inlines);
            inlines1.AssertEqual(((AtxHeaderElement)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_042()
        {
            var doc = MarkdownParser.Parse("# foo ##################################\n##### foo ##");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[1].Type);
            Assert.AreEqual(1, ((AtxHeaderElement)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(5, ((AtxHeaderElement)doc.Elements[1]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo");
            inlines.AssertEqual(((AtxHeaderElement)doc.Elements[0]).Inlines);
            inlines.AssertEqual(((AtxHeaderElement)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_043()
        {
            var doc = MarkdownParser.Parse("### foo ###     ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(3, ((AtxHeaderElement)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo");
            inlines.AssertEqual(((AtxHeaderElement)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_044()
        {
            var doc = MarkdownParser.Parse("### foo ### b");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(3, ((AtxHeaderElement)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo ### b");
            inlines.AssertEqual(((AtxHeaderElement)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_045()
        {
            var doc = MarkdownParser.Parse("# foo#");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(1, ((AtxHeaderElement)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo#");
            inlines.AssertEqual(((AtxHeaderElement)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_046()
        {
            var doc = MarkdownParser.Parse("### foo \\###\n## foo #\\##\n# foo \\#");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[2].Type);
            Assert.AreEqual(3, ((AtxHeaderElement)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(2, ((AtxHeaderElement)doc.Elements[1]).HeaderLevel);
            Assert.AreEqual(1, ((AtxHeaderElement)doc.Elements[2]).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo ###");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "foo #");
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[0]).Inlines);
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[1]).Inlines);
            inline2.AssertEqual(((AtxHeaderElement)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_047()
        {
            var doc = MarkdownParser.Parse("****\n## foo\n****");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[2].Type);
            Assert.AreEqual(2, ((AtxHeaderElement)doc.Elements[1]).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(((AtxHeaderElement)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_048()
        {
            var doc = MarkdownParser.Parse("Foo bar\n# baz\nBar foo");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);
            Assert.AreEqual(1, ((AtxHeaderElement)doc.Elements[1]).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo bar");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "baz");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "Bar foo");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
            inline1.AssertEqual(((AtxHeaderElement)doc.Elements[1]).Inlines);
            inline2.AssertEqual(((Paragraph)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_049()
        {
            var doc = MarkdownParser.Parse("## \n#\n### ###");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[2].Type);
            Assert.AreEqual(2, ((AtxHeaderElement)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(1, ((AtxHeaderElement)doc.Elements[1]).HeaderLevel);
            Assert.AreEqual(3, ((AtxHeaderElement)doc.Elements[2]).HeaderLevel);
            Assert.AreEqual(0, ((AtxHeaderElement)doc.Elements[0]).Inlines.Count);
            Assert.AreEqual(0, ((AtxHeaderElement)doc.Elements[1]).Inlines.Count);
            Assert.AreEqual(0, ((AtxHeaderElement)doc.Elements[2]).Inlines.Count);
        }

        #endregion

        #region Setext heading

        [TestMethod]
        public void TestCase_050()
        {
            var doc = MarkdownParser.Parse("Foo *bar*\n=========\n\nFoo *bar*\n---------");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[1].Type);
            Assert.AreEqual(1, ((SetextHeader)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(2, ((SetextHeader)doc.Elements[1]).HeaderLevel);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(((SetextHeader)doc.Elements[0]).Inlines);
            inline.AssertEqual(((SetextHeader)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_051()
        {
            var doc = MarkdownParser.Parse("Foo *bar\nbaz*\n====");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(1, ((SetextHeader)doc.Elements[0]).HeaderLevel);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar"),
                    new InlineStructure(InlineElementType.SoftLineBreak, ""),
                    new InlineStructure(InlineElementType.InlineText, "baz")));
            inline.AssertEqual(((SetextHeader)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_052()
        {
            var doc = MarkdownParser.Parse("Foo\n-------------------------\n\nFoo\n=");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[1].Type);
            Assert.AreEqual(2, ((SetextHeader)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(1, ((SetextHeader)doc.Elements[1]).HeaderLevel);
            var inline = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline.AssertEqual(((SetextHeader)doc.Elements[0]).Inlines);
            inline.AssertEqual(((SetextHeader)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_053()
        {
            var doc = MarkdownParser.Parse("   Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[2].Type);
            Assert.AreEqual(2, ((SetextHeader)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(2, ((SetextHeader)doc.Elements[1]).HeaderLevel);
            Assert.AreEqual(1, ((SetextHeader)doc.Elements[2]).HeaderLevel);
            var inline = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline.AssertEqual(((SetextHeader)doc.Elements[0]).Inlines);
            inline.AssertEqual(((SetextHeader)doc.Elements[1]).Inlines);
            inline.AssertEqual(((SetextHeader)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_054()
        {
            var doc = MarkdownParser.Parse("    Foo\n    ---\n\n    Foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
            Assert.AreEqual("Foo\r\n---\r\n\r\nFoo", ((IndentedCodeBlock)doc.Elements[0]).Content);
        }

        [TestMethod]
        public void TestCase_055()
        {
            var doc = MarkdownParser.Parse("Foo\n   ----      ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((SetextHeader)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo");
            inlines.AssertEqual(((SetextHeader)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_056()
        {
            var doc = MarkdownParser.Parse("Foo\n    ---");
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
        public void TestCase_057()
        {
            var doc = MarkdownParser.Parse("Foo\n= =\n\nFoo\n--- -");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[2].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "= ="));
            var inline1 = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
            inline1.AssertEqual(((Paragraph)doc.Elements[1]).Inlines);
        }

        [TestMethod]
        public void TestCase_058()
        {
            var doc = MarkdownParser.Parse("Foo  \n-----");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((SetextHeader)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo");
            inlines.AssertEqual(((SetextHeader)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_059()
        {
            var doc = MarkdownParser.Parse("Foo\\\n-----");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((SetextHeader)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo\\");
            inlines.AssertEqual(((SetextHeader)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_060()
        {
            var doc = MarkdownParser.Parse("`Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>");
            Assert.AreEqual(4, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[3].Type);
            Assert.AreEqual(2, ((SetextHeader)doc.Elements[0]).HeaderLevel);
            Assert.AreEqual(2, ((SetextHeader)doc.Elements[2]).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "`Foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "`");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "<a title=\"a lot");
            var inline3 = new InlineStructure(InlineElementType.InlineText, "of dashes\"/>");
            inline0.AssertEqual(((SetextHeader)doc.Elements[0]).Inlines);
            inline1.AssertEqual(((Paragraph)doc.Elements[1]).Inlines);
            inline2.AssertEqual(((SetextHeader)doc.Elements[2]).Inlines);
            inline3.AssertEqual(((Paragraph)doc.Elements[3]).Inlines);
        }

        [TestMethod]
        public void TestCase_061()
        {
            var doc = MarkdownParser.Parse("> Foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            block.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);

            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo");
            inlines.AssertEqual(((Paragraph)((BlockQuote)doc.Elements[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_062()
        {
            var doc = MarkdownParser.Parse("> foo\nbar\n===");
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
        public void TestCase_063()
        {
            var doc = MarkdownParser.Parse("- foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            block.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);

            var inlines = new InlineStructure(InlineElementType.InlineText, "foo");
            inlines.AssertEqual(((Paragraph)((ListItem)((ListBlock)doc.Elements[0]).Children[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_064()
        {
            var doc = MarkdownParser.Parse("Foo\nBar\n---");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((HeaderElementBase)doc.Elements[0]).HeaderLevel);

            var inlines = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "Bar"));
            inlines.AssertEqual(((HeaderElementBase)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_065()
        {
            var doc = MarkdownParser.Parse("---\nFoo\n---\nBar\n---\nBaz");
            Assert.AreEqual(4, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[3].Type);
            Assert.AreEqual(2, ((HeaderElementBase)doc.Elements[1]).HeaderLevel);
            Assert.AreEqual(2, ((HeaderElementBase)doc.Elements[2]).HeaderLevel);

            var inline1 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "Bar");
            var inline3 = new InlineStructure(InlineElementType.InlineText, "Baz");
            inline1.AssertEqual(((HeaderElementBase)doc.Elements[1]).Inlines);
            inline2.AssertEqual(((HeaderElementBase)doc.Elements[2]).Inlines);
            inline3.AssertEqual(((Paragraph)doc.Elements[3]).Inlines);
        }

        [TestMethod]
        public void TestCase_066()
        {
            var doc = MarkdownParser.Parse("\n====");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "====");
            inline.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_067()
        {
            var doc = MarkdownParser.Parse("---\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
        }

        [TestMethod]
        public void TestCase_068()
        {
            var doc = MarkdownParser.Parse("- foo\n-----");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blockStructure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blockStructure.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo");
            inlines.AssertEqual(((Paragraph)((ListItem)((ListBlock)doc.Elements[0]).Children[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_069()
        {
            var doc = MarkdownParser.Parse("    foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
            Assert.AreEqual("foo", ((CodeBlockBase)doc.Elements[0]).Content);
        }

        [TestMethod]
        public void TestCase_070()
        {
            var doc = MarkdownParser.Parse("> foo\n-----");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blockStructure = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blockStructure.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
            var inlines = new InlineStructure(InlineElementType.InlineText, "foo");
            inlines.AssertEqual(((Paragraph)((BlockQuote)doc.Elements[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_071()
        {
            var doc = MarkdownParser.Parse("\\> foo\n------");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(2, (doc.Elements[0] as SetextHeader).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "> foo");
            inlines.AssertEqual(((SetextHeader)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_072()
        {
            var doc = MarkdownParser.Parse("Foo\n\nbar\n---\nbaz");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[2].Type);
            Assert.AreEqual(2, (doc.Elements[1] as SetextHeader).HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline0.AssertEqual(((LeafElement)doc.Elements[0]).Inlines);
            inline1.AssertEqual(((LeafElement)doc.Elements[1]).Inlines);
            inline2.AssertEqual(((LeafElement)doc.Elements[2]).Inlines);
        }

        [TestMethod]
        public void TestCase_073()
        {
            var doc = MarkdownParser.Parse("Foo\nbar\n\n---\n\nbaz");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
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
        public void TestCase_074()
        {
            var doc = MarkdownParser.Parse("Foo\nbar\n* * *\nbaz");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
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
            var doc = MarkdownParser.Parse("Foo\nbar\n\\---\nbaz");
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

        #endregion

        #region Indented code block

        [TestMethod]
        public void TestCase_076()
        {
            var doc = MarkdownParser.Parse("    a simple\n      indented code block");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("a simple\r\n  indented code block",
                (doc.Elements[0] as IndentedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_077()
        {
            var doc = MarkdownParser.Parse("  - foo\n\n    bar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blockStructure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blockStructure.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline0.AssertEqual(
                (((doc.Elements[0] as ListBlock).Children[0] as ListItem).Children[0] as Paragraph).Inlines);
            inline1.AssertEqual(
                (((doc.Elements[0] as ListBlock).Children[0] as ListItem).Children[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_078()
        {
            var doc = MarkdownParser.Parse("1.  foo\n\n    - bar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blockStructure = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph)))));
            blockStructure.AssertTypeEqual(doc.Elements[0]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(
                (((doc.Elements[0] as ListBlock).Children[0] as ListItem).Children[0] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_079()
        {
            var doc = MarkdownParser.Parse("    <a/>\n    *hi*\n\n    - one");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("<a/>\r\n*hi*\r\n\r\n- one", (doc.Elements[0] as IndentedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_080()
        {
            var doc = MarkdownParser.Parse("    chunk1\n\n    chunk2\n  \n \n \n    chunk3");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("chunk1\r\n\r\nchunk2\r\n\r\n\r\n\r\nchunk3", (doc.Elements[0] as IndentedCodeBlock).Content);
        }


        [TestMethod]
        public void TestCase_081()
        {
            var doc = MarkdownParser.Parse("    chunk1\n      \n      chunk2");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("chunk1\r\n  \r\n  chunk2", (doc.Elements[0] as IndentedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_082()
        {
            var doc = MarkdownParser.Parse("Foo\n    bar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual((doc.Elements[0] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_083()
        {
            var doc = MarkdownParser.Parse("    Foo\nbar");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("Foo", (doc.Elements[0] as IndentedCodeBlock).Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "bar");
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_084()
        {
            var doc = MarkdownParser.Parse("# Heading\n    foo\nHeading\n------\n    foo\n----");
            Assert.AreEqual(5, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[3].Type);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[4].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "Heading");
            inline.AssertEqual((doc.Elements[0] as HeaderElementBase).Inlines);
            Assert.AreEqual("foo", (doc.Elements[1] as IndentedCodeBlock).Content);
            inline.AssertEqual((doc.Elements[2] as HeaderElementBase).Inlines);
            Assert.AreEqual("foo", (doc.Elements[3] as IndentedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_085()
        {
            var doc = MarkdownParser.Parse("        foo\n    bar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("    foo\r\nbar", (doc.Elements[0] as IndentedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_086()
        {
            var doc = MarkdownParser.Parse("\n    \n    foo\n    ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo", (doc.Elements[0] as IndentedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_087()
        {
            var doc = MarkdownParser.Parse("    foo  ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo  ", (doc.Elements[0] as IndentedCodeBlock).Content);
        }

        #endregion

        #region Fenced code block

        [TestMethod]
        public void TestCase_088()
        {
            var doc = MarkdownParser.Parse("```\n<\n >\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("<\r\n >", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_089()
        {
            var doc = MarkdownParser.Parse("~~~\n<\n >\n~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("<\r\n >", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_090()
        {
            var doc = MarkdownParser.Parse("``\nfoo\n``");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_091()
        {
            var doc = MarkdownParser.Parse("```\naaa\n~~~\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_092()
        {
            var doc = MarkdownParser.Parse("~~~\naaa\n```\n~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n```", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_093()
        {
            var doc = MarkdownParser.Parse("````\naaa\n```\n``````");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n```", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_094()
        {
            var doc = MarkdownParser.Parse("~~~~\naaa\n~~~\n~~~~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_095()
        {
            var doc = MarkdownParser.Parse("```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_096()
        {
            var doc = MarkdownParser.Parse("`````\n\n```\naaa");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\r\n```\r\naaa", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_097()
        {
            var doc = MarkdownParser.Parse("> ```\n> aaa\n\nbbb");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blockStructure = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.FencedCodeBlock));
            blockStructure.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("aaa", ((doc.Elements[0] as BlockQuote).Children[0] as CodeBlockBase).Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_098()
        {
            var doc = MarkdownParser.Parse("```\n\n  \n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\r\n  ", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_099()
        {
            var doc = MarkdownParser.Parse("```\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_100()
        {
            var doc = MarkdownParser.Parse(" ```\n aaa\naaa\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\naaa", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_101()
        {
            var doc = MarkdownParser.Parse("  ```\naaa\n  aaa\naaa\n  ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\naaa\r\naaa", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_102()
        {
            var doc = MarkdownParser.Parse("   ```\n   aaa\n    aaa\n  aaa\n   ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n aaa\r\naaa", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_103()
        {
            var doc = MarkdownParser.Parse("    ```\n    aaa\n    ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("```\r\naaa\r\n```", (doc.Elements[0] as CodeBlockBase).Content);
        }

        [TestMethod]
        public void TestCase_104()
        {
            var doc = MarkdownParser.Parse("```\naaa\n  ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa", (doc.Elements[0] as CodeBlockBase).Content);
        }

        [TestMethod]
        public void TestCase_105()
        {
            var doc = MarkdownParser.Parse("   ```\naaa\n   ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa", (doc.Elements[0] as CodeBlockBase).Content);
        }

        [TestMethod]
        public void TestCase_106()
        {
            var doc = MarkdownParser.Parse("```\naaa\n    ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n    ```", (doc.Elements[0] as CodeBlockBase).Content);
        }

        [TestMethod]
        public void TestCase_107()
        {
            var doc = MarkdownParser.Parse("``` ```\naaa");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.CodeSpan, ""),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "aaa"));
            inline.AssertEqual((doc.Elements[0] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_108()
        {
            var doc = MarkdownParser.Parse("~~~~~~\naaa\n~~~ ~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~ ~~", (doc.Elements[0] as CodeBlockBase).Content);
        }

        [TestMethod]
        public void TestCase_109()
        {
            var doc = MarkdownParser.Parse("foo\n```\nbar\n```\nbaz");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline0.AssertEqual((doc.Elements[0] as Paragraph).Inlines);
            Assert.AreEqual("bar", (doc.Elements[1] as CodeBlockBase).Content);
            inline2.AssertEqual((doc.Elements[2] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_110()
        {
            var doc = MarkdownParser.Parse("foo\n---\n~~~\nbar\n~~~\n# baz");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.SetextHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[2].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline0.AssertEqual((doc.Elements[0] as LeafElement).Inlines);
            Assert.AreEqual("bar", (doc.Elements[1] as CodeBlockBase).Content);
            inline2.AssertEqual((doc.Elements[2] as LeafElement).Inlines);
        }

        [TestMethod]
        public void TestCase_111()
        {
            var doc = MarkdownParser.Parse("```ruby\ndef foo(x)\n  return 3\nend\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("def foo(x)\r\n  return 3\r\nend", (doc.Elements[0] as CodeBlockBase).Content);
            Assert.AreEqual("ruby", (doc.Elements[0] as CodeBlockBase).InfoString);

        }

        [TestMethod]
        public void TestCase_112()
        {
            var doc = MarkdownParser.Parse("~~~~    ruby startline=3 $%@#$\ndef foo(x)\n  return 3\nend\n~~~~~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("def foo(x)\r\n  return 3\r\nend", (doc.Elements[0] as CodeBlockBase).Content);
            Assert.AreEqual("ruby startline=3 $%@#$", (doc.Elements[0] as CodeBlockBase).InfoString);
        }

        [TestMethod]
        public void TestCase_113()
        {
            var doc = MarkdownParser.Parse("````;\n````");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("", (doc.Elements[0] as CodeBlockBase).Content);
            Assert.AreEqual(";", (doc.Elements[0] as CodeBlockBase).InfoString);
        }

        [TestMethod]
        public void TestCase_114()
        {
            var doc = MarkdownParser.Parse("``` aa ```\nfoo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.CodeSpan, "aa"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual((doc.Elements[0] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_115()
        {
            var doc = MarkdownParser.Parse("```\n```aaa\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("```aaa", (doc.Elements[0] as CodeBlockBase).Content);
        }

        #endregion

        #region HTML block

        [TestMethod]
        public void TestCase_116()
        {
            var doc = MarkdownParser.Parse(
                "<table><tr><td>\n<pre>\n**Hello**,\n\n_world_.\n</pre>\n</td></tr></table>");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[2].Type);
            Assert.AreEqual("<table><tr><td>\r\n<pre>\r\n**Hello**,", (doc.Elements[0] as HtmlBlock).Content);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "world")),
                new InlineStructure(InlineElementType.InlineText, "."),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineHtml, "</pre>"));
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
            Assert.AreEqual("</td></tr></table>", (doc.Elements[2] as HtmlBlock).Content);
        }


        [TestMethod]
        public void TestCase_117()
        {
            var doc = MarkdownParser.Parse(
                "<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n\nokay.");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(
                "<table>\r\n  <tr>\r\n    <td>\r\n           hi\r\n    </td>\r\n  </tr>\r\n</table>",
                (doc.Elements[0] as HtmlBlock).Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "okay.");
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_118()
        {
            var doc = MarkdownParser.Parse(" <div>\r\n  *hello*\r\n         <foo><a>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(" <div>\r\n  *hello*\r\n         <foo><a>", (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_119()
        {
            var doc = MarkdownParser.Parse("</div>\r\n*foo*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("</div>\r\n*foo*", (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_120()
        {
            var doc = MarkdownParser.Parse("<DIV CLASS=\"foo\">\n\n*Markdown*\n\n</DIV>");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[2].Type);
            Assert.AreEqual("<DIV CLASS=\"foo\">", (doc.Elements[0] as HtmlBlock).Content);
            Assert.AreEqual("</DIV>", (doc.Elements[2] as HtmlBlock).Content);
            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "Markdown"));
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_121()
        {
            var doc = MarkdownParser.Parse("<div id=\"foo\"\r\n  class=\"bar\">\r\n</div>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div id=\"foo\"\r\n  class=\"bar\">\r\n</div>",
                (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_122()
        {
            var doc = MarkdownParser.Parse("<div id=\"foo\" class=\"bar\r\n  baz\">\r\n</div>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div id=\"foo\" class=\"bar\r\n  baz\">\r\n</div>",
                (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_123()
        {
            var doc = MarkdownParser.Parse("<div>\r\n*foo*\r\n\r\n*bar*");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<div>\r\n*foo*", (doc.Elements[0] as HtmlBlock).Content);
            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_124()
        {
            var doc = MarkdownParser.Parse("<div id=\"foo\"\r\n*hi*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div id=\"foo\"\r\n*hi*", (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_125()
        {
            var doc = MarkdownParser.Parse("<div class\r\nfoo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div class\r\nfoo", (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_126()
        {
            var doc = MarkdownParser.Parse("<div *???-&&&-<---\r\n*foo*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div *???-&&&-<---\r\n*foo*", (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_127()
        {
            var doc = MarkdownParser.Parse("<div><a href=\"bar\">*foo*</a></div>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div><a href=\"bar\">*foo*</a></div>", (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_128()
        {
            var doc = MarkdownParser.Parse("<table><tr><td>\r\nfoo\r\n</td></tr></table>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<table><tr><td>\r\nfoo\r\n</td></tr></table>",
                (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_129()
        {
            var html = "<div></div>\r\n``` c\r\nint x = 33;\r\n```";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_130()
        {
            var html = "<a href=\"foo\">\r\n*bar*\r\n</a>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_131()
        {
            var html = "<Warning>\r\n*bar*\r\n</Warning>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_132()
        {
            var html = "<i class=\"foo\">\r\n*bar*\r\n</i>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_133()
        {
            var html = "</ins>\r\n*bar*";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_134()
        {
            var html = "<del>\r\n*foo*\r\n</del>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_135()
        {
            var html = "<del>\r\n\r\n*foo*\r\n\r\n</del>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[2].Type);
            Assert.AreEqual("<del>", (doc.Elements[0] as HtmlBlock).Content);
            var inlines = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inlines.AssertEqual((doc.Elements[1] as LeafElement).Inlines);
            Assert.AreEqual("</del>", (doc.Elements[2] as HtmlBlock).Content);
        }

        [TestMethod]
        public void TestCase_136()
        {
            var html = "<del>*foo*</del>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inlines = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineHtml, "<del>"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineHtml, "</del>"));

            inlines.AssertEqual((doc.Elements[0] as LeafElement).Inlines);
        }

        [TestMethod]
        public void TestCase_137()
        {
            var html = "<pre language=\"haskell\"><code>\r\nimport Text.HTML.TagSoup\r\n\r\n" +
                "main :: IO ()\r\nmain = print $ parseTags tags\r\n</code></pre>\r\nokay";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<pre language=\"haskell\"><code>\r\nimport Text.HTML.TagSoup\r\n\r\n" +
                "main :: IO ()\r\nmain = print $ parseTags tags\r\n</code></pre>",
                (doc.Elements[0] as HtmlBlock).Content);

            var inlines = new InlineStructure(InlineElementType.InlineText, "okay");
            inlines.AssertEqual((doc.Elements[1] as LeafElement).Inlines);
        }

        [TestMethod]
        public void TestCase_138()
        {
            var html = "<script type=\"text/javascript\">\r\n// JavaScript example\r\n\r\n" +
                "document.getElementById(\"demo\").innerHTML=\"Hello JavaScript!\";\r\n</script>\r\nokay";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<script type=\"text/javascript\">\r\n// JavaScript example\r\n\r\n" +
                "document.getElementById(\"demo\").innerHTML=\"Hello JavaScript!\";\r\n</script>",
                doc.Elements[0].Content);

            var inlines = new InlineStructure(InlineElementType.InlineText, "okay");
            inlines.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_139()
        {
            var html = "<style\r\n  type=\"text/css\">\r\nh1 {color:red;}\r\n\r\n" +
                "p {color:blue;}\r\n</style>\r\nokay";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(
                "<style\r\n  type=\"text/css\">\r\nh1 {color:red;}\r\n\r\np {color:blue;}\r\n</style>",
                doc.Elements[0].Content);
            var inlines = new InlineStructure(InlineElementType.InlineText, "okay");
            inlines.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_140()
        {
            var html = "<style\r\n  type=\"text/css\">\r\n\r\nfoo";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<style\r\n  type=\"text/css\">\r\n\r\nfoo",
                doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_141()
        {
            var html = "> <div>\r\n> foo\r\n\r\nbar";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.HtmlBlock));
            blocks.AssertTypeEqual(doc.Elements[0]);

            Assert.AreEqual("<div>\r\nfoo", doc.Elements[0].GetChildren()[0].Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "bar");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_142()
        {
            var html = "- <div>\n- foo";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.HtmlBlock)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            Assert.AreEqual("<div>", doc.Elements[0].GetChild(0).GetChild(0).Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetChild(1).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_143()
        {
            var html = "<style>p{color:red;}</style>\n*foo*";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<style>p{color:red;}</style>", doc.Elements[0].Content);
            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_144()
        {
            var html = "<!-- foo -->*bar*\n*baz*";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<!-- foo -->*bar*", doc.Elements[0].Content);
            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_145()
        {
            var html = "<script>\r\nfoo\r\n</script>1. *bar*";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_146()
        {
            var html = "<!-- Foo\r\n\r\nbar\r\n   baz -->\r\nokay";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<!-- Foo\r\n\r\nbar\r\n   baz -->", doc.Elements[0].Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "okay");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_147()
        {
            var html = "<?php\r\n\r\n  echo '>';\r\n\r\n?>\r\nokay";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<?php\r\n\r\n  echo '>';\r\n\r\n?>", doc.Elements[0].Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "okay");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_148()
        {
            var html = "<!DOCTYPE html>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<!DOCTYPE html>", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_149()
        {
            var html = "<![CDATA[\r\nfunction matchwo(a,b)\r\n{\r\n  if (a < b && a < 0) then {\r\n" +
                "    return 1;\r\n\r\n  } else {\r\n\r\n    return 0;\r\n  }\r\n}\r\n]]>\r\nokay";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<![CDATA[\r\nfunction matchwo(a,b)\r\n{\r\n" +
                "  if (a < b && a < 0) then {\r\n    return 1;\r\n\r\n  } else {\r\n\r\n" +
                "    return 0;\r\n  }\r\n}\r\n]]>", doc.Elements[0].Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "okay");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_150()
        {
            var html = "  <!-- foo -->\r\n\r\n    <!-- foo -->";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[1].Type);
            Assert.AreEqual("  <!-- foo -->", doc.Elements[0].Content);
            Assert.AreEqual("<!-- foo -->", doc.Elements[1].Content);
        }

        [TestMethod]
        public void TestCase_151()
        {
            var html = "  <div>\r\n\r\n    <div>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[1].Type);
            Assert.AreEqual("  <div>", doc.Elements[0].Content);
            Assert.AreEqual("<div>", doc.Elements[1].Content);
        }

        [TestMethod]
        public void TestCase_152()
        {
            var html = "Foo\r\n<div>\r\nbar\r\n</div>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[1].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("<div>\r\nbar\r\n</div>", doc.Elements[1].Content);
        }

        [TestMethod]
        public void TestCase_153()
        {
            var html = "<div>\r\nbar\r\n</div>\r\n*foo*";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_154()
        {
            var html = "Foo\r\n<a href=\"bar\">\r\nbaz";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"bar\">"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_155()
        {
            var html = "<div>\r\n\r\n*Emphasized* text.\r\n\r\n</div>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[2].Type);
            Assert.AreEqual("<div>", doc.Elements[0].Content);
            var inline = new InlineStructure(InlineElementType.SoftLineBreak,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "Emphasized")),
                new InlineStructure(InlineElementType.InlineText, " text."));
            inline.AssertEqual(doc.Elements[1].GetInlines());
            Assert.AreEqual("</div>", doc.Elements[2].Content);
        }

        [TestMethod]
        public void TestCase_156()
        {
            var html = "<div>\r\n*Emphasized* text.\r\n</div>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_157()
        {
            var html = "<table>\r\n\r\n<tr>\r\n\r\n<td>\r\nHi\r\n</td>\r\n\r\n</tr>\r\n\r\n</table>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(5, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[3].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[4].Type);
            Assert.AreEqual("<table>", doc.Elements[0].Content);
            Assert.AreEqual("<tr>", doc.Elements[1].Content);
            Assert.AreEqual("<td>\r\nHi\r\n</td>", doc.Elements[2].Content);
            Assert.AreEqual("</tr>", doc.Elements[3].Content);
            Assert.AreEqual("</table>", doc.Elements[4].Content);
        }

        [TestMethod]
        public void TestCase_158()
        {
            var html = "<table>\r\n\r\n  <tr>\r\n\r\n    <td>\r\n      Hi\r\n" +
                "    </td>\r\n\r\n  </tr>\r\n\r\n</table>";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(5, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[3].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[4].Type);
            Assert.AreEqual("<table>", doc.Elements[0].Content);
            Assert.AreEqual("  <tr>", doc.Elements[1].Content);
            Assert.AreEqual("<td>\r\n  Hi\r\n</td>", doc.Elements[2].Content);
            Assert.AreEqual("  </tr>", doc.Elements[3].Content);
            Assert.AreEqual("</table>", doc.Elements[4].Content);
        }

        #endregion

        #region Link reference definitions

        [TestMethod]
        public void TestCase_159()
        {
            var html = "[foo]: /url \"title\"\r\n\r\n[foo]";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_160()
        {
            var html = "   [foo]: \r\n      /url  \r\n           'the title'  \r\n\r\n[foo]";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_161()
        {
            var html = "[Foo*bar\\]]:my_(url) 'title (with parens)'\r\n\r\n[Foo*bar\\]]";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_162()
        {
            var html = "[Foo bar]:\r\n<my%20url>\r\n'title'\r\n\r\n[Foo bar]";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_163()
        {
            var html = "[foo]: /url '\r\ntitle\r\nline1\r\nline2\r\n'\r\n\r\n[foo]";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_164()
        {
            var html = "[foo]: /url 'title\r\n\r\nwith blank line'\r\n\r\n[foo]";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_165()
        {
            var html = "[foo]:\r\n/url\r\n\r\n[foo]";
            var doc = MarkdownParser.Parse(html);
            var label = "foo";
            var dest = "/url";
            string title = null;
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
        public void TestCase_167()
        {
            var html = "[foo]: /url\\bar\\*baz \"foo\\\"bar\\baz\"\r\n\r\n[foo]";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_168()
        {
            var html = "[foo]\r\n\r\n[foo]: url";
            var doc = MarkdownParser.Parse(html);
            var label = "foo";
            var dest = "url";
            string title = null;
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
        public void TestCase_169()
        {
            var html = "[foo]\r\n\r\n[foo]: first\r\n[foo]: second";
            var doc = MarkdownParser.Parse(html);
            var label = "foo";
            var dest = "first";
            string title = null;
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
        public void TestCase_170()
        {
            var html = "[FOO]: /url\r\n\r\n[Foo]";
            var doc = MarkdownParser.Parse(html);
            var label = "Foo";
            var dest = "/url";
            string title = null;
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
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
        public void TestCase_171()
        {
            var html = "[ΑΓΩ]: /φου\r\n\r\n[αγω]";
            var doc = MarkdownParser.Parse(html);
            var label = "αγω";
            var dest = "/%CF%86%CE%BF%CF%85";
            string title = null;
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
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
            var html = "[foo]: /url";
            var doc = MarkdownParser.Parse(html);
            var label = "foo";
            var dest = "/url";
            string title = null;
            Assert.AreEqual(0, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(title, doc.LinkDefinition[label].Title);
        }

        [TestMethod]
        public void TestCase_173()
        {
            var html = "[\r\nfoo\r\n]: /url\r\nbar";
            var doc = MarkdownParser.Parse(html);
            var label = "foo";
            var dest = "/url";
            string title = null;
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(title, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "bar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_174()
        {
            var html = "[foo]: /url \"title\" ok";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "[foo]: /url \"title\" ok");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_175()
        {
            var html = "[foo]: /url\r\n\"title\" ok";
            var doc = MarkdownParser.Parse(html);
            var label = "foo";
            var dest = "/url";
            string title = null;
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(title, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "\"title\" ok");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_176()
        {
            var html = "    [foo]: /url \"title\"\r\n\r\n[foo]";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("[foo]: /url \"title\"", doc.Elements[0].Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "[foo]");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_177()
        {
            var html = "```\r\n[foo]: /url\r\n```\r\n\r\n[foo]";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("[foo]: /url", doc.Elements[0].Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "[foo]");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_178()
        {
            var html = "Foo\r\n[bar]: /baz\r\n\r\n[bar]";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_179()
        {
            var html = "# [Foo]\r\n[foo]: /url\r\n> bar";
            var doc = MarkdownParser.Parse(html);
            var label = "foo";
            var dest = "/url";
            string title = null;
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(title, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.AtxHeading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.BlockQuote, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].GetChild(0).Type);
            var inline0 = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "Foo"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual(title, (doc.Elements[0].GetInline(0) as Link).Title);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(doc.Elements[1].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_180()
        {
            var html = "[foo]: /foo-url \"foo\"\r\n[bar]: /bar-url\r\n  \"bar\"\r\n" +
                "[baz]: /baz-url\r\n\r\n[foo],\r\n[bar],\r\n[baz]";
            var doc = MarkdownParser.Parse(html);
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
        }

        [TestMethod]
        public void TestCase_181()
        {
            var html = "[foo]\r\n\r\n> [foo]: /url";
            var doc = MarkdownParser.Parse(html);
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

        #endregion

        #region Paragraph

        [TestMethod]
        public void TestCase_182()
        {
            var html = "aaa\r\n\r\nbbb";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "aaa");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_183()
        {
            var html = "aaa\r\nbbb\r\n\r\nccc\r\nddd";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "aaa"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bbb"));
            var inline1 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "ccc"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "ddd"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_184()
        {
            var html = "aaa\r\n\r\n\r\nbbb";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "aaa");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            inline1.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_185()
        {
            var html = "  aaa\r\n bbb";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "aaa"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bbb"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_186()
        {
            var html = "aaa\r\n             bbb\r\n                                       ccc";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "aaa"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bbb"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "ccc"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_187()
        {
            var html = "   aaa\r\nbbb";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "aaa"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bbb"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_188()
        {
            var html = "    aaa\r\nbbb";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("aaa", doc.Elements[0].Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_189()
        {
            var html = "aaa     \r\nbbb     ";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "aaa"),
                new InlineStructure(InlineElementType.HardLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bbb"));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        #endregion

        #region Blank line

        [TestMethod]
        public void TestCase_190()
        {
            var html = "\r\n\r\n  \r\n\r\naaa\r\n  \r\n\r\n# aaa\r\n\r\n  \r\n\r\n";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "aaa");
            inline.AssertEqual(doc.Elements[0].GetInlines());
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        #endregion

        #region Block quote

        [TestMethod]
        public void TestCase_191()
        {
            var html = "> # Foo\r\n> bar\r\n> baz";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.AtxHeading),
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
        public void TestCase_192()
        {
            var html = "># Foo\r\n>bar\r\n> baz";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.AtxHeading),
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
        public void TestCase_193()
        {
            var html = "   > # Foo\r\n   > bar\r\n > baz";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.AtxHeading),
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
        public void TestCase_194()
        {
            var html = "    > # Foo\r\n    > bar\r\n    > baz";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("> # Foo\r\n> bar\r\n> baz", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_195()
        {
            var html = "> # Foo\r\n> bar\r\nbaz";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.AtxHeading),
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
        public void TestCase_196()
        {
            var html = "> bar\r\nbaz\r\n> foo";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_197()
        {
            var html = "> foo\r\n---";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_198()
        {
            var html = "> - foo\r\n- bar";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_199()
        {
            var html = ">     foo\r\n    bar";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block1 = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.IndentedCodeBlock));
            block1.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[1].Type);
            Assert.AreEqual("foo", doc.Elements[0].GetChild(0).Content);
            Assert.AreEqual("bar", doc.Elements[1].Content);
        }

        [TestMethod]
        public void TestCase_200()
        {
            var html = "> ```\r\nfoo\r\n```";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block1 = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.FencedCodeBlock));
            block1.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[2].Type);
            Assert.AreEqual("", doc.Elements[0].GetChild(0).Content);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[1].GetInlines());

            Assert.AreEqual("", doc.Elements[2].Content);
        }

        [TestMethod]
        public void TestCase_201()
        {
            var html = "> foo\r\n    - bar";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_202()
        {
            var html = ">";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.BlockQuote, doc.Elements[0].Type);
            Assert.AreEqual(0, doc.Elements[0].GetChildren().Count);
        }

        [TestMethod]
        public void TestCase_203()
        {
            var html = ">\r\n>  \r\n> ";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.BlockQuote, doc.Elements[0].Type);
            Assert.AreEqual(0, doc.Elements[0].GetChildren().Count);
        }

        [TestMethod]
        public void TestCase_204()
        {
            var html = ">\r\n> foo\r\n>  ";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());

        }

        [TestMethod]
        public void TestCase_205()
        {
            var html = "> foo\r\n\r\n> bar";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_206()
        {
            var html = "> foo\r\n> bar";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_207()
        {
            var html = "> foo\r\n>\r\n> bar";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_208()
        {
            var html = "foo\r\n> bar";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_209()
        {
            var html = "> aaa\r\n***\r\n> bbb";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThemanticBreak, doc.Elements[1].Type);
            blocks.AssertTypeEqual(doc.Elements[2]);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "aaa");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
            inline1.AssertEqual(doc.Elements[2].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_210()
        {
            var html = "> bar\r\nbaz";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_211()
        {
            var html = "> bar\r\n\r\nbaz";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_212()
        {
            var html = "> bar\r\n>\r\nbaz";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_213()
        {
            var html = "> > > foo\r\nbar";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_214()
        {
            var html = ">>> foo\r\n> bar\r\n>>baz";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_215()
        {
            var html = ">     code\r\n\r\n>    not code";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks0 = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.IndentedCodeBlock));
            blocks0.AssertTypeEqual(doc.Elements[0]);
            var blocks1 = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks1.AssertTypeEqual(doc.Elements[1]);

            Assert.AreEqual("code", doc.Elements[0].GetChild(0).Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "not code");
            inline.AssertEqual(doc.Elements[1].GetChild(0).GetInlines());
        }

        #endregion

        #region List item

        [TestMethod]
        public void TestCase_216()
        {
            var html = "A paragraph\r\nwith two lines.\r\n\r\n    indented code\r\n\r\n> A block quote.";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[1].Type);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[2]);


            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("indented code", doc.Elements[1].Content);
            var inline2 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline2.AssertEqual(doc.Elements[2].GetChild(0).GetInlines());
        }


        [TestMethod]
        public void TestCase_218()
        {
            var html = "- one\r\n\r\n two";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_219()
        {
            var html = "- one\r\n\r\n  two";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_220()
        {
            var html = " -    one\r\n\r\n     two";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[1].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "one");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());
            Assert.AreEqual(" two", doc.Elements[1].Content);
        }

        [TestMethod]
        public void TestCase_221()
        {
            var html = " -    one\r\n\r\n      two";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_222()
        {
            var html = "   > > 1.  one\r\n>>\r\n>>     two";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_223()
        {
            var html = ">>- one\r\n>>\r\n  >  > two";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_224()
        {
            var html = "-one\r\n\r\n2.two";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_225()
        {
            var html = "- foo\r\n\r\n\r\n  bar";
            var doc = MarkdownParser.Parse(html);
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
        public void TestCase_226()
        {
            var html = "1.  foo\r\n\r\n    ```\r\n    bar\r\n    ```\r\n\r\n" +
                "    baz\r\n\r\n    > bam";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.FencedCodeBlock),
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual("bar", listItem.GetChild(1).Content);
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline2.AssertEqual(listItem.GetChild(2).GetInlines());
            var inline3 = new InlineStructure(InlineElementType.InlineText, "bam");
            inline3.AssertEqual(listItem.GetChild(3).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_227()
        {
            var html = "- Foo\n\n      bar\n\n\n      baz";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual("bar\r\n\r\n\r\nbaz", listItem.GetChild(1).Content);
        }

        [TestMethod]
        public void TestCase_228()
        {
            var html = "123456789. ok";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = (ListItem)doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "ok");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual(123456789, listItem.Index);
            Assert.AreEqual(123456789, (doc.Elements[0] as ListBlock).StartIndex);

        }

        [TestMethod]
        public void TestCase_229()
        {
            var html = "1234567890. not ok";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, html);
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_230()
        {
            var html = "0. ok";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = (ListItem)doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "ok");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual(0, listItem.Index);
            Assert.AreEqual(0, (doc.Elements[0] as ListBlock).StartIndex);
        }

        [TestMethod]
        public void TestCase_231()
        {
            var html = "003. ok";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = (ListItem)doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "ok");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual(3, listItem.Index);
            Assert.AreEqual(3, (doc.Elements[0] as ListBlock).StartIndex);
        }

        [TestMethod]
        public void TestCase_232()
        {
            var html = "-1. not ok";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, html);
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_233()
        {
            var html = "- foo\n\n      bar";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual("bar", listItem.GetChild(1).Content);
        }

        [TestMethod]
        public void TestCase_234()
        {
            var html = "  10.  foo\n\n           bar";
            var doc = MarkdownParser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            Assert.AreEqual("bar", listItem.GetChild(1).Content);
            Assert.AreEqual(10, (doc.Elements[0] as ListBlock).StartIndex);
        }

        [TestMethod]
        public void TestCase_235()
        {
            var code = "    indented code\n\nparagraph\n\n    more code";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[2].Type);

            Assert.AreEqual("indented code", doc.Elements[0].Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "paragraph");
            inline.AssertEqual(doc.Elements[1].GetInlines());
            Assert.AreEqual("more code", doc.Elements[2].Content);
        }

        [TestMethod]
        public void TestCase_236()
        {
            var code = "1.     indented code\n\n   paragraph\n\n       more code";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock),
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            Assert.AreEqual("indented code", listItem.GetChild(0).Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "paragraph");
            inline.AssertEqual(listItem.GetChild(1).GetInlines());
            Assert.AreEqual("more code", listItem.GetChild(2).Content);
        }

        [TestMethod]
        public void TestCase_237()
        {
            var code = "1.      indented code\n\n   paragraph\n\n       more code";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock),
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);
            Assert.AreEqual(" indented code", listItem.GetChild(0).Content);
            var inline = new InlineStructure(InlineElementType.InlineText, "paragraph");
            inline.AssertEqual(listItem.GetChild(1).GetInlines());
            Assert.AreEqual("more code", listItem.GetChild(2).Content);
        }

        [TestMethod]
        public void TestCase_238()
        {
            var code = "   foo\n\nbar";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_239()
        {
            var code = "-    foo\n\n  bar";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_240()
        {
            var code = "-  foo\n\n   bar";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_241()
        {
            var code = "-\n  foo\n-\n  ```\n  bar\n  ```\n-\n      baz";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.FencedCodeBlock)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());
            Assert.AreEqual("bar", doc.Elements[0].GetChild(1).GetChild(0).Content);
            Assert.AreEqual("baz", doc.Elements[0].GetChild(2).GetChild(0).Content);
        }

        [TestMethod]
        public void TestCase_242()
        {
            var code = "-   \n  foo\n";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_243()
        {
            var code = "-\n\n  foo\n";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_244()
        {
            var code = "- foo\n-\n- bar";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_245()
        {
            var code = "- foo\n-   \n- bar";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_246()
        {
            var code = "1. foo\n2.\n3. bar";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_247()
        {
            var code = "*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem));
            blocks.AssertTypeEqual(doc.Elements[0]);
        }

        [TestMethod]
        public void TestCase_248()
        {
            var code = "\n\nfoo\n*\n\nfoo\n1.\n\n";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_249()
        {
            var code = " 1.  A paragraph\n     with two lines.\n\n         indented code\n\n     > A block quote.";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("indented code", listItem.GetChild(1).Content);

            var inline1 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline1.AssertEqual(listItem.GetChild(2).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_250()
        {
            var code = "  1.  A paragraph\n      with two lines.\n\n          indented code\n\n      > A block quote.";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("indented code", listItem.GetChild(1).Content);

            var inline1 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline1.AssertEqual(listItem.GetChild(2).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_251()
        {
            var code = "   1.  A paragraph\n       with two lines.\n\n           indented code\n\n       > A block quote.";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("indented code", listItem.GetChild(1).Content);

            var inline1 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline1.AssertEqual(listItem.GetChild(2).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_252()
        {
            var code = "    1.  A paragraph\n        with two lines.\n\n            indented code\n\n        > A block quote.";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);

            Assert.AreEqual("1.  A paragraph\r\n    with two lines.\r\n\r\n        indented code\r\n\r\n    > A block quote.",
                doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_253()
        {
            var code = "  1.  A paragraph\nwith two lines.\n\n          indented code\n\n      > A block quote.";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.IndentedCodeBlock),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "A paragraph"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "with two lines."));
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("indented code", listItem.GetChild(1).Content);

            var inline1 = new InlineStructure(InlineElementType.InlineText, "A block quote.");
            inline1.AssertEqual(listItem.GetChild(2).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_254()
        {
            var code = "  1.  A paragraph\n    with two lines.";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_255()
        {
            var code = "> 1. > Blockquote\ncontinued here.";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_256()
        {
            var code = "> 1. > Blockquote\n> continued here.";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_257()
        {
            var code = "- foo\n  - bar\n    - baz\n      - boo\n";
            var doc = MarkdownParser.Parse(code);
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
            var para3 = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(0);

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
        public void TestCase_258()
        {
            var code = "- foo\n - bar\n  - baz\n   - boo";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_259()
        {
            var code = "10) foo\n    - bar";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_260()
        {
            var code = "10) foo\n   - bar";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_261()
        {
            var code = "- - foo";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_262()
        {
            var code = "1. - 2. foo";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_263()
        {
            var code = "- # Foo\n- Bar\n  ---\n  baz";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.AtxHeading)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.SetextHeading),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var head0 = doc.Elements[0].GetChild(0).GetChild(0) as AtxHeaderElement;
            Assert.AreEqual(1, head0.HeaderLevel);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline0.AssertEqual(head0.GetInlines());

            var head1 = doc.Elements[0].GetChild(1).GetChild(0) as SetextHeader;
            Assert.AreEqual(2, head1.HeaderLevel);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "Bar");
            inline1.AssertEqual(head1.GetInlines());

            var para0 = doc.Elements[0].GetChild(1).GetChild(1);
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline2.AssertEqual(para0.GetInlines());
        }

        #endregion

        #region List

        [TestMethod]
        public void TestCase_264()
        {
            var code = "- foo\n- bar\n+ baz";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks0 = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks0.AssertTypeEqual(doc.Elements[0]);

            var blocks1 = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks1.AssertTypeEqual(doc.Elements[1]);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(para0.GetInlines());

            var para1 = doc.Elements[0].GetChild(1).GetChild(0);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(para1.GetInlines());

            var para2 = doc.Elements[1].GetChild(0).GetChild(0);
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline2.AssertEqual(para2.GetInlines());
        }

        [TestMethod]
        public void TestCase_265()
        {
            var code = "1. foo\n2. bar\n3) baz";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks0 = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks0.AssertTypeEqual(doc.Elements[0]);

            var blocks1 = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks1.AssertTypeEqual(doc.Elements[1]);
            Assert.AreEqual(3, (doc.Elements[1] as ListBlock).StartIndex);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(para0.GetInlines());

            var para1 = doc.Elements[0].GetChild(1).GetChild(0);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(para1.GetInlines());

            var para2 = doc.Elements[1].GetChild(0).GetChild(0);
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline2.AssertEqual(para2.GetInlines());
        }

        [TestMethod]
        public void TestCase_266()
        {
            var code = "Foo\n- bar\n- baz";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var blocks1 = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks1.AssertTypeEqual(doc.Elements[1]);

            var para0 = doc.Elements[0];
            var inline0 = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline0.AssertEqual(para0.GetInlines());

            var para1 = doc.Elements[1].GetChild(0).GetChild(0);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(para1.GetInlines());

            var para2 = doc.Elements[1].GetChild(1).GetChild(0);
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline2.AssertEqual(para2.GetInlines());
        }

        [TestMethod]
        public void TestCase_267()
        {
            var code = "The number of windows in my house is\n14.  The number of doors is 6.";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var para0 = doc.Elements[0];
            var inline0 = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "The number of windows in my house is"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "14.  The number of doors is 6."));
            inline0.AssertEqual(para0.GetInlines());
        }

        [TestMethod]
        public void TestCase_268()
        {
            var code = "The number of windows in my house is\n1.  The number of doors is 6.";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var blocks1 = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks1.AssertTypeEqual(doc.Elements[1]);

            var para0 = doc.Elements[0];
            var inline0 = new InlineStructure(InlineElementType.InlineText, "The number of windows in my house is");
            inline0.AssertEqual(para0.GetInlines());

            var para1 = doc.Elements[1].GetChild(0).GetChild(0);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "The number of doors is 6.");
            inline1.AssertEqual(para1.GetInlines());
        }

        [TestMethod]
        public void TestCase_269()
        {
            var code = "- foo\n\n- bar\n\n\n- baz";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
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

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");

            inline0.AssertEqual(para0.GetInlines());
            inline1.AssertEqual(para1.GetInlines());
            inline2.AssertEqual(para2.GetInlines());
        }

        [TestMethod]
        public void TestCase_270()
        {
            var code = "- foo\n  - bar\n    - baz\n\n\n      bim";
            var doc = MarkdownParser.Parse(code);
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
                                    new BlockElementStructure(BlockElementType.Paragraph)))))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var para1 = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(0);
            var para2 = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(0);
            var para3 = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(1);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            var inline3 = new InlineStructure(InlineElementType.InlineText, "bim");

            inline0.AssertEqual(para0.GetInlines());
            inline1.AssertEqual(para1.GetInlines());
            inline2.AssertEqual(para2.GetInlines());
            inline3.AssertEqual(para3.GetInlines());
        }

        [TestMethod]
        public void TestCase_271()
        {
            var code = "- foo\n- bar\n\n<!-- -->\n\n- baz\n- bim";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[1].Type);
            blocks.AssertTypeEqual(doc.Elements[2]);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var para1 = doc.Elements[0].GetChild(1).GetChild(0);
            var para2 = doc.Elements[2].GetChild(0).GetChild(0);
            var para3 = doc.Elements[2].GetChild(1).GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            var inline3 = new InlineStructure(InlineElementType.InlineText, "bim");

            inline0.AssertEqual(para0.GetInlines());
            inline1.AssertEqual(para1.GetInlines());
            inline2.AssertEqual(para2.GetInlines());
            inline3.AssertEqual(para3.GetInlines());

            Assert.AreEqual("<!-- -->", doc.Elements[1].Content);
        }

        [TestMethod]
        public void TestCase_272()
        {
            var code = "-   foo\n\n    notcode\n\n-   foo\n\n<!-- -->\n\n    code";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[2].Type);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var para1 = doc.Elements[0].GetChild(0).GetChild(1);
            var para2 = doc.Elements[0].GetChild(1).GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "notcode");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "foo");

            inline0.AssertEqual(para0.GetInlines());
            inline1.AssertEqual(para1.GetInlines());
            inline2.AssertEqual(para2.GetInlines());

            Assert.AreEqual("<!-- -->", doc.Elements[1].Content);
            Assert.AreEqual("code", doc.Elements[2].Content);
        }

        [TestMethod]
        public void TestCase_273()
        {
            var code = "- a\n - b\n  - c\n   - d\n    - e\n   - f\n  - g\n - h\n- i";
            var doc = MarkdownParser.Parse(code);
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
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(1).GetChild(0);
            var paraC = doc.Elements[0].GetChild(2).GetChild(0);
            var paraD = doc.Elements[0].GetChild(3).GetChild(0);
            var paraE = doc.Elements[0].GetChild(4).GetChild(0);
            var paraF = doc.Elements[0].GetChild(5).GetChild(0);
            var paraG = doc.Elements[0].GetChild(6).GetChild(0);
            var paraH = doc.Elements[0].GetChild(7).GetChild(0);
            var paraI = doc.Elements[0].GetChild(8).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");
            var inlineD = new InlineStructure(InlineElementType.InlineText, "d");
            var inlineE = new InlineStructure(InlineElementType.InlineText, "e");
            var inlineF = new InlineStructure(InlineElementType.InlineText, "f");
            var inlineG = new InlineStructure(InlineElementType.InlineText, "g");
            var inlineH = new InlineStructure(InlineElementType.InlineText, "h");
            var inlineI = new InlineStructure(InlineElementType.InlineText, "i");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());
            inlineD.AssertEqual(paraD.GetInlines());
            inlineE.AssertEqual(paraE.GetInlines());
            inlineF.AssertEqual(paraF.GetInlines());
            inlineG.AssertEqual(paraG.GetInlines());
            inlineH.AssertEqual(paraH.GetInlines());
            inlineI.AssertEqual(paraI.GetInlines());
        }

        [TestMethod]
        public void TestCase_274()
        {
            var code = "1. a\n\n  2. b\n\n    3. c";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(1).GetChild(0);
            var paraC = doc.Elements[0].GetChild(2).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());
        }

        [TestMethod]
        public void TestCase_275()
        {
            var code = "- a\n- b\n\n- c";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(1).GetChild(0);
            var paraC = doc.Elements[0].GetChild(2).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());
        }

        [TestMethod]
        public void TestCase_276()
        {
            var code = "* a\n*\n\n* c";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraC = doc.Elements[0].GetChild(2).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());
        }

        [TestMethod]
        public void TestCase_277()
        {
            var code = "- a\n- b\n\n  c\n- d";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(1).GetChild(0);
            var paraC = doc.Elements[0].GetChild(1).GetChild(1);
            var paraD = doc.Elements[0].GetChild(2).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");
            var inlineD = new InlineStructure(InlineElementType.InlineText, "d");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());
            inlineD.AssertEqual(paraD.GetInlines());
        }

        [TestMethod]
        public void TestCase_278()
        {
            var code = "- a\n- b\n\n  [ref]: /url\n- d";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsFalse((doc.Elements[0] as ListBlock).IsTight);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(1).GetChild(0);
            var paraD = doc.Elements[0].GetChild(2).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineD = new InlineStructure(InlineElementType.InlineText, "d");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineD.AssertEqual(paraD.GetInlines());

            Assert.AreEqual("/url", doc.LinkDefinition["ref"].Destination);
            Assert.AreEqual(null, doc.LinkDefinition["ref"].Title);
        }

        [TestMethod]
        public void TestCase_279()
        {
            var code = "- a\n- ```\n  b\n\n\n  ```\n- c";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.FencedCodeBlock)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsTrue((doc.Elements[0] as ListBlock).IsTight);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraC = doc.Elements[0].GetChild(2).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());

            Assert.AreEqual("b\r\n\r\n", doc.Elements[0].GetChild(1).GetChild(0).Content);
        }

        [TestMethod]
        public void TestCase_280()
        {
            var code = "- a\n  - b\n\n    c\n- d\n";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph),
                            new BlockElementStructure(BlockElementType.Paragraph)))),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsTrue((doc.Elements[0] as ListBlock).IsTight);
            Assert.IsFalse((doc.Elements[0].GetChild(0).GetChild(1) as ListBlock).IsTight);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(0);
            var paraC = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(1);
            var paraD = doc.Elements[0].GetChild(1).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");
            var inlineD = new InlineStructure(InlineElementType.InlineText, "d");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());
            inlineD.AssertEqual(paraD.GetInlines());
        }

        [TestMethod]
        public void TestCase_281()
        {
            var code = "* a\n  > b\n  >\n* c\n";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph))),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsTrue((doc.Elements[0] as ListBlock).IsTight);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0);
            var paraC = doc.Elements[0].GetChild(1).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());
        }

        [TestMethod]
        public void TestCase_282()
        {
            var code = "- a\n  > b\n  ```\n  c\n  ```\n- d\n";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph)),
                    new BlockElementStructure(BlockElementType.FencedCodeBlock)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsTrue((doc.Elements[0] as ListBlock).IsTight);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0);
            var paraD = doc.Elements[0].GetChild(1).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineD = new InlineStructure(InlineElementType.InlineText, "d");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineD.AssertEqual(paraD.GetInlines());
            Assert.AreEqual("c", doc.Elements[0].GetChild(0).GetChild(2).Content);
        }

        [TestMethod]
        public void TestCase_283()
        {
            var code = "- a";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsTrue((doc.Elements[0] as ListBlock).IsTight);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");

            inlineA.AssertEqual(paraA.GetInlines());
        }

        [TestMethod]
        public void TestCase_284()
        {
            var code = "- a\n  - b";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph)))));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsTrue((doc.Elements[0] as ListBlock).IsTight);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
        }

        [TestMethod]
        public void TestCase_285()
        {
            var code = "1. ```\n   foo\n   ```\n\n   bar\n";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.FencedCodeBlock),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsFalse((doc.Elements[0] as ListBlock).IsTight);

            Assert.AreEqual("foo", doc.Elements[0].GetChild(0).GetChild(0).Content);

            var para1 = doc.Elements[0].GetChild(0).GetChild(1);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(para1.GetInlines());
        }

        [TestMethod]
        public void TestCase_286()
        {
            var code = "* foo\n  * bar\n\n  baz";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph))),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsFalse((doc.Elements[0] as ListBlock).IsTight);
            Assert.IsTrue((doc.Elements[0].GetChild(0).GetChild(1) as ListBlock).IsTight);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var para1 = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(0);
            var para2 = doc.Elements[0].GetChild(0).GetChild(2);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");

            inline0.AssertEqual(para0.GetInlines());
            inline1.AssertEqual(para1.GetInlines());
            inline2.AssertEqual(para2.GetInlines());
        }

        [TestMethod]
        public void TestCase_287()
        {
            var code = "- a\n  - b\n  - c\n\n- d\n  - e\n  - f\n";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph)),
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph)))),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.List,
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph)),
                        new BlockElementStructure(BlockElementType.ListItem,
                            new BlockElementStructure(BlockElementType.Paragraph)))));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsFalse((doc.Elements[0] as ListBlock).IsTight);
            Assert.IsTrue((doc.Elements[0].GetChild(0).GetChild(1) as ListBlock).IsTight);
            Assert.IsTrue((doc.Elements[0].GetChild(1).GetChild(1) as ListBlock).IsTight);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(0).GetChild(1).GetChild(0).GetChild(0);
            var paraC = doc.Elements[0].GetChild(0).GetChild(1).GetChild(1).GetChild(0);
            var paraD = doc.Elements[0].GetChild(1).GetChild(0);
            var paraE = doc.Elements[0].GetChild(1).GetChild(1).GetChild(0).GetChild(0);
            var paraF = doc.Elements[0].GetChild(1).GetChild(1).GetChild(1).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");
            var inlineD = new InlineStructure(InlineElementType.InlineText, "d");
            var inlineE = new InlineStructure(InlineElementType.InlineText, "e");
            var inlineF = new InlineStructure(InlineElementType.InlineText, "f");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());
            inlineD.AssertEqual(paraD.GetInlines());
            inlineE.AssertEqual(paraE.GetInlines());
            inlineF.AssertEqual(paraF.GetInlines());
        }

        #endregion

        [TestMethod]
        public void TestCase_288()
        {
            var code = "`hi`lo`";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, "hi"),
                new InlineStructure(InlineElementType.InlineText, "lo`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        #region Backslash Escape

        [TestMethod]
        public void TestCase_289()
        {
            var code = "\\!\\\"\\#\\$\\%\\&\\'\\(\\)\\*\\+\\,\\-\\.\\/\\:\\;\\<\\=\\>\\?\\@\\[\\\\\\]\\^\\_\\`\\{\\|\\}\\~";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_290()
        {
            var code = "\\\t\\A\\a\\ \\3\\φ\\«";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "\\\t\\A\\a\\ \\3\\φ\\«");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_291()
        {
            var code = "\\*not emphasized*\n\\<br/> not a tag\n\\[not a link](/foo)\n\\`not code`\n1\\." +
                " not a list\n\\* not a list\n\\# not a heading\n\\[foo]: /url \"not a reference\"";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*not emphasized*"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "<br/> not a tag"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "[not a link](/foo)"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "`not code`"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "1. not a list"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "* not a list"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "# not a heading"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "[foo]: /url \"not a reference\""));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_292()
        {
            var code = "\\\\*emphasis*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "\\"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "emphasis")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_293()
        {
            var code = "foo\\\nbar\n";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.HardLineBreak),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_294()
        {
            var code = "`` \\[\\` ``";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "\\[\\`");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_295()
        {
            var code = "    \\[\\]";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\\[\\]", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_296()
        {
            var code = "~~~\n\\[\\]\n~~~";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\\[\\]", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_297()
        {
            var code = "<http://example.com?find=\\*>";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://example.com?find=\\*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_298()
        {
            var code = "<a href=\"/bar\\/)\">";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<a href=\"/bar\\/)\">", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_299()
        {
            var code = "[foo](/bar\\* \"ti\\*tle\")";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("/bar*", (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual("ti*tle", (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_300()
        {
            var code = "[foo]\n\n[foo]: /bar\\* \"ti\\*tle\"";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("/bar*", (doc.Elements[0].GetInline(0) as Link).Destination);
            Assert.AreEqual("ti*tle", (doc.Elements[0].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_301()
        {
            var code = "``` foo\\+bar\nfoo\n```";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            var block = (FencedCodeBlock)doc.Elements[0];
            Assert.AreEqual("foo", block.Content);
            Assert.AreEqual("foo+bar", block.InfoString);
        }

        #endregion

        #region Entity and numeric character references 

        [TestMethod]
        public void TestCase_302()
        {
            var code = "&nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace;" +
                " &DifferentialD;\n&ClockwiseContourIntegral; &ngE;";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "\x00A0 & © Æ Ď"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "¾ \x210B \x2146"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "∲ ≧̸"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_303()
        {
            var code = "&#35; &#1234; &#992; &#98765432; &#0;";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "# Ӓ Ϡ \xFFFD \xFFFD");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_304()
        {
            var code = "&#X22; &#XD06; &#xcab;";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "\" ആ ಫ");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_305()
        {
            var code = "&nbsp &x; &#; &#x;\n&ThisIsNotDefined; &hi?;";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "&nbsp &x; &#; &#x;"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "&ThisIsNotDefined; &hi?;"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_306()
        {
            var code = "&copy";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "&copy");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_307()
        {
            var code = "&MadeUpEntity;";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "&MadeUpEntity;");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_308()
        {
            var code = "<a href=\"&ouml;&ouml;.html\">";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<a href=\"&ouml;&ouml;.html\">", doc.Elements[0].Content);
        }

        [TestMethod]
        public void TestCase_309()
        {
            var code = "[foo](/f&ouml;&ouml; \"f&ouml;&ouml;\")";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("föö", (doc.Elements[0].GetInline(0) as Link).Title);
            Assert.AreEqual("/f%C3%B6%C3%B6", (doc.Elements[0].GetInline(0) as Link).Destination, true);
        }

        [TestMethod]
        public void TestCase_310()
        {
            var code = "[foo]\n\n[foo]: /f&ouml;&ouml; \"f&ouml;&ouml;\"";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("föö", (doc.Elements[0].GetInline(0) as Link).Title);
            Assert.AreEqual("/f%C3%B6%C3%B6", (doc.Elements[0].GetInline(0) as Link).Destination, true);
        }

        [TestMethod]
        public void TestCase_311()
        {
            var code = "``` f&ouml;&ouml;\nfoo\n```";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo", doc.Elements[0].Content);
            Assert.AreEqual("föö", (doc.Elements[0] as FencedCodeBlock).InfoString);
        }

        [TestMethod]
        public void TestCase_312()
        {
            var code = "`f&ouml;&ouml;`";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "f&ouml;&ouml;");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_313()
        {
            var code = "    f&ouml;f&ouml;";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("f&ouml;f&ouml;", doc.Elements[0].Content);
        }

        #endregion

        #region Code spans

        [TestMethod]
        public void TestCase_314()
        {
            var code = "`foo`";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_315()
        {
            var code = "`` foo ` bar  ``";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo ` bar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_316()
        {
            var code = "` `` `";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "``");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_317()
        {
            var code = "``\nfoo\n``";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_318()
        {
            var code = "`foo   bar\n  baz`";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo bar baz");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_319()
        {
            var code = "`a\xA0\xA0b`";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "a\xA0\xA0b");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_320()
        {
            var code = "`foo `` bar`";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo `` bar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_321()
        {
            var code = "`foo\\`bar`\n";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, "foo\\"),
                new InlineStructure(InlineElementType.InlineText, "bar`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_322()
        {
            var code = "*foo`*`";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*foo"),
                new InlineStructure(InlineElementType.CodeSpan, "*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_323()
        {
            var code = "[not a `link](/foo`)\n";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_324()
        {
            var code = "`<a href=\"`\">`";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, "<a href=\""),
                new InlineStructure(InlineElementType.InlineText, "\">`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_325()
        {
            var code = "<a href=\"`\">`";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"`\">"),
                new InlineStructure(InlineElementType.InlineText, "`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_326()
        {
            var code = "`<http://foo.bar.`baz>`";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, "<http://foo.bar."),
                new InlineStructure(InlineElementType.InlineText, "baz>`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_327()
        {
            var code = "<http://foo.bar.`baz>`";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_328()
        {
            var code = "```foo``";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_329()
        {
            var code = "`foo";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_330()
        {
            var code = "`foo``bar``";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "`foo"),
                new InlineStructure(InlineElementType.CodeSpan, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        #endregion

        #region Emphasis and strong emphasis 

        [TestMethod]
        public void TestCase_331()
        {
            var code = "*foo bar*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_332()
        {
            var code = "a * foo bar*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "a * foo bar*");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_333()
        {
            var code = "a*\"foo\"*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "a*\"foo\"*");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }


        [TestMethod]
        public void TestCase_334()
        {
            var code = "*\xA0a\xA0*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "*\xA0a\xA0*");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_335()
        {
            var code = "foo*bar*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_336()
        {
            var code = "5*6*78";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "5"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "6")),
                new InlineStructure(InlineElementType.InlineText, "78"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_337()
        {
            var code = "_foo bar_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_338()
        {
            var code = "_ foo bar_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "_ foo bar_");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_339()
        {
            var code = "a_\"foo\"_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "a_\"foo\"_");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_340()
        {
            var code = "foo_bar_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_341()
        {
            var code = "5_6_78";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_342()
        {
            var code = "пристаням_стремятся_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_343()
        {
            var code = "aa_\"bb\"_cc";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_344()
        {
            var code = "foo-_(bar)_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo-"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "(bar)")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_345()
        {
            var code = "_foo*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_346()
        {
            var code = "*foo bar *";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_347()
        {
            var code = "*foo bar\n*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*foo bar"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_348()
        {
            var code = "*(*foo)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_349()
        {
            var code = "*(*foo*)*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "("),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, ")"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_350()
        {
            var code = "*foo*bar";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_351()
        {
            var code = "_foo bar _";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_352()
        {
            var code = "_(_foo)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_353()
        {
            var code = "_(_foo_)_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "("),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, ")"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_354()
        {
            var code = "_foo_bar";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_355()
        {
            var code = "_пристаням_стремятся";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_356()
        {
            var code = "_foo_bar_baz_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo_bar_baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_357()
        {
            var code = "_(bar)_.";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "(bar)")),
                new InlineStructure(InlineElementType.InlineText, "."));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_358()
        {
            var code = "**foo bar**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_359()
        {
            var code = "** foo bar**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_360()
        {
            var code = "a**\"foo\"**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_361()
        {
            var code = "foo**bar**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_362()
        {
            var code = "**foo bar**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_363()
        {
            var code = "__ foo bar__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_364()
        {
            var code = "__\nfoo bar__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "__"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "foo bar__"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_365()
        {
            var code = "a__\"foo\"__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_366()
        {
            var code = "foo__bar__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_367()
        {
            var code = "5__6__78";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_368()
        {
            var code = "пристаням__стремятся__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_369()
        {
            var code = "__foo, __bar__, baz__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo, "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, ", baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_370()
        {
            var code = "foo-__(bar)__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo-"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "(bar)")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_371()
        {
            var code = "**foo bar **";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_372()
        {
            var code = "**(**foo)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_373()
        {
            var code = "*(**foo**)*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "("),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, ")"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_374()
        {
            var code = "**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\n*Asclepias physocarpa*)**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "Gomphocarpus ("),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "Gomphocarpus physocarpus")),
                new InlineStructure(InlineElementType.InlineText, ", syn."),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "Asclepias physocarpa")),
                new InlineStructure(InlineElementType.InlineText, ")"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_375()
        {
            var code = "**foo \"*bar*\" foo**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo \""),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, "\" foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_376()
        {
            var code = "**foo**bar";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
            new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_377()
        {
            var code = "__foo bar __";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_378()
        {
            var code = "__(__foo)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_379()
        {
            var code = "_(__foo__)_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "("),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, ")"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_380()
        {
            var code = "__foo__bar";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_381()
        {
            var code = "__пристаням__стремятся";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_382()
        {
            var code = "__foo__bar__baz__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo__bar__baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_383()
        {
            var code = "__(bar)__.";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "(bar)")),
            new InlineStructure(InlineElementType.InlineText, "."));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_384()
        {
            var code = "*foo [bar](/url)*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_385()
        {
            var code = "*foo\nbar*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_386()
        {
            var code = "_foo __bar__ baz_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, " baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_387()
        {
            var code = "_foo _bar_ baz_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, " baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_388()
        {
            var code = "__foo_ bar_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_389()
        {
            var code = "*foo *bar**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_390()
        {
            var code = "*foo **bar** baz*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, " baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_391()
        {
            var code = "*foo**bar**baz*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_392()
        {
            var code = "***foo** bar*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_393()
        {
            var code = "*foo **bar***";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_394()
        {
            var code = "*foo**bar***";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_395()
        {
            var code = "*foo **bar *baz* bim** bop*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar "),
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "baz")),
                    new InlineStructure(InlineElementType.InlineText, " bim")),
                new InlineStructure(InlineElementType.InlineText, " bop"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_396()
        {
            var code = "*foo [*bar*](/url)*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "bar"))));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_397()
        {
            var code = "** is not an empty emphasis";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_398()
        {
            var code = "**** is not an empty strong emphasis";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_399()
        {
            var code = "**foo [bar](/url)**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_400()
        {
            var code = "**foo\nbar**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_401()
        {
            var code = "__foo _bar_ baz__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, " baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_402()
        {
            var code = "__foo __bar__ baz__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, " baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_403()
        {
            var code = "____foo__ bar__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_404()
        {
            var code = "**foo **bar****";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_405()
        {
            var code = "**foo *bar* baz**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, " baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_406()
        {
            var code = "**foo*bar*baz**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_407()
        {
            var code = "***foo* bar**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_408()
        {
            var code = "**foo *bar***";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_409()
        {
            var code = "**foo *bar **baz**\nbim* bop**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar "),
                    new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "baz")),
                    new InlineStructure(InlineElementType.SoftLineBreak),
                    new InlineStructure(InlineElementType.InlineText, "bim")),
                new InlineStructure(InlineElementType.InlineText, " bop"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_410()
        {
            var code = "**foo [*bar*](/url)**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "bar"))));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_411()
        {
            var code = "__ is not an empty emphasis";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_412()
        {
            var code = "____ is not an empty strong emphasis";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_413()
        {
            var code = "foo ***";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_414()
        {
            var code = "foo *\\**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "*")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_415()
        {
            var code = "foo *_*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "_")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_416()
        {
            var code = "foo *****";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_417()
        {
            var code = "foo **\\***";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "*")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_418()
        {
            var code = "foo **_**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "_")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_419()
        {
            var code = "**foo*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_420()
        {
            var code = "*foo**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_421()
        {
            var code = "***foo**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_422()
        {
            var code = "****foo*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "***"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_423()
        {
            var code = "**foo***";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_424()
        {
            var code = "*foo****";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "***"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_425()
        {
            var code = "foo ___";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_426()
        {
            var code = "foo _\\__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "_")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_427()
        {
            var code = "foo _*_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "*")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_428()
        {
            var code = "foo _____";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_429()
        {
            var code = "foo __\\___";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "_")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_430()
        {
            var code = "foo __*__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "*")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_431()
        {
            var code = "__foo_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "_"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_432()
        {
            var code = "_foo__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "_"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_433()
        {
            var code = "___foo__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "_"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_434()
        {
            var code = "____foo_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "___"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_435()
        {
            var code = "__foo___";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "_"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_436()
        {
            var code = "_foo____";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "___"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_437()
        {
            var code = "**foo**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_438()
        {
            var code = "*_foo_*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_439()
        {
            var code = "__foo__";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_440()
        {
            var code = "_*foo*_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_441()
        {
            var code = "****foo****";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_442()
        {
            var code = "____foo____";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_443()
        {
            var code = "******foo******";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo"))));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_444()
        {
            var code = "***foo***";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_445()
        {
            var code = "_____foo_____";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo"))));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_446()
        {
            var code = "*foo _bar* baz_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo _bar")),
                new InlineStructure(InlineElementType.InlineText, " baz_"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_447()
        {
            var code = "*foo __bar *baz bim__ bam*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar *baz bim")),
                new InlineStructure(InlineElementType.InlineText, " bam"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_448()
        {
            var code = "**foo **bar baz**";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "**foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar baz")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_449()
        {
            var code = "*foo *bar baz*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*foo "),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar baz")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_450()
        {
            var code = "*[bar*](/url)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar*")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_451()
        {
            var code = "_foo [bar_](/url)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "_foo "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "bar_")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_452()
        {
            var code = "*<img src=\"foo\" title=\"*\"/>";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.InlineHtml, "<img src=\"foo\" title=\"*\"/>"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_453()
        {
            var code = "**<a href=\"**\">";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "**"),
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"**\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_454()
        {
            var code = "__<a href=\"__\">";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "__"),
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"__\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_455()
        {
            var code = "*a `*`*";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "a "),
                new InlineStructure(InlineElementType.CodeSpan, "*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_456()
        {
            var code = "_a `_`_";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "a "),
                new InlineStructure(InlineElementType.CodeSpan, "_"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_457()
        {
            var code = "**a<http://foo.bar/?q=**>";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "**a"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "http://foo.bar/?q=**")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_458()
        {
            var code = "__a<http://foo.bar/?q=__>";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "__a"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "http://foo.bar/?q=__")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        #endregion

        #region Links

        [TestMethod]
        public void TestCase_459()
        {
            var code = "[link](/uri \"title\")";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_460()
        {
            var code = "[link](/uri)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_461()
        {
            var code = "[link]()";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_462()
        {
            var code = "[link](<>)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_463()
        {
            var code = "[link](/my uri)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_464()
        {
            var code = "[link](</my uri>)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_465()
        {
            var code = "[link](foo\nbar)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_466()
        {
            var code = "[link](<foo\nbar>)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_467()
        {
            var code = "[link](\\(foo\\))";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_468()
        {
            var code = "[link](foo(and(bar)))";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_469()
        {
            var code = "[link](foo\\(and\\(bar\\))";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_470()
        {
            var code = "[link](<foo(and(bar)>)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_471()
        {
            var code = "[link](foo\\)\\:)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_472()
        {
            var code = "[link](#fragment)\n\n[link](http://example.com#fragment)\n\n[link](http://example.com?foo=3#frag)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_473()
        {
            var code = "[link](foo\\bar)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_474()
        {
            var code = "[link](foo%20b&auml;)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_475()
        {
            var code = "[link](\"title\")";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_476()
        {
            var code = "[link](/url \"title\")\n[link](/url 'title')\n[link](/url (title))";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_477()
        {
            var code = "[link](/url \"title \\\"&quot;\")";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_478()
        {
            var code = "[link](/url\xA0\"title\")";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_479()
        {
            var code = "[link](/url \"title \"and\" title\")";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_480()
        {
            var code = "[link](/url 'title \"and\" title')";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_481()
        {
            var code = "[link](   /uri\n  \"title\"  )";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_482()
        {
            var code = "[link] (/uri)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_483()
        {
            var code = "[link [foo [bar]]](/uri)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_484()
        {
            var code = "[link] bar](/uri)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_485()
        {
            var code = "[link [bar](/uri)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_486()
        {
            var code = "[link \\[bar](/uri)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_487()
        {
            var code = "[link *foo **bar** `#`*](/uri)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_488()
        {
            var code = "[![moon](moon.jpg)](/uri)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_489()
        {
            var code = "[foo [bar](/uri)](/uri)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_490()
        {
            var code = "[foo *[bar [baz](/uri)](/uri)*](/uri)";
            var doc = MarkdownParser.Parse(code);
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
        public void TestCase_491()
        {
            var code = "![[[foo](uri1)](uri2)](uri3)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Image,
                new InlineStructure(InlineElementType.InlineText, "[foo](uri2)"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var image = doc.Elements[0].GetInline(0) as Image;

            Assert.AreEqual("uri3", image.Source);
            Assert.AreEqual("[foo](uri2)", image.Alt);
            Assert.AreEqual(null, image.Title);
        }

        [TestMethod]
        public void TestCase_492()
        {
            var code = "*[foo*](/uri)";
            var doc = MarkdownParser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo *bar")));
            var link = doc.Elements[0].GetInline(1) as Link;

            Assert.AreEqual("/uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }



        #endregion
    }
}
