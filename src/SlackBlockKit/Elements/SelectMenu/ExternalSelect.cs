using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.SelectMenu;

/// <summary>
/// Represents a select menu element with an external data source dynamically loaded via typeahead.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/select-menu-element#external_select">Slack External Select Menu Documentation</seealso>
public class ExternalSelect : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxPlaceholderLength = 150;

    public ExternalSelect(
        string? actionId = null,
        OptionObject? initialOption = null,
        int? minQueryLength = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
    {
        ActionId = actionId;
        InitialOption = initialOption;
        MinQueryLength = minQueryLength;
        Confirm = confirm;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
    }

    public override string Type => "external_select";

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("initial_option")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OptionObject? InitialOption { get; set; }

    [JsonPropertyName("min_query_length")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinQueryLength { get; set; }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}