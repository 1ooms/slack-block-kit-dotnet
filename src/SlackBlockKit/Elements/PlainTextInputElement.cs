using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Elements;

public class PlainTextInputElement(string actionId, TextObject placeholder, string initialValue = "")
    : Element, IInputElement
{
    [JsonPropertyName("action_id")] public string ActionId { get; set; } = actionId;
    [JsonPropertyName("placeholder")] public TextObject Placeholder { get; set; } = placeholder;
    [JsonPropertyName("initial_value")] public string InitialValue { get; set; } = initialValue;
    public override string Type => "plain_text_input";
}