using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.MultiSelectMenu;

/// <summary>
/// Represents a multi-select menu element with an external data source dynamically loaded via typeahead.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/multi-select-menu-element#external_multi_select">Slack Multi-Select Menu External Data Source Documentation</seealso>
public class MultiExternalSelect : MultiSelectMenuBase
{
    public MultiExternalSelect(
        string? actionId = null,
        int? minQueryLength = null,
        List<PlainTextOptionObject>? initialOptions = null,
        ConfirmationDialogObject? confirm = null,
        int? maxSelectedItems = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, maxSelectedItems)
    {
        MinQueryLength = minQueryLength;
        InitialOptions = initialOptions;
    }

    public override string Type => "multi_external_select";

    [JsonPropertyName("initial_options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<PlainTextOptionObject>? InitialOptions
    {
        get;
        set
        {
            ValidateInitialSelectionCount(value?.Count, MaxSelectedItems);
            field = value;
        }
    }

    protected override int? InitialSelectionCount => InitialOptions?.Count;

    [JsonPropertyName("min_query_length")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinQueryLength { get; set; }
}