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
            var type = BlockElementUtil.DetermineNewBlockType("---");
            Assert.AreEqual(BlockElementType.ThemanticBreak, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   ---");
            Assert.AreEqual(BlockElementType.ThemanticBreak, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("- -  ---------- -");
            Assert.AreEqual(BlockElementType.ThemanticBreak, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("  ***\t*");
            Assert.AreEqual(BlockElementType.ThemanticBreak, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("__ _ _");
            Assert.AreEqual(BlockElementType.ThemanticBreak, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_6()
        {
            var type = BlockElementUtil.DetermineNewBlockType("\t__ _ _");
            Assert.AreNotEqual(BlockElementType.ThemanticBreak, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_7()
        {
            var type = BlockElementUtil.DetermineNewBlockType("--");
            Assert.AreNotEqual(BlockElementType.ThemanticBreak, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ThemanticBreak_8()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   ");
            Assert.AreNotEqual(BlockElementType.ThemanticBreak, type);
        }

        #endregion

        #region ATX Heading

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("##");
            Assert.AreEqual(BlockElementType.AtxHeading, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("## Hello");
            Assert.AreEqual(BlockElementType.AtxHeading, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("##\tTitle");
            Assert.AreEqual(BlockElementType.AtxHeading, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("#");
            Assert.AreEqual(BlockElementType.AtxHeading, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("  #");
            Assert.AreEqual(BlockElementType.AtxHeading, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_6()
        {
            var type = BlockElementUtil.DetermineNewBlockType("###### Title");
            Assert.AreEqual(BlockElementType.AtxHeading, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_7()
        {
            var type = BlockElementUtil.DetermineNewBlockType("####### Title");
            Assert.AreNotEqual(BlockElementType.AtxHeading, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_8()
        {
            var type = BlockElementUtil.DetermineNewBlockType("\\####### Title");
            Assert.AreNotEqual(BlockElementType.AtxHeading, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_AtxHeading_9()
        {
            var type = BlockElementUtil.DetermineNewBlockType("###Title");
            Assert.AreNotEqual(BlockElementType.AtxHeading, type);
        }

        #endregion

        #region IndentedCodeBlock

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("    aa");
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("\taa");
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("                        aa");
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("\t\taa");
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("     ");
            Assert.AreNotEqual(BlockElementType.IndentedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_IndentedCodeBlock_6()
        {
            var type = BlockElementUtil.DetermineNewBlockType("\t\r\n");
            Assert.AreNotEqual(BlockElementType.IndentedCodeBlock, type);
        }

        #endregion

        #region FencedCodeBlock

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("```");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("~~~");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("``````");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   `````");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("    `````\n");
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_6()
        {
            var type = BlockElementUtil.DetermineNewBlockType("\t`````");
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_7()
        {
            var type = BlockElementUtil.DetermineNewBlockType("`````csharp");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_8()
        {
            var type = BlockElementUtil.DetermineNewBlockType("~~~~~````");
            Assert.AreEqual(BlockElementType.FencedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_9()
        {
            var type = BlockElementUtil.DetermineNewBlockType("````aa``");
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_FencedCodeBlock_10()
        {
            var type = BlockElementUtil.DetermineNewBlockType("~~~aa~~");
            Assert.AreNotEqual(BlockElementType.FencedCodeBlock, type);
        }
        #endregion

        #region HtmlBlock

        #region Type1

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<script");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<pre");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<style");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   <style");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("  <style ");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }


        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_6()
        {
            var type = BlockElementUtil.DetermineNewBlockType("  <style\r\n");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_7()
        {
            var type = BlockElementUtil.DetermineNewBlockType("  <style>");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_8()
        {
            var type = BlockElementUtil.DetermineNewBlockType("  <style type=");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_9()
        {
            var type = BlockElementUtil.DetermineNewBlockType("    <style");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_10()
        {
            var type = BlockElementUtil.DetermineNewBlockType("\t<style");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_11()
        {
            var type = BlockElementUtil.DetermineNewBlockType("< style");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_12()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<styles");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_13()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<STYLE");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_1_14()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<StYlE");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }


        #endregion

        #region Type2

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<!--");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   <!--");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<!--aaaa");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("< !--aaaa");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<! --aaaa");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_2_6()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<!-- aaaa");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        #endregion

        #region Type3

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<?");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   <?");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("    <?");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("\t<?");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }


        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<?>");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_3_6()
        {
            var type = BlockElementUtil.DetermineNewBlockType("< ?");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        #endregion

        #region Type4

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<!A");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   <!A");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<!a");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("< !A");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_4_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<!ZABC");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        #endregion

        #region Type5

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<![CDATA[");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   <![CDATA[[[[");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   <![CDATA[????]>");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_5_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<! [ CDATA[");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        #endregion

        #region Type6

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<address");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("</address");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<AddReSs");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<address ");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<address/>");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_6()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   <address>");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_7()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<address/\r\n");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_8()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<addressA");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_6_9()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<address");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<article");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<aside");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<base");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<basefont");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<blockquote");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<body");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<caption");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<center");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<col");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<colgroup");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<dd");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<details");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<dialog");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<dir");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<div");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<dl");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<dt");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<fieldset");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<figcaption");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<figure");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<footer");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<form");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<frame");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<frameset");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<h1");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<h2");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<h3");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<h4");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<h5");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<h6");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<head");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<header");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<hr");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<html");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<iframe");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<legend");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<li");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<link");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<main");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<menu");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<menuitem");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<meta");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<nav");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<noframes");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<ol");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<optgroup");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<option");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<p");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<param");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<section");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<source");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<summary");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<table");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<tbody");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<td");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<tfoot");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<th");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<thead");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<title");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<tr");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<track");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);

            type = BlockElementUtil.DetermineNewBlockType("<ul");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        #endregion

        #region Type7

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc>");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc >");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc />");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc aa=\"bb\">");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc aa =\"bb\">");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_6()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc aa= \"bb\">");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_7()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc _aa='bb' >");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_8()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc aa='bb' bb = cc />");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_9()
        {
            var type = BlockElementUtil.DetermineNewBlockType(
                "<abc :aa='bb' b-b = cc d_: = \"ee\" a.1 = ??!  />");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_10()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc :ab_c. />");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_11()
        {
            var type = BlockElementUtil.DetermineNewBlockType("</aaa>");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_12()
        {
            var type = BlockElementUtil.DetermineNewBlockType("</aaa >");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_13()
        {
            var type = BlockElementUtil.DetermineNewBlockType("</a>");
            Assert.AreEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_14()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc/ >");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_15()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc/ >");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_16()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc 123>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_17()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc .aaa>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_18()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc -aaa>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_19()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc abc=\"a\"b\">");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_20()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc abc='b'b'>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_21()
        {
            var type = BlockElementUtil.DetermineNewBlockType("<abc abc=ab`c>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_HtmlBlock_7_22()
        {
            var type = BlockElementUtil.DetermineNewBlockType("</ abc>");
            Assert.AreNotEqual(BlockElementType.HtmlBlock, type);
        }

        #endregion

        #endregion

        #region BlockQuote

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("> ");
            Assert.AreEqual(BlockElementType.BlockQuote, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("  > > >");
            Assert.AreEqual(BlockElementType.BlockQuote, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   >>");
            Assert.AreEqual(BlockElementType.BlockQuote, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType(" ");
            Assert.AreNotEqual(BlockElementType.BlockQuote, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlockQuote_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType(" >            ");
            Assert.AreEqual(BlockElementType.BlockQuote, type);
        }

        #endregion

        #region ListBlock

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("- Title");
            Assert.AreEqual(BlockElementType.List, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("* Title");
            Assert.AreEqual(BlockElementType.List, type);
        }


        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("+ Title");
            Assert.AreEqual(BlockElementType.List, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("-");
            Assert.AreEqual(BlockElementType.List, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_5()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   -");
            Assert.AreEqual(BlockElementType.List, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_6()
        {
            var type = BlockElementUtil.DetermineNewBlockType("-title");
            Assert.AreNotEqual(BlockElementType.List, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_7()
        {
            var type = BlockElementUtil.DetermineNewBlockType("123. title");
            Assert.AreEqual(BlockElementType.List, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_8()
        {
            var type = BlockElementUtil.DetermineNewBlockType("123) title");
            Assert.AreEqual(BlockElementType.List, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_9()
        {
            var type = BlockElementUtil.DetermineNewBlockType("123456789. title");
            Assert.AreEqual(BlockElementType.List, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_10()
        {
            var type = BlockElementUtil.DetermineNewBlockType("123456789.");
            Assert.AreEqual(BlockElementType.List, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_11()
        {
            var type = BlockElementUtil.DetermineNewBlockType("123456789.title");
            Assert.AreNotEqual(BlockElementType.List, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_ListBlock_12()
        {
            var type = BlockElementUtil.DetermineNewBlockType("1234567890. title");
            Assert.AreNotEqual(BlockElementType.List, type);
        }

        #endregion

        #region BlankLine

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("");
            Assert.AreEqual(BlockElementType.BlankLine, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("   ");
            Assert.AreEqual(BlockElementType.BlankLine, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_3()
        {
            var type = BlockElementUtil.DetermineNewBlockType("\t");
            Assert.AreEqual(BlockElementType.BlankLine, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_BlankLine_4()
        {
            var type = BlockElementUtil.DetermineNewBlockType("\r\n");
            Assert.AreEqual(BlockElementType.BlankLine, type);
        }

        #endregion

        #region Unknown

        [TestMethod]
        public void DetermineNewBlockTypeTest_Unknown_1()
        {
            var type = BlockElementUtil.DetermineNewBlockType("Hello World");
            Assert.AreEqual(BlockElementType.Unknown, type);
        }

        [TestMethod]
        public void DetermineNewBlockTypeTest_Unknown_2()
        {
            var type = BlockElementUtil.DetermineNewBlockType("[foo]: ./hoge.jpg \"title\"");
            Assert.AreEqual(BlockElementType.Unknown, type);
        }

        #endregion
    }
}
