using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.BlockElement;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    [TestClass]
    public class HtmlBlockTest
    {

        #region AddLine

        [TestMethod]
        public void AddLineTest_01()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<pre><code>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("// Code example"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("</code></pre>"));
            Assert.AreEqual(1, GetBlockType(block));
            Assert.AreEqual("<pre><code>\r\n// Code example\r\n\r\n</code></pre>",
                block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_02()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<script type=\"text / javascript\">"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("document.getElementById(\"demo\").innerHTML = \"Hello!\";"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("</script>"));
            Assert.AreEqual(1, GetBlockType(block));
            Assert.AreEqual("<script type=\"text / javascript\">\r\n" +
                "document.getElementById(\"demo\").innerHTML = \"Hello!\";\r\n</script>",
                block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_03()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   <style"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    type=\"text / css\">"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("h1 {color:red;}"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("</style>"));
            Assert.AreEqual(1, GetBlockType(block));
            Assert.AreEqual("   <style\r\n    type=\"text / css\">\r\nh1 {color:red;}\r\n</style>",
                block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_04()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("<style>p{color:red;}</style>"));
            Assert.AreEqual(1, GetBlockType(block));
            Assert.AreEqual("<style>p{color:red;}</style>", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_05()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<script>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("foo"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("</script>1. *bar*"));
            Assert.AreEqual(1, GetBlockType(block));
            Assert.AreEqual("<script>\r\nfoo\r\n</script>1. *bar*", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_06()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("  <!--foo bar-->"));
            Assert.AreEqual(2, GetBlockType(block));
            Assert.AreEqual("  <!--foo bar-->", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_07()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("<!--foo bar-->baz"));
            Assert.AreEqual(2, GetBlockType(block));
            Assert.AreEqual("<!--foo bar-->baz", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_08()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<!--"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("foo"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("-->bar "));
            Assert.AreEqual(2, GetBlockType(block));
            Assert.AreEqual("<!--\r\nfoo\r\n-->bar ", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_09()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("<!---->"));
            Assert.AreEqual(2, GetBlockType(block));
            Assert.AreEqual("<!---->", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_10()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<?php"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  echo 'foo';"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("?>"));
            Assert.AreEqual(3, GetBlockType(block));
            Assert.AreEqual("<?php\r\n  echo 'foo';\r\n?>", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_11()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(" <?php "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  echo '>';"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine(" ?>bar "));
            Assert.AreEqual(3, GetBlockType(block));
            Assert.AreEqual(" <?php \r\n  echo '>';\r\n ?>bar ", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_12()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("<!DOCTYPE html>"));
            Assert.AreEqual(4, GetBlockType(block));
            Assert.AreEqual("<!DOCTYPE html>", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_13()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   <!DOCTYPE html"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("\">\""));
            Assert.AreEqual(4, GetBlockType(block));
            Assert.AreEqual("   <!DOCTYPE html\r\n\">\"", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_14()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   <!DOCTYPE html"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("> foo  "));
            Assert.AreEqual(4, GetBlockType(block));
            Assert.AreEqual("   <!DOCTYPE html\r\n\r\n> foo  ", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_15()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<![CDATA["));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("]]>"));
            Assert.AreEqual(5, GetBlockType(block));
            Assert.AreEqual("<![CDATA[\r\n\r\n]]>", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_16()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("<![CDATA[]]>"));
            Assert.AreEqual(5, GetBlockType(block));
            Assert.AreEqual("<![CDATA[]]>", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_17()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<AddRess"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine(""));
            Assert.AreEqual(6, GetBlockType(block));
            Assert.AreEqual("<AddRess", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_18()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<div>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("</div>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("foo"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine(""));
            Assert.AreEqual(6, GetBlockType(block));
            Assert.AreEqual("<div>\r\n</div>\r\nfoo", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_19()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<tag-name>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("foo"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine(""));
            Assert.AreEqual(7, GetBlockType(block));
            Assert.AreEqual("<tag-name>\r\nfoo", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_20()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<tag-name attr-name='bar'>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<pre>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("foo"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine(""));
            Assert.AreEqual(7, GetBlockType(block));
            Assert.AreEqual("<tag-name attr-name='bar'>\r\n<pre>\r\nfoo", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void AddLineTest_21()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("</tag-name>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<pre>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("foo"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine(""));
            Assert.AreEqual(7, GetBlockType(block));
            Assert.AreEqual("</tag-name>\r\n<pre>\r\nfoo", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        #endregion

        #region Close

        [TestMethod]
        public void CloseTest_01()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<pre><code>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("// Code example"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(1, GetBlockType(block));
            Assert.AreEqual("<pre><code>\r\n// Code example\r\n",
                block.Content);
            var closed = (HtmlBlock)block.Close();
            Assert.AreNotEqual(0, closed.Warnings.Count);
            Assert.AreEqual("<pre><code>\r\n// Code example", closed.Content);
        }

        [TestMethod]
        public void CloseTest_02()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<pre><code>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("// Code example"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("</pre>"));
            Assert.AreEqual(1, GetBlockType(block));
            Assert.AreEqual("<pre><code>\r\n// Code example\r\n\r\n</pre>",
                block.Content);
            var closed = (HtmlBlock)block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
            Assert.AreEqual("<pre><code>\r\n// Code example\r\n\r\n</pre>", closed.Content);
        }

        [TestMethod]
        public void CloseTest_03()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<!--"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(2, GetBlockType(block));
            Assert.AreEqual("<!--\r\nfoo\r\n", block.Content);
            var closed = (HtmlBlock)block.Close();
            Assert.AreNotEqual(0, closed.Warnings.Count);
            Assert.AreEqual("<!--\r\nfoo", closed.Content);
        }

        [TestMethod]
        public void CloseTest_04()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<!--"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("foo"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("-->"));
            Assert.AreEqual(2, GetBlockType(block));
            Assert.AreEqual("<!--\r\nfoo\r\n-->", block.Content);
            var closed = (HtmlBlock)block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
            Assert.AreEqual("<!--\r\nfoo\r\n-->", block.Content);
        }

        [TestMethod]
        public void CloseTest_05()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<?php"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  echo 'foo';"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(3, GetBlockType(block));
            Assert.AreEqual("<?php\r\n  echo 'foo';\r\n  \r\n", block.Content);
            var closed = (HtmlBlock)block.Close();
            Assert.AreNotEqual(0, closed.Warnings.Count);
            Assert.AreEqual("<?php\r\n  echo 'foo';", closed.Content);
        }


        [TestMethod]
        public void CloseTest_06()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<?php"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("  echo 'foo';"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("?>"));
            Assert.AreEqual(3, GetBlockType(block));
            Assert.AreEqual("<?php\r\n  echo 'foo';\r\n?>", block.Content);
            var closed = (HtmlBlock)block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
            Assert.AreEqual("<?php\r\n  echo 'foo';\r\n?>", closed.Content);
        }

        [TestMethod]
        public void CloseTest_07()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   <!DOCTYPE html"));
            Assert.AreEqual(4, GetBlockType(block));
            Assert.AreEqual("   <!DOCTYPE html", block.Content);
            var closed = (HtmlBlock)block.Close();
            Assert.AreNotEqual(0, closed.Warnings.Count);
            Assert.AreEqual("   <!DOCTYPE html", closed.Content);
        }

        [TestMethod]
        public void CloseTest_08()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<![CDATA["));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(5, GetBlockType(block));
            Assert.AreEqual("<![CDATA[\r\n", block.Content);
            var closed = (HtmlBlock)block.Close();
            Assert.AreNotEqual(0, closed.Warnings.Count);
            Assert.AreEqual("<![CDATA[", closed.Content);
        }

        [TestMethod]
        public void CloseTest_09()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<![CDATA["));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, block.AddLine("]]>"));
            Assert.AreEqual(5, GetBlockType(block));
            Assert.AreEqual("<![CDATA[\r\n\r\n]]>", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }

        [TestMethod]
        public void CloseTest_10()
        {
            HtmlBlock block = TestUtils.CreateInternal<HtmlBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<tag-name attr-name='bar'>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("<pre>"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("foo"));
            Assert.AreEqual(7, GetBlockType(block));
            Assert.AreEqual("<tag-name attr-name='bar'>\r\n<pre>\r\nfoo", block.Content);
            var closed = block.Close();
            Assert.AreEqual(0, closed.Warnings.Count);
        }



        #endregion

        private int GetBlockType(HtmlBlock html)
        {
            return (int)typeof(HtmlBlock)
                .GetField("blockType", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(html);
        }
    }


}
