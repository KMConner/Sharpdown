using System;
using System.Collections.Generic;
using System.Text;
using Sharpdown.MarkdownElement.InlineElement;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.MarkdownElementTest.InlineElementTest
{
    [TestClass]
    public class InlineElementUtilsTest
    {

        [TestMethod]
        public void ParseLinkEmphasisTest_001()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("a * foo bar*", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "a * foo bar*");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_002()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("a*\"foo\"*", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "a*\"foo\"*");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_003()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("5*6*78", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "5"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "6")),
                new InlineStructure(InlineElementType.InlineText, "78")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_004()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("foo*bar*", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_005()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("_foo bar_", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_006()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("foo_bar_", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "foo_bar_");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_007()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("foo-_(bar)_", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo-"),
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "(bar)"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_008()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("_foo*", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "_foo*");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_009()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("_foo_bar_baz_", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo_bar_baz"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_010()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("**foo bar**", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo bar"));
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_011()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("foo**bar**", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_012()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("__ foo bar__", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText, "__ foo bar__");
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_013()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("__foo, __bar__, baz__", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo, "),
                    new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "bar")),
                    new InlineStructure(InlineElementType.InlineText, ", baz")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_014()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("*(**foo**)*", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "("),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, ")")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_015()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("__foo_ bar_", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_016()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("*foo **bar** baz*", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar")),
                new InlineStructure(InlineElementType.InlineText, " baz")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_017()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("***foo** bar*", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_018()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("*foo**bar***", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "bar"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_019()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("*foo [*bar*](/url)*", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "bar")))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_020()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("**foo [bar](/url)**", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "foo "),
                new InlineStructure(InlineElementType.Link,
                        new InlineStructure(InlineElementType.InlineText, "bar"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_021()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("____foo__ bar__", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, " bar")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_022()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("**foo*", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_023()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("***foo**", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.StrongEmphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_024()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("*[bar*](/url)", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.StrongEmphasis,
                new InlineStructure(InlineElementType.InlineText, "*"),
                new InlineStructure(InlineElementType.Link,
                        new InlineStructure(InlineElementType.InlineText, "bar*"))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_025()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("*foo _bar* baz_", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.InlineText,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo _bar")),
                new InlineStructure(InlineElementType.InlineText, " baz_")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_026()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("*[*foo*][]*", new[] { "*foo*" }).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo")))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_027()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("*[*foo*]*", new[] { "*foo*" }).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo")))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_028()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("*[*foo*][bar]*", new[] { "bar" }).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Link,
                    new InlineStructure(InlineElementType.Emphasis,
                        new InlineStructure(InlineElementType.InlineText, "foo")))
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_029()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("***foo**bar*", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.StrongEmphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.InlineText, "bar")
                );
            structure.AssertEqual(ret);
        }

        [TestMethod]
        public void ParseLinkEmphasisTest_030()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("foo\r\nbar  \nbaz", new string[0]).ToArray();
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
        public void ParseLinkEmphasisTest_031()
        {
            var ret = InlineElementUtils.ParseLinkEmphasis("*foo*\n**bar  \nbaz**", new string[0]).ToArray();
            var structure = new InlineStructure(InlineElementType.Emphasis,
                new InlineStructure(InlineElementType.Emphasis,
                    new InlineStructure(InlineElementType.InlineText, "foo")),
                new InlineStructure(InlineElementType.SoftLineBreak, ""),
                new InlineStructure(InlineElementType.StrongEmphasis, 
                    new InlineStructure(InlineElementType.InlineText,"bar"),
                    new InlineStructure(InlineElementType.HardLineBreak,""),
                    new InlineStructure(InlineElementType.InlineText, "baz"))
                );
            structure.AssertEqual(ret);
        }

    }
}
