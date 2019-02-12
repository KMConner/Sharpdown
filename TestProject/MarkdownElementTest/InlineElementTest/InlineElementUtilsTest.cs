using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.InlineElement;
using Sharpdown.MarkdownElement.BlockElement;
using System.Collections.Generic;

namespace TestProject.MarkdownElementTest.InlineElementTest
{
    [TestClass]
    public class InlineElementUtilsTest
    {
        #region ParseInlineElements

        [TestMethod]
        public void ParseInlineElementsTest_001()
        {
            var ret = InlineElementUtils.ParseInlineElements("a * foo bar*",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "a * foo bar*");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_002()
        {
            var ret = InlineElementUtils.ParseInlineElements("a*\"foo\"*",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "a*\"foo\"*");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_003()
        {
            var ret = InlineElementUtils.ParseInlineElements("5*6*78",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "5"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "6")),
                new InlineStructure(InlineElementType.InlineText, "78")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_004()
        {
            var ret = InlineElementUtils.ParseInlineElements("foo*bar*",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_005()
        {
            var ret = InlineElementUtils.ParseInlineElements("_foo bar_",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_006()
        {
            var ret = InlineElementUtils.ParseInlineElements("foo_bar_",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "foo_bar_");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_007()
        {
            var ret = InlineElementUtils.ParseInlineElements("foo-_(bar)_",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo-"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "(bar)"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_008()
        {
            var ret = InlineElementUtils.ParseInlineElements("_foo*",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "_foo*");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_009()
        {
            var ret = InlineElementUtils.ParseInlineElements("_foo_bar_baz_",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo_bar_baz"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_010()
        {
            var ret = InlineElementUtils.ParseInlineElements("**foo bar**",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_011()
        {
            var ret = InlineElementUtils.ParseInlineElements("foo**bar**",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_012()
        {
            var ret = InlineElementUtils.ParseInlineElements("__ foo bar__",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "__ foo bar__");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_013()
        {
            var ret = InlineElementUtils.ParseInlineElements("__foo, __bar__, baz__",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo, "),
                    new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "bar")),
                    new InlineStructure(InlineElementType.InlineText, ", baz")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_014()
        {
            var ret = InlineElementUtils.ParseInlineElements("*(**foo**)*",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "("),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, ")")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_015()
        {
            var ret = InlineElementUtils.ParseInlineElements("__foo_ bar_",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_016()
        {
            var ret = InlineElementUtils.ParseInlineElements("*foo **bar** baz*",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, " baz")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_017()
        {
            var ret = InlineElementUtils.ParseInlineElements("***foo** bar*",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_018()
        {
            var ret = InlineElementUtils.ParseInlineElements("*foo**bar***",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_019()
        {
            var ret = InlineElementUtils.ParseInlineElements("*foo [*bar*](/url)*",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "bar")))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_020()
        {
            var ret = InlineElementUtils.ParseInlineElements("**foo [bar](/url)**",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Link,
                        new InlineStructure(InlineElementType.InlineText, "bar"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_021()
        {
            var ret = InlineElementUtils.ParseInlineElements("____foo__ bar__",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_022()
        {
            var ret = InlineElementUtils.ParseInlineElements("**foo*",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_023()
        {
            var ret = InlineElementUtils.ParseInlineElements("***foo**",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_024()
        {
            var ret = InlineElementUtils.ParseInlineElements("*[bar*](/url)",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.Link,
                        new InlineStructure(InlineElementType.InlineText, "bar*"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_025()
        {
            var ret = InlineElementUtils.ParseInlineElements("*foo _bar* baz_",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo _bar")),
                new InlineStructure(InlineElementType.InlineText, " baz_")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_026()
        {
            var t = typeof(string);
            var ret = InlineElementUtils.ParseInlineElements("*[*foo*][]*",
                new Dictionary<string, LinkReferenceDefinition>
                {
                    { "*foo*" ,TestUtils.CreateInternal<LinkReferenceDefinition>(
                        new[]{ t,t,t, typeof(UnknownElement)},
                        new object[]{"*foo*","./url", "title", null })}
                }).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo")))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_027()
        {
            var t = typeof(string);
            var ret = InlineElementUtils.ParseInlineElements("*[*foo*]*",
                new Dictionary<string, LinkReferenceDefinition>
                {
                    { "*foo*" ,TestUtils.CreateInternal<LinkReferenceDefinition>(
                        new[]{ t,t,t, typeof(UnknownElement)},
                        new object[]{"*foo*","./url", "title", null })}
                }).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo")))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_028()
        {
            var t = typeof(string);
            var ret = InlineElementUtils.ParseInlineElements("*[*foo*][bar]*",
                new Dictionary<string, LinkReferenceDefinition>
                {
                    { "bar" ,TestUtils.CreateInternal<LinkReferenceDefinition>(
                        new[]{ t,t,t, typeof(UnknownElement)},
                    new object[]{"*foo*", "./url", "title" ,null})}
                }).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo")))
                );
            structure.AssertEqual(ret);
            Assert.AreEqual(((ret[0] as Emphasis).Children[0] as Link).Destination, "./url");
            Assert.AreEqual(((ret[0] as Emphasis).Children[0] as Link).Title, "title");
        }

        [TestMethod]
        public void ParseInlineElementsTest_029()
        {
            var ret = InlineElementUtils.ParseInlineElements("***foo**bar*",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "bar")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_030()
        {
            var ret = InlineElementUtils.ParseInlineElements("foo\r\nbar  \nbaz",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.HardLineBreak, ""),
                new InlineStructure(InlineElementType.InlineText, "baz")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_031()
        {
            var ret = InlineElementUtils.ParseInlineElements("*foo*\n**bar  \nbaz**",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar"),
                    new InlineStructure(InlineElementType.HardLineBreak, ""),
                    new InlineStructure(InlineElementType.InlineText, "baz"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_032()
        {
            var ret = InlineElementUtils.ParseInlineElements("``foo``",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.CodeSpan, "foo");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_033()
        {
            var ret = InlineElementUtils.ParseInlineElements("``foo`",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "``foo`");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_034()
        {
            var ret = InlineElementUtils.ParseInlineElements("``foo```",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "``foo```");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_035()
        {
            var ret = InlineElementUtils.ParseInlineElements("``foo``bar",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.CodeSpan,
                new InlineStructure(InlineElementType.CodeSpan, "foo"),
                new InlineStructure(InlineElementType.InlineText, "bar")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_036()
        {
            var ret = InlineElementUtils.ParseInlineElements("``foo```bar",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "``foo```bar");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_037()
        {
            var ret = InlineElementUtils.ParseInlineElements("bar``foo``",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.CodeSpan,
                new InlineStructure(InlineElementType.InlineText, "bar"),
                new InlineStructure(InlineElementType.CodeSpan, "foo")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_038()
        {
            var ret = InlineElementUtils.ParseInlineElements("bar``foo```",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "bar``foo```");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_039()
        {
            var ret = InlineElementUtils.ParseInlineElements("``bar`foo``",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.CodeSpan, "bar`foo");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_040()
        {
            var ret = InlineElementUtils.ParseInlineElements("``bar```foo``",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.CodeSpan, "bar```foo");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_041()
        {
            var ret = InlineElementUtils.ParseInlineElements("``bar`` ```foo```",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.CodeSpan,
                new InlineStructure(InlineElementType.CodeSpan, "bar"),
                new InlineStructure(InlineElementType.InlineText, " "),
                new InlineStructure(InlineElementType.CodeSpan, "foo"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_042()
        {
            var ret = InlineElementUtils.ParseInlineElements("``\r\nfoo    bar\r\nbaz\tboo\n``",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.CodeSpan, "foo bar baz boo");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_043()
        {
            var ret = InlineElementUtils.ParseInlineElements("<http://foo.bar.baz>",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "http://foo.bar.baz"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_044()
        {
            var ret = InlineElementUtils.ParseInlineElements("<irc://foo.bar:2233/baz>",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "irc://foo.bar:2233/baz"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_045()
        {
            var ret = InlineElementUtils.ParseInlineElements("<made-up-scheme://foo,bar>",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "made-up-scheme://foo,bar"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_046()
        {
            var ret = InlineElementUtils.ParseInlineElements("<http://example.com/\\[\\>",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "http://example.com/\\[\\"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_047()
        {
            var ret = InlineElementUtils.ParseInlineElements("<foo@bar.example.com><foo\\+@bar.example.com>",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.InlineText, "foo@bar.example.com")),
                new InlineStructure(InlineElementType.InlineText, "<foo+@bar.example.com>"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_048()
        {
            var ret = InlineElementUtils.ParseInlineElements("< http://foo.bar >",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "< http://foo.bar >");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_049()
        {
            var ret = InlineElementUtils.ParseInlineElements("<a><bab><c2c><a/><b2/>",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineHtml, "<a>"),
                new InlineStructure(InlineElementType.InlineHtml, "<bab>"),
                new InlineStructure(InlineElementType.InlineHtml, "<c2c>"),
                new InlineStructure(InlineElementType.InlineHtml, "<a/>"),
                new InlineStructure(InlineElementType.InlineHtml, "<b2/>"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_050()
        {
            var ret = InlineElementUtils.ParseInlineElements("<a  /><b2\n data=\"foo\" >",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineHtml, "<a  />"),
                new InlineStructure(InlineElementType.InlineHtml, "<b2\n data=\"foo\" >"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_051()
        {
            var ret = InlineElementUtils.ParseInlineElements("Foo <responsive-image src=\"foo.jpg\" />",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "Foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<responsive-image src=\"foo.jpg\" />"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_052()
        {
            var ret = InlineElementUtils.ParseInlineElements("<33> <__><p>",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "<33> <__>"),
                new InlineStructure(InlineElementType.InlineHtml, "<p>"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_053()
        {
            var ret = InlineElementUtils.ParseInlineElements("foo <!-- this is a\r\ncomment - with hyphen -->",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<!-- this is a\r\ncomment - with hyphen -->"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_054()
        {
            var ret = InlineElementUtils.ParseInlineElements("foo <!-- not a comment -- two hyphens -->",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "foo <!-- not a comment -- two hyphens -->");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_055()
        {
            var ret = InlineElementUtils.ParseInlineElements("foo <?php echo $a; ?>",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<?php echo $a; ?>"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_056()
        {
            var ret = InlineElementUtils.ParseInlineElements("foo <!ELEMENT br EMPTY>",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<!ELEMENT br EMPTY>"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseInlineElementsTest_057()
        {
            var ret = InlineElementUtils.ParseInlineElements("foo <![CDATA[>&<]]>",
                new Dictionary<string, LinkReferenceDefinition>()).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.InlineHtml, "<![CDATA[>&<]]>"));
            structure.AssertEqual(ret);
        }

        #endregion

    }
}
