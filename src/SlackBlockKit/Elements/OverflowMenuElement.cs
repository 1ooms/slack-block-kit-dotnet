using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents an overflow menu element that allows users to press a button to view a list of options.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/overflow-menu-element">Slack Overflow Menu Element Documentation</seealso>
public class OverflowMenuElement : ActionElement, IActionsElement, ISectionElement
{
    private const int MaxOptions = 5;

    public OverflowMenuElement(
        List<OptionObject> options,
        string? actionId = null,
        ConfirmationDialogObject? confirm = null)
    {
        Options = options;
        ActionId = actionId;
        Confirm = confirm;
    }

    public override string Type => "overflow";

    [JsonPropertyName("options")]
    public List<OptionObject> Options
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsCount(value, MaxOptions);
    }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }
}