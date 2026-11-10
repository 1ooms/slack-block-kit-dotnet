using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a section block (<c>type: "section"</c>) that displays text, two-column fields, and an optional accessory element.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/section-block">Slack Section Block Documentation</seealso>
public class SectionBlock : Block, IMessageBlock, IModalBlock, IHomeBlock
{
    private const int MaxTextLength = 3000;
    private const int MaxFields = 10;
    private const int MaxFieldLength = 2000;

    [JsonConstructor]
    public SectionBlock(
        TextObject? text = null,
        List<TextObject>? fields = null,
        ISectionElement? accessory = null,
        bool? expand = null,
        string? blockId = null)
    {
        if (text == null && (fields == null || fields.Count == 0))
            throw new ArgumentException("Either text or fields must be provided for SectionBlock.");

        Text = text;
        Fields = fields;
        Accessory = accessory;
        Expand = expand;
        BlockId = blockId;
    }

    public SectionBlock(string markdownText, ISectionElement? accessory = null, string? blockId = null)
        : this(new MarkdownTextObject(markdownText), null, accessory, null, blockId)
    {
    }

    public override string Type => "section";

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextObject? Text
    {
        get;
        init => field = ThrowIfExceedsLength(value, MaxTextLength);
    }

    [JsonPropertyName("fields")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<TextObject>? Fields
    {
        get;
        init
        {
            if (value != null)
            {
                ThrowIfEmptyOrExceedsCount(value, MaxFields);

                foreach (var text in value)
                {
                    ThrowIfExceedsLength(text, MaxFieldLength);
                }
            }

            field = value;
        }
    }

    [JsonPropertyName("accessory")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ISectionElement? Accessory { get; set; }

    [JsonPropertyName("expand")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Expand { get; set; }
}