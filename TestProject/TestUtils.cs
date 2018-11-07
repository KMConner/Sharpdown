using System;

namespace TestProject
{
    class TestUtils
    {
        public static T CreateInternal<T>(params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return (T)Activator.CreateInstance(typeof(T), true);
            }
            return (T)Activator.CreateInstance(typeof(T), parameters);
        }
    }
}
