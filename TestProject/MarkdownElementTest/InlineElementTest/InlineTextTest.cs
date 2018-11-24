using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.InlineElementTest
{
    [TestClass]
    public class InlineTextTest
    {
        [TestMethod]
        public void CreateFromTextTest_01()
        {
            InlineText text = CreateInlineTextFromString("Foo Bar");
            Assert.AreEqual("Foo Bar", text.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFromTextTest_02()
        {
            InlineText text = CreateInlineTextFromString("Foo\rBar");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFromTextTest_03()
        {
            InlineText text = CreateInlineTextFromString("Foo\nBar");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFromTextTest_04()
        {
            InlineText text = CreateInlineTextFromString("Foo\r\nBar");
        }

        [TestMethod]
        public void CreateFromTextTest_05()
        {
            InlineText text = CreateInlineTextFromString(@"\!\""\#\$\%\&\'\(\)\*\+\,\-\.\/\:\;\<\=\>\?\@\[\\\]\^\_\`\{\|\}\~");
            Assert.AreEqual("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~", text.Content);
        }

        [TestMethod]
        public void CreateFromTextTest_06()
        {
            InlineText text = CreateInlineTextFromString(@"\→\A\a\ \3\φ\«");
            Assert.AreEqual(@"\→\A\a\ \3\φ\«", text.Content);
        }

        [TestMethod]
        public void CreateFromTextTest_07()
        {
            InlineText text = CreateInlineTextFromString(@"Foo \");
            Assert.AreEqual(@"Foo \", text.Content);
        }

        [TestMethod]
        public void CreateFromTextTest_08()
        {
            InlineText text = CreateInlineTextFromString(
                "&nbsp; &amp; &copy; &AElig; &Dcaron; &frac34; &HilbertSpace;" +
                " &DifferentialD; &ClockwiseContourIntegral; &ngE;");
            Assert.AreEqual("\x00A0 & © Æ Ď ¾ ℋ ⅆ ∲ ≧̸", text.Content);
        }

        [TestMethod]
        public void CreateFromTextTest_09()
        {
            InlineText text = CreateInlineTextFromString("&#35; &#1234; &#992; &#98765432; &#0;");
            Assert.AreEqual("# Ӓ Ϡ \xFFFD \xFFFD", text.Content);
        }

        [TestMethod]
        public void CreateFromTextTest_10()
        {
            InlineText text = CreateInlineTextFromString("&#X22; &#XD06; &#xcab; &;");
            Assert.AreEqual("\" ആ ಫ &;", text.Content);
        }

        [TestMethod]
        public void CreateFromTextTest_11()
        {
            InlineText text = CreateInlineTextFromString("&nbsp &x; &#; &#x; &ThisIsNotDefined; &hi?;");
            Assert.AreEqual("&nbsp &x; &#; &#x; &ThisIsNotDefined; &hi?;", text.Content);
        }

        private InlineText CreateInlineTextFromString(string text)
        {
            var type = typeof(InlineText);
            MethodInfo method = type.GetMethod("CreateFromText", BindingFlags.NonPublic | BindingFlags.Static);
            try
            {
                return (InlineText)method.Invoke(null, new object[] { text, true });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
