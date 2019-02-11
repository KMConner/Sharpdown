using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.BlockElement;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    [TestClass]
    public class UnknownElementTest
    {
        #region AddLine

        [TestMethod]
        public void AddLineTest_01()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret = element.AddLine("Hello");
            Assert.AreEqual(AddLineResult.Consumed, ret);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBlockFormatException))]
        public void AddLineTest_02()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret = element.AddLine("");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBlockFormatException))]
        public void AddLineTest_03()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret = element.AddLine("   ");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBlockFormatException))]
        public void AddLineTest_04()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret = element.AddLine("    ");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBlockFormatException))]
        public void AddLineTest_05()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret = element.AddLine("\t");
        }

        [TestMethod]
        public void AddLineTest_06()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("");
            Assert.AreEqual(AddLineResult.NeedClose, ret2);
        }

        [TestMethod]
        public void AddLineTest_07()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("---");
            Assert.AreEqual(AddLineResult.NeedClose | AddLineResult.Consumed, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
        }

        [TestMethod]
        public void AddLineTest_08()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("***");
            Assert.AreEqual(AddLineResult.NeedClose, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_09()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("---");
            Assert.AreEqual(AddLineResult.NeedClose | AddLineResult.Consumed, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
        }

        [TestMethod]
        public void AddLineTest_10()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("# Titile");
            Assert.AreEqual(AddLineResult.NeedClose, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_11()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("===");
            Assert.AreEqual(AddLineResult.NeedClose | AddLineResult.Consumed, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
        }

        [TestMethod]
        public void AddLineTest_12()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("=");
            Assert.AreEqual(AddLineResult.NeedClose | AddLineResult.Consumed, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
        }

        [TestMethod]
        public void AddLineTest_13()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("   ---");
            Assert.AreEqual(AddLineResult.NeedClose | AddLineResult.Consumed, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
        }

        [TestMethod]
        public void AddLineTest_14()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("    ---");
            Assert.AreEqual(AddLineResult.Consumed, ret2);
        }

        [TestMethod]
        public void AddLineTest_15()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("   - --");
            Assert.AreEqual(AddLineResult.NeedClose, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_16()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("  =  = ");
            Assert.AreEqual(AddLineResult.Consumed, ret2);
        }

        [TestMethod]
        public void AddLineTest_17()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("  ---  ");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
        }

        [TestMethod]
        public void AddLineTest_18()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("Bar");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("  ---  ");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
        }

        [TestMethod]
        public void AddLineTest_19()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("===");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
        }

        [TestMethod]
        public void AddLineTest_20()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("     Bar");
            Assert.AreEqual(AddLineResult.Consumed, ret2);
        }

        [TestMethod]
        public void AddLineTest_21()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("```");
            Assert.AreEqual(AddLineResult.NeedClose, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_22()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("~~~~");
            Assert.AreEqual(AddLineResult.NeedClose, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_23()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("```c# .number");
            Assert.AreEqual(AddLineResult.NeedClose, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_24()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("<div  id='foo' class='bar'");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_25()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("<a href=\"bar\">");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("Baz");
        }

        [TestMethod]
        public void AddLineTest_26()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("[foo]: bar 'baz'");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
        }

        [TestMethod]
        public void AddLineTest_27()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]: bar 'baz'");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, actualType);
        }

        [TestMethod]
        public void AddLineTest_28()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]: bar");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, actualType);
        }

        [TestMethod]
        public void AddLineTest_29()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]:bar");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, actualType);
        }

        [TestMethod]
        public void AddLineTest_30()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]: <bar>");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("'baz'");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, actualType);
        }

        [TestMethod]
        public void AddLineTest_31()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]: (bar)");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("(baz)");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, actualType);
        }

        [TestMethod]
        public void AddLineTest_32()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]: (b(a)r)");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("\"baz\"");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, actualType);
        }

        [TestMethod]
        public void AddLineTest_33()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]:");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("\t\thoge");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("\"baz\"");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, actualType);
        }

        [TestMethod]
        public void AddLineTest_34()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]:");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("(h(oge)");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("boo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("   ");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_35()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]:<bar>");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("[boo]: baz");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, actualType);
        }

        [TestMethod]
        public void AddLineTest_36()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("[boo]: baz");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_37()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[" + new string('A', 999) + "]: bar 'baz'");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, actualType);
        }

        [TestMethod]
        public void AddLineTest_38()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[" + new string('A', 1000) + "]: bar 'baz'");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
        }

        [TestMethod]
        public void AddLineTest_39()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("        baz");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
        }

        [TestMethod]
        public void AddLineTest_40()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("> baz");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_41()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine(">baz");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_42()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine(">>baz");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_43()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("- bar");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_44()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("   - bar");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_45()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("1. bar");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_46()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("   1. bar");
            Assert.AreEqual(AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.Paragraph, actualType);
        }

        [TestMethod]
        public void AddLineTest_47()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("123456. bar");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
        }

        #endregion

        #region Close

        [TestMethod]
        public void CloseTest_01()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret = element.AddLine("Hello");
            Assert.AreEqual(AddLineResult.Consumed, ret);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.Paragraph, closed.Type);
            Assert.AreEqual("Hello", ((Paragraph)closed).Content);
        }

        [TestMethod]
        public void CloseTest_02()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("");
            Assert.AreEqual(AddLineResult.NeedClose, ret2);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.Paragraph, closed.Type);
            Assert.AreEqual("Foo", ((Paragraph)closed).Content);
        }

        [TestMethod]
        public void CloseTest_03()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("---");
            Assert.AreEqual(AddLineResult.NeedClose | AddLineResult.Consumed, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.SetextHeading, closed.Type);
            Assert.AreEqual(2, ((SetextHeader)closed).HeaderLevel);
            Assert.AreEqual("Foo", ((SetextHeader)closed).Content);
        }

        [TestMethod]
        public void CloseTest_04()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("===");
            Assert.AreEqual(AddLineResult.NeedClose | AddLineResult.Consumed, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.SetextHeading, closed.Type);
            Assert.AreEqual(1, ((SetextHeader)closed).HeaderLevel);
            Assert.AreEqual("Foo", ((SetextHeader)closed).Content);
        }

        [TestMethod]
        public void CloseTest_05()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("   Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("---");
            Assert.AreEqual(AddLineResult.NeedClose | AddLineResult.Consumed, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.SetextHeading, closed.Type);
            Assert.AreEqual(2, ((SetextHeader)closed).HeaderLevel);
            Assert.AreEqual("Foo", ((SetextHeader)closed).Content);
        }

        [TestMethod]
        public void CloseTest_06()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("Foo");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("Bar");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var ret2 = element.AddLine("  ---  ");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret2);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.SetextHeading, actualType);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.SetextHeading, closed.Type);
            Assert.AreEqual(2, ((SetextHeader)closed).HeaderLevel);
            Assert.AreEqual("Foo\r\nBar", ((SetextHeader)closed).Content);
        }

        [TestMethod]
        public void CloseTest_07()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]: bar 'baz'");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret1);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, closed.Type);
            Assert.AreEqual("foo", ((LinkReferenceDefinition)closed).Label);
            Assert.AreEqual("bar", ((LinkReferenceDefinition)closed).Destination);
            Assert.AreEqual("baz", ((LinkReferenceDefinition)closed).Title);
        }

        [TestMethod]
        public void CloseTest_08()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]: <bar> \"baz\"");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret1);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, closed.Type);
            Assert.AreEqual("foo", ((LinkReferenceDefinition)closed).Label);
            Assert.AreEqual("bar", ((LinkReferenceDefinition)closed).Destination);
            Assert.AreEqual("baz", ((LinkReferenceDefinition)closed).Title);
        }

        [TestMethod]
        public void CloseTest_09()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]: <bar> 'baz");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("boo'");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret1);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, closed.Type);
            Assert.AreEqual("foo", ((LinkReferenceDefinition)closed).Label);
            Assert.AreEqual("bar", ((LinkReferenceDefinition)closed).Destination);
            Assert.AreEqual("baz\nboo", ((LinkReferenceDefinition)closed).Title);
        }

        [TestMethod]
        public void CloseTest_10()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]: <bar>");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            ret1 = element.AddLine("'baz'");
            Assert.AreEqual(AddLineResult.Consumed | AddLineResult.NeedClose, ret1);
            var actualType = GetActualType(element);
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, actualType);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, closed.Type);
            Assert.AreEqual("foo", ((LinkReferenceDefinition)closed).Label);
            Assert.AreEqual("bar", ((LinkReferenceDefinition)closed).Destination);
            Assert.AreEqual("baz", ((LinkReferenceDefinition)closed).Title);
        }

        [TestMethod]
        public void CloseTest_11()
        {
            UnknownElement element = TestUtils.CreateInternal<UnknownElement>();
            var ret1 = element.AddLine("[foo]: <bar>");
            Assert.AreEqual(AddLineResult.Consumed, ret1);
            var actualType = GetActualType(element);
            var closed = element.Close();
            Assert.AreEqual(BlockElementType.LinkReferenceDefinition, closed.Type);
            Assert.AreEqual("foo", ((LinkReferenceDefinition)closed).Label);
            Assert.AreEqual("bar", ((LinkReferenceDefinition)closed).Destination);
            Assert.AreEqual(null, ((LinkReferenceDefinition)closed).Title);
        }

        #endregion

        private BlockElementType GetActualType(UnknownElement elem)
        {
            Type elemType = elem.GetType();
            var member = elemType
                .GetField("actualType", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(elem);
            return (BlockElementType)member;
        }
    }
}
