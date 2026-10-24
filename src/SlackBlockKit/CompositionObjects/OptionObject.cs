using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

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
            if (value.Text.Length > MaxTextLength)
                throw new ArgumentException($"Option text cannot exceed {MaxTextLength} characters.", nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("value")]
    public string Value
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value.Length == 0) throw  new ArgumentException("Options cannot be an empty string.", nameof(value));
            if (value.Length > MaxValueLength)
                throw new ArgumentException($"Option value cannot exceed {MaxValueLength} characters.", nameof(value));
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
            if (value != null && value.Text.Length > MaxDescriptionLength)
                throw new ArgumentException($"Option description cannot exceed {MaxDescriptionLength} characters.",
                    nameof(value));
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
            if (value != null && value.Length > MaxUrlLength)
                throw new ArgumentException($"Option url cannot exceed {MaxUrlLength} characters.", nameof(value));
            if (value != null && value.Length == 0)
                throw new ArgumentException("Options cannot be an empty string.", nameof(value));
            field = value;
        }
    }
}