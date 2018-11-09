using System;

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
        /// <param name="parameters">The parameters passed to the constructor.</param>
        /// <returns>The created object.</returns>
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
