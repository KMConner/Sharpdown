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
            var block = BlockElementUtil.CreateBlockFromLine("---");
            Assert.AreEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   ---");
            Assert.AreEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("- -  ---------- -");
            Assert.AreEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  ***\t*");
            Assert.AreEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("__ _ _");
            Assert.AreEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t__ _ _");
            Assert.AreNotEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("--");
            Assert.AreNotEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   ");
            Assert.AreNotEqual(BlockElementType.ThemanticBreak, block.Type);
        }

        #endregion

        #region ATX Heading

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("##");
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("## Hello");
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("##\tTitle");
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("#");
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  #");
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("###### Title");
            Assert.AreEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("####### Title");
            Assert.AreNotEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\\####### Title");
            Assert.AreNotEqual(BlockElementType.AtxHeading, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine("###Title");
            Assert.AreNotEqual(BlockElementType.AtxHeading, block.Type);
        }

        #endregion

        #region IndentedCodeBlock

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("    aa");
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\taa");
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("                        aa");
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t\taa");
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("     ");
            Assert.AreNotEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t\r\n");
            Assert.AreNotEqual(BlockElementType.IndentedCodeBlock, block.Type);
        }

        #endregion

        #region FencedCodeBlock

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("```");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("~~~");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("``````");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   `````");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("    `````\n");
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t`````");
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("`````csharp");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("~~~~~````");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine("````aa``");
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_10()
        {
            var block = BlockElementUtil.CreateBlockFromLine("~~~aa~~");
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, block.Type);
        }
        #endregion

        #region HtmlBlock

        #region Type1

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<script");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<pre");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<style");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <style");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  <style ");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }


        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  <style\r\n");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  <style>");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  <style block=");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine("    <style");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_10()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t<style");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_11()
        {
            var block = BlockElementUtil.CreateBlockFromLine("< style");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_12()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<styles");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_13()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<STYLE");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_14()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<StYlE");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }


        #endregion

        #region Type2

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!--");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <!--");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!--aaaa");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("< !--aaaa");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<! --aaaa");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!-- aaaa");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #region Type3

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<?");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <?");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("    <?");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t<?");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }


        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<?>");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("< ?");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #region Type4

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!A");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <!A");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!a");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("< !A");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<!ZABC");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #region Type5

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<![CDATA[");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <![CDATA[[[[");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <![CDATA[????]>");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<! [ CDATA[");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #region Type6

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<address");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("</address");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<AddReSs");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<address ");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<address/>");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   <address>");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<address/\r\n");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<addressA");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<address");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<article");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<aside");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<base");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<basefont");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<blockquote");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<body");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<caption");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<center");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<col");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<colgroup");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<dd");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<details");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<dialog");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<dir");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<div");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<dl");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<dt");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<fieldset");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<figcaption");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<figure");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<footer");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<form");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<frame");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<frameset");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h1");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h2");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h3");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h4");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h5");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<h6");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<head");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<header");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<hr");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<html");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<iframe");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<legend");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<li");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<link");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<main");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<menu");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<menuitem");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<meta");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<nav");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<noframes");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<ol");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<optgroup");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<option");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<p");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<param");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<section");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<source");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<summary");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<table");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<tbody");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<td");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<tfoot");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<th");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<thead");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<title");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<tr");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<track");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);

            block = BlockElementUtil.CreateBlockFromLine("<ul");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #region Type7

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc>");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc >");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc />");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc aa=\"bb\">");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc aa =\"bb\">");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc aa= \"bb\">");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc _aa='bb' >");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc aa='bb' bb = cc />");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine(
                "<abc :aa='bb' b-b = cc d_: = \"ee\" a.1 = ??!  />");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_10()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc :ab_c. />");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_11()
        {
            var block = BlockElementUtil.CreateBlockFromLine("</aaa>");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_12()
        {
            var block = BlockElementUtil.CreateBlockFromLine("</aaa >");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_13()
        {
            var block = BlockElementUtil.CreateBlockFromLine("</a>");
            Assert.AreEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_14()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc/ >");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_15()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc/ >");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_16()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc 123>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_17()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc .aaa>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_18()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc -aaa>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_19()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc abc=\"a\"b\">");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_20()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc abc='b'b'>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_21()
        {
            var block = BlockElementUtil.CreateBlockFromLine("<abc abc=ab`c>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_22()
        {
            var block = BlockElementUtil.CreateBlockFromLine("</ abc>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, block.Type);
        }

        #endregion

        #endregion

        #region BlockQuote

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("> ");
            Assert.AreEqual(BlockElementType.BlockQuote, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("  > > >");
            Assert.AreEqual(BlockElementType.BlockQuote, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   >>");
            Assert.AreEqual(BlockElementType.BlockQuote, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine(" ");
            Assert.AreNotEqual(BlockElementType.BlockQuote, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine(" >            ");
            Assert.AreEqual(BlockElementType.BlockQuote, block.Type);
        }

        #endregion

        #region ListBlock

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("- Title");
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("* Title");
            Assert.AreEqual(BlockElementType.List, block.Type);
        }


        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("+ Title");
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("-");
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_5()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   -");
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_6()
        {
            var block = BlockElementUtil.CreateBlockFromLine("-title");
            Assert.AreNotEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_7()
        {
            var block = BlockElementUtil.CreateBlockFromLine("123. title");
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_8()
        {
            var block = BlockElementUtil.CreateBlockFromLine("123) title");
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_9()
        {
            var block = BlockElementUtil.CreateBlockFromLine("123456789. title");
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_10()
        {
            var block = BlockElementUtil.CreateBlockFromLine("123456789.");
            Assert.AreEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_11()
        {
            var block = BlockElementUtil.CreateBlockFromLine("123456789.title");
            Assert.AreNotEqual(BlockElementType.List, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_12()
        {
            var block = BlockElementUtil.CreateBlockFromLine("1234567890. title");
            Assert.AreNotEqual(BlockElementType.List, block.Type);
        }

        #endregion

        #region BlankLine

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("");
            Assert.AreEqual(BlockElementType.BlankLine, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("   ");
            Assert.AreEqual(BlockElementType.BlankLine, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_3()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\t");
            Assert.AreEqual(BlockElementType.BlankLine, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_4()
        {
            var block = BlockElementUtil.CreateBlockFromLine("\r\n");
            Assert.AreEqual(BlockElementType.BlankLine, block.Type);
        }

        #endregion

        #region Unknown

        [TestMethod]
        public void DetermineNewBlockTypeTest_Unknown_1()
        {
            var block = BlockElementUtil.CreateBlockFromLine("Hello World");
            Assert.AreEqual(BlockElementType.Unknown, block.Type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_Unknown_2()
        {
            var block = BlockElementUtil.CreateBlockFromLine("[foo]: ./hoge.jpg \"title\"");
            Assert.AreEqual(BlockElementType.Unknown, block.Type);
        }

        #endregion
    }
}
