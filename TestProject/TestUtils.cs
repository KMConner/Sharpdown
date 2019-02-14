using System;
using System.Reflection;

namespace TestProject
{
    /// <summary>
    /// Provides methods used in test project.
    /// </summary>
    class TestUtils
    {
        /// <summary>
        /// Creates a new instance of <typeparamref name="T"/> whose constructor
        /// are not accessible from this project.
        /// </summary>
        /// <typeparam name="T">The object type to initialize.</typeparam>
        /// <returns>The created object.</returns>
        public static T CreateInternal<T>()
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }

        public static T CreateInternal<T>(Type[] types, object[] parameters)
        {
            ConstructorInfo constructorInfo = typeof(T).GetConstructor(
                BindingFlags.NonPublic|BindingFlags.Instance, null, types, null);
            return (T)constructorInfo.Invoke(parameters);
        }
    }
}
