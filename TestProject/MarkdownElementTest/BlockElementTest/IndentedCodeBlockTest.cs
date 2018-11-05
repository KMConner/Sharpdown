using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.BlockElement;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    [TestClass]
    public class IndentedCodeBlockTest
    {
        [TestMethod]
        public void AddLineTest_01()
        {
            IndentedCodeBlock block = TestUtils.CreateInternal<IndentedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    Bar"));
            Assert.AreEqual("Foo\r\nBar", block.Content);
        }

        [TestMethod]
        public void AddLineTest_02()
        {
            IndentedCodeBlock block = TestUtils.CreateInternal<IndentedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tFoo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tBar"));
            Assert.AreEqual("Foo\r\nBar", block.Content);
        }

        [TestMethod]
        public void AddLineTest_03()
        {
            IndentedCodeBlock block = TestUtils.CreateInternal<IndentedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tFoo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tBar"));
            Assert.AreEqual("Foo\r\n\r\nBar", block.Content);
        }

        [TestMethod]
        public void AddLineTest_04()
        {
            IndentedCodeBlock block = TestUtils.CreateInternal<IndentedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tFoo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tBar"));
            Assert.AreEqual("Foo\r\n\r\nBar", block.Content);
        }

        [TestMethod]
        public void AddLineTest_05()
        {
            IndentedCodeBlock block = TestUtils.CreateInternal<IndentedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tFoo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("   "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tBar"));
            Assert.AreEqual("Foo\r\n\r\nBar", block.Content);
        }

        [TestMethod]
        public void AddLineTest_06()
        {
            IndentedCodeBlock block = TestUtils.CreateInternal<IndentedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tFoo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("     "));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tBar"));
            Assert.AreEqual("Foo\r\n \r\nBar", block.Content);
        }

        [TestMethod]
        public void AddLineTest_07()
        {
            IndentedCodeBlock block = TestUtils.CreateInternal<IndentedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    Foo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tBar"));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine("Baz"));
            Assert.AreEqual("Foo\r\nBar", block.Content);
        }

        [TestMethod]
        public void AddLineTest_08()
        {
            IndentedCodeBlock block = TestUtils.CreateInternal<IndentedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tFoo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tBar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("    \t"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine("Baz"));
            Assert.AreEqual("Foo\r\nBar\r\n\t", block.Content);
        }

        [TestMethod]
        public void AddLineTest_09()
        {
            IndentedCodeBlock block = TestUtils.CreateInternal<IndentedCodeBlock>();
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tFoo"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine("\tBar"));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed, block.AddLine(""));
            Assert.AreEqual(AddLineResult.NeedClose, block.AddLine("Baz"));
            Assert.AreEqual("Foo\r\nBar", block.Content);
        }

    }
}
