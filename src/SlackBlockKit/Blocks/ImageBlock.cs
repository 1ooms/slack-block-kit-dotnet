using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents an image block (<c>type: "image"</c>) that displays a simple image, with an optional title and alt text.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/image-block">Slack Image Block Documentation</seealso>
public class ImageBlock : Block, IMessageBlock, IModalBlock, IHomeBlock
{
    private const int MaxAltTextLength = 2000;
    private const int MaxImageUrlLength = 3000;
    private const int MaxTitleLength = 2000;

    public ImageBlock(
        string altText,
        string? imageUrl = null,
        SlackFileObject? slackFile = null,
        PlainTextObject? title = null,
        string? blockId = null)
    {
        ValidateImageSource(imageUrl, slackFile);

        AltText = altText;
        if (imageUrl != null) ImageUrl = imageUrl;
        if (slackFile != null) SlackFile = slackFile;
        Title = title;
        BlockId = blockId;
    }

    public override string Type => "image";

    [JsonPropertyName("alt_text")]
    public string AltText
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxAltTextLength);
    }

    /// <remarks>
    /// At least one of <see cref="ImageUrl"/> or <see cref="SlackFile"/> must be set.
    /// If both are set, <see cref="ImageUrl"/> takes precedence over <see cref="SlackFile"/>.
    /// </remarks>
    [JsonPropertyName("image_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImageUrl
    {
        get;
        set
        {
            ThrowIfEmptyOrExceedsLength(value, MaxImageUrlLength);

            // The Slack API rejects a whitespace-only image_url for image blocks. ("Downloading image failed").
            if (value != null && string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ImageUrl cannot be whitespace.", nameof(ImageUrl));

            ValidateImageSource(value, SlackFile);
            field = value;
        }
    }

    /// <remarks>
    /// At least one of <see cref="ImageUrl"/> or <see cref="SlackFile"/> must be set.
    /// If both are set, <see cref="ImageUrl"/> takes precedence over <see cref="SlackFile"/>.
    /// </remarks>
    [JsonPropertyName("slack_file")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SlackFileObject? SlackFile
    {
        get;
        set
        {
            ValidateImageSource(ImageUrl, value);
            field = value;
        }
    }

    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Title
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxTitleLength);
    }

    public static ImageBlock FromImageUrl(string altText, string imageUrl, string? title = null,
        string? blockId = null)
    {
        return new ImageBlock(altText, imageUrl, title: title != null ? new PlainTextObject(title) : null,
            blockId: blockId);
    }

    public static ImageBlock FromSlackFile(string altText, SlackFileObject slackFile, string? title = null,
        string? blockId = null)
    {
        return new ImageBlock(altText, slackFile: slackFile, title: title != null ? new PlainTextObject(title) : null,
            blockId: blockId);
    }

    private static void ValidateImageSource(string? imageUrl, SlackFileObject? slackFile)
    {
        if (imageUrl == null && slackFile == null)
            throw new ArgumentException("Either ImageUrl or SlackFile must be provided.");
    }
}