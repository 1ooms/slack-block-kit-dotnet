using System.Text.Json.Serialization;

namespace SlackBlockKit.TextObjects;

/// <summary>
/// Represents a markdown text object in Slack Block Kit (<c>type: "mrkdwn"</c>).
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/text#mrkdwn">Slack mrkdwn Object Documentation</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(MarkdownTextObject), "mrkdwn")]
public class MarkdownTextObject(string text, bool? verbatim = null) : TextObject(text)
{
    public override string Type => "mrkdwn";

    [JsonPropertyName("verbatim")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Verbatim { get; set; } = verbatim;

    public static implicit operator MarkdownTextObject(string text)
    {
        return new MarkdownTextObject(text);
    }
}