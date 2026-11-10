using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a header block (<c>type: "header"</c>) that displays larger, bold plain-text heading for content.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/header-block">Slack Header Block Documentation</seealso>
public class HeaderBlock : Block, IMessageBlock, IModalBlock, IHomeBlock
{
    private const int MaxTextLength = 150;
    private const int MinLevel = 1;
    private const int MaxLevel = 4;

    [JsonConstructor]
    public HeaderBlock(PlainTextObject text, string? blockId = null, int? level = null)
    {
        Text = text;
        BlockId = blockId;
        Level = level;
    }

    public HeaderBlock(string text, string? blockId = null, int? level = null)
        : this(new PlainTextObject(text), blockId, level)
    {
    }

    public override string Type => "header";

    [JsonPropertyName("text")]
    public PlainTextObject Text
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxTextLength);
    }

    [JsonPropertyName("level")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Level
    {
        get;
        set => field = ThrowIfNotBetween(value, MinLevel, MaxLevel);
    }
}