using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Represents an option in a checkbox group or radio button group.
/// Its text and description can be <c>plain_text</c> or <c>mrkdwn</c>.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/option-object">Slack Option Object Documentation</seealso>
public class TextOptionObject : OptionObject
{
    [JsonConstructor]
    public TextOptionObject(TextObject text, string value, TextObject? description = null)
        : base(value)
    {
        Text = text;
        Description = description;
    }
    
    public TextOptionObject(string text, string value, string? description = null)
        : this(new PlainTextObject(text), value, description != null ? new PlainTextObject(description) : null)
    {
    }

    [JsonPropertyName("text")]
    public TextObject Text
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxTextLength);
    }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextObject? Description
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxDescriptionLength);
    }

    internal override TextObject OptionText => Text;

    internal override TextObject? OptionDescription => Description;
}
