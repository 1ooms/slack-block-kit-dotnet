using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a plan block (<c>type: "plan"</c>) that organizes and tracks a list of tasks.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/plan-block">Slack Plan Block Documentation</seealso>
public class PlanBlock : Block
{
    private const int MaxTasks = 50;

    public PlanBlock(string title, List<TaskCardBlock> tasks, string? blockId = null)
    {
        Title = title;
        Tasks = tasks;
        BlockId = blockId;
    }

    public PlanBlock(string title, params TaskCardBlock[] tasks)
        : this(title, tasks.ToList())
    {
    }

    public override string Type => "plan";

    [JsonPropertyName("title")]
    public string Title
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("tasks")]
    public List<TaskCardBlock> Tasks
    {
        get;
        set => field = ThrowIfNullOrExceedsCount(value, MaxTasks);
    }
}