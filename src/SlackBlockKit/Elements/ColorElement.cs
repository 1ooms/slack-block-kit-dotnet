using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a color element that displays a color swatch from a hex value.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/color-element">Slack Color Element Documentation</seealso>
public class ColorElement : Element, IRichTextSubElement
{
    public ColorElement(string value, RichTextStyle? style = null)
    {
        Value = value;
        Style = style;
    }

    public override string Type => "color";

    [JsonPropertyName("value")]
    public string Value
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }
}
