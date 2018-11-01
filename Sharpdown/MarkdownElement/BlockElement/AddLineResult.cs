using System;

namespace Sharpdown.MarkdownElement.BlockElement
{
    [Flags]
    public enum AddLineResult
    {
        NeedClose = 1,
        Consumed = 2,
    };
}
