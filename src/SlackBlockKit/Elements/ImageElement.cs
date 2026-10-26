using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents an image element that displays an image as part of a larger block of content.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/image-element">Slack Image Element Documentation</seealso>
public class ImageElement : Element, IContextElement, ISectionElement
{
    private const int MaxImageUrlLength = 3000;

    public ImageElement(string altText, string? imageUrl = null, SlackFileObject? slackFile = null)
    {
        ValidateAtLeastOne(imageUrl, slackFile);

        AltText = altText;
        ImageUrl = imageUrl;
        SlackFile = slackFile;
    }

    public override string Type => "image";


    [JsonPropertyName("alt_text")]
    public string AltText
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            field = value;
        }
    }

    [JsonPropertyName("image_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImageUrl
    {
        get;
        set
        {
            ValidateAtLeastOne(value, SlackFile);
            if (value != null)
            {
                ThrowIfEmpty(value);
                ThrowIfExceedsLength(value, MaxImageUrlLength);
            }

            field = value;
        }
    }

    [JsonPropertyName("slack_file")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SlackFileObject? SlackFile
    {
        get;
        set
        {
            ValidateAtLeastOne(ImageUrl, value);
            field = value;
        }
    }

    public static ImageElement FromImageUrl(string altText, string imageUrl)
    {
        return new ImageElement(altText: altText, imageUrl: imageUrl);
    }

    public static ImageElement FromSlackFile(string altText, SlackFileObject slackFile)
    {
        return new ImageElement(altText: altText, slackFile: slackFile);
    }

    private static void ValidateAtLeastOne(string? imageUrl, SlackFileObject? slackFile)
    {
        if (string.IsNullOrWhiteSpace(imageUrl) && slackFile == null)
        {
            throw new ArgumentException("Either imageUrl or slackFile must be provided for ImageElement.");
        }
    }
}