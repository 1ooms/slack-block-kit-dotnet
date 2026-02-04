using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Elements;

public class DatePickerElement : Element, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxPlaceHolderLength = 150;

    [JsonPropertyName("initial_date")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    private string? _initialDateString;

    public DatePickerElement(string? actionId = null, DateTime? initialDate = null,
        ConfirmationDialogObject? confirm = null, bool? focusOnLoad = null, PlainTextObject? placeholder = null)
    {
        ActionId = actionId;
        InitialDate = initialDate;
        _initialDateString = InitialDate?.ToString("yyyy-MM-dd");
        Confirm = confirm;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
    }

    public override string Type => "datepicker";

    [JsonIgnore] public DateTime? InitialDate { get; set; }

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
            value?.MaxLength = MaxPlaceHolderLength;
            field = value;
        }
    }
}