using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a datetime picker element that allows users to select both a date and a time of day.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/datetime-picker-element">Slack Datetime Picker Element Documentation</seealso>
public class DateTimePickerElement : ActionElement, IActionsElement, IInputElement
{
    public DateTimePickerElement(
        string? actionId = null,
        DateTime? initialDateTime = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null)
    {
        ActionId = actionId;
        InitialDateTime = initialDateTime;
        Confirm = confirm;
        FocusOnLoad = focusOnLoad;
    }

    public override string Type => "datetimepicker";

    [JsonIgnore] public DateTime? InitialDateTime { get; set; }

    [JsonPropertyName("initial_date_time")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? InitialDateTimeSeconds =>
        InitialDateTime.HasValue ? ((DateTimeOffset)InitialDateTime.Value).ToUnixTimeSeconds() : null;

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}