using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.InlineElementTest
{
    class InlineStructure
    {
        public InlineElementType Content { get; set; }

        public InlineStructure[] Children { get; set; }

        public string Text { get; set; }

        public InlineStructure(InlineElementType type, params InlineStructure[] structures)
        {
            Content = type;
            Children = structures ?? throw new ArgumentNullException(nameof(structures));
        }

        public InlineStructure(InlineElementType type, string content)
        {
            Text = content;
            Content = type;
        }

        public void AssertEqual(InlineElementBase element)
        {
            Assert.AreEqual(Content, element.Type);
            if (element is InlineText text)
            {
                Assert.AreEqual(0, Children?.Length ?? 0);
                Assert.AreEqual(Text, text.Content);
            }
            else if(element is CodeSpan code)
            {
                Assert.AreEqual(0, Children?.Length ?? 0);
                Assert.AreEqual(Text, code.Code);
            }
            else if (element is InlineHtml html)
            {
                Assert.AreEqual(0, Children?.Length ?? 0);
                Assert.AreEqual(Text, html.Content);
            }
            else if (element is ContainerInlineElement container)
            {
                Assert.AreEqual(Children.Length, container.Children.Length);
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].AssertEqual(container.Children[i]);
                }
            }
        }

        public void AssertEqual(InlineElementBase[] element)
        {
            if (element.Length == 1)
            {
                AssertEqual(element[0]);
            }
            else
            {
                Assert.AreEqual(Children.Length, element.Length);
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].AssertEqual(element[i]);
                }
            }
        }
    }
}
