using System.Text.Json.Serialization;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a context block (<c>type: "context"</c>) that displays contextual information with small text and images.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/context-block">Slack Context Block Documentation</seealso>
public class ContextBlock : Block
{
    private const int MaxElements = 10;

    [JsonConstructor]
    public ContextBlock(List<IContextElement> elements, string? blockId = null)
    {
        Elements = elements;
        BlockId = blockId;
    }

    public ContextBlock(params IContextElement[] elements)
        : this(elements.ToList())
    {
    }

    public override string Type => "context";

    [JsonPropertyName("elements")]
    public List<IContextElement> Elements
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsCount(value, MaxElements);
    }
}