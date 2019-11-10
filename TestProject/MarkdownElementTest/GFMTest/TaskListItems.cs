using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{
    [TestClass]
    public class TaskListItems
    {
        private readonly MarkdownParser parser;

        public TaskListItems()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_279()
        {
            const string html = "- [ ] foo\n- [x] bar";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);

            var blocks = new BlockElementStructure(BlockElementType.List,
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)),
                new BlockElementStructure(BlockElementType.ListItem,
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, " foo");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());

            Assert.AreEqual(false, (doc.Elements[0].GetChild(0) as ListItem).IsChecked);
            var inline2 = new InlineStructure(InlineElementType.InlineText, " bar");
            inline2.AssertEqual(doc.Elements[0].GetChild(1).GetChild(0).GetInlines());
            Assert.AreEqual(true, (doc.Elements[0].GetChild(1) as ListItem).IsChecked);
        }

        [TestMethod]
        public void TestCase_280()
        {
            const string html = "- [x] foo\n  - [ ] bar\n  - [x] baz\n- [ ] bim";
            var doc = parser.Parse(html);
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
                    new BlockElementStructure(BlockElementType.Paragraph)));
            blocks.AssertTypeEqual(doc.Elements[0]);

            var inline0 = new InlineStructure(InlineElementType.InlineText, " foo");
            inline0.AssertEqual(doc.Elements[0].GetChild(0).GetChild(0).GetInlines());
            Assert.AreEqual(true, (doc.Elements[0].GetChild(0) as ListItem).IsChecked);

            var innerList = doc.Elements[0].GetChild(0).GetChild(1) as ListBlock;
            var inline1 = new InlineStructure(InlineElementType.InlineText, " bar");
            inline1.AssertEqual(innerList.GetChild(0).GetChild(0).GetInlines());
            Assert.AreEqual(false, (innerList.GetChild(0) as ListItem).IsChecked);

            var inline2 = new InlineStructure(InlineElementType.InlineText, " baz");
            inline2.AssertEqual(innerList.GetChild(1).GetChild(0).GetInlines());
            Assert.AreEqual(true, (innerList.GetChild(1) as ListItem).IsChecked);

            var inline3 = new InlineStructure(InlineElementType.InlineText, " bim");
            inline3.AssertEqual(doc.Elements[0].GetChild(1).GetChild(0).GetInlines());
            Assert.AreEqual(false, (doc.Elements[0].GetChild(1) as ListItem).IsChecked);
        }

    }
}
