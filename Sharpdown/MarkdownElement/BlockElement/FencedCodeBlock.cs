using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class FencedCodeBlock : CodeBlockBase
    {
        public override BlockElementType Type => BlockElementType.FencedCodeBlock;

        public override string InfoString => infoString ?? string.Empty;

        public override string Content => string.Join("\r\n", contents);

        private static readonly Regex openFenceRegex = new Regex(
            @"^(?<indent> {0,3})(?<fence>`{3,}|~{3,})[ \t]*(?<info>[^`]*?)[ \t]*$",
            RegexOptions.Compiled);

        private static readonly Regex closeFenceRegex = new Regex(
            @"^ {0,3}(?<fence>`{3,}|~{3,})[ \t]*$", RegexOptions.Compiled);

        private List<string> contents;

        private string infoString;

        private int indentNum;

        private int fenceLength;

        private char fenceChar;

        private bool initialized;

        internal FencedCodeBlock()
        {
            contents = new List<string>();
            indentNum = -1;
        }

        public static bool CanStartBlock(string line)
        {
            if (line.GetIndentNum() >= 4)
            {
                return false;
            }

            var trimmed = line.TrimStart(whiteSpaceShars);

            if (trimmed.Length < 3 || trimmed[0] != '`' && trimmed[0] != '~')
            {
                return false;
            }

            int fence = trimmed.Length;
            for (int i = 1; i < trimmed.Length; i++)
            {
                if (trimmed[i] != trimmed[0])
                {
                    fence = i;
                    break;
                }
            }

            if (fence < 3)
            {
                return false;
            }

            string infoString = trimmed.Substring(fence);

            return !infoString.Contains(trimmed[0]);
        }

        internal override AddLineResult AddLine(string line)
        {
            if (!initialized)
            {
                Match match = openFenceRegex.Match(line);
                if (!match.Success)
                {
                    throw new InvalidBlockFormatException(BlockElementType.FencedCodeBlock);
                }
                string fence = match.Groups["fence"].Value;
                fenceLength = fence.Length;
                fenceChar = fence[0];
                indentNum = match.Groups["indent"].Length;
                if (match.Groups["info"].Success)
                {
                    infoString = match.Groups["info"].Value;
                }
                initialized = true;
                return AddLineResult.Consumed;
            }

            Match closeMatch = closeFenceRegex.Match(line);

            if (!closeMatch.Success)
            {
                contents.Add(RemoveIndent(line, indentNum));
                return AddLineResult.Consumed;
            }

            string closeFence = closeMatch.Groups["fence"].Value;
            if (closeFence[0] == fenceChar && closeFence.Length >= fenceLength)
            {
                return AddLineResult.Consumed | AddLineResult.NeedClose;
            }
            else
            {
                contents.Add(RemoveIndent(line, indentNum));
                return AddLineResult.Consumed;
            }
        }
    }
}
