using System.Text.Json.Serialization;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

public class ContextActionsBlock : Block
{
    private const int MaxElements = 5;

    public ContextActionsBlock(List<IContextActionsElement> elements, string? blockId = null)
    {
        Elements = elements;
        BlockId = blockId;
    }

    public override string Type => "context_actions";

    [JsonPropertyName("elements")]
    public List<IContextActionsElement> Elements
    {
        get;
        set
        {
            if (value.Count > MaxElements)
                throw new ArgumentException($"ContextActionsBlock cannot contain more than {MaxElements} elements.",
                    nameof(value));

            field = value;
        }
    }
}