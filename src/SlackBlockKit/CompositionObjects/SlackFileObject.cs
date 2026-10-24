using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Defines an object containing Slack file information to be used in an image block or element.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/slack-file">Slack File Object Documentation</seealso>
public partial class SlackFileObject : CompositionObject
{
    [GeneratedRegex(@"^F[A-Z0-9]{8,}$")]
    private static partial Regex FileIdRegex();

    public SlackFileObject(string? id = null, string? url = null)
    {
        if (string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Either id or url must be provided for SlackFileObject.");

        if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Provide either id or url, but not both, for SlackFileObject.");

        Id = id;
        Url = url;
    }

    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Id
    {
        get;
        set
        {
            if (value != null && !FileIdRegex().IsMatch(value))
                throw new ArgumentException(
                    $"Invalid Slack file ID '{value}'. Slack file IDs must match regex pattern '^[F][A-Z0-9]{{8,}}$'.",
                    nameof(value));

            field = value;
        }
    }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

    public static SlackFileObject FromId(string id)
    {
        return new SlackFileObject(id: id);
    }

    public static SlackFileObject FromUrl(string url)
    {
        return new SlackFileObject(url: url);
    }
}
