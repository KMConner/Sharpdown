using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sharpdown
{
    public class MarkdownParser
    {
        public ParserConfig Config { get; }

        public MarkdownParser()
        {
            Config = ParserConfigBuilder.CommonMark.ToParserConfig();
        }

        public MarkdownParser(ParserConfig config)
        {
            Config = config;
        }

        public MarkdownDocument Parse(IEnumerable<string> lines)
        {
            var ret = new MarkdownDocument(Config);
            foreach (var line in lines)
            {
                ret.AddLine(line);
            }

            ret.Close();
            ret.ParseInline();
            return ret;
        }

        public MarkdownDocument Parse(string text)
        {
            return Parse(new MemoryStream(Encoding.UTF8.GetBytes(text), false));
        }

        public MarkdownDocument Parse(Stream stream)
        {
            return Parse(EnumerateLines(stream));
        }

        private IEnumerable<string> EnumerateLines(Stream stream, Encoding encoding = null)
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
