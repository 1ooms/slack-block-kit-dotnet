using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Represents an option in an overflow menu. Overflow menus only support <c>plain_text</c>,
/// but unlike other menus their options can have a URL.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/option-object">Slack Option Object Documentation</seealso>
public class OverflowOptionObject : OptionObject
{
    private const int MaxUrlLength = 3000;

    [JsonConstructor]
    public OverflowOptionObject(PlainTextObject text, string value, PlainTextObject? description = null,
        string? url = null)
        : base(value)
    {
        Text = text;
        Description = description;
        Url = url;
    }

    public OverflowOptionObject(string text, string value, string? description = null, string? url = null)
        : this(new PlainTextObject(text), value, description != null ? new PlainTextObject(description) : null, url)
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

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url
    {
        get;
        set => field = ThrowIfEmptyOrExceedsLength(value, MaxUrlLength);
    }

    internal override TextObject OptionText => Text;

    internal override TextObject? OptionDescription => Description;
}
