using System.Linq;
using System.Text;

namespace Sharpdown.MarkdownElement.InlineElement
{
    /// <summary>
    /// Provides some methods for Inline parsing.
    /// </summary>
    public static class InlineElementUtils
    {
        private static readonly char[] unreservedChars =
        {
            'A', 'B', 'C', 'D', 'E',
            'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O',
            'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y',
            'Z',
            'a', 'b', 'c', 'd', 'e',
            'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n', 'o',
            'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y',
            'z',
            '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9',
            ';', '/', '?', ':', '@',
            '&', '=', '+', '$', ',',
            '-', '_', '.', '!', '~',
            '*', '\'', '(', ')', '#',
        };

        private static bool IsPercentEncoded(string text, int index)
        {
            bool IsHexChar(char ch)
            {
                if (ch >= 0x30 && ch <= 0x39)
                {
                    return true;
                }

                if (ch >= 0x41 && ch <= 0x5A)
                {
                    return true;
                }

                if (ch >= 0x61 && ch <= 0x7A)
                {
                    return true;
                }

                return false;
            }

            if (index >= text.Length - 2)
            {
                return false;
            }

            return text[index] == '%'
                   && IsHexChar(text[index + 1])
                   && IsHexChar(text[index + 2]);
        }

        /// <summary>
        /// Do percent encode to the specific string.
        /// </summary>
        /// <returns>The text to encode.</returns>
        /// <param name="text">The encoded text.</param>
        /// <remarks>
        /// Ascii alphabets, digits and some other ascii characters (see <see cref="unreservedChars"/>)
        /// will not be encoded which is different from the specification of url.
        /// % characters will be escaped only when it is not the beginning of percent encoded letters.
        /// </remarks>
        public static string UrlEncode(string text)
        {
            var builder = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                if (unreservedChars.Contains(text[i]))
                {
                    builder.Append(text[i]);
                }
                else if (text[i] == '%' && IsPercentEncoded(text, i))
                {
                    builder.Append(text[i]);
                }
                else
                {
                    var bytes = Encoding.UTF8.GetBytes(new[] {text[i]});
                    foreach (var b in bytes)
                    {
                        builder.Append("%");
                        builder.Append(b.ToString("X2"));
                    }
                }
            }

            return builder.ToString();
        }
    }
}
