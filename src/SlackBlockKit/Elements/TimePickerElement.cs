using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a time picker element that allows users to select a time of day from an interactive UI.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/time-picker-element">Slack Time Picker Element Documentation</seealso>
public class TimePickerElement : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxPlaceholderLength = 150;

    public TimePickerElement(
        string? actionId = null,
        DateTime? initialTime = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null,
        TimeZoneInfo? timezone = null)
    {
        ActionId = actionId;
        InitialTime = initialTime;
        Confirm = confirm;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
        Timezone = timezone;
    }

    public override string Type => "timepicker";

    [JsonIgnore] public DateTime? InitialTime { get; set; }

    [JsonPropertyName("initial_time")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    private string? InitialTimeString => InitialTime?.ToString("HH:mm");

    [JsonIgnore] public TimeZoneInfo? Timezone { get; set; }

    [JsonPropertyName("timezone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TimezoneString
    {
        get
        {
            if (Timezone == null)
            {
                return null;
            }

            if (Timezone.HasIanaId)
            {
                return Timezone.Id;
            }

            if (TimeZoneInfo.TryConvertWindowsIdToIanaId(Timezone.Id, out var ianaId))
            {
                return ianaId;
            }

            return Timezone.Id;
        }
    }

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
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }
}