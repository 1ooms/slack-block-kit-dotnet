using System.Text.Json.Serialization;

namespace SlackBlockKit.CompositionObjects.TextObjects;

/// <summary>
/// Represents a plain text object in Slack Block Kit (<c>type: "plain_text"</c>).
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/text#plain_text">Slack plain_text Object Documentation</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PlainTextObject), "plain_text")]
public class PlainTextObject(string text, bool? emoji = null) : TextObject(text)
{
    public override string Type => "plain_text";

    [JsonPropertyName("emoji")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Emoji { get; set; } = emoji;

    public static implicit operator PlainTextObject(string text)
    {
        return new PlainTextObject(text);
    }
}