using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;

namespace SlackBlockKit.Elements;

public class CheckboxesElement : Element, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxOptions = 10;

    public CheckboxesElement(List<OptionObject> options, string? actionId = null, bool? focusOnLoad = null, List<OptionObject>? initialOptions = null,
        ConfirmationDialogObject? confirm = null)
    {
        Options = options;
        ActionId = actionId;
        FocusOnLoad = focusOnLoad;
        InitialOptions = initialOptions;
        Confirm = confirm;
        ValidateInitialOptions();
    }

    public override string Type => "checkboxes";

    [JsonPropertyName("options")]
    public List<OptionObject> Options
    {
        get;
        set
        {
            if (value.Count > MaxOptions)
                throw new ArgumentException($"Options cannot contain more than {MaxOptions} items.", nameof(value));
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
            field = value;
            ValidateInitialOptions();
        }
    }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }

    private void ValidateInitialOptions()
    {
        if (InitialOptions == null) return;

        foreach (var initialOption in InitialOptions)
        {
            if (!Options.Contains(initialOption))
            {
                throw new ArgumentException("All items in InitialOptions must be present in Options.", nameof(InitialOptions));
            }
        }
    }
}