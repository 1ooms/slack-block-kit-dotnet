namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a visual divider block (<c>type: "divider"</c>) to cleanly separate sections within a layout.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/divider-block">Slack Divider Block Documentation</seealso>
public class DividerBlock : Block, IMessageBlock, IModalBlock, IHomeBlock
{
    public DividerBlock(string? blockId = null)
    {
        BlockId = blockId;
    }

    public override string Type => "divider";
}