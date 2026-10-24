using System.Text.Json;
using System.Text.Json.Serialization;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Defines an object containing Slack icon information to be used within a card block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/slack-icon-object">Slack Icon Object Documentation</seealso>
public class SlackIconObject(SlackIcon name) : CompositionObject
{
    [JsonPropertyName("type")]
    public string Type => "icon";

    [JsonPropertyName("name")]
    [JsonConverter(typeof(SlackIconConverter))]
    public SlackIcon Name { get; set; } = name;

    public static implicit operator SlackIconObject(SlackIcon name) => new(name);
}

public class SlackIconConverter() : JsonStringEnumConverter<SlackIcon>(JsonNamingPolicy.KebabCaseLower);

/// <summary>
/// Defines the valid Slack icons.
/// </summary>
[JsonConverter(typeof(SlackIconConverter))]
public enum SlackIcon
{
    Archive,
    Book,
    Bookmark,
    Bot,
    Bug,
    Calendar,
    Call,
    CaretLeft,
    CaretRight,
    Check,
    Clipboard,
    Code,
    Comment,
    Compass,
    Copy,
    Cube,
    Download,
    Edit,
    Email,
    EyeClosed,
    EyeOpen,
    File,
    Flag,
    Folder,
    Gear,
    Globe,
    Heart,
    Help,
    Image,
    Info,
    Key,
    Lightbulb,
    Link,
    Map,
    Mobile,
    NewWindow,
    Pin,
    Plus,
    Refine,
    Refresh,
    Rocket,
    Save,
    Screen,
    Share,
    Sparkle,
    Star,
    StarFilled,
    Tag,
    ThumbsDown,
    ThumbsUp,
    Trash,
    Upload,
    User,
    Warning
}
