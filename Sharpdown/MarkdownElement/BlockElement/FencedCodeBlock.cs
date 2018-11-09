﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents fenced code blocks in markdown documents.
    /// </summary>
    /// <remarks>
    /// The typical fenced code block is following.
    /// <![CDATA[
    /// ```csharp
    /// int main()
    /// {
    ///     Console.WriteLine("Hello World!");
    /// }
    /// ```
    /// ]]>
    /// </remarks>
    public class FencedCodeBlock : CodeBlockBase
    {
        /// <summary>
        /// Regular expression which matches the open fence of fenced code block.
        /// </summary>
        private static readonly Regex openFenceRegex = new Regex(
            @"^(?<indent> {0,3})(?<fence>`{3,}|~{3,})[ \t]*(?<info>[^`]*?)[ \t]*$",
            RegexOptions.Compiled);

        /// <summary>
        /// Regular expression which matches the close fence of fenced code block.
        /// </summary>
        private static readonly Regex closeFenceRegex = new Regex(
            @"^ {0,3}(?<fence>`{3,}|~{3,})[ \t]*$", RegexOptions.Compiled);

        /// <summary>
        /// The contents of this block.
        /// </summary>
        private List<string> contents;

        /// <summary>
        /// The info string of this block.
        /// </summary>
        private string infoString;

        /// <summary>
        /// Indent size of the open fence.
        /// </summary>
        private int indentNum;

        /// <summary>
        /// The number of characters of open fence.
        /// </summary>
        private int fenceLength;

        /// <summary>
        /// The character which constitutes open fence.
        /// (` or ~.)
        /// </summary>
        private char fenceChar;

        /// <summary>
        /// Wether the first line is added to this block.
        /// </summary>
        private bool initialized;

        /// <summary>
        /// Wether the line which contains the close fence has ocuuerd.
        /// </summary>
        private bool closed;

        /// <summary>
        /// Gets the type of this block.
        /// </summary>
        public override BlockElementType Type => BlockElementType.FencedCodeBlock;

        /// <summary>
        /// Gets the info string of this block.
        /// </summary>
        public override string InfoString => infoString ?? string.Empty;

        /// <summary>
        /// Gets the content of this block.
        /// </summary>
        public override string Content => string.Join("\r\n", contents);

        /// <summary>
        /// Initializes a new instance of <see cref="FencedCodeBlock"/>.
        /// </summary>
        internal FencedCodeBlock() : base()
        {
            contents = new List<string>();
            indentNum = -1;
            closed = false;
        }

        /// <summary>
        /// Returns whether the specified line can be a start line of <see cref="FencedCodeBlock"/>.
        /// </summary>
        /// <remarks>
        /// These requirements must be satisfied to be the start line.
        /// 
        /// <list type="bullet">
        /// <item>Starts with three or more ` or ~ characters. (Called open fence.)</item>
        /// <item>Open fence must be indented with 0-3 speces.</item>
        /// <item>
        /// Info string after the open fence must not contain ` characters.
        /// (Even if back slash escaped.).
        /// </item>
        /// <item>Characters which consists open fence and close fence must be the same.</item>
        /// <item>The length of close fence must be equal or greater than the length of open fence.</item>
        /// </list>
        /// </remarks>
        /// <param name="line">Single line string.</param>
        /// <returns>
        /// Returns <c>true</c> if <paramref name="line"/> can be a start line of <see cref="FencedCodeBlock"/>.
        /// Otherwise, returns <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Adds a line of string to this <see cref="FencedCodeBlock"/>.
        /// </summary>
        /// <param name="line">A single line to add to this element.</param>
        /// <returns>
        /// Returns <c>AddLineResult.Consumed</c> except when <paramref name="line"/>
        /// contains the close fence.
        /// </returns>
        internal override AddLineResult AddLine(string line)
        {
            if (!initialized) // When the fiest line is slecified
            {
                Match match = openFenceRegex.Match(line);
                if (!match.Success) // When the first line does not contain open fence.
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
                closed = true;
                return AddLineResult.Consumed | AddLineResult.NeedClose;
            }
            else
            {
                contents.Add(RemoveIndent(line, indentNum));
                return AddLineResult.Consumed;
            }
        }

        /// <summary>
        /// Closes the current <see cref="FencedCodeBlock"/>.
        /// 
        /// When the close fence was not appear, a warning message is added.
        /// </summary>
        /// <returns>
        /// The currnt object.
        /// </returns>
        internal override BlockElement Close()
        {
            if (!closed)
            {
                warnings.Add("Code Block is not closed.");
            }
            return this;
        }
    }
}
