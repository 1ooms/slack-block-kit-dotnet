using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a date picker element that allows users to select a date from a calendar-style UI.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/date-picker-element">Slack Date Picker Element Documentation</seealso>
public class DatePickerElement : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxPlaceholderLength = 150;

    public DatePickerElement(
        string? actionId = null,
        DateTime? initialDate = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
    {
        ActionId = actionId;
        InitialDate = initialDate;
        Confirm = confirm;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
    }

    public override string Type => "datepicker";

    [JsonIgnore] public DateTime? InitialDate { get; set; }

    [JsonPropertyName("initial_date")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InitialDateString => InitialDate?.ToString("yyyy-MM-dd");

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set
        {
            if (value != null)
            {
                ThrowIfExceedsLength(value.Text, MaxPlaceholderLength);
            }

            field = value;
        }
    }
}