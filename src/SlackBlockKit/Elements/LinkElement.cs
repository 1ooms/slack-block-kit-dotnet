using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a link element that displays an inline hyperlink.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/link-element">Slack Link Element Documentation</seealso>
public class LinkElement : Element, IRichTextPreformattedSubElement
{
    public LinkElement(
        string url,
        string? text = null,
        bool? @unsafe = null,
        bool? fromLlm = null,
        bool? isSlackUrl = null,
        bool? truncated = null,
        RichTextStyle? style = null)
    {
        Url = url;
        Text = text;
        Unsafe = @unsafe;
        FromLlm = fromLlm;
        IsSlackUrl = isSlackUrl;
        Truncated = truncated;
        Style = style;
    }

    [JsonPropertyName("url")]
    public string Url
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; set; }

    [JsonPropertyName("unsafe")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Unsafe { get; set; }

    [JsonPropertyName("from_llm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FromLlm { get; set; }

    [JsonPropertyName("is_slack_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsSlackUrl { get; set; }

    [JsonPropertyName("truncated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Truncated { get; set; }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "link";
}