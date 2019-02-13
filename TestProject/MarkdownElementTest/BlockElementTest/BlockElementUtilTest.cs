using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.BlockElement;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    [TestClass]
    public class BlockElementUtilTest
    {
        #region Themantic Break

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("---", 0);
            Assert.AreEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   ---", 0);
            Assert.AreEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("- -  ---------- -", 0);
            Assert.AreEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  ***\t*", 0);
            Assert.AreEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("__ _ _", 0);
            Assert.AreEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t__ _ _", 0);
            Assert.AreNotEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("--", 0);
            Assert.AreNotEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   ", 0);
            Assert.AreNotEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        #endregion

        #region ATX Heading

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("##", 0);
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("## Hello", 0);
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("##\tTitle", 0);
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("#", 0);
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  #", 0);
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("###### Title", 0);
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("####### Title", 0);
            Assert.AreNotEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\\####### Title", 0);
            Assert.AreNotEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine("###Title", 0);
            Assert.AreNotEqual(BlockElementType.AtxHeading, block.Type);
        }

        #endregion

        #region IndentedCodeBlock

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("    aa", 0);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\taa", 0);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("                        aa", 0);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t\taa", 0);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("     ", 0);
            Assert.AreNotEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t\r\n", 0);
            Assert.AreNotEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        #endregion

        #region FencedCodeBlock

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("```", 0);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("~~~", 0);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("``````", 0);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   `````", 0);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("    `````\n", 0);
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t`````", 0);
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("`````csharp", 0);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("~~~~~````", 0);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine("````aa``", 0);
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_10()
        {
            var block = BlockElementUtil.CreateBlockFromLine("~~~aa~~", 0);
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, block.Type);
        }
        #endregion

        #region HtmlBlock

        #region Type1

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<script", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<pre", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<style", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <style", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  <style ", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }


        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  <style\r\n", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  <style>", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  <style block=", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine("    <style", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_10()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t<style", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_11()
        {
            var block = BlockElementUtil.CreateBlockFromLine("< style", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_12()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<styles", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_13()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<STYLE", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_14()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<StYlE", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }


        #endregion

        #region Type2

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!--", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <!--", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!--aaaa", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("< !--aaaa", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<! --aaaa", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!-- aaaa", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #region Type3

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<?", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <?", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("    <?", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t<?", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }


        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<?>", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("< ?", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #region Type4

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!A", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <!A", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!a", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("< !A", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!ZABC", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #region Type5

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<![CDATA[", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <![CDATA[[[[", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <![CDATA[????]>", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<! [ CDATA[", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #region Type6

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<address", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("</address", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<AddReSs", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<address ", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<address/>", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <address>", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<address/\r\n", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<addressA", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<address", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<article", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<aside", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<base", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<basefont", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<blockquote", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<body", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<caption", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<center", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<col", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<colgroup", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<dd", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<details", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<dialog", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<dir", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<div", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<dl", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<dt", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<fieldset", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<figcaption", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<figure", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<footer", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<form", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<frame", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<frameset", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h1", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h2", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h3", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h4", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h5", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h6", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<head", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<header", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<hr", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<html", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<iframe", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<legend", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<li", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<link", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<main", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<menu", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<menuitem", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<meta", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<nav", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<noframes", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<ol", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<optgroup", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<option", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<p", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<param", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<section", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<source", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<summary", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<table", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<tbody", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<td", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<tfoot", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<th", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<thead", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<title", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<tr", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<track", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<ul", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #region Type7

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc>", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc >", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc />", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc aa=\"bb\">", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc aa =\"bb\">", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc aa= \"bb\">", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc _aa='bb' >", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc aa='bb' bb = cc />", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine(
                "<abc :aa='bb' b-b = cc d_: = \"ee\" a.1 = ??!  />", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_10()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc :ab_c. />", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_11()
        {
            var block = BlockElementUtil.CreateBlockFromLine("</aaa>", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_12()
        {
            var block = BlockElementUtil.CreateBlockFromLine("</aaa >", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_13()
        {
            var block = BlockElementUtil.CreateBlockFromLine("</a>", 0);
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_14()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc/ >", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_15()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc/ >", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_16()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc 123>", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_17()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc .aaa>", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_18()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc -aaa>", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_19()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc abc=\"a\"b\">", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_20()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc abc='b'b'>", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_21()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc abc=ab`c>", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_22()
        {
            var block = BlockElementUtil.CreateBlockFromLine("</ abc>", 0);
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #endregion

        #region BlockQuote

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("> ", 0);
            Assert.AreEqual(BlockElementType.BlockQuote, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  > > >", 0);
            Assert.AreEqual(BlockElementType.BlockQuote, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   >>", 0);
            Assert.AreEqual(BlockElementType.BlockQuote, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine(" ", 0);
            Assert.AreNotEqual(BlockElementType.BlockQuote, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine(" >            ", 0);
            Assert.AreEqual(BlockElementType.BlockQuote, block.Type);
        }

        #endregion

        #region ListBlock

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("- Title", 0);
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("* Title", 0);
            Assert.AreEqual(BlockElementType.List, block.Type);
        }


        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("+ Title", 0);
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("-", 0);
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   -", 0);
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("-title", 0);
            Assert.AreNotEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("123. title", 0);
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("123) title", 0);
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine("123456789. title", 0);
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_10()
        {
            var block = BlockElementUtil.CreateBlockFromLine("123456789.", 0);
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_11()
        {
            var block = BlockElementUtil.CreateBlockFromLine("123456789.title", 0);
            Assert.AreNotEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_12()
        {
            var block = BlockElementUtil.CreateBlockFromLine("1234567890. title", 0);
            Assert.AreNotEqual(BlockElementType.List, block.Type);
        }

        #endregion

        #region BlankLine

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("", 0);
            Assert.AreEqual(BlockElementType.BlankLine, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   ", 0);
            Assert.AreEqual(BlockElementType.BlankLine, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t", 0);
            Assert.AreEqual(BlockElementType.BlankLine, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\r\n", 0);
            Assert.AreEqual(BlockElementType.BlankLine, block.Type);
        }

        #endregion

        #region Unknown

        [TestMethod]
        public void DetermineNewBlockTypeTest_Unknown_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("Hello World", 0);
            Assert.AreEqual(BlockElementType.Unknown, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_Unknown_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("[foo]: ./hoge.jpg \"title\"", 0);
            Assert.AreEqual(BlockElementType.Unknown, block.Type);
        }

        #endregion
    }
}
