namespace Sharpdown
{
    /// <summary>
    /// Provides features to build <see cref="ParserConfig"/>.
    /// </summary>
    public class ParserConfigBuilder
    {
        /// <summary>
        /// Gets the configuration of CommonMark.
        /// </summary>
        public static ParserConfigBuilder CommonMark => new ParserConfigBuilder();

        /// <summary>
        /// Returns a equivalent <see cref="ParserConfig"/> to the current builder.
        /// </summary>
        /// <returns></returns>
        public ParserConfig ToParserConfig()
        {
            return new ParserConfig();
        }
    }
}
