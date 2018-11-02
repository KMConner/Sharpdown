namespace Sharpdown.MarkdownElement.BlockElement
{
    public class BlankLine : LeafElementBase
    {
        public override BlockElementType Type => BlockElementType.BlankLine;

        internal static bool CanStartBlock(string line)
        {
            return line.TrimStart(whiteSpaceShars).Length == 0;
        }

        internal override AddLineResult AddLine(string line)
        {
            if (!CanStartBlock(line))
            {
                throw new InvalidBlockFormatException(BlockElementType.BlankLine);
            }
            return AddLineResult.Consumed | AddLineResult.NeedClose;
        }
    }
}
