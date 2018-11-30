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
            Assert.AreEqual("<\r\n >", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_089()
        {
            var doc = MarkdownParser.Parse("~~~\n<\n >\n~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("<\r\n >", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_090()
        {
            var doc = MarkdownParser.Parse("``\nfoo\n``");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo");
            inline.AssertEqual((doc.Elements[0] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_091()
        {
            var doc = MarkdownParser.Parse("```\naaa\n~~~\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_092()
        {
            var doc = MarkdownParser.Parse("~~~\naaa\n```\n~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n```", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_093()
        {
            var doc = MarkdownParser.Parse("````\naaa\n```\n``````");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n```", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_094()
        {
            var doc = MarkdownParser.Parse("~~~~\naaa\n~~~\n~~~~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_095()
        {
            var doc = MarkdownParser.Parse("```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("", (doc.Elements[0] as FencedCodeBlock).Content);
        }

        [TestMethod]
        public void TestCase_096()
        {
            var doc = MarkdownParser.Parse("`````\n\n```\naaa");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\r\n```\r\naaa", (doc.Elements[0] as FencedCodeBlock).Content);
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
    }
}
