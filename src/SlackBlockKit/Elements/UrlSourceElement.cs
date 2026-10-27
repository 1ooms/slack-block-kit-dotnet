using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a URL source element for referencing within a task card block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/url-source-element">Slack URL Source Element Documentation</seealso>
public class UrlSourceElement : Element
{
    public UrlSourceElement(string url, string text)
    {
        Url = url;
        Text = text;
    }

    public override string Type => "url";

    [JsonPropertyName("url")]
    public string Url
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("text")]
    public string Text
    {
        get;
        set => field = ThrowIfNull(value);
    }
}