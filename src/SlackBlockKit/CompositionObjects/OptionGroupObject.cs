using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

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
        set => field = ThrowIfNullOrExceedsLength(value, MaxLabelLength);
    }

    [JsonPropertyName("options")]
    public List<OptionObject> Options
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsCount(value, MaxOptions);
    }
}