using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a container block (<c>type: "container"</c>) that groups other blocks with optional collapsible controls and header styling.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/container-block">Slack Container Block Documentation</seealso>
public class ContainerBlock : Block, IMessageBlock, IHomeBlock
{
    private const int MaxTitleLength = 150;
    private const int MaxSubtitleLength = 150;
    private const int MaxChildBlocks = 10;

    public ContainerBlock(
        List<Block> childBlocks,
        PlainTextObject? title = null,
        RichTextBlock? richTextTitle = null,
        TextObject? subtitle = null,
        string? blockId = null,
        ContainerWidth? width = null,
        ImageElement? icon = null,
        bool? isCollapsible = null,
        bool? defaultCollapsed = null,
        bool? hasHeaderDivider = null)
    {
        ValidateTitle(title, richTextTitle);

        if (title != null) Title = title;
        if (richTextTitle != null) RichTextTitle = richTextTitle;
        Subtitle = subtitle;
        ChildBlocks = childBlocks;
        BlockId = blockId;
        Width = width;
        Icon = icon;
        IsCollapsible = isCollapsible;
        DefaultCollapsed = defaultCollapsed;
        HasHeaderDivider = hasHeaderDivider;
    }

    public override string Type => "container";

    /// <remarks>
    /// At least one of <see cref="Title"/> or <see cref="RichTextTitle"/> must be set.
    /// Both may be set, in which case Slack displays <see cref="RichTextTitle"/> and ignores <see cref="Title"/>.
    /// </remarks>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Title
    {
        get;
        set
        {
            ThrowIfExceedsLength(value, MaxTitleLength);
            ValidateTitle(value, RichTextTitle);
            field = value;
        }
    }

    /// <remarks>
    /// At least one of <see cref="Title"/> or <see cref="RichTextTitle"/> must be set.
    /// If both are set, <see cref="RichTextTitle"/> takes precedence over <see cref="Title"/>.
    /// </remarks>
    [JsonPropertyName("rich_text_title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextBlock? RichTextTitle
    {
        get;
        set
        {
            ValidateTitle(Title, value);
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

    [JsonPropertyName("child_blocks")]
    public List<Block> ChildBlocks
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsCount(value, MaxChildBlocks);
    }

    [JsonPropertyName("width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ContainerWidth? Width { get; set; }

    [JsonPropertyName("icon")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ImageElement? Icon { get; set; }

    [JsonPropertyName("is_collapsible")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsCollapsible { get; set; }

    [JsonPropertyName("default_collapsed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DefaultCollapsed { get; set; }

    [JsonPropertyName("has_header_divider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? HasHeaderDivider { get; set; }

    private static void ValidateTitle(PlainTextObject? title, RichTextBlock? richTextTitle)
    {
        if (title == null && richTextTitle == null)
            throw new ArgumentException("Either title or rich_text_title is required for a ContainerBlock.");
    }
}

/// <summary>
/// Specifies the width layout options for a container block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/container-block">Slack Container Block Documentation</seealso>
public enum ContainerWidth
{
    Narrow,
    Standard,
    Wide,
    Full
}