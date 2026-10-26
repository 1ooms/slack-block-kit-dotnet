using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Specifies the broadcast range for a broadcast mention element.
/// </summary>
public enum BroadcastRange
{
    Here,
    Channel,
    Everyone
}

/// <summary>
/// Represents a broadcast element that displays a broadcast mention such as @here, @channel, or @everyone.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/broadcast-element">Slack Broadcast Element Documentation</seealso>
public class BroadcastElement : Element, IRichTextSubElement
{
    [JsonConstructor]
    public BroadcastElement(BroadcastRange range, RichTextStyle? style = null)
    {
        Range = range;
        Style = style;
    }

    public override string Type => "broadcast";

    [JsonPropertyName("range")]
    public BroadcastRange Range { get; set; }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }
}
