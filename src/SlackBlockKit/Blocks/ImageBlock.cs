using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Blocks;

public class ImageBlock : Block
{
    private const int MaxAltTextLength = 2000;
    private const int MaxImageUrlLength = 3000;

    public ImageBlock(string altText, string? imageUrl = null, SlackFileObject? slackFile = null,
        PlainTextObject? title = null, string? blockId = null)
    {
        AltText = altText;
        ImageUrl = imageUrl;
        SlackFile = slackFile;
        Title = title;
        BlockId = blockId;

        if (ImageUrl == null && SlackFile == null)
            throw new ArgumentException("Either ImageUrl or SlackFile must be provided.");
    }

    public override string Type => "image";

    [JsonPropertyName("alt_text")]
    public string AltText
    {
        get;
        set
        {
            if (value.Length > MaxAltTextLength)
                throw new ArgumentException($"Text cannot be longer than {MaxAltTextLength} characters.",
                    nameof(AltText));

            field = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("image_url")]
    public string? ImageUrl
    {
        get;
        set
        {
            if (value != null && value.Length > MaxImageUrlLength)
                throw new ArgumentException($"ImageUrl cannot be longer than {MaxImageUrlLength} characters.",
                    nameof(ImageUrl));

            field = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("slack_file")]
    public SlackFileObject? SlackFile { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("title")]
    public PlainTextObject? Title
    {
        get;
        set
        {
            value?.MaxLength = 2000;
            value?.Text = value.Text;
            field = value;
        }
    }
}