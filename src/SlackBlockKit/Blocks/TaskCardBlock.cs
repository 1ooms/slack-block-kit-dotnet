using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a task card block (<c>type: "task_card"</c>) that displays a card for an individual task with status, details, and output.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/task-card-block">Slack Task Card Block Documentation</seealso>
public class TaskCardBlock : Block, IMessageBlock
{
    public TaskCardBlock(
        string taskId,
        string title,
        TaskCardStatus status,
        RichTextBlock? details = null,
        RichTextBlock? output = null,
        List<UrlSourceElement>? sources = null,
        SlackIcon? icon = null,
        bool? hideTitle = null,
        string? blockId = null)
    {
        TaskId = taskId;
        Title = title;
        Status = status;
        Details = details;
        Output = output;
        Sources = sources;
        Icon = icon;
        HideTitle = hideTitle;
        BlockId = blockId;
    }

    public override string Type => "task_card";

    [JsonPropertyName("task_id")]
    public string TaskId
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("title")]
    public string Title
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("status")]
    public TaskCardStatus Status
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("details")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextBlock? Details { get; set; }

    [JsonPropertyName("output")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextBlock? Output { get; set; }

    [JsonPropertyName("sources")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<UrlSourceElement>? Sources { get; set; }

    [JsonPropertyName("icon")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(SlackIconConverter))]
    public SlackIcon? Icon { get; set; }

    [JsonPropertyName("hide_title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? HideTitle { get; set; }
}

/// <summary>
/// Predefined status values for a task card block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/task-card-block">Slack Task Card Block Documentation</seealso>
public enum TaskCardStatus
{
    InProgress,
    Complete,
    Error
}