using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a channel element that renders as a mention of a Slack channel.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/channel-element">Slack Channel Element Documentation</seealso>
public class ChannelElement : Element, IRichTextSubElement
{
    public ChannelElement(
        string channelId,
        string? tabId = null,
        RichTextStyle? style = null,
        bool? fromLlm = null)
    {
        ChannelId = channelId;
        TabId = tabId;
        Style = style;
        FromLlm = fromLlm;
    }

    public override string Type => "channel";

    [JsonPropertyName("channel_id")]
    public string ChannelId
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            field = value;
        }
    }

    [JsonPropertyName("tab_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TabId
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    [JsonPropertyName("from_llm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FromLlm { get; set; }
}
