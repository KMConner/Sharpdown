using System;

namespace Sharpdown.MarkdownElement.BlockElement
{
    public class InvalidBlockFormatException : Exception
    {
        public BlockElementType ElementType { get; private set; }

        public InvalidBlockFormatException(BlockElementType elementType)
            : base()
        {
            ElementType = elementType;
        }

        public override string Message
        {
            get
            {
                return $"Given format is invalid for block type {ElementType}";
            }
        }
    }
}
