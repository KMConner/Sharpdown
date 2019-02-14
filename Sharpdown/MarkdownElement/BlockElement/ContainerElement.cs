using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents container blocks in markdown documents.
    /// 
    /// Unlike <see cref="LeafElement"/>, <see cref="ContainerElement"/>
    /// may have other <see cref="BlockElement"/> as children.
    /// </summary>
    /// <seealso cref="ListBlock"/>
    /// <seealso cref="ListItem"/>
    /// <seealso cref="BlockQuote"/>
    public abstract class ContainerElement : BlockElement
    {
        /// <summary>
        /// <see cref="BlockElement"/> which is not closed.
        /// </summary>
        protected BlockElement openElement;

        /// <summary>
        /// Child elements of this block.
        /// </summary>
        internal readonly List<BlockElement> children;

        /// <summary>
        /// Gets child elements of this block.
        /// </summary>
        public IReadOnlyList<BlockElement> Children => children.AsReadOnly();

        public override string Content => throw new NotImplementedException();

        /// <summary>
        /// Warning raised while parsing.
        /// </summary>
        public override IReadOnlyList<string> Warnings =>
            new List<string>(children.Select(c => c.Warnings.AsEnumerable())
                    .Aggregate((l1, l2) => l1.Union(l2)).Union(warnings))
                .AsReadOnly();

        /// <summary>
        /// Initializes a new instance of <see cref="ContainerElement"/>.
        /// </summary>
        internal ContainerElement()
        {
            children = new List<BlockElement>();
        }

        /// <summary>
        /// Returns whether the specified line satisfied proper conditions to 
        /// continue (without lazy continuation).
        /// </summary>
        /// <param name="line">The line to continue.</param>
        /// <param name="markRemoved">
        /// A line of string after removed mark.
        /// (e.g. Extra indent, '> ' mark.)
        /// </param>
        /// <returns>
        /// Whether the specified line satisfied proper conditions to 
        /// continue (without lazy continuation).
        /// </returns>
        internal abstract bool HasMark(string line, int currentIndent, out string markRemoved, out int markLength);

        /// <summary>
        /// Closes <see cref="openElement"/>.
        /// </summary>
        /// <seealso cref="BlockElement.Close"/>
        protected void CloseOpenElement()
        {
            if (openElement != null)
            {
                children[children.Count - 1] = openElement.Close();
                openElement = null;
            }
        }

        /// <summary>
        /// Appends the specified <see cref="BlockElement"/> to this children.
        /// </summary>
        /// <param name="elem"><see cref="BlockElement"/> to add.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when there is a child which is not closed.
        /// </exception>
        protected void AddChild(BlockElement elem)
        {
            if (openElement != null)
            {
                throw new InvalidOperationException("openElement is not null.");
            }

            openElement = elem;
            children.Add(elem);
        }

        /// <summary>
        /// Returns whether this element allow lazy continuation.
        /// </summary>
        /// <returns>
        /// Returns <c>true</c> when lazy continuation is allowed, <c>false</c> otherwise.
        /// </returns>
        protected bool CanLazyContinue()
        {
            if (children.Count == 0)
            {
                return false;
            }

            BlockElementType lastElementType = children.Last().Type;

            if (children.Last() is ContainerElement container)
            {
                return container.CanLazyContinue();
            }

            return lastElementType == BlockElementType.Unknown
                   || lastElementType == BlockElementType.Paragraph;
        }

        /// <summary>
        /// Closes this <see cref="BlockElement"/> after closes all children
        /// and removing all <see cref="BlankLine"/> elements.
        /// 
        /// Some block types cannot be determined with only its first line.
        /// Then, the actual type is determined after appending the last line of this block.
        /// </summary>
        /// <returns>
        /// Ths closed block.
        /// The <see cref="Type"/> may be different between the two.
        /// </returns>
        internal override BlockElement Close()
        {
            if (openElement != null)
            {
                children[children.Count - 1] = openElement.Close();
            }

            for (int i = children.Count - 1; i >= 0; i--)
            {
                if (children[i].Type == BlockElementType.BlankLine)
                {
                    children.RemoveAt(i);
                }
            }

            return this;
        }

        /// <summary>
        /// Adds a line of string to this block.
        /// </summary>
        /// <param name="line">A single line to add to this element.</param>
        /// <returns></returns>
        internal override AddLineResult AddLine(string line, bool lazy, int currentIndent)
        {
            if (HasMark(line, currentIndent, out string markRemoved, out int markLength))
            {
                AddLineResult addLineResult;
                do
                {
                    if (openElement == null)
                    {
                        openElement = BlockElementUtil.CreateBlockFromLine(markRemoved, currentIndent,
                            Type == BlockElementType.List);
                        children.Add(openElement);
                    }

                    addLineResult = openElement.AddLine(markRemoved, lazy, currentIndent + markLength);
                    if ((addLineResult & AddLineResult.NeedClose) != 0)
                    {
                        children[children.Count - 1] = openElement.Close();
                        openElement = null;
                    }
                } while ((addLineResult & AddLineResult.Consumed) == 0);

                return addLineResult & AddLineResult.Consumed;
            }

            line = markRemoved ?? line;

            var newElem = BlockElementUtil.CreateBlockFromLine(line, currentIndent, Type == BlockElementType.List);
            if ((newElem.Type != BlockElementType.Unknown && newElem.Type != BlockElementType.BlankLine &&
                 newElem.Type != BlockElementType.IndentedCodeBlock)
                || !CanLazyContinue())
            {
                return AddLineResult.NeedClose;
            }

            return openElement?.AddLine(line, true, currentIndent) ?? throw new Exception();
        }

        internal override void ParseInline(Dictionary<string, LinkReferenceDefinition> linkDefinitions)
        {
            foreach (var child in children)
            {
                child.ParseInline(linkDefinitions);
            }
        }

        protected string SubStringExpandingTabs(string str, int count, int currentIndent)
        {
            if (count <= 0)
            {
                return str;
            }

            if (str[0] == '\t')
            {
                int tabWidth = 4 - (currentIndent % 4);
                return SubStringExpandingTabs(new string(' ', tabWidth - 1) + str.Substring(1), count - 1,
                    currentIndent + 1);
            }

            return SubStringExpandingTabs(str.Substring(1), count - 1, currentIndent + 1);
        }
    }
}
