using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.SelectMenu;

/// <summary>
/// Represents a select menu element with an external data source dynamically loaded via typeahead.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/select-menu-element#external_select">Slack External Select Menu Documentation</seealso>
public class ExternalSelect : SelectMenuBase
{
    public ExternalSelect(
        string? actionId = null,
        OptionObject? initialOption = null,
        int? minQueryLength = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, focusOnLoad)
    {
        InitialOption = initialOption;
        MinQueryLength = minQueryLength;
    }

    public override string Type => "external_select";

    [JsonPropertyName("initial_option")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OptionObject? InitialOption { get; set; }

    [JsonPropertyName("min_query_length")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinQueryLength { get; set; }
}