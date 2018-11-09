using System;

namespace Sharpdown.MarkdownElement.BlockElement
{
    /// <summary>
    /// Represents error when constructing block structure,
    /// </summary>
    public class InvalidBlockFormatException : Exception
    {
        /// <summary>
        /// The type of element which is under parsing process.
        /// </summary>
        public BlockElementType ElementType { get; private set; }

        /// <summary>
        /// Initializes new instance of <see cref="InvalidBlockFormatException"/>.
        /// </summary>
        /// <param name="elementType"></param>
        public InvalidBlockFormatException(BlockElementType elementType)
            : base()
        {
            ElementType = elementType;
        }

        /// <summary>
        /// Gets the error message which explains the current exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return $"Given format is invalid for block type {ElementType}";
            }
        }
    }
}
