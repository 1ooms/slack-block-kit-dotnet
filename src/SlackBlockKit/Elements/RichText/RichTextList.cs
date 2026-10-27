using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements.RichText;

public enum RichTextListStyle
{
    Bullet,
    Ordered
}

/// <summary>
/// Represents a rich text list element that displays an ordered or bulleted list of rich text items.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/rich-text-list-element">Slack Rich Text List Element Documentation</seealso>
public class RichTextListElement : IRichTextElement
{
    [JsonConstructor]
    public RichTextListElement(
        List<RichTextSectionElement> elements,
        RichTextListStyle style = RichTextListStyle.Bullet,
        int? indent = null,
        int? offset = null,
        int? border = null)
    {
        Elements = elements;
        Style = style;
        Indent = indent;
        Offset = offset;
        Border = border;
    }

    public RichTextListElement(RichTextListStyle style, params RichTextSectionElement[] elements)
        : this(elements.ToList(), style)
    {
    }

    public RichTextListElement(params RichTextSectionElement[] elements)
        : this(elements.ToList())
    {
    }


    [JsonPropertyName("style")] public RichTextListStyle Style { get; set; }

    [JsonPropertyName("elements")]
    public List<RichTextSectionElement> Elements
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("indent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Indent { get; set; }

    [JsonPropertyName("offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Offset { get; set; }

    [JsonPropertyName("border")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Border { get; set; }

    public string Type => "rich_text_list";
}