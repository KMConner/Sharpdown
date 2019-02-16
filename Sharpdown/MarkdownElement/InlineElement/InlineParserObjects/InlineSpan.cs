using System;
using System.Collections.Generic;
using Sharpdown.MarkdownElement.BlockElement;

namespace Sharpdown.MarkdownElement.InlineElement.InlineParserObjects
{
    /// <summary>
    /// Represents span in a string.
    /// </summary>
    /// <remarks>
    /// This object is used in order to represent ranges of each inline element.
    /// This object can also represents syntax tree with nesting.
    /// The difference from <see cref="InlineElement"/> is that this object does not always contain string
    /// (usually, just holding indices in the string.)
    /// </remarks>
    internal class InlineSpan
    {
        private int? parseBegin;
        private int? parseEnd;

        /// <summary>
        /// Gets or sets the index at the beginning of this span.
        /// </summary>
        public int Begin { get; set; }

        /// <summary>
        /// Gets or sets the index one after the end of this span.
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// If this span represents substring of links or images, gets or sets the destination.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// If this span represents substring of links or images, gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the index at the beginning of parsing range.
        /// (Equal to <see cref="Begin"/> by default.)
        /// </summary>
        public int ParseBegin
        {
            get => parseBegin ?? Begin;
            set => parseBegin = value;
        }

        /// <summary>
        /// Gets or sets the index at the end of parsing range.
        /// (Equal to <see cref="End"/> by default.)
        /// </summary>
        public int ParseEnd
        {
            get => parseEnd ?? End;
            set => parseEnd = value;
        }

        /// <summary>
        /// Gets or sets the type of this span.
        /// </summary>
        public InlineSpanType SpanType { get; set; }

        /// <summary>
        /// Gets the children of this span.
        /// </summary>
        public SortedList<int, InlineSpan> Children { get; }

        /// <summary>
        /// Gets or sets the parent span of the current span.
        /// </summary>
        public InlineSpan Parent { get; set; }

        /// <summary>
        /// Gets or sets <see cref="InlineElement"/> which is equivalent to the current span.
        /// </summary>
        public InlineElement DelimElem { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="InlineSpan"/>.
        /// </summary>
        public InlineSpan()
        {
            Children = new SortedList<int, InlineSpan>();
        }

        public static InlineSpan FromDeliminatorInfo(DeliminatorInfo info, int spanEnd, int parseEnd,
            LinkReferenceDefinition definition = null)
        {
            return FromDeliminatorInfo(info, spanEnd, parseEnd, definition?.Destination, definition?.Title);
        }

        public static InlineSpan FromDeliminatorInfo(DeliminatorInfo info, int spanEnd, int parseEnd,
            string destination, string title)
        {
            InlineSpanType type;
            switch (info.Type)
            {
                case DeliminatorType.OpenLink:
                    type = InlineSpanType.Link;
                    break;
                case DeliminatorType.OpenImage:
                    type = InlineSpanType.Image;
                    break;
//                case DeliminatorType.Star:
//                    break;
//                case DeliminatorType.UnderBar:
//                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new InlineSpan
            {
                SpanType = type,
                Begin = info.Index,
                End = spanEnd,
                ParseBegin = info.Index + info.DeliminatorLength,
                ParseEnd = parseEnd,
                Destination = destination,
                Title = title,
            };
        }
    }
}
