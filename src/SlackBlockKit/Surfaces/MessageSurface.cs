using System.Text.Json.Serialization;
using SlackBlockKit.Blocks;

namespace SlackBlockKit.Surfaces;

/// <summary>
/// Represents a message payload with fallback text, blocks and threading, as sent with the
/// <c>chat.postMessage</c> Web API method, incoming webhooks or a <c>response_url</c>.
/// </summary>
/// <remarks>Messages have no <c>"type"</c> property, so none is written.</remarks>
/// <seealso href="https://docs.slack.dev/messaging/#payloads">Slack Message Payload Documentation</seealso>
public class MessageSurface
{
    private const int MaxBlocks = 50;

    [JsonConstructor]
    public MessageSurface(
        string? text = null,
        List<IMessageBlock>? blocks = null,
        string? threadTs = null,
        bool? mrkdwn = null)
    {
        Text = text;
        Blocks = blocks;
        ThreadTs = threadTs;
        Mrkdwn = mrkdwn;
    }

    public MessageSurface(params IMessageBlock[] blocks)
        : this(null, blocks.ToList())
    {
    }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; set; }

    [JsonPropertyName("blocks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<IMessageBlock>? Blocks
    {
        get;
        set => field = ThrowIfExceedsCount(value, MaxBlocks);
    }

    [JsonPropertyName("thread_ts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ThreadTs { get; set; }

    [JsonPropertyName("mrkdwn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Mrkdwn { get; set; }
}