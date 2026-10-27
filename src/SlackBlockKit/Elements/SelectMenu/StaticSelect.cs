using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.SelectMenu;

/// <summary>
/// Represents a select menu element with a static list of options.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/select-menu-element#static_select">Slack Static Select Menu Documentation</seealso>
public class StaticSelect : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxOptions = 100;
    private const int MaxPlaceholderLength = 150;

    public StaticSelect(
        string? actionId = null,
        List<OptionObject>? options = null,
        List<OptionGroupObject>? optionGroups = null,
        OptionObject? initialOption = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
    {
        ValidateAtLeastOneAndNotBoth(options, optionGroups);
        ValidateInitialOption(options, optionGroups, initialOption);

        ActionId = actionId;
        Options = options;
        OptionGroups = optionGroups;
        InitialOption = initialOption;
        Confirm = confirm;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
    }

    public StaticSelect(
        PlainTextObject? placeholder,
        string? actionId = null,
        List<OptionObject>? options = null,
        List<OptionGroupObject>? optionGroups = null,
        OptionObject? initialOption = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null)
        : this(actionId, options, optionGroups, initialOption, confirm, focusOnLoad, placeholder)
    {
    }

    public override string Type => "static_select";

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OptionObject>? Options
    {
        get;
        init => field = ThrowIfEmptyOrExceedsCount(value, MaxOptions);
    }

    [JsonPropertyName("option_groups")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OptionGroupObject>? OptionGroups
    {
        get;
        init => field = ThrowIfEmptyOrExceedsCount(value, MaxOptions);
    }

    [JsonPropertyName("initial_option")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OptionObject? InitialOption { get; init; }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }

    private static void ValidateAtLeastOneAndNotBoth(List<OptionObject>? options, List<OptionGroupObject>? optionGroups)
    {
        if (options == null && optionGroups == null)
            throw new ArgumentException("Either options or optionGroups must be provided for StaticSelect.");

        if (options != null && optionGroups != null)
            throw new ArgumentException("Provide either options or optionGroups, but not both, for StaticSelect.");
    }

    private static void ValidateInitialOption(
        List<OptionObject>? options,
        List<OptionGroupObject>? optionGroups,
        OptionObject? initialOption)
    {
        if (initialOption == null)
            return;

        var matches = false;

        if (options != null)
        {
            matches = options.Any(option => OptionMatches(option, initialOption));
        }
        else if (optionGroups != null)
        {
            matches = optionGroups.Any(group => group.Options.Any(option => OptionMatches(option, initialOption)));
        }

        if (!matches)
        {
            throw new ArgumentException(
                "InitialOption must match one of the options in either Options or OptionGroups.Options.",
                nameof(InitialOption));
        }
    }

    private static bool OptionMatches(OptionObject option, OptionObject initialOption)
    {
        if (ReferenceEquals(option, initialOption))
            return true;

        if (option.Value != initialOption.Value)
            return false;

        if (option.Text.Text != initialOption.Text.Text)
            return false;

        return true;
    }
}