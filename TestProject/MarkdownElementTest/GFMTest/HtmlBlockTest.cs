using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;


namespace TestProject.MarkdownElementTest.GFMTest
{

    [TestClass]
    public class HtmlBlockTest
    {
        private readonly MarkdownParser parser;

        public HtmlBlockTest()
        {
            parser = new MarkdownParser(ParserConfigBuilder.GithubFlavored.ToParserConfig());
        }

        [TestMethod]
        public void TestCase_118()
        {
            var doc = parser.Parse(
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
            var doc = parser.Parse(
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
            var doc = parser.Parse(" <div>\r\n  *hello*\r\n         <foo><a>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(" <div>\r\n  *hello*\r\n         <foo><a>", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_121()
        {
            var doc = parser.Parse("</div>\r\n*foo*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("</div>\r\n*foo*", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_122()
        {
            var doc = parser.Parse("<DIV CLASS=\"foo\">\n\n*Markdown*\n\n</DIV>");
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
            var doc = parser.Parse("<div id=\"foo\"\r\n  class=\"bar\">\r\n</div>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div id=\"foo\"\r\n  class=\"bar\">\r\n</div>",
                (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_124()
        {
            var doc = parser.Parse("<div id=\"foo\" class=\"bar\r\n  baz\">\r\n</div>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div id=\"foo\" class=\"bar\r\n  baz\">\r\n</div>",
                (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_125()
        {
            var doc = parser.Parse("<div>\r\n*foo*\r\n\r\n*bar*");
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
            var doc = parser.Parse("<div id=\"foo\"\r\n*hi*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div id=\"foo\"\r\n*hi*", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_127()
        {
            var doc = parser.Parse("<div class\r\nfoo");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div class\r\nfoo", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_128()
        {
            var doc = parser.Parse("<div *???-&&&-<---\r\n*foo*");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div *???-&&&-<---\r\n*foo*", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_129()
        {
            var doc = parser.Parse("<div><a href=\"bar\">*foo*</a></div>");
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual("<div><a href=\"bar\">*foo*</a></div>", (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_130()
        {
            var doc = parser.Parse("<table><tr><td>\r\nfoo\r\n</td></tr></table>");
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
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_132()
        {
            const string html = "<a href=\"foo\">\r\n*bar*\r\n</a>";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_133()
        {
            const string html = "<Warning>\r\n*bar*\r\n</Warning>";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_134()
        {
            const string html = "<i class=\"foo\">\r\n*bar*\r\n</i>";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_135()
        {
            const string html = "</ins>\r\n*bar*";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_136()
        {
            const string html = "<del>\r\n*foo*\r\n</del>";
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_137()
        {
            const string html = "<del>\r\n\r\n*foo*\r\n\r\n</del>";
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_148()
        {
            const string html = "<!-- Foo\r\n\r\nbar\r\n   baz -->\r\nokay";
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_156()
        {
            const string html = "Foo\r\n<a href=\"bar\">\r\nbaz";
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
            Assert.AreEqual(1, doc.Elements.Count);
            Assert.AreEqual(0, doc.LinkDefinition.Count);
            Assert.AreEqual(BlockElementType.HtmlBlock, doc.Elements[0].Type);
            Assert.AreEqual(html, (doc.Elements[0] as HtmlBlock).Code);
        }

        [TestMethod]
        public void TestCase_159()
        {
            const string html = "<table>\r\n\r\n<tr>\r\n\r\n<td>\r\nHi\r\n</td>\r\n\r\n</tr>\r\n\r\n</table>";
            var doc = parser.Parse(html);
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
            var doc = parser.Parse(html);
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
    }
}
