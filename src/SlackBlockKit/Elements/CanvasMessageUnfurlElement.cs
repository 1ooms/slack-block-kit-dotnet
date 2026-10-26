using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a canvas message unfurl element that renders as an inline preview of a message inside a canvas.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/canvas-message-unfurl-element">Slack Canvas Message Unfurl Element Documentation</seealso>
public class CanvasMessageUnfurlElement : Element, IRichTextSubElement
{
    public CanvasMessageUnfurlElement(
        string rootMessageTs,
        string rootMessageChannel,
        RichTextStyle? style = null)
    {
        RootMessageTs = rootMessageTs;
        RootMessageChannel = rootMessageChannel;
        Style = style;
    }

    public override string Type => "canvas_message_unfurl";

    [JsonPropertyName("root_message_ts")]
    public string RootMessageTs
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("root_message_channel")]
    public string RootMessageChannel
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }
}
