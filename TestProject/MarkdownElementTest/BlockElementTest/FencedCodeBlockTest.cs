using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.BlockElement;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    [TestClass]
    public class FencedCodeBlockTest
    {
        [TestMethod]
        public void AddLineTest_01()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("```"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("hogehoge"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("```"));
            Assert.AreEqual("hogehoge", block.Content);
            Assert.AreEqual(string.Empty, block.InfoString);
        }

        [TestMethod]
        public void AddLineTest_02()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   ```"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("hogehoge"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("```"));
            Assert.AreEqual("hogehoge", block.Content);
            Assert.AreEqual(string.Empty, block.InfoString);
        }

        [TestMethod]
        public void AddLineTest_03()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   ```"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   hogehoge"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine(" ```"));
            Assert.AreEqual("hogehoge", block.Content);
            Assert.AreEqual(string.Empty, block.InfoString);
        }

        [TestMethod]
        public void AddLineTest_04()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   ````"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   hogehoge"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("   `````"));
            Assert.AreEqual("hogehoge", block.Content);
            Assert.AreEqual(string.Empty, block.InfoString);
        }

        [TestMethod]
        public void AddLineTest_05()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   ````"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    hogehoge "));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("`````"));
            Assert.AreEqual(" hogehoge ", block.Content);
            Assert.AreEqual(string.Empty, block.InfoString);
        }

        [TestMethod]
        public void AddLineTest_06()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   ````python"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    hogehoge "));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("`````"));
            Assert.AreEqual(" hogehoge ", block.Content);
            Assert.AreEqual("python", block.InfoString);
        }

        [TestMethod]
        public void AddLineTest_07()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   ````  python\t"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    hogehoge "));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("`````"));
            Assert.AreEqual(" hogehoge ", block.Content);
            Assert.AreEqual("python", block.InfoString);
        }

        [TestMethod]
        public void AddLineTest_08()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   ````python"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    hogehoge "));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("`````"));
            Assert.AreEqual(" hogehoge ", block.Content);
            Assert.AreEqual("python", block.InfoString);
        }

        [TestMethod]
        public void AddLineTest_09()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   ````p y t h o n"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    hogehoge "));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("`````"));
            Assert.AreEqual(" hogehoge ", block.Content);
            Assert.AreEqual("p y t h o n", block.InfoString);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBlockFormatException))]
        public void AddLineTest_10()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("````python`java"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBlockFormatException))]
        public void AddLineTest_11()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("``"));
        }

        [TestMethod]
        public void AddLineTest_12()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("````"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    hogehoge "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("```"));
        }

        [TestMethod]
        public void AddLineTest_13()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("~~~"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("hogehoge"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("~~~"));
            Assert.AreEqual(string.Empty, block.InfoString);
            Assert.AreEqual("hogehoge", block.Content);
        }

        [TestMethod]
        public void AddLineTest_14()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(" ~~~  foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(" bar baz"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("~~~~~"));
            Assert.AreEqual("foo", block.InfoString);
            Assert.AreEqual("bar baz", block.Content);
        }

        [TestMethod]
        public void AddLineTest_15()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(" ~~~  foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(" bar baz"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("````"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("~~~~~"));
            Assert.AreEqual("foo", block.InfoString);
            Assert.AreEqual("bar baz\r\n````", block.Content);
        }

        [TestMethod]
        public void AddLineTest_16()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("````  foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(" bar baz"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("~~~~"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("```````"));
            Assert.AreEqual("foo", block.InfoString);
            Assert.AreEqual(" bar baz\r\n~~~~", block.Content);
        }

        [TestMethod]
        public void AddLineTest_17()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("````foo"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("```````"));
            Assert.AreEqual("foo", block.InfoString);
            Assert.AreEqual(string.Empty, block.Content);
        }

        [TestMethod]
        public void AddLineTest_18()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("~~~~foo"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("~~~~~"));
            Assert.AreEqual("foo", block.InfoString);
            Assert.AreEqual(string.Empty, block.Content);
        }

        [TestMethod]
        public void AddLineTest_19()
        {
            FencedCodeBlock block = TestUtils.CreateInternal<FencedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("````foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("```"));
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose,
                block.AddLine("```````"));
            Assert.AreEqual("foo", block.InfoString);
            Assert.AreEqual("```", block.Content);
        }
    }
}
