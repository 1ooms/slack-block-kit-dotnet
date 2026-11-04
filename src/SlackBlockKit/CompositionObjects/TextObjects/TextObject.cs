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

    protected TextObject(string text, int maxLength = DefaultMaxLength)
    {
        MaxLength = maxLength;
        Text = text;
    }

    [JsonPropertyName("text")]
    public string Text
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsLength(value, MaxLength);
    }

    [JsonIgnore] private int MaxLength { get; }

    [JsonIgnore] public abstract string Type { get; }

    public override string ToString()
    {
        return Text;
    }
}