using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements.RichText;

/// <summary>
/// Represents a rich text quote element that displays a block quote.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/rich-text-quote-element">Slack Rich Text Quote Element Documentation</seealso>
public class RichTextQuoteElement : IRichTextElement
{
    [JsonConstructor]
    public RichTextQuoteElement(List<IRichTextElement> elements, int? border = null)
    {
        Elements = elements;
        Border = border;
    }

    public RichTextQuoteElement(params IRichTextElement[] elements)
        : this(elements.ToList())
    {
    }

    [JsonPropertyName("elements")]
    public List<IRichTextElement> Elements
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

    public string Type => "rich_text_quote";
}