using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements.RichText;

/// <summary>
/// Represents a rich text preformatted code element.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/rich-text-preformatted-element">Slack Rich Text Preformatted Element Documentation</seealso>
public class RichTextPreformattedElement : IRichTextElement
{
    [JsonConstructor]
    public RichTextPreformattedElement(
        List<IRichTextPreformattedSubElement> elements,
        int? border = null,
        string? language = null)
    {
        Elements = elements;
        Border = border;
        Language = language;
    }

    public RichTextPreformattedElement(params IRichTextPreformattedSubElement[] elements)
        : this(elements.ToList())
    {
    }

    [JsonPropertyName("elements")]
    public List<IRichTextPreformattedSubElement> Elements
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            field = value;
        }
    }

    [JsonPropertyName("border")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Border { get; set; }
    
    [JsonPropertyName("language")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Language { get; set; }

    public string Type => "rich_text_preformatted";
}
