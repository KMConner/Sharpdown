using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;


namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class PrecedenceTest
    {
        private readonly MarkdownParser parser;

        public PrecedenceTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_012()
        {
            var doc = parser.Parse("- `one\n- two`");
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
    }
}
