using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

public class CarouselBlock : Block
{
    private const int MaxCards = 10;

    public CarouselBlock(List<CardBlock> elements, string? blockId = null)
    {
        Elements = elements;
        BlockId = blockId;
    }

    public CarouselBlock(params CardBlock[] elements)
        : this(elements.ToList())
    {
    }

    public override string Type => "carousel";

    [JsonPropertyName("elements")]
    public List<CardBlock> Elements
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsCount(value, MaxCards);
    }
}