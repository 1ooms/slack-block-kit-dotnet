using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements.RichText;

/// <summary>
/// Represents a rich text section element that contains inline rich text elements.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/rich-text-section-element">Slack Rich Text Section Element Documentation</seealso>
public class RichTextSectionElement : IRichTextElement
{
    [JsonConstructor]
    public RichTextSectionElement(List<IRichTextSubElement> elements)
    {
        Elements = elements;
    }

    public RichTextSectionElement(params IRichTextSubElement[] elements)
        : this(elements.ToList())
    {
    }

    [JsonPropertyName("elements")]
    public List<IRichTextSubElement> Elements
    {
        get;
        set => field = ThrowIfNull(value);
    }

    public string Type => "rich_text_section";
}