using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Defines an object containing Slack file information to be used in an image block or element.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/slack-file">Slack File Object Documentation</seealso>
public partial class SlackFileObject : CompositionObject
{
    public SlackFileObject(string? id = null, string? url = null)
    {
        ValidateAtLeastOneAndNotBoth(id, url);

        if (id != null) Id = id;
        if (url != null) Url = url;
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
                    nameof(Id));

            ValidateAtLeastOneAndNotBoth(value, Url);
            field = value;
        }
    }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url
    {
        get;
        set
        {
            // Whitespace-only URLs are deliberately allowed: the Slack API accepts them.
            ThrowIfEmpty(value);
            ValidateAtLeastOneAndNotBoth(Id, value);
            field = value;
        }
    }

    [GeneratedRegex(@"^F[A-Z0-9]{8,}$")]
    private static partial Regex FileIdRegex();

    public static SlackFileObject FromId(string id)
    {
        return new SlackFileObject(id: id);
    }

    public static SlackFileObject FromUrl(string url)
    {
        return new SlackFileObject(url: url);
    }

    private static void ValidateAtLeastOneAndNotBoth(string? id, string? url)
    {
        if (id == null && url == null)
            throw new ArgumentException("Either id or url must be provided for SlackFileObject.");

        if (id != null && url != null)
            throw new ArgumentException("Provide either id or url, but not both, for SlackFileObject.");
    }
}