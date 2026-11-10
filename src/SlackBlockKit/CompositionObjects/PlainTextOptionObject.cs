using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Represents an option in a select or multi-select menu. These menus only support <c>plain_text</c>.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/option-object">Slack Option Object Documentation</seealso>
public class PlainTextOptionObject : OptionObject
{
    [JsonConstructor]
    public PlainTextOptionObject(PlainTextObject text, string value, PlainTextObject? description = null)
        : base(value)
    {
        Text = text;
        Description = description;
    }

    public PlainTextOptionObject(string text, string value, string? description = null)
        : this(new PlainTextObject(text), value, description != null ? new PlainTextObject(description) : null)
    {
    }

    [JsonPropertyName("text")]
    public PlainTextObject Text
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxTextLength);
    }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Description
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxDescriptionLength);
    }

    internal override TextObject OptionText => Text;

    internal override TextObject? OptionDescription => Description;
}
