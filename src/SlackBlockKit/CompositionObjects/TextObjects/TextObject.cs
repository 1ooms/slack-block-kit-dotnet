using System.Text.Json.Serialization;
using SlackBlockKit.Elements;

namespace SlackBlockKit.CompositionObjects.TextObjects;

/// <summary>
/// Base class representing a Slack Text composition object (<c>plain_text</c> or <c>mrkdwn</c>).
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/text">Slack Text Object Documentation</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PlainTextObject), "plain_text")]
[JsonDerivedType(typeof(MarkdownTextObject), "mrkdwn")]
public abstract class TextObject : IContextElement
{
    private const int DefaultMaxLength = 3000;
    private string _text;

    protected TextObject(string text, int maxLength = DefaultMaxLength)
    {
        ValidateText(text, maxLength);
        _text = text;
        MaxLength = maxLength;
    }

    [JsonPropertyName("text")]
    public string Text
    {
        get => _text;
        set
        {
            ValidateText(value, MaxLength);
            _text = value;
        }
    }

    [JsonIgnore] 
    private int MaxLength { get; }

    [JsonIgnore]
    public abstract string Type { get; }

    private static void ValidateText(string text, int maxLength)
    {
        ThrowIfNullOrEmpty(text);
        ThrowIfExceedsLength(text, maxLength);
    }

    public override string ToString()
    {
        return Text;
    }
}
