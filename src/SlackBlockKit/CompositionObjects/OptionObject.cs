using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.CompositionObjects;

public class OptionObject(TextObject text, string value)
{
    [JsonPropertyName("text")] public TextObject Text { get; set; } = text;

    [JsonPropertyName("value")] public string Value { get; set; } = value;

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Description { get; set; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is OptionObject other) return Value == other.Value;
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}