using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Base class for an option in select menus, multi-select menus, overflow menus, checkboxes or radio button groups.
/// </summary>
/// <remarks>
/// Use <see cref="PlainTextOptionObject"/> for select and multi-select menus, which only support <c>plain_text</c>,
/// <see cref="OverflowOptionObject"/> for overflow menus, which only support <c>plain_text</c> but also allow a URL,
/// and <see cref="TextOptionObject"/> for checkboxes and radio buttons, which also support <c>mrkdwn</c>.
/// </remarks>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/option-object">Slack Option Object Documentation</seealso>
public abstract class OptionObject : CompositionObject
{
    protected const int MaxTextLength = 75;
    protected const int MaxDescriptionLength = 75;
    private const int MaxValueLength = 75;

    protected OptionObject(string value)
    {
        Value = value;
    }

    [JsonPropertyName("value")]
    public string Value
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsLength(value, MaxValueLength);
    }

    /// <summary>The option's text, regardless of which text object type the subclass allows.</summary>
    internal abstract TextObject OptionText { get; }

    /// <summary>The option's description, regardless of which text object type the subclass allows.</summary>
    internal abstract TextObject? OptionDescription { get; }
}
