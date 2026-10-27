using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a radio button group element that allows users to choose one item from a list of possible options.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/radio-button-group-element">Slack Radio Button Group Element Documentation</seealso>
public class RadioButtonGroupElement : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxOptions = 10;

    public RadioButtonGroupElement(
        List<OptionObject> options,
        string? actionId = null,
        OptionObject? initialOption = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null)
    {
        Options = options;
        ActionId = actionId;
        InitialOption = initialOption;
        Confirm = confirm;
        FocusOnLoad = focusOnLoad;
    }

    public override string Type => "radio_buttons";

    [JsonPropertyName("options")]
    public List<OptionObject> Options
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsCount(value, MaxOptions);
    }

    [JsonPropertyName("initial_option")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OptionObject? InitialOption { get; set; }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}