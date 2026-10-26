using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Represents an option in select menus, multi-select menus, checkbox groups, radio button groups, or overflow menus.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/option">Slack Option Object Documentation</seealso>
public class OptionObject : CompositionObject
{
    private const int MaxTextLength = 75;
    private const int MaxValueLength = 75;
    private const int MaxDescriptionLength = 75;
    private const int MaxUrlLength = 3000;

    public OptionObject(TextObject text, string value, PlainTextObject? description = null, string? url = null)
    {
        Text = text;
        Value = value;
        Description = description;
        Url = url;
    }

    public OptionObject(string text, string value, string? description = null, string? url = null)
        : this(new PlainTextObject(text), value, description != null ? new PlainTextObject(description) : null, url)
    {
    }

    [JsonPropertyName("text")]
    public TextObject Text
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            ThrowIfExceedsLength(value.Text, MaxTextLength);
            field = value;
        }
    }

    [JsonPropertyName("value")]
    public string Value
    {
        get;
        set
        {
            ThrowIfNullOrEmpty(value);
            ThrowIfExceedsLength(value, MaxValueLength);
            field = value;
        }
    }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Description
    {
        get;
        set
        {
            if (value != null)
            {
                ThrowIfExceedsLength(value.Text, MaxDescriptionLength);
            }
            field = value;
        }
    }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url
    {
        get;
        set
        {
            if (value != null)
            {
                ThrowIfEmpty(value);
                ThrowIfExceedsLength(value, MaxUrlLength);
            }
            field = value;
        }
    }
}