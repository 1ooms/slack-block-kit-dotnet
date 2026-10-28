using System.Text.Json.Serialization;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a context actions block (<c>type: "context_actions"</c>) that displays interactive action elements in a contextual layout.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/context-actions-block">Slack Context Actions Block Documentation</seealso>
public class ContextActionsBlock : Block
{
    private const int MaxElements = 5;

    public ContextActionsBlock(List<IContextActionsElement> elements, string? blockId = null)
    {
        Elements = elements;
        BlockId = blockId;
    }

    public ContextActionsBlock(params IContextActionsElement[] elements)
        : this(elements.ToList())
    {
    }

    public override string Type => "context_actions";

    [JsonPropertyName("elements")]
    public List<IContextActionsElement> Elements
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsCount(value, MaxElements);
    }
}