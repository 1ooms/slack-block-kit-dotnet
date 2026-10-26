using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements;

/// <summary>
/// Defines formatting styles (bold, italic, strike, highlight, underline, unlink, code) for rich text elements.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/text-element">Slack Text Element Documentation</seealso>
public class RichTextStyle(
    bool? bold = null,
    bool? italic = null,
    bool? strike = null,
    bool? highlight = null,
    bool? clientHighlight = null,
    bool? underline = null,
    bool? unlink = null,
    bool? code = null)
{
    [JsonPropertyName("bold")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Bold { get; set; } = bold;

    [JsonPropertyName("italic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Italic { get; set; } = italic;

    [JsonPropertyName("strike")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Strike { get; set; } = strike;

    [JsonPropertyName("highlight")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Highlight { get; set; } = highlight;

    [JsonPropertyName("client_highlight")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ClientHighlight { get; set; } = clientHighlight;

    [JsonPropertyName("underline")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Underline { get; set; } = underline;

    [JsonPropertyName("unlink")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Unlink { get; set; } = unlink;

    [JsonPropertyName("code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Code { get; set; } = code;
}