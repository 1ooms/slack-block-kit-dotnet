using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Provides a way to group options in a select menu or multi-select menu.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/option-group">Slack Option Group Documentation</seealso>
public class OptionGroupObject : CompositionObject
{
    private const int MaxLabelLength = 75;
    private const int MaxOptions = 100;

    public OptionGroupObject(PlainTextObject label, List<OptionObject> options)
    {
        Label = label;
        Options = options;
    }

    public OptionGroupObject(string label, List<OptionObject> options)
        : this(new PlainTextObject(label), options)
    {
    }

    [JsonPropertyName("label")]
    public PlainTextObject Label
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value.Text.Length > MaxLabelLength)
                throw new ArgumentException($"Label text cannot exceed {MaxLabelLength} characters.", nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("options")]
    public List<OptionObject> Options
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value.Count == 0)
                throw new ArgumentException("Options cannot be an empty list.", nameof(value));
            if (value.Count > MaxOptions)
                throw new ArgumentException($"Options cannot contain more than {MaxOptions} items.", nameof(value));
            field = value;
        }
    }
}