using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a card block (<c>type: "card"</c>) that provides a structured, hierarchical layout for information with titles, images, and actions.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/card-block">Slack Card Block Documentation</seealso>
public class CardBlock : Block, IMessageBlock, IModalBlock, IHomeBlock
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
        ValidateRequiredContent(heroImage, title, actions, body);
        ValidateIcons(icon, slackIcon);
        
        if (title != null) Title = title;
        if (body != null) Body = body;
        if (heroImage != null) HeroImage = heroImage;
        if (actions != null) Actions = actions;
        if (icon != null) Icon = icon;
        if (slackIcon != null) SlackIcon = slackIcon;
        Subtitle = subtitle;
        Subtext = subtext;
        BlockId = blockId;
    }

    public override string Type => "card";

    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextObject? Title
    {
        get;
        set
        {
            ThrowIfExceedsLength(value, MaxTitleLength);
            ValidateRequiredContent(HeroImage, value, Actions, Body);
            field = value;
        }
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
        set
        {
            ThrowIfExceedsLength(value, MaxBodyLength);
            ValidateRequiredContent(HeroImage, Title, Actions, value);
            field = value;
        }
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
    public ImageElement? HeroImage
    {
        get;
        set
        {
            ValidateRequiredContent(value, Title, Actions, Body);
            field = value;
        }
    }

    [JsonPropertyName("icon")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ImageElement? Icon
    {
        get;
        set
        {
            ValidateIcons(value, SlackIcon);
            field = value;
        }
    }

    [JsonPropertyName("slack_icon")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SlackIconObject? SlackIcon
    {
        get;
        set
        {
            ValidateIcons(Icon, value);
            field = value;
        }
    }

    [JsonPropertyName("actions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ButtonElement>? Actions
    {
        get;
        set
        {
            ThrowIfEmptyOrExceedsCount(value, MaxActions);
            ValidateRequiredContent(HeroImage, Title, value, Body);
            field = value;
        }
    }

    private static void ValidateRequiredContent(
        ImageElement? heroImage,
        TextObject? title,
        List<ButtonElement>? actions,
        TextObject? body)
    {
        if (heroImage == null && title == null && (actions == null || actions.Count == 0) && body == null)
            throw new ArgumentException(
                "At least one of hero_image, title, actions, or body is required for a CardBlock.");
    }

    private static void ValidateIcons(ImageElement? icon, SlackIconObject? slackIcon)
    {
        if (icon != null && slackIcon != null)
            throw new ArgumentException("Don't specify both icon and slackIcon for CardBlock.");
    }
}