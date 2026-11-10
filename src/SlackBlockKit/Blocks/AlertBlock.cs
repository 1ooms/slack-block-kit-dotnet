using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents an alert block (<c>type: "alert"</c>) that displays a callout message with a specific severity level.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/alert-block">Slack Alert Block Documentation</seealso>
public class AlertBlock : Block, IModalBlock, IHomeBlock
{
    private const int MaxTextLength = 200;

    [JsonConstructor]
    public AlertBlock(TextObject text, AlertLevel level = AlertLevel.Default, string? blockId = null)
    {
        Text = text;
        Level = level;
        BlockId = blockId;
    }

    public AlertBlock(string markdownText, AlertLevel level = AlertLevel.Default, string? blockId = null)
        : this(new MarkdownTextObject(markdownText), level, blockId)
    {
    }

    public override string Type => "alert";

    [JsonPropertyName("text")]
    public TextObject Text
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxTextLength);
    }

    [JsonPropertyName("level")] public AlertLevel Level { get; set; }
}

/// <summary>
/// Specifies the severity level of an alert block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/alert-block#levels">Slack Alert Block Documentation</seealso>
public enum AlertLevel
{
    Default,
    Info,
    Warning,
    Error,
    Success
}