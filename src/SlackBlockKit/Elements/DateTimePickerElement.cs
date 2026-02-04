using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;

namespace SlackBlockKit.Elements;

public class DateTimePickerElement : Element, IActionsElement, IInputElement
{
    [JsonPropertyName("initial_date_time")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    private long? _initialDateTimeLong;

    public DateTimePickerElement(string? actionId = null, DateTime? initialDateTime = null,
        ConfirmationDialogObject? confirm = null, bool? focusOnLoad = null)
    {
        ActionId = actionId;
        InitialDateTime = initialDateTime;
        if (InitialDateTime != null) _initialDateTimeLong = ((DateTimeOffset)InitialDateTime).ToUnixTimeSeconds();
        Confirm = confirm;
        FocusOnLoad = focusOnLoad;
    }

    public override string Type => "datetimepicker";

    [JsonIgnore] public DateTime? InitialDateTime { get; set; }


    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}