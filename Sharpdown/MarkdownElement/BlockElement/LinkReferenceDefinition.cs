using System;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class LinkReferenceDefinition : LeafElement
    {
        public string Label { get; private set; }
        public string Destination { get; private set; }
        public string Title { get; private set; }

        internal LinkReferenceDefinition(string label, string destination, string title, UnknownElement elem) : base()
        {
            Label = label ?? throw new ArgumentNullException(nameof(title));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            Title = title ?? string.Empty;
            warnings.AddRange(elem.Warnings);
        }

        public override BlockElementType Type => BlockElementType.LinkReferenceDefinition;

        internal override AddLineResult AddLine(string line)
        {
            throw new InvalidOperationException();
        }

        internal override BlockElement Close()
        {
            throw new InvalidCastException();
        }
    }
}
