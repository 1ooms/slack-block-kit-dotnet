using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a rich text text element that displays text, optionally with styling.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/text-element">Slack Text Element Documentation</seealso>
public class TextElement : Element, IRichTextPreformattedSubElement
{
    public TextElement(
        string text,
        RichTextStyle? style = null)
    {
        Text = text;
        Style = style;
    }

    [JsonPropertyName("text")]
    public string Text
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "text";
}