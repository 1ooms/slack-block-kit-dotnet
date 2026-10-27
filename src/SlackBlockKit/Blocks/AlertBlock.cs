using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Blocks;

public class AlertBlock : Block
{
    /// <summary>
    /// Specifies the style of an alert block.
    /// </summary>
    public enum AlertLevel
    {
        Default,
        Info,
        Warning,
        Error,
        Success
    }

    private const int MaxTextLength = 200;

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