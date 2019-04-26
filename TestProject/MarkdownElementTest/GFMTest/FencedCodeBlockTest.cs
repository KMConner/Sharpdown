using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;


namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class FencedCodeBlockTest
    {
        private readonly MarkdownParser parser;

        public FencedCodeBlockTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_089()
        {
            var doc = parser.Parse("```\n<\n >\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("<\r\n >", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_090()
        {
            var doc = parser.Parse("~~~\n<\n >\n~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("<\r\n >", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_091()
        {
            var doc = parser.Parse("``\nfoo\n``");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);
            var inline = new InlineStructure(InlineElementType.CodeSpan, "foo");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_092()
        {
            var doc = parser.Parse("```\naaa\n~~~\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_093()
        {
            var doc = parser.Parse("~~~\naaa\n```\n~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n```", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_094()
        {
            var doc = parser.Parse("````\naaa\n```\n``````");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n```", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_095()
        {
            var doc = parser.Parse("~~~~\naaa\n~~~\n~~~~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_096()
        {
            var doc = parser.Parse("```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_097()
        {
            var doc = parser.Parse("`````\n\n```\naaa");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\r\n```\r\naaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_098()
        {
            var doc = parser.Parse("> ```\n> aaa\n\nbbb");
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
            var doc = parser.Parse("```\n\n  \n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("\r\n  ", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_100()
        {
            var doc = parser.Parse("```\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_101()
        {
            var doc = parser.Parse(" ```\n aaa\naaa\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\naaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_102()
        {
            var doc = parser.Parse("  ```\naaa\n  aaa\naaa\n  ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\naaa\r\naaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_103()
        {
            var doc = parser.Parse("   ```\n   aaa\n    aaa\n  aaa\n   ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n aaa\r\naaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_104()
        {
            var doc = parser.Parse("    ```\n    aaa\n    ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("```\r\naaa\r\n```", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_105()
        {
            var doc = parser.Parse("```\naaa\n  ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_106()
        {
            var doc = parser.Parse("   ```\naaa\n   ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_107()
        {
            var doc = parser.Parse("```\naaa\n    ```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n    ```", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_108()
        {
            var doc = parser.Parse("``` ```\naaa");
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
        public void TestCase_109()
        {
            var doc = parser.Parse("~~~~~~\naaa\n~~~ ~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("aaa\r\n~~~ ~~", (doc.Elements[0] as CodeBlock).Code);
        }

        [TestMethod]
        public void TestCase_110()
        {
            var doc = parser.Parse("foo\n```\nbar\n```\nbaz");
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
            var doc = parser.Parse("foo\n---\n~~~\nbar\n~~~\n# baz");
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
            var doc = parser.Parse("```ruby\ndef foo(x)\n  return 3\nend\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("def foo(x)\r\n  return 3\r\nend", (doc.Elements[0] as CodeBlock).Code);
            Assert.AreEqual("ruby", (doc.Elements[0] as CodeBlock).InfoString);
        }

        [TestMethod]
        public void TestCase_113()
        {
            var doc = parser.Parse("~~~~    ruby startline=3 $%@#$\ndef foo(x)\n  return 3\nend\n~~~~~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("def foo(x)\r\n  return 3\r\nend", (doc.Elements[0] as CodeBlock).Code);
            Assert.AreEqual("ruby startline=3 $%@#$", (doc.Elements[0] as CodeBlock).InfoString);
        }

        [TestMethod]
        public void TestCase_114()
        {
            var doc = parser.Parse("````;\n````");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("", (doc.Elements[0] as CodeBlock).Code);
            Assert.AreEqual(";", (doc.Elements[0] as CodeBlock).InfoString);
        }

        [TestMethod]
        public void TestCase_115()
        {
            var doc = parser.Parse("``` aa ```\nfoo");
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
            var doc = parser.Parse("~~~ aa ``` ~~~\nfoo\n~~~");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("foo", (doc.Elements[0] as CodeBlock).Code);
            Assert.AreEqual("aa ``` ~~~", (doc.Elements[0] as CodeBlock).InfoString);
        }


        [TestMethod]
        public void TestCase_117()
        {
            var doc = parser.Parse("```\n```aaa\n```");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.CodeBlock, doc.Elements[0].Type);
            Assert.AreEqual("```aaa", (doc.Elements[0] as CodeBlock).Code);
        }
    }
}
