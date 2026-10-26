using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a checkboxes element that allows users to choose multiple items from a list of options.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/checkboxes-element">Slack Checkboxes Element Documentation</seealso>
public class CheckboxesElement : Element, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxOptions = 10;

    public CheckboxesElement(
        List<OptionObject> options,
        string? actionId = null,
        bool? focusOnLoad = null,
        List<OptionObject>? initialOptions = null,
        ConfirmationDialogObject? confirm = null,
        List<string>? visibleToUserIds = null)
    {
        ValidateInitialOptions(options, initialOptions);

        Options = options;
        ActionId = actionId;
        FocusOnLoad = focusOnLoad;
        InitialOptions = initialOptions;
        Confirm = confirm;
        VisibleToUserIds = visibleToUserIds;
    }

    public override string Type => "checkboxes";

    [JsonPropertyName("options")]
    public List<OptionObject> Options
    {
        get;
        set
        {
            ThrowIfNullOrEmpty(value);  
            ThrowIfExceedsCount(value, MaxOptions);
            field = value;
        }
    }

    [JsonPropertyName("initial_options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OptionObject>? InitialOptions
    {
        get;
        set
        {
            ValidateInitialOptions(Options, value);
            field = value;
        }
    }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
    
    [JsonPropertyName("visible_to_user_ids")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? VisibleToUserIds { get; set; }

    private void ValidateInitialOptions(List<OptionObject> options, List<OptionObject>? initialOptions)
    {
        if (initialOptions == null) return;
        ThrowIfExceedsCount(initialOptions, MaxOptions);

        foreach (var initialOption in initialOptions)
            if (!options.Contains(initialOption))
                throw new ArgumentException("All items in InitialOptions must be present in Options.",
                    nameof(InitialOptions));
    }
}