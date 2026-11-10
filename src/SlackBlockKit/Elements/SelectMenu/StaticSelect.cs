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
        List<PlainTextOptionObject>? options = null,
        List<OptionGroupObject>? optionGroups = null,
        PlainTextOptionObject? initialOption = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, focusOnLoad)
    {
        OptionValidation.ValidateOptionsAndOptionGroups(options, optionGroups, nameof(StaticSelect));

        if (options != null) Options = options;
        if (optionGroups != null) OptionGroups = optionGroups;
        InitialOption = initialOption;
    }

    public override string Type => "static_select";

    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<PlainTextOptionObject>? Options
    {
        get;
        init
        {
            ThrowIfEmptyOrExceedsCount(value, OptionValidation.MaxOptions);
            OptionValidation.ValidateOptionsAndOptionGroups(value, OptionGroups, nameof(StaticSelect));
            OptionValidation.ValidateInitialOption(value, OptionGroups, InitialOption);
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
            OptionValidation.ValidateOptionsAndOptionGroups(Options, value, nameof(StaticSelect));
            OptionValidation.ValidateInitialOption(Options, value, InitialOption);
            field = value;
        }
    }

    [JsonPropertyName("initial_option")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextOptionObject? InitialOption
    {
        get;
        init
        {
            OptionValidation.ValidateInitialOption(Options, OptionGroups, value);
            field = value;
        }
    }
}