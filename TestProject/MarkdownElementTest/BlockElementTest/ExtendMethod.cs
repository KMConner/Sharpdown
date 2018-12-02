﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Sharpdown.MarkdownElement.BlockElement;
using Sharpdown.MarkdownElement.InlineElement;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    static class ExtendMethod
    {
        public static AddLineResult AddLine(this BlockElement element, string line, bool lazy = false)
        {
            var method = element.GetType().GetMethod("AddLine", BindingFlags.NonPublic | BindingFlags.Instance);
            try
            {
                return (AddLineResult)method.Invoke(element, new object[] { line, lazy });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public static BlockElement Close(this BlockElement element)
        {
            var method = element.GetType().GetMethod("Close", BindingFlags.NonPublic | BindingFlags.Instance);
            try
            {
                return (BlockElement)method.Invoke(element, null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public static IReadOnlyList<InlineElementBase> GetInlines(this BlockElement element)
        {
            if (element is LeafElement leaf)
            {
                return leaf.Inlines;
            }
            else
            {
                throw new Exception("This block is a container block.");
            }
        }

        public static InlineElementBase GetInline(this BlockElement element, int index)
        {
            return element.GetInlines()[index];
        }

        public static IReadOnlyList<BlockElement> GetChildren(this BlockElement element)
        {
            if (element is ContainerElement container)
            {
                return container.Children;
            }
            else
            {
                throw new Exception("This block is not a container block.");
            }
        }

        public static BlockElement GetChild(this BlockElement element, int index)
        {
            return element.GetChildren()[index];
        }

    }
}
