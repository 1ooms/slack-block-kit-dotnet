using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Elements;

public class EmailInputElement : Element, IInputElement
{
    [JsonIgnore] private const int MaxPlaceholderLength = 150;

    [JsonPropertyName("dispatch_action_config")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DispatchActionConfigurationObject? DispatchActionConfig;

    [JsonPropertyName("focus_on_load")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad;

    [JsonPropertyName("initial_value")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InitialValue;

    public EmailInputElement(bool? focusOnLoad = null, string? initialValue = null,
        DispatchActionConfigurationObject? dispatchActionConfig = null, PlainTextObject? placeholder = null)
    {
        FocusOnLoad = focusOnLoad;
        InitialValue = initialValue;
        DispatchActionConfig = dispatchActionConfig;
        Placeholder = placeholder;
    }

    public override string Type => "email_text_input";

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set
        {
            value?.MaxLength = MaxPlaceholderLength;
            field = value;
        }
    }
}