using System;
using System.Collections.Generic;
using System.Text;
using Sharpdown.MarkdownElement.InlineElement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            if (element is LiteralText literal)
            {
                Assert.AreEqual(0, Children?.Length ?? 0);
                Assert.AreEqual(Text, literal.Content);
            }
            else if (element is InlineText text)
            {
                Assert.AreEqual(0, Children?.Length ?? 0);
                Assert.AreEqual(Text, text.Content);
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
