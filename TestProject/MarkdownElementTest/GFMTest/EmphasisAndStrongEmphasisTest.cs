using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class EmphasisAndStrongEmphasisTest
    {
        private readonly MarkdownParser parser;

        public EmphasisAndStrongEmphasisTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_360()
        {
            const string code = "*foo bar*";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_361()
        {
            const string code = "a * foo bar*";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "a * foo bar*");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_362()
        {
            const string code = "a*\"foo\"*";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "a*\"foo\"*");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }


        [TestMethod]
        public void TestCase_363()
        {
            const string code = "*\xA0a\xA0*";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "*\xA0a\xA0*");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_364()
        {
            const string code = "foo*bar*";
            var doc = parser.Parse(code);
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
        public void TestCase_365()
        {
            const string code = "5*6*78";
            var doc = parser.Parse(code);
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
        public void TestCase_366()
        {
            const string code = "_foo bar_";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_367()
        {
            const string code = "_ foo bar_";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "_ foo bar_");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_368()
        {
            const string code = "a_\"foo\"_";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, "a_\"foo\"_");
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_369()
        {
            const string code = "foo_bar_";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_370()
        {
            const string code = "5_6_78";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_371()
        {
            const string code = "пристаням_стремятся_";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_372()
        {
            const string code = "aa_\"bb\"_cc";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_373()
        {
            const string code = "foo-_(bar)_";
            var doc = parser.Parse(code);
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
        public void TestCase_374()
        {
            const string code = "_foo*";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_375()
        {
            const string code = "*foo bar *";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_376()
        {
            const string code = "*foo bar\n*";
            var doc = parser.Parse(code);
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
        public void TestCase_377()
        {
            const string code = "*(*foo)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_378()
        {
            const string code = "*(*foo*)*";
            var doc = parser.Parse(code);
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
        public void TestCase_379()
        {
            const string code = "*foo*bar";
            var doc = parser.Parse(code);
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
        public void TestCase_380()
        {
            const string code = "_foo bar _";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_381()
        {
            const string code = "_(_foo)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_382()
        {
            const string code = "_(_foo_)_";
            var doc = parser.Parse(code);
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
        public void TestCase_383()
        {
            const string code = "_foo_bar";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_384()
        {
            const string code = "_пристаням_стремятся";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_385()
        {
            const string code = "_foo_bar_baz_";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo_bar_baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_386()
        {
            const string code = "_(bar)_.";
            var doc = parser.Parse(code);
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
        public void TestCase_387()
        {
            const string code = "**foo bar**";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_388()
        {
            const string code = "** foo bar**";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_389()
        {
            const string code = "a**\"foo\"**";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_390()
        {
            const string code = "foo**bar**";
            var doc = parser.Parse(code);
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
        public void TestCase_391()
        {
            const string code = "__foo bar__";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }


        [TestMethod]
        public void TestCase_392()
        {
            const string code = "__ foo bar__";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_393()
        {
            const string code = "__\nfoo bar__";
            var doc = parser.Parse(code);
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
        public void TestCase_394()
        {
            const string code = "a__\"foo\"__";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_395()
        {
            const string code = "foo__bar__";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_396()
        {
            const string code = "5__6__78";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_397()
        {
            const string code = "пристаням__стремятся__";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_398()
        {
            const string code = "__foo, __bar__, baz__";
            var doc = parser.Parse(code);
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
        public void TestCase_399()
        {
            const string code = "foo-__(bar)__";
            var doc = parser.Parse(code);
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
        public void TestCase_400()
        {
            const string code = "**foo bar **";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_401()
        {
            const string code = "**(**foo)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_402()
        {
            const string code = "*(**foo**)*";
            var doc = parser.Parse(code);
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
        public void TestCase_403()
        {
            const string code = "**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\n*Asclepias physocarpa*)**";
            var doc = parser.Parse(code);
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
        public void TestCase_404()
        {
            const string code = "**foo \"*bar*\" foo**";
            var doc = parser.Parse(code);
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
        public void TestCase_405()
        {
            const string code = "**foo**bar";
            var doc = parser.Parse(code);
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
        public void TestCase_406()
        {
            const string code = "__foo bar __";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_407()
        {
            const string code = "__(__foo)";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_408()
        {
            const string code = "_(__foo__)_";
            var doc = parser.Parse(code);
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
        public void TestCase_409()
        {
            const string code = "__foo__bar";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_410()
        {
            const string code = "__пристаням__стремятся";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_411()
        {
            const string code = "__foo__bar__baz__";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo__bar__baz"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_412()
        {
            const string code = "__(bar)__.";
            var doc = parser.Parse(code);
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
        public void TestCase_413()
        {
            const string code = "*foo [bar](/url)*";
            var doc = parser.Parse(code);
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
        public void TestCase_414()
        {
            const string code = "*foo\nbar*";
            var doc = parser.Parse(code);
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
        public void TestCase_415()
        {
            const string code = "_foo __bar__ baz_";
            var doc = parser.Parse(code);
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
        public void TestCase_416()
        {
            const string code = "_foo _bar_ baz_";
            var doc = parser.Parse(code);
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
        public void TestCase_417()
        {
            const string code = "__foo_ bar_";
            var doc = parser.Parse(code);
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
        public void TestCase_418()
        {
            const string code = "*foo *bar**";
            var doc = parser.Parse(code);
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
        public void TestCase_419()
        {
            const string code = "*foo **bar** baz*";
            var doc = parser.Parse(code);
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
        public void TestCase_420()
        {
            const string code = "*foo**bar**baz*";
            var doc = parser.Parse(code);
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
        public void TestCase_421()
        {
            const string code = "***foo** bar*";
            var doc = parser.Parse(code);
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
        public void TestCase_422()
        {
            const string code = "*foo**bar***";
            var doc = parser.Parse(code);
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
        public void TestCase_423()
        {
            const string code = "*foo **bar***";
            var doc = parser.Parse(code);
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
        public void TestCase_424()
        {
            const string code = "*foo**bar***";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        // TODO: Uncomment after support 0.29
        //[TestMethod]
        //public void TestCase_425()
        //{
        //    const string code = "foo***bar***baz";
        //    var doc = parser.Parse(code);
        //    Assert.AreEqual(1, doc.Elements.Count);
        //    Assert.AreEqual(0, doc.LinkDefinition.Count);
        //    Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

        //    var inline = new InlineStructure(
        //        new InlineStructure(InlineElementType.InlineText, "foo"),
        //        new InlineStructure(InlineElementType.Emphasis,
        //            new InlineStructure(InlineElementType.StrongEmphasis,
        //                new InlineStructure(InlineElementType.InlineText, "bar"))),
        //        new InlineStructure(InlineElementType.InlineText, "baz"));
        //    inline.AssertEqual(doc.Elements[0].GetInlines());
        //}

        // TODO: Add test case 426

        [TestMethod]
        public void TestCase_427()
        {
            const string code = "*foo **bar *baz* bim** bop*";
            var doc = parser.Parse(code);
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
        public void TestCase_428()
        {
            const string code = "*foo [*bar*](/url)*";
            var doc = parser.Parse(code);
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
        public void TestCase_429()
        {
            const string code = "** is not an empty emphasis";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_430()
        {
            const string code = "**** is not an empty strong emphasis";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_431()
        {
            const string code = "**foo [bar](/url)**";
            var doc = parser.Parse(code);
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
        public void TestCase_432()
        {
            const string code = "**foo\nbar**";
            var doc = parser.Parse(code);
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
        public void TestCase_433()
        {
            const string code = "__foo _bar_ baz__";
            var doc = parser.Parse(code);
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
        public void TestCase_434()
        {
            const string code = "__foo __bar__ baz__";
            var doc = parser.Parse(code);
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
        public void TestCase_435()
        {
            const string code = "____foo__ bar__";
            var doc = parser.Parse(code);
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
        public void TestCase_436()
        {
            const string code = "**foo **bar****";
            var doc = parser.Parse(code);
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
        public void TestCase_437()
        {
            const string code = "**foo *bar* baz**";
            var doc = parser.Parse(code);
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
        public void TestCase_438()
        {
            const string code = "**foo*bar*baz**";
            var doc = parser.Parse(code);
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
        public void TestCase_439()
        {
            const string code = "***foo* bar**";
            var doc = parser.Parse(code);
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
        public void TestCase_440()
        {
            const string code = "**foo *bar***";
            var doc = parser.Parse(code);
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
        public void TestCase_441()
        {
            const string code = "**foo *bar **baz**\nbim* bop**";
            var doc = parser.Parse(code);
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
        public void TestCase_442()
        {
            const string code = "**foo [*bar*](/url)**";
            var doc = parser.Parse(code);
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
        public void TestCase_443()
        {
            const string code = "__ is not an empty emphasis";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_444()
        {
            const string code = "____ is not an empty strong emphasis";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_445()
        {
            const string code = "foo ***";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_446()
        {
            const string code = "foo *\\**";
            var doc = parser.Parse(code);
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
        public void TestCase_447()
        {
            const string code = "foo *_*";
            var doc = parser.Parse(code);
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
        public void TestCase_448()
        {
            const string code = "foo *****";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_449()
        {
            const string code = "foo **\\***";
            var doc = parser.Parse(code);
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
        public void TestCase_450()
        {
            const string code = "foo **_**";
            var doc = parser.Parse(code);
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
        public void TestCase_451()
        {
            const string code = "**foo*";
            var doc = parser.Parse(code);
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
        public void TestCase_452()
        {
            const string code = "*foo**";
            var doc = parser.Parse(code);
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
        public void TestCase_453()
        {
            const string code = "***foo**";
            var doc = parser.Parse(code);
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
        public void TestCase_454()
        {
            const string code = "****foo*";
            var doc = parser.Parse(code);
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
        public void TestCase_455()
        {
            const string code = "**foo***";
            var doc = parser.Parse(code);
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
        public void TestCase_456()
        {
            const string code = "*foo****";
            var doc = parser.Parse(code);
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
        public void TestCase_457()
        {
            const string code = "foo ___";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_458()
        {
            const string code = "foo _\\__";
            var doc = parser.Parse(code);
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
        public void TestCase_459()
        {
            const string code = "foo _*_";
            var doc = parser.Parse(code);
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
        public void TestCase_460()
        {
            const string code = "foo _____";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.InlineText, code);
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_461()
        {
            const string code = "foo __\\___";
            var doc = parser.Parse(code);
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
        public void TestCase_462()
        {
            const string code = "foo __*__";
            var doc = parser.Parse(code);
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
        public void TestCase_463()
        {
            const string code = "__foo_";
            var doc = parser.Parse(code);
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
        public void TestCase_464()
        {
            const string code = "_foo__";
            var doc = parser.Parse(code);
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
        public void TestCase_465()
        {
            const string code = "___foo__";
            var doc = parser.Parse(code);
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
        public void TestCase_466()
        {
            const string code = "____foo_";
            var doc = parser.Parse(code);
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
        public void TestCase_467()
        {
            const string code = "__foo___";
            var doc = parser.Parse(code);
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
        public void TestCase_468()
        {
            const string code = "_foo____";
            var doc = parser.Parse(code);
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
        public void TestCase_469()
        {
            const string code = "**foo**";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_470()
        {
            const string code = "*_foo_*";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_471()
        {
            const string code = "__foo__";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_472()
        {
            const string code = "_*foo*_";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_473()
        {
            const string code = "****foo****";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_474()
        {
            const string code = "____foo____";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_475()
        {
            const string code = "******foo******";
            var doc = parser.Parse(code);
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
        public void TestCase_476()
        {
            const string code = "***foo***";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_477()
        {
            const string code = "_____foo_____";
            var doc = parser.Parse(code);
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
        public void TestCase_478()
        {
            const string code = "*foo _bar* baz_";
            var doc = parser.Parse(code);
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
        public void TestCase_479()
        {
            const string code = "*foo __bar *baz bim__ bam*";
            var doc = parser.Parse(code);
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
        public void TestCase_480()
        {
            const string code = "**foo **bar baz**";
            var doc = parser.Parse(code);
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
        public void TestCase_481()
        {
            const string code = "*foo *bar baz*";
            var doc = parser.Parse(code);
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
        public void TestCase_482()
        {
            const string code = "*[bar*](/url)";
            var doc = parser.Parse(code);
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
        public void TestCase_483()
        {
            const string code = "_foo [bar_](/url)";
            var doc = parser.Parse(code);
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
        public void TestCase_484()
        {
            const string code = "*<img src=\"foo\" title=\"*\"/>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.InlineHtml, "<img src=\"foo\" title=\"*\"/>"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_485()
        {
            const string code = "**<a href=\"**\">";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "**"),
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"**\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_486()
        {
            const string code = "__<a href=\"__\">";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "__"),
                new InlineStructure(InlineElementType.InlineHtml, "<a href=\"__\">"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_487()
        {
            const string code = "*a `*`*";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "a "),
                new InlineStructure(InlineElementType.CodeSpan, "*"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_488()
        {
            const string code = "_a `_`_";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "a "),
                new InlineStructure(InlineElementType.CodeSpan, "_"));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }

        [TestMethod]
        public void TestCase_489()
        {
            const string code = "**a<http://foo.bar/?q=**>";
            var doc = parser.Parse(code);
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
        public void TestCase_490()
        {
            const string code = "__a<http://foo.bar/?q=__>";
            var doc = parser.Parse(code);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.Paragraph, doc.Elements[0].Type);

            var inline = new InlineStructure(
                new InlineStructure(InlineElementType.InlineText, "__a"),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "http://foo.bar/?q=__")));
            inline.AssertEqual(doc.Elements[0].GetInlines());
        }
    }
}
