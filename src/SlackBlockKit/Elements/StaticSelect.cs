using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Elements;

public class StaticSelect(string actionId, List<OptionElement> options) : Element, IActionsElement
{
    [JsonPropertyName("action_id")] public string ActionId { get; set; } = actionId;
    [JsonPropertyName("options")] public List<OptionElement> Options { get; set; } = options;
    public override string Type => "static_select";
}

public class OptionElement(TextObject text, string value)
{
    [JsonPropertyName("text")] public TextObject Text { get; set; } = text;
    [JsonPropertyName("value")] public string Value { get; set; } = value;
}