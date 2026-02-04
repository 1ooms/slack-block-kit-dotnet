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

    public override string Type => "actions";

    [JsonPropertyName("elements")]
    public List<IActionsElement> Elements
    {
        get;
        set
        {
            if (value.Count > MaxElements)
                throw new ArgumentException($"ActionsBlock cannot contain more than {MaxElements} elements.",
                    nameof(value));

            field = value;
        }
    }
}