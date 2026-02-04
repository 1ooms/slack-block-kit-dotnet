using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ButtonElement), "button")]
[JsonDerivedType(typeof(CheckboxesElement), "checkboxes")]
[JsonDerivedType(typeof(DatePickerElement), "datepicker")]
[JsonDerivedType(typeof(DatePickerElement), "datetimepicker")]
[JsonDerivedType(typeof(EmailInputElement), "email_input")]
[JsonDerivedType(typeof(FeedbackButtonsElement), "feedback_buttons")]
[JsonDerivedType(typeof(FileInputElement), "file_input")]
[JsonDerivedType(typeof(IconButtonElement), "icon_button")]
[JsonDerivedType(typeof(ImageElement), "image")]
[JsonDerivedType(typeof(MultiSelectMenuElement), "multi_static_select")]
[JsonDerivedType(typeof(NumberInputElement), "number_input")]
[JsonDerivedType(typeof(OverflowMenuElement), "overflow")]
[JsonDerivedType(typeof(PlainTextInputElement), "plain_text_input")]
[JsonDerivedType(typeof(RadioButtonGroupElement), "radio_buttons")]
[JsonDerivedType(typeof(RichTextInputElement), "rich_text_input")]
[JsonDerivedType(typeof(SelectMenuElement), "select_menu")]
[JsonDerivedType(typeof(StaticSelect), "static_select")]
[JsonDerivedType(typeof(TimePickerElement), "timepicker")]
[JsonDerivedType(typeof(UrlInputElement), "url_input")]
[JsonDerivedType(typeof(WorkflowButtonElement), "workflow_button")]
public abstract class Element
{
    private const int MaxActionIdLength = 255;

    [JsonPropertyName("action_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ActionId
    {
        get;
        set
        {
            if (value != null && value.Length > MaxActionIdLength)
                throw new ArgumentException($"ActionId cannot be longer than {MaxActionIdLength} characters.",
                    nameof(value));

            field = value;
        }
    }

    [JsonIgnore] public abstract string Type { get; }
}

public interface IActionsElement;

public interface IContextElement;

public interface IContextActionsElement;

public interface IInputElement;

public interface ISectionElement;