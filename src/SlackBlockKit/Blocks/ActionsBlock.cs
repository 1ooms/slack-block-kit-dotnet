using System.Text.Json.Serialization;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

public class ActionsBlock : Block
{
    private const int MaxElements = 25;

    public ActionsBlock(List<IActionsElement> elements, string? blockId = null)
    {
        Elements = elements;
        BlockId = blockId;
    }

    public ActionsBlock(params IActionsElement[] elements)
        : this(elements.ToList())
    {
    }

    public override string Type => "actions";

    [JsonPropertyName("elements")]
    public List<IActionsElement> Elements
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsCount(value, MaxElements);
    }
}