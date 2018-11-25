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

        // TODO: test 02 - 12

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

    }
}
