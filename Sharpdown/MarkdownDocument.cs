using System;
using System.Collections.Generic;
using Sharpdown.MarkdownElement.BlockElement;

namespace Sharpdown
{
    public class MarkdownDocument
    {
        public List<BlockElement> Elements { get; private set; }

        public Dictionary<string, LinkReferenceDefinition> LinkDefinition { get; private set; }

        private BlockElement openElement;

        public BlockElement this[int i] => Elements[i];

        internal MarkdownDocument()
        {
            Elements = new List<BlockElement>();
            LinkDefinition = new Dictionary<string, LinkReferenceDefinition>(
                StringComparer.InvariantCultureIgnoreCase);
        }

        internal void AddLine(string line)
        {
            if (openElement == null)
            {
                openElement = BlockElementUtil.CreateBlockFromLine(line, 0);
            }
            AddLineResult result = openElement.AddLine(line, false, 0);

            if ((result & AddLineResult.NeedClose) != 0)
            {
                if (openElement.Type != BlockElementType.BlankLine)
                {
                    Elements.Add(openElement.Close());
                }
                openElement = null;
            }
            if ((result & AddLineResult.Consumed) == 0)
            {
                AddLine(line);
            }
        }

        private void ExtractLinkDefinition(BlockElement element)
        {
            if (element is ContainerElement container)
            {
                for (int i = container.children.Count - 1; i >= 0; i--)
                {
                    if (container.children[i].Type == BlockElementType.LinkReferenceDefinition)
                    {
                        // TODO: Duplicate warning
                        var definition = (LinkReferenceDefinition)container.children[i];
                        container.children.RemoveAt(i);
                        LinkDefinition[definition.Label] = definition;
                    }
                    else
                    {
                        ExtractLinkDefinition(container.children[i]);
                    }
                }
            }
        }

        internal void ParseInline()
        {
            for (int i = Elements.Count - 1; i >= 0; i--)
            {
                if (Elements[i].Type == BlockElementType.LinkReferenceDefinition)
                {
                    // TODO: Duplicate warning
                    var definition = (LinkReferenceDefinition)Elements[i];
                    Elements.RemoveAt(i);
                    LinkDefinition[definition.Label] = definition;
                }
                else
                {
                    ExtractLinkDefinition(Elements[i]);
                }
            }
            foreach (var element in Elements)
            {
                element.ParseInline(LinkDefinition);
            }
        }

        internal void Close()
        {
            if (openElement != null)
            {
                if (openElement.Type != BlockElementType.BlankLine)
                {
                    Elements.Add(openElement.Close());
                }
                openElement = null;
            }
        }
    }
}
