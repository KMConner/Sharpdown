namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Provides some extend methods.
    /// </summary>
    static class ExtendMethods
    {
        /// <summary>
        /// Remove the all occurences of specified <see cref="string"/> from this.
        /// </summary>
        /// <param name="str">
        /// The <see cref="string"/> to remove <paramref name="remove"/> from.
        /// </param>
        /// <param name="remove">
        /// The <see cref="string"/> to remove from <paramref name="str"/>.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> that is equivalent to the current string
        /// except all occurence of <paramref name="remove"/> is removed.
        /// </returns>
        public static string Remove(this string str, string remove)
        {
            return str.Replace(remove, string.Empty);
        }

        /// <summary>
        /// Returns wether the specified <see cref="char"/> is a ascii white space.
        /// </summary>
        /// <param name="ch">The character to determine.</param>
        /// <returns>Wether thespecified <see cref="char"/> is a ascii white space.</returns>
        public static bool IsAsciiWhiteSpace(this char ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\x000B' || ch == '\x000C';
        }

        /// <summary>
        /// Reterns the indent count of the specified <see cref="string"/>.
        /// 
        /// The tab characters behave as if they are replaced to 4 spaces. 
        /// </summary>
        /// <param name="line">A line to count the indent.</param>
        /// <returns>The indent count of <paramref name="line"/>.</returns>
        public static int GetIndentNum(this string line, int currentIndent)
        {
            if (line.TrimStart(MarkdownElementBase.whiteSpaceShars).Length == 0)
            {
                return -1;
            }

            int ret = 0;
            foreach (var item in line)
            {
                switch (item)
                {
                    case ' ':
                        ret++;
                        break;
                    case '\t':
                        ret += 4 - ((currentIndent + ret) % 4);
                        break;
                    default:
                        return ret;
                }
            }

            return ret;
        }

        /// <summary>
        /// Removes all leading occurences of a set of
        /// <see cref="MarkdownElementBase.whiteSpaceShars"/>.
        /// </summary>
        /// <param name="str">
        /// The string to remove all reading occurences of ascii white space characters.
        /// </param>
        /// <returns>
        /// The string that remains after all occurrences of ascii white space characters
        /// are removed from the start of the current string.
        /// </returns>
        public static string TrimStartAscii(this string str)
        {
            return str.TrimStart(MarkdownElementBase.whiteSpaceShars);
        }
    }
}
