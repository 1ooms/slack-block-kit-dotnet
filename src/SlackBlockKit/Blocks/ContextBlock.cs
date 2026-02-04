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

    public override string Type => "context";

    [JsonPropertyName("elements")]
    public List<IContextElement> Elements
    {
        get;
        set
        {
            if (value.Count > MaxElements)
                throw new ArgumentException($"ContextBlock cannot contain more than {MaxElements} elements.",
                    nameof(value));

            field = value;
        }
    }
}