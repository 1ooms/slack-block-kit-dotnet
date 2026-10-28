using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a card block (<c>type: "card"</c>) that provides a structured, hierarchical layout for information with titles, images, and actions.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/card-block">Slack Card Block Documentation</seealso>
public class CardBlock : Block
{
    private const int MaxTitleLength = 150;
    private const int MaxSubtitleLength = 150;
    private const int MaxBodyLength = 200;
    private const int MaxSubtextLength = 200;
    private const int MaxActions = 3;

    public CardBlock(
        TextObject? title = null,
        TextObject? subtitle = null,
        TextObject? body = null,
        TextObject? subtext = null,
        ImageElement? heroImage = null,
        ImageElement? icon = null,
        SlackIconObject? slackIcon = null,
        List<ButtonElement>? actions = null,
        string? blockId = null)
    {
        if (heroImage == null && title == null && (actions == null || actions.Count == 0) && body == null)
            throw new ArgumentException(
                "At least one of hero_image, title, actions, or body is required for a CardBlock.");

        if (icon != null && slackIcon != null)
            throw new ArgumentException("Don't specify both icon and slackIcon for CardBlock.");

        Title = title;
        Subtitle = subtitle;
        Body = body;
        Subtext = subtext;
        HeroImage = heroImage;
        Icon = icon;
        SlackIcon = slackIcon;
        Actions = actions;
        BlockId = blockId;
    }

    public override string Type => "card";

    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextObject? Title
    {
        get;
        init => field = ThrowIfExceedsLength(value, MaxTitleLength);
    }

    [JsonPropertyName("subtitle")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextObject? Subtitle
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxSubtitleLength);
    }

    [JsonPropertyName("body")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextObject? Body
    {
        get;
        init => field = ThrowIfExceedsLength(value, MaxBodyLength);
    }

    [JsonPropertyName("subtext")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextObject? Subtext
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxSubtextLength);
    }

    [JsonPropertyName("hero_image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ImageElement? HeroImage { get; init; }

    [JsonPropertyName("icon")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ImageElement? Icon { get; init; }

    [JsonPropertyName("slack_icon")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SlackIconObject? SlackIcon { get; init; }

    [JsonPropertyName("actions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ButtonElement>? Actions
    {
        get;
        init => field = ThrowIfEmptyOrExceedsCount(value, MaxActions);
    }
}