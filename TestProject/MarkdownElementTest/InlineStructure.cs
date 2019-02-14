using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpdown.MarkdownElement.InlineElement;
using System.Linq;
using System.Collections.Generic;

namespace TestProject.MarkdownElementTest
{
    class InlineStructure
    {
        public InlineElementType Content { get; }

        private InlineStructure[] Children { get; }

        private string Text { get; }

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

        public InlineStructure(params InlineStructure[] structures)
        {
            Content = (InlineElementType)(-1);
            Children = structures;
        }

        public InlineStructure(InlineElementType type)
        {
            Content = type;
        }

        private void AssertEqual(InlineElement element)
        {
            Assert.AreEqual(Content, element.Type);
            if (element is InlineText text)
            {
                Assert.AreEqual(0, Children?.Length ?? 0);
                Assert.AreEqual(Text, text.Content);
            }
            else if (element is CodeSpan code)
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

        public void AssertEqual(IEnumerable<InlineElement> element)
        {
            IEnumerable<InlineElement> inlineElementBases = element as InlineElement[] ?? element.ToArray();
            if (inlineElementBases.Count() == 1)
            {
                AssertEqual(inlineElementBases.First());
            }
            else
            {
                Assert.AreEqual(Children.Length, inlineElementBases.Count());
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].AssertEqual(inlineElementBases.ElementAt(i));
                }
            }
        }
    }
}
