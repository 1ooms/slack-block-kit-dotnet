using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a markdown block (<c>type: "markdown"</c>) that renders formatted markdown text.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/markdown-block">Slack Markdown Block Documentation</seealso>
public class MarkdownBlock : Block, IMessageBlock
{
    public MarkdownBlock(string text, string? blockId = null)
    {
        Text = text;
        BlockId = blockId;
    }

    public override string Type => "markdown";

    [JsonPropertyName("text")]
    public string Text
    {
        get;
        set => field = ThrowIfNull(value);
    }
}