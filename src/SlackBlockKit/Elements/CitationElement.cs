using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a citation element that renders as an AI citation referencing a file, external app, web page, message, or memory.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/citation-element">Slack Citation Element Documentation</seealso>
public class CitationElement : Element, IRichTextSubElement
{
    public CitationElement(
        string url,
        string text,
        int index,
        CitationDetails details,
        bool? fromLlm = null,
        bool? isSlackUrl = null)
    {
        Url = url;
        Text = text;
        Index = index;
        Details = details;
        FromLlm = fromLlm;
        IsSlackUrl = isSlackUrl;
    }

    [JsonPropertyName("url")]
    public string Url
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("text")]
    public string Text
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("index")]
    public int Index
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("details")] public CitationDetails Details { get; set; }

    [JsonPropertyName("from_llm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FromLlm { get; set; }

    [JsonPropertyName("is_slack_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsSlackUrl { get; set; }

    public override string Type => "citation";
}