using System.Text.Json.Serialization;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

public class ContextBlock : Block
{
    private const int MaxElements = 10;

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