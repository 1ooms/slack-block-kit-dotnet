using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a message mention element that renders as a link to a Slack message.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/message-mention-element">Slack Message Mention Element Documentation</seealso>
public class MessageMentionElement : Element, IRichTextSubElement
{
    public MessageMentionElement(
        string channelId,
        string messageTs,
        string? authorId = null,
        string? threadTs = null,
        string? text = null,
        string? url = null,
        RichTextStyle? style = null)
    {
        ChannelId = channelId;
        MessageTs = messageTs;
        AuthorId = authorId;
        ThreadTs = threadTs;
        Text = text;
        Url = url;
        Style = style;
    }

    [JsonPropertyName("channel_id")]
    public string ChannelId
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("message_ts")]
    public string MessageTs
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("author_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AuthorId
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("thread_ts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ThreadTs { get; set; }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "message_mention";
}