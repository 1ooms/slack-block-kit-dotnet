using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a video block (<c>type: "video"</c>) that displays an embedded video player.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/video-block">Slack Video Block Documentation</seealso>
public class VideoBlock : Block
{
    private const int MaxAuthorNameLength = 50;
    private const int MaxDescriptionLength = 200;
    private const int MaxTitleLength = 200;

    public VideoBlock(
        string altText,
        PlainTextObject title,
        string thumbnailUrl,
        string videoUrl,
        string? titleUrl = null,
        string? providerName = null,
        string? authorName = null,
        PlainTextObject? description = null,
        string? providerIconUrl = null,
        string? blockId = null)
    {
        AltText = altText;
        Title = title;
        ThumbnailUrl = thumbnailUrl;
        VideoUrl = videoUrl;
        TitleUrl = titleUrl;
        ProviderName = providerName;
        AuthorName = authorName;
        Description = description;
        ProviderIconUrl = providerIconUrl;
        BlockId = blockId;
    }

    public VideoBlock(
        string altText,
        string title,
        string thumbnailUrl,
        string videoUrl,
        string? titleUrl = null,
        string? providerName = null,
        string? blockId = null)
        : this(
            altText,
            new PlainTextObject(title),
            thumbnailUrl,
            videoUrl,
            titleUrl,
            providerName,
            null,
            null,
            null,
            blockId)
    {
    }

    public override string Type => "video";

    [JsonPropertyName("alt_text")]
    public string AltText
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("author_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AuthorName
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxAuthorNameLength);
    }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Description
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxDescriptionLength);
    }

    [JsonPropertyName("provider_icon_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProviderIconUrl { get; set; }

    [JsonPropertyName("provider_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProviderName { get; set; }

    [JsonPropertyName("title")]
    public PlainTextObject Title
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxTitleLength);
    }

    [JsonPropertyName("title_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TitleUrl
    {
        get;
        set
        {
            ThrowIfExceedsLength(value, MaxTitleLength);
            field = ThrowIfNotValidHttpsUrl(value);
        }
    }

    [JsonPropertyName("thumbnail_url")]
    public string ThumbnailUrl
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    // TODO: Validate all requirements defined in https://docs.slack.dev/reference/block-kit/blocks/video-block
    [JsonPropertyName("video_url")]
    public string VideoUrl
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }
}