using System;
using System.Collections.Generic;
using System.Text;
using Sharpdown.MarkdownElement.BlockElement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace TestProject.MarkdownElementTest.BlockElementTest
{
    static class ExtendMethod
    {
        public static AddLineResult AddLine(this BlockElement element, string line)
        {
            var method = element.GetType().GetMethod("AddLine", BindingFlags.NonPublic | BindingFlags.Instance);
            try
            {
                return (AddLineResult)method.Invoke(element, new[] { line });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
