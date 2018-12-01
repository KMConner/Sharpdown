using System;
using System.Collections.Generic;
using System.Text;
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
            LinkDefinition = new Dictionary<string, LinkReferenceDefinition>();
        }

        internal void AddLine(string line)
        {
            if (openElement == null)
            {
                openElement = BlockElementUtil.CreateBlockFromLine(line);
            }
            AddLineResult result = openElement.AddLine(line, false);

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
            if (element.Type == BlockElementType.LinkReferenceDefinition)
            {
                var def = (LinkReferenceDefinition)element;
                LinkDefinition[def.Label] = def;
            }
            else if (element is ContainerElement container)
            {
                foreach (var child in container.Children)
                {
                    ExtractLinkDefinition(child);
                }
            }
        }

        internal void ParseInline()
        {
            foreach (var element in Elements)
            {
                ExtractLinkDefinition(element);
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
