using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a canvas user mention element that renders as a user mention in canvas content.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/canvas-user-mention-element">Slack Canvas User Mention Element Documentation</seealso>
public class CanvasUserMentionElement : Element, IRichTextSubElement
{
    public CanvasUserMentionElement(
        string userId,
        string? threadId = null,
        RichTextStyle? style = null)
    {
        UserId = userId;
        ThreadId = threadId;
        Style = style;
    }

    public override string Type => "canvas_user_mention";

    [JsonPropertyName("user_id")]
    public string UserId
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("thread_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ThreadId
    {
        get;
        set
        {
            if (value != null)
            {
                ThrowIfEmpty(value);
            }

            field = value;
        }
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }
}
