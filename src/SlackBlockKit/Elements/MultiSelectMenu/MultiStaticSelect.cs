using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.MultiSelectMenu;

/// <summary>
/// Represents a multi-select menu element with a static list of options.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/multi-select-menu-element#static_multi_select">Slack Multi-Select Menu Static Options Documentation</seealso>
public class MultiStaticSelect : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxOptions = 100;
    private const int MaxPlaceholderLength = 150;

    public MultiStaticSelect(
        string? actionId = null,
        List<OptionObject>? options = null,
        List<OptionGroupObject>? optionGroups = null,
        List<OptionObject>? initialOptions = null,
        ConfirmationDialogObject? confirm = null,
        int? maxSelectedItems = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
    {
        ValidateAtLeastOneAndNotBoth(options, optionGroups);
        ValidateInitialOptions(options, optionGroups, initialOptions);

        ActionId = actionId;
        if (options != null) Options = options;
        if (optionGroups != null) OptionGroups = optionGroups;
        if (initialOptions != null) InitialOptions = initialOptions;
        Confirm = confirm;
        MaxSelectedItems = maxSelectedItems;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
    }

    public override string Type => "multi_static_select";

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

    [JsonPropertyName("initial_options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OptionObject>? InitialOptions
    {
        get;
        init => field = ThrowIfExceedsCount(value, MaxOptions);
    }

    [JsonPropertyName("max_selected_items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxSelectedItems { get; set; }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }

    private static void ValidateAtLeastOneAndNotBoth(List<OptionObject>? options, List<OptionGroupObject>? optionGroups)
    {
        if (options == null && optionGroups == null)
            throw new ArgumentException("Either options or optionGroups must be provided for MultiStaticSelect.");

        if (options != null && optionGroups != null)
            throw new ArgumentException("Provide either options or optionGroups, but not both, for MultiStaticSelect.");
    }

    private static void ValidateInitialOptions(
        List<OptionObject>? options,
        List<OptionGroupObject>? optionGroups,
        List<OptionObject>? initialOptions)
    {
        if (initialOptions == null)
            return;

        foreach (var initialOption in initialOptions)
        {
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
                    "All OptionObjects within InitialOptions must match an OptionObject in Options or OptionGroups.Options.",
                    nameof(InitialOptions));
            }
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