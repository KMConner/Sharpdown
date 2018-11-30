using System.Reflection;
using Sharpdown.MarkdownElement.BlockElement;

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

    }
}
