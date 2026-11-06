using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a carousel block (<c>type: "carousel"</c>) that enables a horizontally scrollable sequence of card blocks.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/carousel-block">Slack Carousel Block Documentation</seealso>
public class CarouselBlock : Block
{
    private const int MaxCards = 10;

    [JsonConstructor]
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