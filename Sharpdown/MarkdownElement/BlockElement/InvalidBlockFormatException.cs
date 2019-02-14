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

        private string message;

        /// <summary>
        /// Initializes new instance of <see cref="InvalidBlockFormatException"/>.
        /// </summary>
        /// <param name="elementType"></param>
        public InvalidBlockFormatException(BlockElementType elementType)
            : base()
        {
            ElementType = elementType;
        }

        public InvalidBlockFormatException(BlockElementType elementType, string message)
            : this(elementType)
        {
            this.message = message;
        }

        /// <summary>
        /// Gets the error message which explains the current exception.
        /// </summary>
        public override string Message
        {
            get { return message ?? $"Given format is invalid for block type {ElementType}"; }
        }
    }
}
