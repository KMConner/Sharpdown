using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.BlockElement;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    [TestClass]
    public class AtxHeaderElementTest
    {
        [TestMethod]
        public void AddLineTest_01()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("# hoge");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret);
            Assert.AreEqual(1, elem.Level);
            Assert.AreEqual("hoge", elem.Content);
        }

        [TestMethod]
        public void AddLineTest_02()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("#");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret);
            Assert.AreEqual(1, elem.Level);
            Assert.AreEqual(string.Empty, elem.Content);
        }

        [TestMethod]
        public void AddLineTest_03()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("###    hogehoge");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret);
            Assert.AreEqual(3, elem.Level);
            Assert.AreEqual("hogehoge", elem.Content);
        }

        [TestMethod]
        public void AddLineTest_04()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("###    hogehoge  ###");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret);
            Assert.AreEqual(3, elem.Level);
            Assert.AreEqual("hogehoge", elem.Content);
        }

        [TestMethod]
        public void AddLineTest_05()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("######    hogehoge");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret);
            Assert.AreEqual(6, elem.Level);
            Assert.AreEqual("hogehoge", elem.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBlockFormatException))]
        public void AddLineTest_06()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("###hoge");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBlockFormatException))]
        public void AddLineTest_07()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("####### hoge");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBlockFormatException))]
        public void AddLineTest_08()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("    ### hoge");
        }

        [TestMethod]
        public void AddLineTest_09()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("   ### hoge");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret);
            Assert.AreEqual(3, elem.Level);
            Assert.AreEqual("hoge", elem.Content);
        }

        [TestMethod]
        public void AddLineTest_10()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("   ### hoge ####  ###");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret);
            Assert.AreEqual(3, elem.Level);
            Assert.AreEqual("hoge ####", elem.Content);
        }

        [TestMethod]
        public void AddLineTest_11()
        {
            AtxHeaderElement elem = new AtxHeaderElement();
            var ret = elem.AddLine("   ### foo bar ");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret);
            Assert.AreEqual(3, elem.Level);
            Assert.AreEqual("foo bar", elem.Content);
        }
    }
}
