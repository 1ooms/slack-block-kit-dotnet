using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Base class for all Slack Block Kit layout blocks.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks">Slack Blocks Documentation</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ActionsBlock), "actions")]
[JsonDerivedType(typeof(AlertBlock), "alert")]
[JsonDerivedType(typeof(CardBlock), "card")]
[JsonDerivedType(typeof(CarouselBlock), "carousel")]
[JsonDerivedType(typeof(ContainerBlock), "container")]
[JsonDerivedType(typeof(ContextActionsBlock), "context_actions")]
[JsonDerivedType(typeof(ContextBlock), "context")]
[JsonDerivedType(typeof(DataTableBlock), "data_table")]
[JsonDerivedType(typeof(DataVisualizationBlock), "data_visualization")]
[JsonDerivedType(typeof(DividerBlock), "divider")]
[JsonDerivedType(typeof(FileBlock), "file")]
[JsonDerivedType(typeof(HeaderBlock), "header")]
[JsonDerivedType(typeof(ImageBlock), "image")]
[JsonDerivedType(typeof(InputBlock), "input")]
[JsonDerivedType(typeof(MarkdownBlock), "markdown")]
[JsonDerivedType(typeof(PlanBlock), "plan")]
[JsonDerivedType(typeof(RichTextBlock), "rich_text")]
[JsonDerivedType(typeof(SectionBlock), "section")]
[JsonDerivedType(typeof(TableBlock), "table")]
[JsonDerivedType(typeof(TaskCardBlock), "task_card")]
[JsonDerivedType(typeof(VideoBlock), "video")]
public abstract class Block
{
    private const int MaxBlockIdLength = 255;

    [JsonPropertyName("block_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BlockId
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxBlockIdLength);
    }

    [JsonIgnore] public abstract string Type { get; }
}