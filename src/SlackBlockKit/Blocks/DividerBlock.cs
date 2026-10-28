namespace SlackBlockKit.Blocks;

public class DividerBlock : Block
{
    public DividerBlock(string? blockId = null)
    {
        BlockId = blockId;
    }

    public override string Type => "divider";
}