using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.SelectMenu;

/// <summary>
/// Represents a select menu element with a static list of options.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/select-menu-element#static_select">Slack Static Select Menu Documentation</seealso>
public class StaticSelect : SelectMenuBase
{
    public StaticSelect(
        string? actionId = null,
        List<OptionObject>? options = null,
        List<OptionGroupObject>? optionGroups = null,
        OptionObject? initialOption = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, focusOnLoad)
    {
        StaticSelectValidation.ValidateOptionsAndOptionGroups(options, optionGroups, nameof(StaticSelect));
        StaticSelectValidation.ValidateInitialOption(options, optionGroups, initialOption);

        Options = options;
        OptionGroups = optionGroups;
        InitialOption = initialOption;
    }

    public override string Type => "static_select";

    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OptionObject>? Options
    {
        get;
        init => field = ThrowIfEmptyOrExceedsCount(value, StaticSelectValidation.MaxOptions);
    }

    [JsonPropertyName("option_groups")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OptionGroupObject>? OptionGroups
    {
        get;
        init => field = ThrowIfEmptyOrExceedsCount(value, StaticSelectValidation.MaxOptions);
    }

    [JsonPropertyName("initial_option")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OptionObject? InitialOption { get; init; }
}