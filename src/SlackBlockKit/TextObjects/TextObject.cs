using System.Text.Json.Serialization;
using SlackBlockKit.Elements;

namespace SlackBlockKit.TextObjects;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PlainTextObject), "plain_text")]
[JsonDerivedType(typeof(RawTextObject), "raw_text")]
[JsonDerivedType(typeof(MarkdownTextObject), "mrkdwn")]
[JsonDerivedType(typeof(HeaderTextObject), "header")]
public abstract class TextObject(string text) : IContextElement
{
    [JsonPropertyName("text")] public virtual string Text { get; set; } = text;

    [JsonIgnore] public abstract string Type { get; }
}

public interface ITableTextObject;