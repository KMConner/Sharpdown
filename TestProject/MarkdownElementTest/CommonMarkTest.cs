using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class CommonMarkTest
    {
        #region Tabs

        [TestMethod]
        public void TestCase_001()
        {
            var doc = new MarkdownParser().Parse("\tfoo\tbaz\t\tbim");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(1, ((CodeBlock)doc.Elements[0]).Inlines.Count);
            var lit = (LiteralText)((CodeBlock)doc.Elements[0]).Inlines[0];
            Assert.AreEqual(InlineElementType.LiteralText, lit.Type);
            Assert.AreEqual("foo\tbaz\t\tbim", lit.Content);
        }

        [TestMethod]
        public void TestCase_002()
        {
            var doc = new MarkdownParser().Parse("  \tfoo\tbaz\t\tbim");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(1, ((CodeBlock)doc.Elements[0]).Inlines.Count);
            var lit = (LiteralText)((CodeBlock)doc.Elements[0]).Inlines[0];
            Assert.AreEqual(InlineElementType.LiteralText, lit.Type);
            Assert.AreEqual("foo\tbaz\t\tbim", lit.Content);
        }

        [TestMethod]
        public void TestCase_003()
        {
            var doc = new MarkdownParser().Parse("    a\ta\n    ὐ\ta");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(1, ((CodeBlock)doc.Elements[0]).Inlines.Count);
            var lit = (LiteralText)((CodeBlock)doc.Elements[0]).Inlines[0];
            Assert.AreEqual(InlineElementType.LiteralText, lit.Type);
            Assert.AreEqual("a\ta\r\nὐ\ta", lit.Content);
        }

        [TestMethod]
        public void TestCase_004()
        {
            var doc = new MarkdownParser().Parse("  - foo\n\n\tbar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var listItem = doc.Elements[0].GetChild(0) as ListItem;
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            inline1.AssertEqual(listItem.GetChild(1).GetInlines());
        }

        [TestMethod]
        public void TestCase_005()
        {
            var doc = new MarkdownParser().Parse("- foo\n\n\t\tbar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.CodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var listItem = doc.Elements[0].GetChild(0) as ListItem;
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());

            Assert.AreEqual("  bar", (listItem.GetChild(1) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_006()
        {
            var doc = new MarkdownParser().Parse(">\t\tfoo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.CodeBlock));
            blocks.AssertTypeEqual(doc.Elements[0]);

            Assert.AreEqual("  foo", (doc.Elements[0].GetChild(0) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_007()
        {
            var doc = new MarkdownParser().Parse("-\t\tfoo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.CodeBlock)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0) as ListItem;
            Assert.AreEqual("  foo", (listItem.GetChild(0) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_008()
        {
            var doc = new MarkdownParser().Parse("    foo\n\tbar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo\r\nbar", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_009()
        {
            var doc = new MarkdownParser().Parse(" - foo\n   - bar\n\t - baz");
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
                                    new BlockElementStructure(BlockElementType.Paragraph)))))));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem0 = doc.Elements[0].GetChild(0) as ListItem;
            var listItem1 = listItem0.GetChild(1).GetChild(0) as ListItem;
            var listItem2 = listItem1.GetChild(1).GetChild(0) as ListItem;
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline0.AssertEqual(listItem0.GetChild(0).GetInlines());
            inline1.AssertEqual(listItem1.GetChild(0).GetInlines());
            inline2.AssertEqual(listItem2.GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_010()
        {
            var doc = new MarkdownParser().Parse("#\tFoo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);

            Assert.AreEqual("Foo", (doc.Elements[0].GetInline(0) as InlineText).Content);
        }

        [TestMethod]
        public void TestCase_011()
        {
            var doc = new MarkdownParser().Parse("*\t*\t*\t");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        #endregion

        #region Precedence

        [TestMethod]
        public void TestCase_012()
        {
            var doc = new MarkdownParser().Parse("- `one\n- two`");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var listItem = doc.Elements[0].GetChild(0) as ListItem;
            var listItem2 = doc.Elements[0].GetChild(1) as ListItem;
            var inline0 = new InlineStructure(InlineElementType.InlineText, "`one");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "two`");
            inline0.AssertEqual(listItem.GetChild(0).GetInlines());
            inline1.AssertEqual(listItem2.GetChild(0).GetInlines());
        }

        #endregion

        #region Thematic break

        [TestMethod]
        public void TestCase_013()
        {
            var doc = new MarkdownParser().Parse("***\n---\n___");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[2].Type);
        }

        [TestMethod]
        public void TestCase_014()
        {
            var doc = new MarkdownParser().Parse("+++");
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
            var doc = new MarkdownParser().Parse("===");
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
            var doc = new MarkdownParser().Parse("--\n**\n__");
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
            var doc = new MarkdownParser().Parse(" ***\n  ***\n   ***");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[2].Type);
        }

        [TestMethod]
        public void TestCase_018()
        {
            var doc = new MarkdownParser().Parse("    ***");
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
            var doc = new MarkdownParser().Parse("Foo\n    ***");
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
            var doc = new MarkdownParser().Parse("_____________________________________");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_021()
        {
            var doc = new MarkdownParser().Parse(" - - -");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_022()
        {
            var doc = new MarkdownParser().Parse(" **  * ** * ** * **");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_023()
        {
            var doc = new MarkdownParser().Parse("-     -      -      -");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_024()
        {
            var doc = new MarkdownParser().Parse("- - - -    ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
        }

        [TestMethod]
        public void TestCase_025()
        {
            var doc = new MarkdownParser().Parse("_ _ _ _ a\n\na------\n\n---a---");
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
            var doc = new MarkdownParser().Parse(" *-*");
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
            var doc = new MarkdownParser().Parse("- foo\n***\n- bar");
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
            var doc = new MarkdownParser().Parse("Foo\n***\nbar");
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
            var doc = new MarkdownParser().Parse("Foo\n---\nbar");
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
            var doc = new MarkdownParser().Parse("* Foo\n* * *\n* bar");
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
            var doc = new MarkdownParser().Parse("- Foo\n- * * *");
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

        #endregion

        #region ATX heading

        [TestMethod]
        public void TestCase_032()
        {
            var doc = new MarkdownParser().Parse("# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo");
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
            var doc = new MarkdownParser().Parse("####### foo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "####### foo");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_034()
        {
            var doc = new MarkdownParser().Parse("#5 bolt\n\n#hashtag");
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
            var doc = new MarkdownParser().Parse("\\## foo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "## foo");
            inline0.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_036()
        {
            var doc = new MarkdownParser().Parse("# foo *bar* \\*baz\\*");
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
            var doc = new MarkdownParser().Parse("#                  foo                     ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline0.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_038()
        {
            var doc = new MarkdownParser().Parse(" ### foo\n  ## foo\n   # foo");
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
            var doc = new MarkdownParser().Parse("    # foo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("# foo", ((CodeBlock)doc.Elements[0]).Code);
        }

        [TestMethod]
        public void TestCase_040()
        {
            var doc = new MarkdownParser().Parse("foo\n    # bar");
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
            var doc = new MarkdownParser().Parse("## foo ##\n  ###   bar    ###");
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
            var doc = new MarkdownParser().Parse("# foo ##################################\n##### foo ##");
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
            var doc = new MarkdownParser().Parse("### foo ###     ");
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
            var doc = new MarkdownParser().Parse("### foo ### b");
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
            var doc = new MarkdownParser().Parse("# foo#");
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
            var doc = new MarkdownParser().Parse("### foo \\###\n## foo #\\##\n# foo \\#");
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
            var doc = new MarkdownParser().Parse("****\n## foo\n****");
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
            var doc = new MarkdownParser().Parse("Foo bar\n# baz\nBar foo");
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
            var doc = new MarkdownParser().Parse("## \n#\n### ###");
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

        #endregion

        #region Setext heading

        [TestMethod]
        public void TestCase_050()
        {
            var doc = new MarkdownParser().Parse("Foo *bar*\n=========\n\nFoo *bar*\n---------");
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
            var doc = new MarkdownParser().Parse("Foo *bar\nbaz*\n====");
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
            var doc = new MarkdownParser().Parse("Foo\n-------------------------\n\nFoo\n=");
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
        public void TestCase_053()
        {
            var doc = new MarkdownParser().Parse("   Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===");
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
        public void TestCase_054()
        {
            var doc = new MarkdownParser().Parse("    Foo\n    ---\n\n    Foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual("Foo\r\n---\r\n\r\nFoo", ((CodeBlock)doc.Elements[0]).Code);
        }

        [TestMethod]
        public void TestCase_055()
        {
            var doc = new MarkdownParser().Parse("Foo\n   ----      ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_056()
        {
            var doc = new MarkdownParser().Parse("Foo\n    ---");
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
            var doc = new MarkdownParser().Parse("Foo\n= =\n\nFoo\n--- -");
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
        public void TestCase_058()
        {
            var doc = new MarkdownParser().Parse("Foo  \n-----");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_059()
        {
            var doc = new MarkdownParser().Parse("Foo\\\n-----");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(2, ((Heading)doc.Elements[0]).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "Foo\\");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_060()
        {
            var doc = new MarkdownParser().Parse("`Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>");
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
        public void TestCase_061()
        {
            var doc = new MarkdownParser().Parse("> Foo\n---");
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
        public void TestCase_062()
        {
            var doc = new MarkdownParser().Parse("> foo\nbar\n===");
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
            var doc = new MarkdownParser().Parse("- foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var block = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            block.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);

            var inlines = new InlineStructure(InlineElementType.InlineText, "foo");
            inlines.AssertEqual(((Paragraph)((ListItem)((ListBlock)doc.Elements[0]).Children[0]).Children[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_064()
        {
            var doc = new MarkdownParser().Parse("Foo\nBar\n---");
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
        public void TestCase_065()
        {
            var doc = new MarkdownParser().Parse("---\nFoo\n---\nBar\n---\nBaz");
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
        public void TestCase_066()
        {
            var doc = new MarkdownParser().Parse("\n====");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "====");
            inline.AssertEqual(((Paragraph)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_067()
        {
            var doc = new MarkdownParser().Parse("---\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
        }

        [TestMethod]
        public void TestCase_068()
        {
            var doc = new MarkdownParser().Parse("- foo\n-----");
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
        public void TestCase_069()
        {
            var doc = new MarkdownParser().Parse("    foo\n---");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[1].Type);
            Assert.AreEqual("foo", ((CodeBlock)doc.Elements[0]).Code);
        }

        [TestMethod]
        public void TestCase_070()
        {
            var doc = new MarkdownParser().Parse("> foo\n-----");
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
        public void TestCase_071()
        {
            var doc = new MarkdownParser().Parse("\\> foo\n------");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(2, (doc.Elements[0] as Heading).HeaderLevel);
            var inlines = new InlineStructure(InlineElementType.InlineText, "> foo");
            inlines.AssertEqual(((Heading)doc.Elements[0]).Inlines);
        }

        [TestMethod]
        public void TestCase_072()
        {
            var doc = new MarkdownParser().Parse("Foo\n\nbar\n---\nbaz");
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
        public void TestCase_073()
        {
            var doc = new MarkdownParser().Parse("Foo\nbar\n\n---\n\nbaz");
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
        public void TestCase_074()
        {
            var doc = new MarkdownParser().Parse("Foo\nbar\n* * *\nbaz");
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
            var doc = new MarkdownParser().Parse("Foo\nbar\n\\---\nbaz");
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
            var doc = new MarkdownParser().Parse("    a simple\n      indented code block");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("a simple\r\n  indented code block",
                (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_077()
        {
            var doc = new MarkdownParser().Parse("  - foo\n\n    bar");
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
            var doc = new MarkdownParser().Parse("1.  foo\n\n    - bar");
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
            var doc = new MarkdownParser().Parse("    <a/>\n    *hi*\n\n    - one");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("<a/>\r\n*hi*\r\n\r\n- one", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_080()
        {
            var doc = new MarkdownParser().Parse("    chunk1\n\n    chunk2\n  \n \n \n    chunk3");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("chunk1\r\n\r\nchunk2\r\n\r\n\r\n\r\nchunk3",
                (doc.Elements[0] as CodeBlock).Code);
        }


        [TestMethod]
        public void TestCase_081()
        {
            var doc = new MarkdownParser().Parse("    chunk1\n      \n      chunk2");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("chunk1\r\n  \r\n  chunk2", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_082()
        {
            var doc = new MarkdownParser().Parse("Foo\n    bar");
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
            var doc = new MarkdownParser().Parse("    Foo\nbar");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("Foo", (doc.Elements[0] as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "bar");
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_084()
        {
            var doc = new MarkdownParser().Parse("# Heading\n    foo\nHeading\n------\n    foo\n----");
            Assert.AreEqual(5, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[3].Type);
            Assert.AreEqual(BlockElementType.ThematicBreak, doc.Elements[4].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "Heading");
            inline.AssertEqual((doc.Elements[0] as Heading).Inlines);
            Assert.AreEqual("foo", (doc.Elements[1] as CodeBlock).Code);
            inline.AssertEqual((doc.Elements[2] as Heading).Inlines);
            Assert.AreEqual("foo", (doc.Elements[3] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_085()
        {
            var doc = new MarkdownParser().Parse("        foo\n    bar");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("    foo\r\nbar", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_086()
        {
            var doc = new MarkdownParser().Parse("\n    \n    foo\n    ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_087()
        {
            var doc = new MarkdownParser().Parse("    foo  ");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo  ", (doc.Elements[0] as CodeBlock).Code);
        }

        #endregion

        #region Fenced code block

        [TestMethod]
        public void TestCase_089()
        {
            var doc = new MarkdownParser().Parse("```\n<\n >\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("<\r\n >", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_090()
        {
            var doc = new MarkdownParser().Parse("~~~\n<\n >\n~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("<\r\n >", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_091()
        {
            var doc = new MarkdownParser().Parse("``\nfoo\n``");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_092()
        {
            var doc = new MarkdownParser().Parse("```\naaa\n~~~\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_093()
        {
            var doc = new MarkdownParser().Parse("~~~\naaa\n```\n~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n```", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_094()
        {
            var doc = new MarkdownParser().Parse("````\naaa\n```\n``````");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n```", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_095()
        {
            var doc = new MarkdownParser().Parse("~~~~\naaa\n~~~\n~~~~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_096()
        {
            var doc = new MarkdownParser().Parse("```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_097()
        {
            var doc = new MarkdownParser().Parse("`````\n\n```\naaa");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\r\n```\r\naaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_098()
        {
            var doc = new MarkdownParser().Parse("> ```\n> aaa\n\nbbb");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blockStructure = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.CodeBlock));
            blockStructure.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("aaa", ((doc.Elements[0] as BlockQuote).Children[0] as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_099()
        {
            var doc = new MarkdownParser().Parse("```\n\n  \n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\r\n  ", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_100()
        {
            var doc = new MarkdownParser().Parse("```\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_101()
        {
            var doc = new MarkdownParser().Parse(" ```\n aaa\naaa\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\naaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_102()
        {
            var doc = new MarkdownParser().Parse("  ```\naaa\n  aaa\naaa\n  ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\naaa\r\naaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_103()
        {
            var doc = new MarkdownParser().Parse("   ```\n   aaa\n    aaa\n  aaa\n   ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n aaa\r\naaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_104()
        {
            var doc = new MarkdownParser().Parse("    ```\n    aaa\n    ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("```\r\naaa\r\n```", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_105()
        {
            var doc = new MarkdownParser().Parse("```\naaa\n  ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_106()
        {
            var doc = new MarkdownParser().Parse("   ```\naaa\n   ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_107()
        {
            var doc = new MarkdownParser().Parse("```\naaa\n    ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n    ```", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_108()
        {
            var doc = new MarkdownParser().Parse("``` ```\naaa");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.CodeSpan, " "),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "aaa"));
            inline.AssertEqual((doc.Elements[0] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_109()
        {
            var doc = new MarkdownParser().Parse("~~~~~~\naaa\n~~~ ~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~ ~~", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_110()
        {
            var doc = new MarkdownParser().Parse("foo\n```\nbar\n```\nbaz");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline0.AssertEqual((doc.Elements[0] as Paragraph).Inlines);
            Assert.AreEqual("bar", (doc.Elements[1] as CodeBlock).Code);
            inline2.AssertEqual((doc.Elements[2] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_111()
        {
            var doc = new MarkdownParser().Parse("foo\n---\n~~~\nbar\n~~~\n# baz");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[2].Type);
            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "baz");
            inline0.AssertEqual((doc.Elements[0] as LeafElement).Inlines);
            Assert.AreEqual("bar", (doc.Elements[1] as CodeBlock).Code);
            inline2.AssertEqual((doc.Elements[2] as LeafElement).Inlines);
        }

        [TestMethod]
        public void TestCase_112()
        {
            var doc = new MarkdownParser().Parse("```ruby\ndef foo(x)\n  return 3\nend\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("def foo(x)\r\n  return 3\r\nend", (doc.Elements[0] as CodeBlock).Code);
            Assert.AreEqual("ruby", (doc.Elements[0] as CodeBlock).InfoString);
        }

        [TestMethod]
        public void TestCase_113()
        {
            var doc = new MarkdownParser().Parse("~~~~    ruby startline=3 $%@#$\ndef foo(x)\n  return 3\nend\n~~~~~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("def foo(x)\r\n  return 3\r\nend", (doc.Elements[0] as CodeBlock).Code);
            Assert.AreEqual("ruby startline=3 $%@#$", (doc.Elements[0] as CodeBlock).InfoString);
        }

        [TestMethod]
        public void TestCase_114()
        {
            var doc = new MarkdownParser().Parse("````;\n````");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("", (doc.Elements[0] as CodeBlock).Code);
            Assert.AreEqual(";", (doc.Elements[0] as CodeBlock).InfoString);
        }

        [TestMethod]
        public void TestCase_115()
        {
            var doc = new MarkdownParser().Parse("``` aa ```\nfoo");
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
        public void TestCase_116()
        {
            var doc = new MarkdownParser().Parse("~~~ aa ``` ~~~\nfoo\n~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_117()
        {
            var doc = new MarkdownParser().Parse("```\n```aaa\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("```aaa", (doc.Elements[0] as CodeBlock).Code);
        }

        #endregion

        #region HTML block

        [TestMethod]
        public void TestCase_118()
        {
            var doc = new MarkdownParser().Parse(
                "<table><tr><td>\n<pre>\n**Hello**,\n\n_world_.\n</pre>\n</td></tr></table>");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[2].Type);
            Assert.AreEqual("<table><tr><td>\r\n<pre>\r\n**Hello**,", (doc.Elements[0] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "world")),
                new InlineStructure(InlineElementType.InlineText, "."),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineHtml, "</pre>"));
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
            Assert.AreEqual("</td></tr></table>", (doc.Elements[2] as HtmlBlock).Code);
        }


        [TestMethod]
        public void TestCase_119()
        {
            var doc = new MarkdownParser().Parse(
                "<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n\nokay.");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(
                "<table>\r\n  <tr>\r\n    <td>\r\n           hi\r\n    </td>\r\n  </tr>\r\n</table>",
                (doc.Elements[0] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "okay.");
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_120()
        {
            var doc = new MarkdownParser().Parse(" <div>\r\n  *hello*\r\n         <foo><a>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(" <div>\r\n  *hello*\r\n         <foo><a>", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_121()
        {
            var doc = new MarkdownParser().Parse("</div>\r\n*foo*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("</div>\r\n*foo*", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_122()
        {
            var doc = new MarkdownParser().Parse("<DIV CLASS=\"foo\">\n\n*Markdown*\n\n</DIV>");
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[2].Type);
            Assert.AreEqual("<DIV CLASS=\"foo\">", (doc.Elements[0] as HtmlBlock).Code);
            Assert.AreEqual("</DIV>", (doc.Elements[2] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "Markdown"));
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_123()
        {
            var doc = new MarkdownParser().Parse("<div id=\"foo\"\r\n  class=\"bar\">\r\n</div>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div id=\"foo\"\r\n  class=\"bar\">\r\n</div>",
                (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_124()
        {
            var doc = new MarkdownParser().Parse("<div id=\"foo\" class=\"bar\r\n  baz\">\r\n</div>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div id=\"foo\" class=\"bar\r\n  baz\">\r\n</div>",
                (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_125()
        {
            var doc = new MarkdownParser().Parse("<div>\r\n*foo*\r\n\r\n*bar*");
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<div>\r\n*foo*", (doc.Elements[0] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual((doc.Elements[1] as Paragraph).Inlines);
        }

        [TestMethod]
        public void TestCase_126()
        {
            var doc = new MarkdownParser().Parse("<div id=\"foo\"\r\n*hi*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div id=\"foo\"\r\n*hi*", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_127()
        {
            var doc = new MarkdownParser().Parse("<div class\r\nfoo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div class\r\nfoo", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_128()
        {
            var doc = new MarkdownParser().Parse("<div *???-&&&-<---\r\n*foo*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div *???-&&&-<---\r\n*foo*", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_129()
        {
            var doc = new MarkdownParser().Parse("<div><a href=\"bar\">*foo*</a></div>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div><a href=\"bar\">*foo*</a></div>", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_130()
        {
            var doc = new MarkdownParser().Parse("<table><tr><td>\r\nfoo\r\n</td></tr></table>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<table><tr><td>\r\nfoo\r\n</td></tr></table>",
                (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_131()
        {
            const string html = "<div></div>\r\n``` c\r\nint x = 33;\r\n```";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_132()
        {
            const string html = "<a href=\"foo\">\r\n*bar*\r\n</a>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_133()
        {
            const string html = "<Warning>\r\n*bar*\r\n</Warning>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_134()
        {
            const string html = "<i class=\"foo\">\r\n*bar*\r\n</i>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_135()
        {
            const string html = "</ins>\r\n*bar*";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_136()
        {
            const string html = "<del>\r\n*foo*\r\n</del>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_137()
        {
            const string html = "<del>\r\n\r\n*foo*\r\n\r\n</del>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[2].Type);
            Assert.AreEqual("<del>", (doc.Elements[0] as HtmlBlock).Code);
            var inlines = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inlines.AssertEqual((doc.Elements[1] as LeafElement).Inlines);
            Assert.AreEqual("</del>", (doc.Elements[2] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_138()
        {
            const string html = "<del>*foo*</del>";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_139()
        {
            const string html = "<pre language=\"haskell\"><code>\r\nimport Text.HTML.TagSoup\r\n\r\n" +
                                "main :: IO ()\r\nmain = print $ parseTags tags\r\n</code></pre>\r\nokay";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<pre language=\"haskell\"><code>\r\nimport Text.HTML.TagSoup\r\n\r\n" +
                            "main :: IO ()\r\nmain = print $ parseTags tags\r\n</code></pre>",
                (doc.Elements[0] as HtmlBlock).Code);

            var inlines = new InlineStructure(InlineElementType.InlineText, "okay");
            inlines.AssertEqual((doc.Elements[1] as LeafElement).Inlines);
        }

        [TestMethod]
        public void TestCase_140()
        {
            const string html = "<script type=\"text/javascript\">\r\n// JavaScript example\r\n\r\n" +
                                "document.getElementById(\"demo\").innerHTML=\"Hello JavaScript!\";\r\n</script>\r\nokay";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<script type=\"text/javascript\">\r\n// JavaScript example\r\n\r\n" +
                            "document.getElementById(\"demo\").innerHTML=\"Hello JavaScript!\";\r\n</script>",
                (doc.Elements[0] as HtmlBlock).Code);

            var inlines = new InlineStructure(InlineElementType.InlineText, "okay");
            inlines.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_141()
        {
            const string html = "<style\r\n  type=\"text/css\">\r\nh1 {color:red;}\r\n\r\n" +
                                "p {color:blue;}\r\n</style>\r\nokay";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(
                "<style\r\n  type=\"text/css\">\r\nh1 {color:red;}\r\n\r\np {color:blue;}\r\n</style>",
                (doc.Elements[0] as HtmlBlock).Code);
            var inlines = new InlineStructure(InlineElementType.InlineText, "okay");
            inlines.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_142()
        {
            const string html = "<style\r\n  type=\"text/css\">\r\n\r\nfoo";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<style\r\n  type=\"text/css\">\r\n\r\nfoo",
                (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_143()
        {
            const string html = "> <div>\r\n> foo\r\n\r\nbar";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.HtmlBlock));
            blocks.AssertTypeEqual(doc.Elements[0]);

            Assert.AreEqual("<div>\r\nfoo", (doc.Elements[0].GetChildren()[0] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "bar");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_144()
        {
            const string html = "- <div>\n- foo";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.HtmlBlock)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            Assert.AreEqual("<div>", (doc.Elements[0].GetChild(0).GetChild(0) as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetChild(1).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_145()
        {
            const string html = "<style>p{color:red;}</style>\n*foo*";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<style>p{color:red;}</style>", (doc.Elements[0] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_146()
        {
            const string html = "<!-- foo -->*bar*\n*baz*";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<!-- foo -->*bar*", (doc.Elements[0] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_147()
        {
            const string html = "<script>\r\nfoo\r\n</script>1. *bar*";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_148()
        {
            const string html = "<!-- Foo\r\n\r\nbar\r\n   baz -->\r\nokay";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<!-- Foo\r\n\r\nbar\r\n   baz -->", (doc.Elements[0] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "okay");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_149()
        {
            const string html = "<?php\r\n\r\n  echo '>';\r\n\r\n?>\r\nokay";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<?php\r\n\r\n  echo '>';\r\n\r\n?>", (doc.Elements[0] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "okay");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_150()
        {
            const string html = "<!DOCTYPE html>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<!DOCTYPE html>", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_151()
        {
            const string html = "<![CDATA[\r\nfunction matchwo(a,b)\r\n{\r\n  if (a < b && a < 0) then {\r\n" +
                                "    return 1;\r\n\r\n  } else {\r\n\r\n    return 0;\r\n  }\r\n}\r\n]]>\r\nokay";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("<![CDATA[\r\nfunction matchwo(a,b)\r\n{\r\n" +
                            "  if (a < b && a < 0) then {\r\n    return 1;\r\n\r\n  } else {\r\n\r\n" +
                            "    return 0;\r\n  }\r\n}\r\n]]>", (doc.Elements[0] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "okay");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_152()
        {
            const string html = "  <!-- foo -->\r\n\r\n    <!-- foo -->";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[1].Type);
            Assert.AreEqual("  <!-- foo -->", (doc.Elements[0] as HtmlBlock).Code);
            Assert.AreEqual("<!-- foo -->", (doc.Elements[1] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_153()
        {
            const string html = "  <div>\r\n\r\n    <div>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[1].Type);
            Assert.AreEqual("  <div>", (doc.Elements[0] as HtmlBlock).Code);
            Assert.AreEqual("<div>", (doc.Elements[1] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_154()
        {
            const string html = "Foo\r\n<div>\r\nbar\r\n</div>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[1].Type);
            var inline = new InlineStructure(InlineElementType.InlineText, "Foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
            Assert.AreEqual("<div>\r\nbar\r\n</div>", (doc.Elements[1] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_155()
        {
            const string html = "<div>\r\nbar\r\n</div>\r\n*foo*";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_156()
        {
            const string html = "Foo\r\n<a href=\"bar\">\r\nbaz";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_157()
        {
            const string html = "<div>\r\n\r\n*Emphasized* text.\r\n\r\n</div>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(3, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[2].Type);
            Assert.AreEqual("<div>", (doc.Elements[0] as HtmlBlock).Code);
            var inline = new InlineStructure(InlineElementType.SoftLineBreak,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "Emphasized")),
                new InlineStructure(InlineElementType.InlineText, " text."));
            inline.AssertEqual(doc.Elements[1].GetInlines());
            Assert.AreEqual("</div>", (doc.Elements[2] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_158()
        {
            const string html = "<div>\r\n*Emphasized* text.\r\n</div>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_159()
        {
            const string html = "<table>\r\n\r\n<tr>\r\n\r\n<td>\r\nHi\r\n</td>\r\n\r\n</tr>\r\n\r\n</table>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(5, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[3].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[4].Type);
            Assert.AreEqual("<table>", (doc.Elements[0] as HtmlBlock).Code);
            Assert.AreEqual("<tr>", (doc.Elements[1] as HtmlBlock).Code);
            Assert.AreEqual("<td>\r\nHi\r\n</td>", (doc.Elements[2] as HtmlBlock).Code);
            Assert.AreEqual("</tr>", (doc.Elements[3] as HtmlBlock).Code);
            Assert.AreEqual("</table>", (doc.Elements[4] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_160()
        {
            const string html = "<table>\r\n\r\n  <tr>\r\n\r\n    <td>\r\n      Hi\r\n" +
                                "    </td>\r\n\r\n  </tr>\r\n\r\n</table>";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(5, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[2].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[3].Type);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[4].Type);
            Assert.AreEqual("<table>", (doc.Elements[0] as HtmlBlock).Code);
            Assert.AreEqual("  <tr>", (doc.Elements[1] as HtmlBlock).Code);
            Assert.AreEqual("<td>\r\n  Hi\r\n</td>", (doc.Elements[2] as CodeBlock).Code);
            Assert.AreEqual("  </tr>", (doc.Elements[3] as HtmlBlock).Code);
            Assert.AreEqual("</table>", (doc.Elements[4] as HtmlBlock).Code);
        }

        #endregion

        #region Link reference definitions

        [TestMethod]
        public void TestCase_161()
        {
            const string html = "[foo]: /url \"title\"\r\n\r\n[foo]";
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
            var label = "foo";
            var dest = "";
            string title = null;
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
        public void TestCase_170()
        {
            const string html = "[foo]: <bar>(baz)\n\n[foo]";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo]: "),
                new InlineStructure(InlineElementType.InlineHtml, "<bar>"),
                new InlineStructure(InlineElementType.InlineText, "(baz)"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var inline2 = new InlineStructure(InlineElementType.InlineText, "[foo]");
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_171()
        {
            const string html = "[foo]: /url\\bar\\*baz \"foo\\\"bar\\baz\"\r\n\r\n[foo]";
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            const string html = "[foo]: /url\nbar\n===\n[foo]";
            var doc = new MarkdownParser().Parse(html);
            var label = "foo";
            var dest = "/url";
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(label, doc.LinkDefinition[label].Label, true);
            Assert.AreEqual(dest, doc.LinkDefinition[label].Destination);
            Assert.AreEqual(null, doc.LinkDefinition[label].Title);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual(1, (doc.Elements[0] as Heading).HeaderLevel);

            var inline0 = new InlineStructure(InlineElementType.InlineText,"bar");
            inline0.AssertEqual(doc.Elements[0].GetInlines());

            var inline1 = new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo"));
            inline1.AssertEqual(doc.Elements[1].GetInlines());
            Assert.AreEqual(dest, (doc.Elements[1].GetInline(0) as Link).Destination);
            Assert.AreEqual(null, (doc.Elements[1].GetInline(0) as Link).Title);
        }

        [TestMethod]
        public void TestCase_185()
        {
            const string html = "[foo]: /url\n===\n[foo]";
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
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
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(0, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual("foo", doc.LinkDefinition["foo"].Label);
            Assert.AreEqual("/url", doc.LinkDefinition["foo"].Destination);
            Assert.AreEqual(null, doc.LinkDefinition["foo"].Title);
        }

        #endregion

        #region Paragraph

        [TestMethod]
        public void TestCase_189()
        {
            const string html = "aaa\r\n\r\nbbb";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_190()
        {
            const string html = "aaa\r\nbbb\r\n\r\nccc\r\nddd";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_191()
        {
            const string html = "aaa\r\n\r\n\r\nbbb";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_192()
        {
            const string html = "  aaa\r\n bbb";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_193()
        {
            const string html = "aaa\r\n             bbb\r\n                                       ccc";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_194()
        {
            const string html = "   aaa\r\nbbb";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_195()
        {
            const string html = "    aaa\r\nbbb";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);
            Assert.AreEqual("aaa", (doc.Elements[0] as CodeBlock).Code);
            var inline = new InlineStructure(InlineElementType.InlineText, "bbb");
            inline.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_196()
        {
            const string html = "aaa     \r\nbbb     ";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_197()
        {
            const string html = "\r\n\r\n  \r\n\r\naaa\r\n  \r\n\r\n# aaa\r\n\r\n  \r\n\r\n";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_198()
        {
            const string html = "> # Foo\r\n> bar\r\n> baz";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_199()
        {
            const string html = "># Foo\r\n>bar\r\n> baz";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_200()
        {
            const string html = "   > # Foo\r\n   > bar\r\n > baz";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_201()
        {
            const string html = "    > # Foo\r\n    > bar\r\n    > baz";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("> # Foo\r\n> bar\r\n> baz", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_202()
        {
            const string html = "> # Foo\r\n> bar\r\nbaz";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_203()
        {
            const string html = "> bar\r\nbaz\r\n> foo";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_204()
        {
            const string html = "> foo\r\n---";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_205()
        {
            const string html = "> - foo\r\n- bar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_206()
        {
            const string html = ">     foo\r\n    bar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_207()
        {
            const string html = "> ```\r\nfoo\r\n```";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_208()
        {
            const string html = "> foo\r\n    - bar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_209()
        {
            const string html = ">";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.BlockQuote, doc.Elements[0].Type);
            Assert.AreEqual(0, doc.Elements[0].GetChildren().Count);
        }

        [TestMethod]
        public void TestCase_210()
        {
            const string html = ">\r\n>  \r\n> ";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.BlockQuote, doc.Elements[0].Type);
            Assert.AreEqual(0, doc.Elements[0].GetChildren().Count);
        }

        [TestMethod]
        public void TestCase_211()
        {
            const string html = ">\r\n> foo\r\n>  ";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            var blocks = new BlockElementStructure(BlockElementType.BlockQuote,
                new BlockElementStructure(BlockElementType.Paragraph));
            blocks.AssertTypeEqual(doc.Elements[0]);
            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_212()
        {
            const string html = "> foo\r\n\r\n> bar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_213()
        {
            const string html = "> foo\r\n> bar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_214()
        {
            const string html = "> foo\r\n>\r\n> bar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_215()
        {
            const string html = "foo\r\n> bar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_216()
        {
            const string html = "> aaa\r\n***\r\n> bbb";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_217()
        {
            const string html = "> bar\r\nbaz";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_218()
        {
            const string html = "> bar\r\n\r\nbaz";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_219()
        {
            const string html = "> bar\r\n>\r\nbaz";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_220()
        {
            const string html = "> > > foo\r\nbar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_221()
        {
            const string html = ">>> foo\r\n> bar\r\n>>baz";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_222()
        {
            const string html = ">     code\r\n\r\n>    not code";
            var doc = new MarkdownParser().Parse(html);
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

        #endregion

        #region List item

        [TestMethod]
        public void TestCase_223()
        {
            const string html = "A paragraph\r\nwith two lines.\r\n\r\n    indented code\r\n\r\n> A block quote.";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_224()
        {
            const string html = "1.  A paragraph\n    with two lines.\n\n        indented code\n\n    > A block quote.";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_225()
        {
            const string html = "- one\r\n\r\n two";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_226()
        {
            const string html = "- one\r\n\r\n  two";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_227()
        {
            const string html = " -    one\r\n\r\n     two";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_228()
        {
            const string html = " -    one\r\n\r\n      two";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_229()
        {
            const string html = "   > > 1.  one\r\n>>\r\n>>     two";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_230()
        {
            const string html = ">>- one\r\n>>\r\n  >  > two";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_231()
        {
            const string html = "-one\r\n\r\n2.two";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_232()
        {
            const string html = "- foo\r\n\r\n\r\n  bar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_233()
        {
            const string html = "1.  foo\r\n\r\n    ```\r\n    bar\r\n    ```\r\n\r\n" +
                                "    baz\r\n\r\n    > bam";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_234()
        {
            const string html = "- Foo\n\n      bar\n\n\n      baz";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_235()
        {
            const string html = "123456789. ok";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_236()
        {
            const string html = "1234567890. not ok";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, html);
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_237()
        {
            const string html = "0. ok";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_238()
        {
            const string html = "003. ok";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_239()
        {
            const string html = "-1. not ok";
            var doc = new MarkdownParser().Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline0 = new InlineStructure(InlineElementType.InlineText, html);
            inline0.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_240()
        {
            const string html = "- foo\n\n      bar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_241()
        {
            const string html = "  10.  foo\n\n           bar";
            var doc = new MarkdownParser().Parse(html);
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
        public void TestCase_242()
        {
            const string code = "    indented code\n\nparagraph\n\n    more code";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_243()
        {
            const string code = "1.     indented code\n\n   paragraph\n\n       more code";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_244()
        {
            const string code = "1.      indented code\n\n   paragraph\n\n       more code";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_245()
        {
            const string code = "   foo\n\nbar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_246()
        {
            const string code = "-    foo\n\n  bar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_247()
        {
            const string code = "-  foo\n\n   bar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_248()
        {
            const string code = "-\n  foo\n-\n  ```\n  bar\n  ```\n-\n      baz";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_249()
        {
            const string code = "-   \n  foo\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_250()
        {
            const string code = "-\n\n  foo\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_251()
        {
            const string code = "- foo\n-\n- bar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_252()
        {
            const string code = "- foo\n-   \n- bar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_253()
        {
            const string code = "1. foo\n2.\n3. bar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_254()
        {
            const string code = "*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem));
            blocks.AssertTypeEqual(doc.Elements[0]);
        }

        [TestMethod]
        public void TestCase_255()
        {
            const string code = "\n\nfoo\n*\n\nfoo\n1.\n\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_256()
        {
            const string code =
                " 1.  A paragraph\n     with two lines.\n\n         indented code\n\n     > A block quote.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_257()
        {
            const string code =
                "  1.  A paragraph\n      with two lines.\n\n          indented code\n\n      > A block quote.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_258()
        {
            const string code =
                "   1.  A paragraph\n       with two lines.\n\n           indented code\n\n       > A block quote.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_259()
        {
            const string code =
                "    1.  A paragraph\n        with two lines.\n\n            indented code\n\n        > A block quote.";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);

            Assert.AreEqual(
                "1.  A paragraph\r\n    with two lines.\r\n\r\n        indented code\r\n\r\n    > A block quote.",
                (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_260()
        {
            const string code =
                "  1.  A paragraph\nwith two lines.\n\n          indented code\n\n      > A block quote.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_261()
        {
            const string code = "  1.  A paragraph\n    with two lines.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_262()
        {
            const string code = "> 1. > Blockquote\ncontinued here.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_263()
        {
            const string code = "> 1. > Blockquote\n> continued here.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_264()
        {
            const string code = "- foo\n  - bar\n    - baz\n      - boo\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_265()
        {
            const string code = "- foo\n - bar\n  - baz\n   - boo";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_266()
        {
            const string code = "10) foo\n    - bar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_267()
        {
            const string code = "10) foo\n   - bar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_268()
        {
            const string code = "- - foo";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_269()
        {
            const string code = "1. - 2. foo";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_270()
        {
            const string code = "- # Foo\n- Bar\n  ---\n  baz";
            var doc = new MarkdownParser().Parse(code);
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

        #endregion

        #region List

        [TestMethod]
        public void TestCase_271()
        {
            const string code = "- foo\n- bar\n+ baz";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_272()
        {
            const string code = "1. foo\n2. bar\n3) baz";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_273()
        {
            const string code = "Foo\n- bar\n- baz";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_274()
        {
            const string code = "The number of windows in my house is\n14.  The number of doors is 6.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_275()
        {
            const string code = "The number of windows in my house is\n1.  The number of doors is 6.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_276()
        {
            const string code = "- foo\n\n- bar\n\n\n- baz";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_277()
        {
            const string code = "- foo\n  - bar\n    - baz\n\n\n      bim";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_278()
        {
            const string code = "- foo\n- bar\n\n<!-- -->\n\n- baz\n- bim";
            var doc = new MarkdownParser().Parse(code);
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

            Assert.AreEqual("<!-- -->", (doc.Elements[1] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_279()
        {
            const string code = "-   foo\n\n    notcode\n\n-   foo\n\n<!-- -->\n\n    code";
            var doc = new MarkdownParser().Parse(code);
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
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[2].Type);

            var para0 = doc.Elements[0].GetChild(0).GetChild(0);
            var para1 = doc.Elements[0].GetChild(0).GetChild(1);
            var para2 = doc.Elements[0].GetChild(1).GetChild(0);

            var inline0 = new InlineStructure(InlineElementType.InlineText, "foo");
            var inline1 = new InlineStructure(InlineElementType.InlineText, "notcode");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "foo");

            inline0.AssertEqual(para0.GetInlines());
            inline1.AssertEqual(para1.GetInlines());
            inline2.AssertEqual(para2.GetInlines());

            Assert.AreEqual("<!-- -->", (doc.Elements[1] as HtmlBlock).Code);
            Assert.AreEqual("code", (doc.Elements[2] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_280()
        {
            const string code = "- a\n - b\n  - c\n   - d\n  - e\n - f\n- g";
            var doc = new MarkdownParser().Parse(code);
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
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(1).GetChild(0);
            var paraC = doc.Elements[0].GetChild(2).GetChild(0);
            var paraD = doc.Elements[0].GetChild(3).GetChild(0);
            var paraE = doc.Elements[0].GetChild(4).GetChild(0);
            var paraF = doc.Elements[0].GetChild(5).GetChild(0);
            var paraG = doc.Elements[0].GetChild(6).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");
            var inlineD = new InlineStructure(InlineElementType.InlineText, "d");
            var inlineE = new InlineStructure(InlineElementType.InlineText, "e");
            var inlineF = new InlineStructure(InlineElementType.InlineText, "f");
            var inlineG = new InlineStructure(InlineElementType.InlineText, "g");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());
            inlineD.AssertEqual(paraD.GetInlines());
            inlineE.AssertEqual(paraE.GetInlines());
            inlineF.AssertEqual(paraF.GetInlines());
            inlineG.AssertEqual(paraG.GetInlines());
        }

        [TestMethod]
        public void TestCase_281()
        {
            const string code = "1. a\n\n  2. b\n\n   3. c";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_282()
        {
            const string code = "- a\n - b\n  - c\n   - d\n    - e";
            var doc = new MarkdownParser().Parse(code);
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

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(1).GetChild(0);
            var paraC = doc.Elements[0].GetChild(2).GetChild(0);
            var paraD = doc.Elements[0].GetChild(3).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");
            var inlineC = new InlineStructure(InlineElementType.InlineText, "c");
            var inlineD = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "d"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "- e"));

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
            inlineC.AssertEqual(paraC.GetInlines());
            inlineD.AssertEqual(paraD.GetInlines());
        }


        [TestMethod]
        public void TestCase_283()
        {
            const string code = "1. a\n\n  2. b\n\n    3. c\n";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[1].Type);

            var paraA = doc.Elements[0].GetChild(0).GetChild(0);
            var paraB = doc.Elements[0].GetChild(1).GetChild(0);

            var inlineA = new InlineStructure(InlineElementType.InlineText, "a");
            var inlineB = new InlineStructure(InlineElementType.InlineText, "b");

            inlineA.AssertEqual(paraA.GetInlines());
            inlineB.AssertEqual(paraB.GetInlines());
        }

        [TestMethod]
        public void TestCase_284()
        {
            const string code = "- a\n- b\n\n- c";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_285()
        {
            const string code = "* a\n*\n\n* c";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_286()
        {
            const string code = "- a\n- b\n\n  c\n- d";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_287()
        {
            const string code = "- a\n- b\n\n  [ref]: /url\n- d";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_288()
        {
            const string code = "- a\n- ```\n  b\n\n\n  ```\n- c";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.CodeBlock)),
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

            Assert.AreEqual("b\r\n\r\n", (doc.Elements[0].GetChild(1).GetChild(0) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_289()
        {
            const string code = "- a\n  - b\n\n    c\n- d\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_290()
        {
            const string code = "* a\n  > b\n  >\n* c\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_291()
        {
            const string code = "- a\n  > b\n  ```\n  c\n  ```\n- d\n";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph),
                    new BlockElementStructure(BlockElementType.BlockQuote,
                        new BlockElementStructure(BlockElementType.Paragraph)),
                    new BlockElementStructure(BlockElementType.CodeBlock)),
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
            Assert.AreEqual("c", (doc.Elements[0].GetChild(0).GetChild(2) as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_292()
        {
            const string code = "- a";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_293()
        {
            const string code = "- a\n  - b";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_294()
        {
            const string code = "1. ```\n   foo\n   ```\n\n   bar\n";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.CodeBlock),
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);
            Assert.IsFalse((doc.Elements[0] as ListBlock).IsTight);

            Assert.AreEqual("foo", (doc.Elements[0].GetChild(0).GetChild(0) as CodeBlock).Code);

            var para1 = doc.Elements[0].GetChild(0).GetChild(1);
            var inline1 = new InlineStructure(InlineElementType.InlineText, "bar");
            inline1.AssertEqual(para1.GetInlines());
        }

        [TestMethod]
        public void TestCase_295()
        {
            const string code = "* foo\n  * bar\n\n  baz";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_296()
        {
            const string code = "- a\n  - b\n  - c\n\n- d\n  - e\n  - f\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_297()
        {
            const string code = "`hi`lo`";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_298()
        {
            const string code =
                "\\!\\\"\\#\\$\\%\\&\\'\\(\\)\\*\\+\\,\\-\\.\\/\\:\\;\\<\\=\\>\\?\\@\\[\\\\\\]\\^\\_\\`\\{\\|\\}\\~";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_299()
        {
            const string code = "\\\t\\A\\a\\ \\3\\φ\\«";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "\\\t\\A\\a\\ \\3\\φ\\«");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_300()
        {
            const string code = "\\*not emphasized*\n\\<br/> not a tag\n\\[not a link](/foo)\n\\`not code`\n1\\. not a list\n\\* not a list\n\\" +
                "# not a heading\n\\[foo]: /url \"not a reference\"\n\\&ouml; not a character entity";
            var doc = new MarkdownParser().Parse(code);
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
                new InlineStructure(InlineElementType.InlineText, "[foo]: /url \"not a reference\""),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "&ouml; not a character entity"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_301()
        {
            const string code = "\\\\*emphasis*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_302()
        {
            const string code = "foo\\\nbar\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_303()
        {
            const string code = "`` \\[\\` ``";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "\\[\\`");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_304()
        {
            const string code = "    \\[\\]";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\\[\\]", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_305()
        {
            const string code = "~~~\n\\[\\]\n~~~";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\\[\\]", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_306()
        {
            const string code = "<http://example.com?find=\\*>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://example.com?find=\\*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("http://example.com?find=%5C*", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_307()
        {
            const string code = "<a href=\"/bar\\/)\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<a href=\"/bar\\/)\">", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_308()
        {
            const string code = "[foo](/bar\\* \"ti\\*tle\")";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_309()
        {
            const string code = "[foo]\n\n[foo]: /bar\\* \"ti\\*tle\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_310()
        {
            const string code = "``` foo\\+bar\nfoo\n```";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            var block = (CodeBlock)doc.Elements[0];
            Assert.AreEqual("foo", block.Code);
            Assert.AreEqual("foo+bar", block.InfoString);
        }

        #endregion

        #region Entity and numeric character references 

        [TestMethod]
        public void TestCase_311()
        {
            const string code = "&nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace;" +
                                " &DifferentialD;\n&ClockwiseContourIntegral; &ngE;";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_312()
        {
            const string code = "&#35; &#1234; &#992; &#0;";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "# Ӓ Ϡ \xFFFD");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_313()
        {
            const string code = "&#X22; &#XD06; &#xcab;";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "\" ആ ಫ");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_314()
        {
            const string code = "&nbsp &x; &#; &#x;\n&#987654321;\n&#abcdef0;\n&ThisIsNotDefined; &hi?;";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "&nbsp &x; &#; &#x;"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "&#987654321;"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "&#abcdef0;"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "&ThisIsNotDefined; &hi?;"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_315()
        {
            const string code = "&copy";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "&copy");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_316()
        {
            const string code = "&MadeUpEntity;";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "&MadeUpEntity;");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_317()
        {
            const string code = "<a href=\"&ouml;&ouml;.html\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<a href=\"&ouml;&ouml;.html\">", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_318()
        {
            const string code = "[foo](/f&ouml;&ouml; \"f&ouml;&ouml;\")";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_319()
        {
            const string code = "[foo]\n\n[foo]: /f&ouml;&ouml; \"f&ouml;&ouml;\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_320()
        {
            const string code = "``` f&ouml;&ouml;\nfoo\n```";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo", (doc.Elements[0] as CodeBlock).Code);
            Assert.AreEqual("föö", (doc.Elements[0] as CodeBlock).InfoString);
        }

        [TestMethod]
        public void TestCase_321()
        {
            const string code = "`f&ouml;&ouml;`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "f&ouml;&ouml;");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_322()
        {
            const string code = "    f&ouml;f&ouml;";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("f&ouml;f&ouml;", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_323()
        {
            const string code = "&#42;foo&#42;\n*foo*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*foo*"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_324()
        {
            const string code = "&#42; foo\n\n* foo";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.List, doc.Elements[1].Type);
            Assert.AreEqual(BlockElementType.ListItem, doc.Elements[1].GetChild(0).Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].GetChild(0).GetChild(0).Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "* foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var inline2 = new InlineStructure(InlineElementType.InlineText, "foo");
            inline2.AssertEqual(doc.Elements[1].GetChild(0).GetChild(0).GetInlines());
        }

        [TestMethod]
        public void TestCase_325()
        {
            const string code = "foo&#10;&#10;bar";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo\n\nbar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_326()
        {
            const string code = "&#9;foo";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "\tfoo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_327()
        {
            const string code = "[a](url &quot;tit&quot;)";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[a](url \"tit\")");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        #endregion

        #region Code spans

        [TestMethod]
        public void TestCase_328()
        {
            const string code = "`foo`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_329()
        {
            const string code = "`` foo ` bar ``";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo ` bar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_330()
        {
            const string code = "` `` `";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "``");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_331()
        {
            const string code = "``\nfoo\n``";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_332()
        {
            const string code = "` a`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, " a");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_333()
        {
            const string code = "`a\xA0\xA0b`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "a\xA0\xA0b");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_334()
        {
            const string code = "` `\n`  `";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, " "),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.CodeSpan, "  "));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_335()
        {
            const string code = "``\nfoo\nbar  \nbaz\n``";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo bar   baz");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_336()
        {
            const string code = "``\nfoo \n``";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo ");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_337()
        {
            const string code = "`foo   bar \nbaz`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo   bar  baz");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_338()
        {
            const string code = "`foo\\`bar`\n";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, "foo\\"),
                new InlineStructure(InlineElementType.InlineText, "bar`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_339()
        {
            const string code = "``foo`bar``";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo`bar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }


        [TestMethod]
        public void TestCase_340()
        {
            const string code = "` foo `` bar `";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo `` bar");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_341()
        {
            const string code = "*foo`*`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*foo"),
                new InlineStructure(InlineElementType.CodeSpan, "*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_342()
        {
            const string code = "[not a `link](/foo`)\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_343()
        {
            const string code = "`<a href=\"`\">`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, "<a href=\""),
                new InlineStructure(InlineElementType.InlineText, "\">`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_344()
        {
            const string code = "<a href=\"`\">`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"`\">"),
                new InlineStructure(InlineElementType.InlineText, "`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_345()
        {
            const string code = "`<http://foo.bar.`baz>`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.CodeSpan, "<http://foo.bar."),
                new InlineStructure(InlineElementType.InlineText, "baz>`"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_346()
        {
            const string code = "<http://foo.bar.`baz>`";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_347()
        {
            const string code = "```foo``";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_348()
        {
            const string code = "`foo";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_349()
        {
            const string code = "`foo``bar``";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_350()
        {
            const string code = "*foo bar*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_351()
        {
            const string code = "a * foo bar*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "a * foo bar*");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_352()
        {
            const string code = "a*\"foo\"*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "a*\"foo\"*");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }


        [TestMethod]
        public void TestCase_353()
        {
            const string code = "*\xA0a\xA0*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "*\xA0a\xA0*");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_354()
        {
            const string code = "foo*bar*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_355()
        {
            const string code = "5*6*78";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_356()
        {
            const string code = "_foo bar_";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_357()
        {
            const string code = "_ foo bar_";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "_ foo bar_");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_358()
        {
            const string code = "a_\"foo\"_";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "a_\"foo\"_");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_359()
        {
            const string code = "foo_bar_";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_360()
        {
            const string code = "5_6_78";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_361()
        {
            const string code = "пристаням_стремятся_";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_362()
        {
            const string code = "aa_\"bb\"_cc";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_363()
        {
            const string code = "foo-_(bar)_";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_364()
        {
            const string code = "_foo*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_365()
        {
            const string code = "*foo bar *";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_366()
        {
            const string code = "*foo bar\n*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_367()
        {
            const string code = "*(*foo)";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_368()
        {
            const string code = "*(*foo*)*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_369()
        {
            const string code = "*foo*bar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_370()
        {
            const string code = "_foo bar _";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_371()
        {
            const string code = "_(_foo)";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_372()
        {
            const string code = "_(_foo_)_";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_373()
        {
            const string code = "_foo_bar";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_374()
        {
            const string code = "_пристаням_стремятся";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_375()
        {
            const string code = "_foo_bar_baz_";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo_bar_baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_376()
        {
            const string code = "_(bar)_.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_377()
        {
            const string code = "**foo bar**";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_378()
        {
            const string code = "** foo bar**";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_379()
        {
            const string code = "a**\"foo\"**";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_380()
        {
            const string code = "foo**bar**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_381()
        {
            const string code = "**foo bar**";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_382()
        {
            const string code = "__ foo bar__";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_383()
        {
            const string code = "__\nfoo bar__";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_384()
        {
            const string code = "a__\"foo\"__";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_385()
        {
            const string code = "foo__bar__";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_386()
        {
            const string code = "5__6__78";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_387()
        {
            const string code = "пристаням__стремятся__";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_388()
        {
            const string code = "__foo, __bar__, baz__";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_389()
        {
            const string code = "foo-__(bar)__";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_390()
        {
            const string code = "**foo bar **";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_391()
        {
            const string code = "**(**foo)";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_392()
        {
            const string code = "*(**foo**)*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_393()
        {
            const string code = "**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\n*Asclepias physocarpa*)**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_394()
        {
            const string code = "**foo \"*bar*\" foo**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_395()
        {
            const string code = "**foo**bar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_396()
        {
            const string code = "__foo bar __";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_397()
        {
            const string code = "__(__foo)";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_398()
        {
            const string code = "_(__foo__)_";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_399()
        {
            const string code = "__foo__bar";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_400()
        {
            const string code = "__пристаням__стремятся";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_401()
        {
            const string code = "__foo__bar__baz__";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo__bar__baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_402()
        {
            const string code = "__(bar)__.";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_403()
        {
            const string code = "*foo [bar](/url)*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_404()
        {
            const string code = "*foo\nbar*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_405()
        {
            const string code = "_foo __bar__ baz_";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_406()
        {
            const string code = "_foo _bar_ baz_";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_407()
        {
            const string code = "__foo_ bar_";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_408()
        {
            const string code = "*foo *bar**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_409()
        {
            const string code = "*foo **bar** baz*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_410()
        {
            const string code = "*foo**bar**baz*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_411()
        {
            const string code = "*foo**bar*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo**bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }


        [TestMethod]
        public void TestCase_412()
        {
            const string code = "***foo** bar*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_413()
        {
            const string code = "*foo **bar***";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_414()
        {
            const string code = "*foo**bar***";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_415()
        {
            const string code = "foo***bar***baz";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.StrongEmphasis,
                            new InlineStructure(InlineElementType.InlineText, "bar"))),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_416()
        {
            const string code = "foo******bar*********baz";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.StrongEmphasis,
                            new InlineStructure(InlineElementType.InlineText, "bar")))),
                new InlineStructure(InlineElementType.InlineText, "***baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_417()
        {
            const string code = "*foo **bar *baz* bim** bop*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_418()
        {
            const string code = "*foo [*bar*](/url)*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_419()
        {
            const string code = "** is not an empty emphasis";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_420()
        {
            const string code = "**** is not an empty strong emphasis";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_421()
        {
            const string code = "**foo [bar](/url)**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_422()
        {
            const string code = "**foo\nbar**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_423()
        {
            const string code = "__foo _bar_ baz__";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_424()
        {
            const string code = "__foo __bar__ baz__";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_425()
        {
            const string code = "____foo__ bar__";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_426()
        {
            const string code = "**foo **bar****";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_427()
        {
            const string code = "**foo *bar* baz**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_428()
        {
            const string code = "**foo*bar*baz**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_429()
        {
            const string code = "***foo* bar**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_430()
        {
            const string code = "**foo *bar***";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_431()
        {
            const string code = "**foo *bar **baz**\nbim* bop**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_432()
        {
            const string code = "**foo [*bar*](/url)**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_433()
        {
            const string code = "__ is not an empty emphasis";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_434()
        {
            const string code = "____ is not an empty strong emphasis";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_435()
        {
            const string code = "foo ***";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_436()
        {
            const string code = "foo *\\**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_437()
        {
            const string code = "foo *_*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_438()
        {
            const string code = "foo *****";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_439()
        {
            const string code = "foo **\\***";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_440()
        {
            const string code = "foo **_**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_441()
        {
            const string code = "**foo*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_442()
        {
            const string code = "*foo**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_443()
        {
            const string code = "***foo**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_444()
        {
            const string code = "****foo*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_445()
        {
            const string code = "**foo***";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_446()
        {
            const string code = "*foo****";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_447()
        {
            const string code = "foo ___";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_448()
        {
            const string code = "foo _\\__";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_449()
        {
            const string code = "foo _*_";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_450()
        {
            const string code = "foo _____";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_451()
        {
            const string code = "foo __\\___";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_452()
        {
            const string code = "foo __*__";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_453()
        {
            const string code = "__foo_";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_454()
        {
            const string code = "_foo__";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_455()
        {
            const string code = "___foo__";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_456()
        {
            const string code = "____foo_";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_457()
        {
            const string code = "__foo___";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_458()
        {
            const string code = "_foo____";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_459()
        {
            const string code = "**foo**";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_460()
        {
            const string code = "*_foo_*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_461()
        {
            const string code = "__foo__";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_462()
        {
            const string code = "_*foo*_";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_463()
        {
            const string code = "****foo****";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_464()
        {
            const string code = "____foo____";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_465()
        {
            const string code = "******foo******";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_466()
        {
            const string code = "***foo***";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_467()
        {
            const string code = "_____foo_____";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_468()
        {
            const string code = "*foo _bar* baz_";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_469()
        {
            const string code = "*foo __bar *baz bim__ bam*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_470()
        {
            const string code = "**foo **bar baz**";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_471()
        {
            const string code = "*foo *bar baz*";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_472()
        {
            const string code = "*[bar*](/url)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_473()
        {
            const string code = "_foo [bar_](/url)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_474()
        {
            const string code = "*<img src=\"foo\" title=\"*\"/>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.InlineHtml, "<img src=\"foo\" title=\"*\"/>"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_475()
        {
            const string code = "**<a href=\"**\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "**"),
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"**\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_476()
        {
            const string code = "__<a href=\"__\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "__"),
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"__\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_477()
        {
            const string code = "*a `*`*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "a "),
                new InlineStructure(InlineElementType.CodeSpan, "*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_478()
        {
            const string code = "_a `_`_";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "a "),
                new InlineStructure(InlineElementType.CodeSpan, "_"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_479()
        {
            const string code = "**a<http://foo.bar/?q=**>";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_480()
        {
            const string code = "__a<http://foo.bar/?q=__>";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_481()
        {
            const string code = "[link](/uri \"title\")";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_482()
        {
            const string code = "[link](/uri)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_483()
        {
            const string code = "[link]()";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_484()
        {
            const string code = "[link](<>)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_485()
        {
            const string code = "[link](/my uri)";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_486()
        {
            const string code = "[link](</my uri>)";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "link"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("/my%20uri", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_487()
        {
            const string code = "[link](foo\nbar)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_488()
        {
            const string code = "[link](<foo\nbar>)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_489()
        {
            const string code = "[a](<b)c>)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_490()
        {
            const string code = "[link](<foo\\>)";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[link](<foo>)");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_491()
        {
            const string code = "[a](<b)c\n[a](<b)c>\n[a](<b>c)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_492()
        {
            const string code = "[link](\\(foo\\))";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_493()
        {
            const string code = "[link](foo(and(bar)))";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_494()
        {
            const string code = "[link](foo\\(and\\(bar\\))";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_495()
        {
            const string code = "[link](<foo(and(bar)>)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_496()
        {
            const string code = "[link](foo\\)\\:)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_497()
        {
            const string code =
                "[link](#fragment)\n\n[link](http://example.com#fragment)\n\n[link](http://example.com?foo=3#frag)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_498()
        {
            const string code = "[link](foo\\bar)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_499()
        {
            const string code = "[link](foo%20b&auml;)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_500()
        {
            const string code = "[link](\"title\")";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_501()
        {
            const string code = "[link](/url \"title\")\n[link](/url 'title')\n[link](/url (title))";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_502()
        {
            const string code = "[link](/url \"title \\\"&quot;\")";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_503()
        {
            const string code = "[link](/url\xA0\"title\")";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_504()
        {
            const string code = "[link](/url \"title \"and\" title\")";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_505()
        {
            const string code = "[link](/url 'title \"and\" title')";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_506()
        {
            const string code = "[link](   /uri\n  \"title\"  )";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_507()
        {
            const string code = "[link] (/uri)";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_508()
        {
            const string code = "[link [foo [bar]]](/uri)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_509()
        {
            const string code = "[link] bar](/uri)";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_510()
        {
            const string code = "[link [bar](/uri)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_511()
        {
            const string code = "[link \\[bar](/uri)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_512()
        {
            const string code = "[link *foo **bar** `#`*](/uri)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_513()
        {
            const string code = "[![moon](moon.jpg)](/uri)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_514()
        {
            const string code = "[foo [bar](/uri)](/uri)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_515()
        {
            const string code = "[foo *[bar [baz](/uri)](/uri)*](/uri)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_516()
        {
            const string code = "![[[foo](uri1)](uri2)](uri3)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_517()
        {
            const string code = "*[foo*](/uri)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_518()
        {
            const string code = "[foo *bar](baz*)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_519()
        {
            const string code = "*foo [bar* baz]";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_520()
        {
            const string code = "[foo <bar attr=\"](baz)\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<bar attr=\"](baz)\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_521()
        {
            const string code = "[foo`](/uri)`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo"),
                new InlineStructure(InlineElementType.CodeSpan, "](/uri)"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_522()
        {
            const string code = "[foo<http://example.com/?search=](uri)>";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_523()
        {
            const string code = "[foo][bar]\n\n[bar]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_524()
        {
            const string code = "[link [foo [bar]]][ref]\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_525()
        {
            const string code = "[link \\[bar][ref]\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_526()
        {
            const string code = "[link *foo **bar** `#`*][ref]\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_527()
        {
            const string code = "[![moon](moon.jpg)][ref]\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_528()
        {
            const string code = "[foo [bar](/uri)][ref]\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_529()
        {
            const string code = "[foo *bar [baz][ref]*][ref]\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_530()
        {
            const string code = "*[foo*][ref]\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_531()
        {
            const string code = "[foo *bar][ref]\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_532()
        {
            const string code = "[foo <bar attr=\"][ref]\">\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<bar attr=\"][ref]\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_533()
        {
            const string code = "[foo`][ref]`\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "[foo"),
                new InlineStructure(InlineElementType.CodeSpan, "][ref]"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_534()
        {
            const string code = "[foo<http://example.com/?search=][ref]>\n\n[ref]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_535()
        {
            const string code = "[foo][BaR]\n\n[bar]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
            const string code = "[Толпой][Толпой] is a Russian word.\n\n[ТОЛПОЙ]: /url";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_537()
        {
            const string code = "[Foo\n  bar]: /url\n\n[Baz][Foo bar]";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_538()
        {
            const string code = "[foo] [bar]\n\n[bar]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_539()
        {
            const string code = "[foo]\n[bar]\n\n[bar]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_540()
        {
            const string code = "[foo]: /url1\n\n[foo]: /url2\n\n[bar][foo]";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_541()
        {
            const string code = "[bar][foo\\!]\n\n[foo!]: /url";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[bar][foo!]");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_542()
        {
            const string code = "[foo][ref[]\n\n[ref[]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_543()
        {
            const string code = "[foo][ref[bar]]\n\n[ref[bar]]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_544()
        {
            const string code = "[[[foo]]]\n\n[[[foo]]]: /url";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_545()
        {
            const string code = "[foo][ref\\[]\n\n[ref\\[]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_546()
        {
            const string code = "[bar\\\\]: /uri\n\n[bar\\\\]";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_547()
        {
            const string code = "[]\n\n[]: /uri";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_548()
        {
            const string code = "[\n ]\n\n[\n ]: /uri\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_549()
        {
            const string code = "[foo][]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_550()
        {
            const string code = "[*foo* bar][]\n\n[*foo* bar]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_551()
        {
            const string code = "[Foo][]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_552()
        {
            const string code = "[foo] \n[]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_553()
        {
            const string code = "[foo]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_554()
        {
            const string code = "[*foo* bar]\n\n[*foo* bar]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_555()
        {
            const string code = "[[*foo* bar]]\n\n[*foo* bar]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_556()
        {
            const string code = "[[bar [foo]\n\n[foo]: /url";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_557()
        {
            const string code = "[Foo]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_558()
        {
            const string code = "[foo] bar\n\n[foo]: /url";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_559()
        {
            const string code = "\\[foo]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "[foo]");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_560()
        {
            const string code = "[foo*]: /url\n\n*[foo*]";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_561()
        {
            const string code = "[foo][bar]\n\n[foo]: /url1\n[bar]: /url2";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_562()
        {
            const string code = "[foo][]\n\n[foo]: /url1";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_563()
        {
            const string code = "[foo]()\n\n[foo]: /url1";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_564()
        {
            const string code = "[foo](not a link)\n\n[foo]: /url1";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_565()
        {
            const string code = "[foo][bar][baz]\n\n[baz]: /url";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_566()
        {
            const string code = "[foo][bar][baz]\n\n[baz]: /url1\n[bar]: /url2";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_567()
        {
            const string code = "[foo][bar][baz]\n\n[baz]: /url1\n[foo]: /url2";
            var doc = new MarkdownParser().Parse(code);
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

        #endregion

        #region Images

        [TestMethod]
        public void TestCase_568()
        {
            const string code = "![foo](/url \"title\")";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_569()
        {
            const string code = "![foo *bar*]\n\n[foo *bar*]: train.jpg \"train & tracks\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_570()
        {
            const string code = "![foo ![bar](/url)](/url2)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_571()
        {
            const string code = "![foo [bar](/url)](/url2)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_572()
        {
            const string code = "![foo *bar*][]\n\n[foo *bar*]: train.jpg \"train & tracks\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_573()
        {
            const string code = "![foo *bar*][foobar]\n\n[FOOBAR]: train.jpg \"train & tracks\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_574()
        {
            const string code = "![foo](train.jpg)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_575()
        {
            const string code = "My ![foo bar](/path/to/train.jpg  \"title\"   )";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_576()
        {
            const string code = "![foo](<url>)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_577()
        {
            const string code = "![](/url)";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_578()
        {
            const string code = "![foo][bar]\n\n[bar]: /url";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_579()
        {
            const string code = "![foo][bar]\n\n[BAR]: /url";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_580()
        {
            const string code = "![foo][]\n\n[foo]: /url \"title\"\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_581()
        {
            const string code = "![*foo* bar][]\n\n[*foo* bar]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_582()
        {
            const string code = "![Foo][]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_583()
        {
            const string code = "![foo] \n[]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_584()
        {
            const string code = "![foo]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_585()
        {
            const string code = "![*foo* bar]\n\n[*foo* bar]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_586()
        {
            const string code = "![[foo]]\n\n[[foo]]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_587()
        {
            const string code = "![Foo]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_588()
        {
            const string code = "!\\[foo]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(1, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "![foo]");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_589()
        {
            const string code = "\\![foo]\n\n[foo]: /url \"title\"";
            var doc = new MarkdownParser().Parse(code);
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

        #endregion

        #region AutoLinks

        [TestMethod]
        public void TestCase_590()
        {
            const string code = "<http://foo.bar.baz>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://foo.bar.baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("http://foo.bar.baz", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_591()
        {
            const string code = "<http://foo.bar.baz/test?q=hello&id=22&boolean>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://foo.bar.baz/test?q=hello&id=22&boolean"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("http://foo.bar.baz/test?q=hello&id=22&boolean", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_592()
        {
            const string code = "<irc://foo.bar:2233/baz>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "irc://foo.bar:2233/baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("irc://foo.bar:2233/baz", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_593()
        {
            const string code = "<MAILTO:FOO@BAR.BAZ>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "MAILTO:FOO@BAR.BAZ"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("MAILTO:FOO@BAR.BAZ", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_594()
        {
            const string code = "<a+b+c:d>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "a+b+c:d"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("a+b+c:d", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_595()
        {
            const string code = "<made-up-scheme://foo,bar>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "made-up-scheme://foo,bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("made-up-scheme://foo,bar", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_596()
        {
            const string code = "<http://../>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://../"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("http://../", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_597()
        {
            const string code = "<localhost:5001/foo>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "localhost:5001/foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("localhost:5001/foo", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_598()
        {
            const string code = "<http://foo.bar/baz bim>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_599()
        {
            const string code = "<http://example.com/\\[\\>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "http://example.com/\\[\\"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("http://example.com/%5C%5B%5C", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_600()
        {
            const string code = "<foo@bar.example.com>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo@bar.example.com"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("mailto:foo@bar.example.com", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_601()
        {
            const string code = "<foo+special@Bar.baz-bar0.com>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Link,
                new InlineStructure(InlineElementType.InlineText, "foo+special@Bar.baz-bar0.com"));
            inline.AssertEqual(doc.Elements[0].GetInlines());

            var link = doc.Elements[0].GetInline(0) as Link;
            Assert.AreEqual("mailto:foo+special@Bar.baz-bar0.com", link.Destination);
            Assert.AreEqual(null, link.Title);
        }

        [TestMethod]
        public void TestCase_602()
        {
            const string code = "<foo\\+@bar.example.com>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "<foo+@bar.example.com>");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_603()
        {
            const string code = "<>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_604()
        {
            const string code = "< http://foo.bar >";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_605()
        {
            const string code = "<m:abc>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_606()
        {
            const string code = "<foo.bar.baz>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_607()
        {
            const string code = "http://example.com";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_608()
        {
            const string code = "foo@bar.example.com";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        #endregion

        #region Raw HTML

        [TestMethod]
        public void TestCase_609()
        {
            const string code = "<a><bab><c2c>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineHtml, "<a>"),
                new InlineStructure(InlineElementType.InlineHtml, "<bab>"),
                new InlineStructure(InlineElementType.InlineHtml, "<c2c>"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_610()
        {
            const string code = "<a/><b2/>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineHtml, "<a/>"),
                new InlineStructure(InlineElementType.InlineHtml, "<b2/>"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_611()
        {
            const string code = "<a  /><b2\ndata=\"foo\" >";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineHtml, "<a  />"),
                new InlineStructure(InlineElementType.InlineHtml, "<b2\r\ndata=\"foo\" >"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_612()
        {
            const string code = "<a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 />";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineHtml,
                "<a foo=\"bar\" bam = 'baz <em>\"</em>'\r\n_boolean zoop:33=zoop:33 />");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_613()
        {
            const string code = "Foo <responsive-image src=\"foo.jpg\" />";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "Foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<responsive-image src=\"foo.jpg\" />"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_614()
        {
            const string code = "<33> <__>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_615()
        {
            const string code = "<a h*#ref=\"hi\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_616()
        {
            const string code = "<a href=\"hi'> <a href=hi'>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_617()
        {
            const string code = "< a><\nfoo><bar/ >\n<foo bar=baz\nbim!bop />";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "< a><"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "foo><bar/ >"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "<foo bar=baz"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "bim!bop />"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_618()
        {
            const string code = "<a href='bar'title=title>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_619()
        {
            const string code = "</a></foo >";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineHtml, "</a>"),
                new InlineStructure(InlineElementType.InlineHtml, "</foo >"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_620()
        {
            const string code = "</a href=\"foo\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_621()
        {
            const string code = "foo <!-- this is a\ncomment - with hyphen -->";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<!-- this is a\r\ncomment - with hyphen -->"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_622()
        {
            const string code = "foo <!-- not a comment -- two hyphens -->";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_623()
        {
            const string code = "foo <!--> foo -->\n\nfoo <!-- foo--->";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(2, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[1].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo <!--> foo -->");
            var inline2 = new InlineStructure(InlineElementType.InlineText, "foo <!-- foo--->");
            inline.AssertEqual(doc.Elements[0].GetInlines());
            inline2.AssertEqual(doc.Elements[1].GetInlines());
        }

        [TestMethod]
        public void TestCase_624()
        {
            const string code = "foo <?php echo $a; ?>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<?php echo $a; ?>"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_625()
        {
            const string code = "foo <!ELEMENT br EMPTY>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<!ELEMENT br EMPTY>"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_626()
        {
            const string code = "foo <![CDATA[>&<]]>";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<![CDATA[>&<]]>"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_627()
        {
            const string code = "foo <a href=\"&ouml;\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"&ouml;\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_628()
        {
            const string code = "foo <a href=\"\\*\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"\\*\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_629()
        {
            const string code = "<a href=\"\\\"\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "<a href=\"\"\">");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        #endregion

        #region Hard line breaks

        [TestMethod]
        public void TestCase_630()
        {
            const string code = "foo  \nbaz";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.HardLineBreak),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_631()
        {
            const string code = "foo\\\nbaz";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.HardLineBreak),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_632()
        {
            const string code = "foo       \nbaz";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.HardLineBreak),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_633()
        {
            const string code = "\n\nfoo  \n     bar\n\n";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_634()
        {
            const string code = "foo\\\n     bar";
            var doc = new MarkdownParser().Parse(code);
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
        public void TestCase_635()
        {
            const string code = "*foo  \nbar*";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.HardLineBreak),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_636()
        {
            const string code = "*foo\\\nbar*\n";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.HardLineBreak),
                new InlineStructure(InlineElementType.InlineText, "bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_637()
        {
            const string code = "`code \nspan`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "code  span");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_638()
        {
            const string code = "`code\\\nspan`";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.CodeSpan, "code\\ span");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_639()
        {
            const string code = "<a href=\"foo  \nbar\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineHtml, "<a href=\"foo  \r\nbar\">");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_640()
        {
            const string code = "<a href=\"foo\\\nbar\">";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineHtml, "<a href=\"foo\\\r\nbar\">");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_641()
        {
            const string code = "foo\\";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo\\");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_642()
        {
            const string code = "foo  ";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_643()
        {
            const string code = "### foo\\";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo\\");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_644()
        {
            const string code = "### foo  ";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Heading, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        #endregion

        #region Soft line breaks

        [TestMethod]
        public void TestCase_645()
        {
            const string code = "foo\nbaz";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_646()
        {
            const string code = "foo \n baz";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak),
                new InlineStructure(InlineElementType.InlineText, "baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        #endregion

        #region Texual content

        [TestMethod]
        public void TestCase_647()
        {
            const string code = "hello $.;'there";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_648()
        {
            const string code = "Foo χρῆν";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_649()
        {
            const string code = "Multiple     spaces";
            var doc = new MarkdownParser().Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        #endregion
    }
}
