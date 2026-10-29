using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a rich text block (<c>type: "rich_text"</c>) used for formatted text, quotes, code blocks, and lists.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/rich-text-block">Slack Rich Text Block Documentation</seealso>
public class RichTextBlock : Block, ITableCell
{
    public RichTextBlock(List<IRichTextElement> elements, string? blockId = null)
    {
        Elements = elements;
        BlockId = blockId;
    }

    [JsonPropertyName("elements")]
    public List<IRichTextElement> Elements
    {
        get;
        set => field = ThrowIfNull(value);
    }

    public override string Type => "rich_text";
}