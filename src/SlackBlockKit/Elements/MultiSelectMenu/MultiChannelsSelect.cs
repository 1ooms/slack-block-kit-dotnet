using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.MultiSelectMenu;

/// <summary>
/// Represents a multi-select menu element that allows selecting multiple public channels in the workspace.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/multi-select-menu-element#channel_multi_select">Slack Multi-Select Menu Channels Documentation</seealso>
public class MultiChannelsSelect : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxPlaceholderLength = 150;
    private const int MinMaxSelectedItems = 1;

    public MultiChannelsSelect(
        string? actionId = null,
        List<string>? initialChannels = null,
        ConfirmationDialogObject? confirm = null,
        int? maxSelectedItems = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
    {
        ActionId = actionId;
        InitialChannels = initialChannels;
        Confirm = confirm;
        MaxSelectedItems = maxSelectedItems;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
    }

    public override string Type => "multi_channels_select";

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("initial_channels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? InitialChannels { get; set; }

    [JsonPropertyName("max_selected_items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxSelectedItems
    {
        get;
        set
        {
            if (value != null && value < MinMaxSelectedItems)
            {
                throw new ArgumentException($"MaxSelectedItems must be larger than 0", nameof(MaxSelectedItems));
            }

            field = value;
        }
    }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}