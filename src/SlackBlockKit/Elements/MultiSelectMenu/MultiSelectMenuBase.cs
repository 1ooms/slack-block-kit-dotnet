using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.MultiSelectMenu;

/// <summary>
/// Abstract base class for all multi-select menu elements in Slack Block Kit.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/multi-select-menu-element">Slack Multi-Select Menu Documentation</seealso>
public abstract class MultiSelectMenuBase : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxPlaceholderLength = 150;
    private const int MinMaxSelectedItems = 1;

    protected MultiSelectMenuBase(
        string? actionId = null,
        PlainTextObject? placeholder = null,
        ConfirmationDialogObject? confirm = null,
        int? maxSelectedItems = null)
    {
        ActionId = actionId;
        Placeholder = placeholder;
        Confirm = confirm;
        MaxSelectedItems = maxSelectedItems;
    }

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("max_selected_items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxSelectedItems
    {
        get;
        set
        {
            if (value != null && value < MinMaxSelectedItems)
            {
                throw new ArgumentException("MaxSelectedItems must be larger than 0", nameof(MaxSelectedItems));
            }

            ValidateInitialSelectionCount(InitialSelectionCount, value);
            field = value;
        }
    }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    protected virtual int? InitialSelectionCount => null;
    
    protected static void ValidateInitialSelectionCount(int? initialSelectionCount, int? maxSelectedItems)
    {
        if (initialSelectionCount > maxSelectedItems)
            throw new ArgumentException(
                $"The number of initially selected items ({initialSelectionCount}) cannot be greater than MaxSelectedItems ({maxSelectedItems}).");
    }
}