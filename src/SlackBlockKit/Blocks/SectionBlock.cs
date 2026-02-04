using System.Text.Json.Serialization;
using SlackBlockKit.Elements;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Blocks;

public class SectionBlock : Block
{
    private const int MaxTextLength = 3000;
    private const int MaxFields = 10;
    private const int MaxFieldLength = 2000;

    public SectionBlock(TextObject? text = null, List<TextObject>? fields = null, ISectionElement? accessory = null,
        bool? expand = null)
    {
        Text = text;
        Fields = fields;
        Accessory = accessory;
        Expand = expand;

        if (Text == null && (Fields == null || Fields.Count == 0))
            throw new ArgumentException("Either Text or Fields must be provided.");
    }

    public override string Type => "section";

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextObject? Text
    {
        get;
        set
        {
            if (value != null && value.Text.Length > MaxTextLength)
                throw new ArgumentException($"Text length cannot exceed {MaxTextLength} characters.");
            field = value;
        }
    }

    [JsonPropertyName("fields")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<TextObject>? Fields
    {
        get;
        set
        {
            field = value;
            if (value == null) return;
            if (value.Count > MaxFields)
                throw new ArgumentException($"Number of fields cannot exceed {MaxFields}.");

            if (value.Any(textField => textField.Text.Length > MaxFieldLength))
                throw new ArgumentException(
                    $"Text length of each field cannot exceed {MaxFieldLength} characters.");
        }
    }

    [JsonPropertyName("accessory")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ISectionElement? Accessory { get; set; }

    [JsonPropertyName("expand")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Expand { get; set; }
}