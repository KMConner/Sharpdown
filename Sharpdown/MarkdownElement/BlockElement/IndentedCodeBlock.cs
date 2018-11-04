using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class IndentedCodeBlock : CodeBlockBase
    {
        public override BlockElementType Type => BlockElementType.IndentedCodeBlock;

        public override string InfoString => string.Empty;

        public override string Content => string.Join("\r\n", contents);

        private readonly List<string> contents;

        internal IndentedCodeBlock():base()
        {
            contents = new List<string>();
        }

        public static bool CanStartBlock(string line)
        {
            return line.GetIndentNum() >= 4;
        }

        internal override AddLineResult AddLine(string line)
        {
            int indent = line.GetIndentNum();
            if (indent >= 0 && indent < 4)
            {
                for (int i = 0; i < contents.Count; i++)
                {
                    if (contents[i] == string.Empty)
                    {
                        contents.RemoveAt(i);
                    }
                }
                return AddLineResult.NeedClose;
            }

            string lineTrimmed = RemoveIndent(line);

            contents.Add(lineTrimmed);
            return AddLineResult.Consumed;
        }

        private string RemoveIndent(string line)
        {
            if (line.Length == 0)
            {
                return line;
            }
            if (line[0] == '\t')
            {
                return line.Substring(1);
            }

            int trimLength = 4;
            for (int i = 0; i < 4; i++)
            {
                if (line.Length < i + 1 || line[i] != ' ')
                {
                    trimLength = i;
                    break;
                }
            }
            return line.Substring(trimLength);
        }
    }
}
