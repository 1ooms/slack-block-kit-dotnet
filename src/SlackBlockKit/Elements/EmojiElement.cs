using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents an emoji element that displays an emoji.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/emoji-element">Slack Emoji Element Documentation</seealso>
public partial class EmojiElement : Element, IRichTextSubElement
{
    public EmojiElement(string name, string? unicode = null)
    {
        Name = name;
        Unicode = unicode;
    }

    [JsonPropertyName("name")]
    public string Name
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("unicode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Unicode
    {
        get;
        set
        {
            if (value != null && !UnicodeRegex().IsMatch(value))
                throw new ArgumentException(
                    $"Invalid Unicode '{value}'. Unicode must match regex pattern '^[0-9a-f-]+$'.",
                    nameof(value));
            field = value;
        }
    }

    public override string Type => "emoji";

    [GeneratedRegex("^[0-9a-f-]+$")]
    private static partial Regex UnicodeRegex();
}