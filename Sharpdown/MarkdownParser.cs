using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sharpdown
{
    public class MarkdownParser
    {
        public static MarkdownDocument Parse(IEnumerable<string> lines)
        {
            var ret = new MarkdownDocument();
            foreach (var line in lines)
            {
                ret.AddLine(line);
            }

            ret.Close();
            ret.ParseInline();
            return ret;
        }

        public static MarkdownDocument Parse(string text)
        {
            return Parse(new MemoryStream(Encoding.UTF8.GetBytes(text), false));
        }

        public static MarkdownDocument Parse(Stream stream)
        {
            return Parse(EnumurateLines(stream));
        }

        private static IEnumerable<string> EnumurateLines(Stream stream, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            using (var reader = new StreamReader(stream, encoding))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }
    }
}
