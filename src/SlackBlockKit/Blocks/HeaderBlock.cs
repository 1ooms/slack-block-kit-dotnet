using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Blocks;

public class HeaderBlock : Block
{
    private const int MaxTextLength = 150;

    public HeaderBlock(PlainTextObject text, string? blockId = null)
    {
        Text = text;
        BlockId = blockId;
    }

    public override string Type => "header";

    [JsonPropertyName("text")]
    public PlainTextObject Text
    {
        get;
        set
        {
            if (value.Text.Length > MaxTextLength)
                throw new ArgumentException($"Text cannot be longer than {MaxTextLength} characters.",
                    nameof(Text));

            field = value;
        }
    }
}