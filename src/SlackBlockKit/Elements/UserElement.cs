using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a user element that renders as a mention of a Slack user.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/user-element">Slack User Element Documentation</seealso>
public class UserElement : Element, IRichTextSubElement
{
    public UserElement(
        string userId,
        RichTextStyle? style = null,
        bool? fromLlm = null)
    {
        UserId = userId;
        Style = style;
        FromLlm = fromLlm;
    }

    [JsonPropertyName("user_id")]
    public string UserId
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    [JsonPropertyName("from_llm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FromLlm { get; set; }

    public override string Type => "user";
}