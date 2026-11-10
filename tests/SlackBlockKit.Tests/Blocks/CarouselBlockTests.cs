namespace SlackBlockKit.Tests.Blocks;

public class CarouselBlockTests
{
    private static CardBlock Card(string title = "Card") => new(title: new PlainTextObject(title));

    [Fact]
    public void ListConstructor_SetsProperties()
    {
        var cards = new List<CardBlock> { Card() };

        var block = new CarouselBlock(cards, "carousel_1");

        Assert.Same(cards, block.Elements);
        Assert.Equal("carousel_1", block.BlockId);
        Assert.Equal("carousel", block.Type);
    }

    [Fact]
    public void ParamsConstructor_CollectsCards()
    {
        Assert.Equal(2, new CarouselBlock(Card("a"), Card("b")).Elements.Count);
    }

    [Fact]
    public void Serialize_WritesCardsWithDiscriminators()
    {
        var json = ToJson(new CarouselBlock([Card("a"), Card("b")], "carousel_1"));

        AssertProperties(json, "type", "elements", "block_id");
        Assert.Equal("carousel", json.GetProperty("type").GetString());
        Assert.All(json.GetProperty("elements").EnumerateArray(),
            card => Assert.Equal("card", card.GetProperty("type").GetString()));
    }

    [Fact]
    public void Elements_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new CarouselBlock((List<CardBlock>)null!));
        Assert.Throws<ArgumentException>(() => new CarouselBlock());
    }

    [Fact]
    public void Elements_AllowsUpTo10()
    {
        var cards = Enumerable.Range(0, 10).Select(i => Card(i.ToString())).ToList();

        Assert.Equal(10, new CarouselBlock(cards).Elements.Count);

        cards.Add(Card());
        Assert.Throws<ArgumentException>(() => new CarouselBlock(cards));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new CarouselBlock([Card("a"), Card("b")], "carousel_1"));

        Assert.Equal(["a", "b"], block.Elements.Select(card => card.Title!.Text));
        Assert.Equal("carousel_1", block.BlockId);
    }
}
