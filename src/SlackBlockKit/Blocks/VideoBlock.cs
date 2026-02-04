using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Blocks;

public class VideoBlock : Block
{
    private const int MaxAuthorNameLength = 50;

    private const int MaxDescriptionLength = 200;

    private const int MaxTitleLength = 200;

    public VideoBlock(string altText,
        PlainTextObject title, string thumbnailUrl,
        string videoUrl, string? titleUrl = null, string? providerName = null, string? authorName = null,
        PlainTextObject? description = null,
        string? providerIconUrl = null)
    {
        AltText = altText;
        AuthorName = authorName;
        Description = description;
        ProviderIconUrl = providerIconUrl;
        ProviderName = providerName;
        Title = title;
        TitleUrl = titleUrl;
        ThumbnailUrl = thumbnailUrl;
        VideoUrl = videoUrl;
    }

    public override string Type => "video";

    [JsonPropertyName("alt_text")] public string AltText { get; set; }

    [JsonPropertyName("author_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AuthorName
    {
        get;
        set
        {
            if (value != null && value.Length > MaxAuthorNameLength)
                throw new ArgumentException($"Author name cannot be longer than {MaxAuthorNameLength} characters.",
                    nameof(value));

            field = value;
        }
    }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Description
    {
        get;
        set
        {
            if (value != null && value.Text.Length > MaxDescriptionLength)
                throw new ArgumentException($"Description cannot be longer than {MaxDescriptionLength} characters.",
                    nameof(value));

            field = value;
        }
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
        set
        {
            if (value.Text.Length > MaxTitleLength)
                throw new ArgumentException($"Title cannot be longer than {MaxTitleLength} characters.",
                    nameof(value));

            field = value;
        }
    }

    [JsonPropertyName("title_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TitleUrl { get; set; }

    [JsonPropertyName("thumbnail_url")] public string ThumbnailUrl { get; set; }

    [JsonPropertyName("video_url")] public string VideoUrl { get; set; }
}