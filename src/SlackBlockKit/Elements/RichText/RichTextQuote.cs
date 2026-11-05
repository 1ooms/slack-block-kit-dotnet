using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements.RichText;

/// <summary>
/// Represents a rich text quote element that displays a block quote.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/rich-text-quote-element">Slack Rich Text Quote Element Documentation</seealso>
public class RichTextQuoteElement : RichTextElement
{
    [JsonConstructor]
    public RichTextQuoteElement(List<IRichTextSubElement> elements, int? border = null)
    {
        Elements = elements;
        Border = border;
    }

    public RichTextQuoteElement(params IRichTextSubElement[] elements)
        : this(elements.ToList())
    {
    }

    [JsonPropertyName("elements")]
    public List<IRichTextSubElement> Elements
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("border")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Border { get; set; }

    public override string Type => "rich_text_quote";
}