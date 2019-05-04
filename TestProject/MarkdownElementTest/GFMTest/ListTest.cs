using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class ListTest
    {
        private readonly MarkdownParser parser;

        public ListTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_281()
        {
            const string code = "- foo\n- bar\n+ baz";
            var doc = parser.Parse(code);
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
        public void TestCase_282()
        {
            const string code = "1. foo\n2. bar\n3) baz";
            var doc = parser.Parse(code);
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
        public void TestCase_283()
        {
            const string code = "Foo\n- bar\n- baz";
            var doc = parser.Parse(code);
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
        public void TestCase_284()
        {
            const string code = "The number of windows in my house is\n14.  The number of doors is 6.";
            var doc = parser.Parse(code);
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
        public void TestCase_285()
        {
            const string code = "The number of windows in my house is\n1.  The number of doors is 6.";
            var doc = parser.Parse(code);
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
        public void TestCase_286()
        {
            const string code = "- foo\n\n- bar\n\n\n- baz";
            var doc = parser.Parse(code);
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
        public void TestCase_287()
        {
            const string code = "- foo\n  - bar\n    - baz\n\n\n      bim";
            var doc = parser.Parse(code);
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
        public void TestCase_288()
        {
            const string code = "- foo\n- bar\n\n<!-- -->\n\n- baz\n- bim";
            var doc = parser.Parse(code);
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
        public void TestCase_289()
        {
            const string code = "-   foo\n\n    notcode\n\n-   foo\n\n<!-- -->\n\n    code";
            var doc = parser.Parse(code);
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

        // TODO: Update for v0.29

        [TestMethod]
        public void TestCase_290()
        {
            const string code = "- a\n - b\n  - c\n   - d\n    - e\n   - f\n  - g\n - h\n- i";
            var doc = parser.Parse(code);
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
        public void TestCase_291()
        {
            const string code = "1. a\n\n  2. b\n\n    3. c";
            var doc = parser.Parse(code);
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

        // TODO: Add test 292, 293

        [TestMethod]
        public void TestCase_294()
        {
            const string code = "- a\n- b\n\n- c";
            var doc = parser.Parse(code);
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
        public void TestCase_295()
        {
            const string code = "* a\n*\n\n* c";
            var doc = parser.Parse(code);
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
        public void TestCase_296()
        {
            const string code = "- a\n- b\n\n  c\n- d";
            var doc = parser.Parse(code);
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
        public void TestCase_297()
        {
            const string code = "- a\n- b\n\n  [ref]: /url\n- d";
            var doc = parser.Parse(code);
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
        public void TestCase_298()
        {
            const string code = "- a\n- ```\n  b\n\n\n  ```\n- c";
            var doc = parser.Parse(code);
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
        public void TestCase_299()
        {
            const string code = "- a\n  - b\n\n    c\n- d\n";
            var doc = parser.Parse(code);
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
        public void TestCase_300()
        {
            const string code = "* a\n  > b\n  >\n* c\n";
            var doc = parser.Parse(code);
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
        public void TestCase_301()
        {
            const string code = "- a\n  > b\n  ```\n  c\n  ```\n- d\n";
            var doc = parser.Parse(code);
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
        public void TestCase_302()
        {
            const string code = "- a";
            var doc = parser.Parse(code);
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
        public void TestCase_303()
        {
            const string code = "- a\n  - b";
            var doc = parser.Parse(code);
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
        public void TestCase_304()
        {
            const string code = "1. ```\n   foo\n   ```\n\n   bar\n";
            var doc = parser.Parse(code);
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
        public void TestCase_305()
        {
            const string code = "* foo\n  * bar\n\n  baz";
            var doc = parser.Parse(code);
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
        public void TestCase_306()
        {
            const string code = "- a\n  - b\n  - c\n\n- d\n  - e\n  - f\n";
            var doc = parser.Parse(code);
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


    }
}
