using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Specifies the color for a tag element.
/// </summary>
public enum TagColor
{
    Gray,
    Brown,
    Purple,
    Indigo,
    Blue,
    Green,
    Yellow,
    Orange,
    Red
}

/// <summary>
/// Represents a tag element that renders as a colored tag or pill.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/tag-element">Slack Tag Element Documentation</seealso>
public class TagElement : Element, IRichTextSubElement
{
    [JsonConstructor]
    public TagElement(
        string text,
        TagColor? color = null,
        RichTextStyle? style = null)
    {
        Text = text;
        Color = color;
        Style = style;
    }

    [JsonPropertyName("text")]
    public string Text
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TagColor? Color { get; set; }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "tag";
}