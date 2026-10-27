using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.MultiSelectMenu;

/// <summary>
/// Represents a multi-select menu element with an external data source dynamically loaded via typeahead.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/multi-select-menu-element#external_multi_select">Slack Multi-Select Menu External Data Source Documentation</seealso>
public class MultiExternalSelect : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxPlaceholderLength = 150;
    private const int MinMaxSelectedItems = 1;

    public MultiExternalSelect(
        string? actionId = null,
        int? minQueryLength = null,
        List<OptionObject>? initialOptions = null,
        ConfirmationDialogObject? confirm = null,
        int? maxSelectedItems = null,
        PlainTextObject? placeholder = null)
    {
        ActionId = actionId;
        MinQueryLength = minQueryLength;
        InitialOptions = initialOptions;
        Confirm = confirm;
        MaxSelectedItems = maxSelectedItems;
        Placeholder = placeholder;
    }

    public override string Type => "multi_external_select";

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("initial_options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OptionObject>? InitialOptions { get; set; }

    [JsonPropertyName("min_query_length")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinQueryLength { get; set; }

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
}