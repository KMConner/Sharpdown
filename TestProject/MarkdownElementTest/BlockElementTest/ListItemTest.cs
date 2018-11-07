using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.BlockElement;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    [TestClass]
    public class ListItemTest
    {
        [TestMethod]
        public void AddLineTest_01()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("- foo"));
            Assert.AreEqual(2, GetIndent(item));
            Assert.AreEqual(AddLineResult.NeedClose, item.AddLine("- foo"));
        }

        [TestMethod]
        public void AddLineTest_02()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("   -   foo"));
            Assert.AreEqual(7, GetIndent(item));
        }

        [TestMethod]
        public void AddLineTest_03()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("- foo"));
            Assert.AreEqual(2, GetIndent(item));
            Assert.AreEqual(AddLineResult.NeedClose, item.AddLine(" - foo"));
        }

        [TestMethod]
        public void AddLineTest_04()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("- foo"));
            Assert.AreEqual(2, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("  - foo"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.List, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_05()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("-     foo"));
            Assert.AreEqual(2, GetIndent(item));
        }

        [TestMethod]
        public void AddLineTest_06()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("3. foo"));
            Assert.AreEqual(3, GetIndent(item));
            Assert.AreEqual(AddLineResult.NeedClose, item.AddLine("334. foo"));
        }

        [TestMethod]
        public void AddLineTest_07()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("  1.   foo"));
            Assert.AreEqual(7, GetIndent(item));
        }

        [TestMethod]
        public void AddLineTest_08()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("12. foo"));
            Assert.AreEqual(4, GetIndent(item));
            Assert.AreEqual(AddLineResult.NeedClose, item.AddLine("   1. foo"));
        }

        [TestMethod]
        public void AddLineTest_09()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("1. foo"));
            Assert.AreEqual(3, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("   1. foo"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.List, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_10()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("1234.     foo"));
            Assert.AreEqual(6, GetIndent(item));
        }

        [TestMethod]
        public void AddLineTest_11()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("1. foo"));
            Assert.AreEqual(3, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("   2. foo"));
            Assert.AreEqual(1, item.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[0].Type);
        }

        [TestMethod]
        public void AddLineTest_12()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("1. foo"));
            Assert.AreEqual(3, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(""));
            Assert.AreEqual(AddLineResult.NeedClose, item.AddLine("bar"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_13()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("1. foo"));
            Assert.AreEqual(3, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("   bar"));
            Assert.AreEqual(3, item.Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[1].Type);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[2].Type);
        }

        [TestMethod]
        public void AddLineTest_14()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(" -    foo"));
            Assert.AreEqual(6, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(""));
            Assert.AreEqual(AddLineResult.NeedClose, item.AddLine("     bar"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_15()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed,  item.AddLine("-    foo"));
            Assert.AreEqual(5, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("     bar"));
            Assert.AreEqual(3, item.Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[1].Type);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[2].Type);
        }

        [TestMethod]
        public void AddLineTest_16()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("1.  foo"));
            Assert.AreEqual(4, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("    ```"));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("    bar"));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("    ```"));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("    baz"));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("    > bam"));
            Assert.AreEqual(6, item.Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.FencedCodeBlock, item.Children[1].Type);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[2].Type);
            Assert.AreEqual(BlockElementType.Paragraph, item.Children[3].Type);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[4].Type);
            Assert.AreEqual(BlockElementType.BlockQuote, item.Children[5].Type);
        }

        [TestMethod]
        public void AddLineTest_17()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(" - foo"));
            Assert.AreEqual(3, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("       bar"));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("       baz"));
            Assert.AreEqual(3, item.Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[1].Type);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, item.Children[2].Type);
        }

        [TestMethod]
        public void AddLineTest_18()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(" 334. foo"));
            Assert.AreEqual(6, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(""));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("          bar"));
            Assert.AreEqual(3, item.Children.Count);
            Assert.AreEqual(BlockElementType.Paragraph, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[1].Type);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, item.Children[2].Type);
        }

        [TestMethod]
        public void AddLineTest_19()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("-     foo"));
            Assert.AreEqual(2, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("  bar"));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("      baz"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.IndentedCodeBlock, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_20()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("- "));
            Assert.AreEqual(2, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("  bar"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_21()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("-  "));
            Assert.AreEqual(2, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("  bar"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[1].Type);
        }


        [TestMethod]
        public void AddLineTest_22()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("-"));
            Assert.AreEqual(2, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("  bar"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_23()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("1. "));
            Assert.AreEqual(3, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("   bar"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_24()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("1.   "));
            Assert.AreEqual(3, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("   bar"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_25()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(" 2."));
            Assert.AreEqual(4, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("    bar"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_26()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("- "));
            Assert.AreEqual(2, GetIndent(item));
            Assert.AreEqual(AddLineResult.NeedClose, item.AddLine("bar"));
            Assert.AreEqual(1, item.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[0].Type);
        }

        [TestMethod]
        public void AddLineTest_27()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("- "));
            Assert.AreEqual(2, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine(""));
            Assert.AreEqual(AddLineResult.NeedClose, item.AddLine("bar"));
            Assert.AreEqual(2, item.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[0].Type);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[1].Type);
        }

        [TestMethod]
        public void AddLineTest_28()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("*"));
            Assert.AreEqual(2, GetIndent(item));
            Assert.AreEqual(1, item.Children.Count);
            Assert.AreEqual(BlockElementType.BlankLine, item.Children[0].Type);
        }

        [TestMethod]
        public void AddLineTest_29()
        {
            ListItem item = TestUtils.CreateInternal<ListItem>();
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("  - Foo"));
            Assert.AreEqual(4, GetIndent(item));
            Assert.AreEqual(AddLineResult.Consumed, item.AddLine("Bar"));
            Assert.AreEqual(1, item.Children.Count);
            Assert.AreEqual(BlockElementType.Unknown, item.Children[0].Type);
        }




        private int GetIndent(ListItem item)
        {
            var variable = typeof(ListItem).GetField("contentIndent", BindingFlags.Instance | BindingFlags.NonPublic);
            return (int)variable.GetValue(item);
        }
    }
}
