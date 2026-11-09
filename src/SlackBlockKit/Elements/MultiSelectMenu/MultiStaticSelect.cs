using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.MultiSelectMenu;

/// <summary>
/// Represents a multi-select menu element with a static list of options.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/multi-select-menu-element#static_multi_select">Slack Multi-Select Menu Static Options Documentation</seealso>
public class MultiStaticSelect : MultiSelectMenuBase
{
    public MultiStaticSelect(
        string? actionId = null,
        List<OptionObject>? options = null,
        List<OptionGroupObject>? optionGroups = null,
        List<OptionObject>? initialOptions = null,
        ConfirmationDialogObject? confirm = null,
        int? maxSelectedItems = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, maxSelectedItems)
    {
        OptionValidation.ValidateOptionsAndOptionGroups(options, optionGroups, nameof(MultiStaticSelect));

        if (options != null) Options = options;
        if (optionGroups != null) OptionGroups = optionGroups;
        if (initialOptions != null) InitialOptions = initialOptions;
        FocusOnLoad = focusOnLoad;
    }

    public override string Type => "multi_static_select";

    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OptionObject>? Options
    {
        get;
        init
        {
            ThrowIfEmptyOrExceedsCount(value, OptionValidation.MaxOptions);
            OptionValidation.ValidateOptionsAndOptionGroups(value, OptionGroups, nameof(MultiStaticSelect));
            OptionValidation.ValidateInitialOptions(value, OptionGroups, InitialOptions);
            field = value;
        }
    }

    [JsonPropertyName("option_groups")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OptionGroupObject>? OptionGroups
    {
        get;
        init
        {
            ThrowIfEmptyOrExceedsCount(value, OptionValidation.MaxOptions);
            OptionValidation.ValidateOptionsAndOptionGroups(Options, value, nameof(MultiStaticSelect));
            OptionValidation.ValidateInitialOptions(Options, value, InitialOptions);
            field = value;
        }
    }

    [JsonPropertyName("initial_options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OptionObject>? InitialOptions
    {
        get;
        init
        {
            ThrowIfExceedsCount(value, OptionValidation.MaxOptions);
            OptionValidation.ValidateInitialOptions(Options, OptionGroups, value);
            field = value;
        }
    }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}