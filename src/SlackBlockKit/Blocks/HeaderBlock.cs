using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a header block (<c>type: "header"</c>) that displays larger, bold plain-text heading for content.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/header-block">Slack Header Block Documentation</seealso>
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