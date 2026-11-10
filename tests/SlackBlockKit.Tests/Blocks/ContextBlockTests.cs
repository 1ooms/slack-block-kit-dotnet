namespace SlackBlockKit.Tests.Blocks;

public class ContextBlockTests
{
    [Fact]
    public void ListConstructor_SetsProperties()
    {
        var elements = new List<IContextElement> { new PlainTextObject("Text") };

        var block = new ContextBlock(elements, "context_1");

        Assert.Same(elements, block.Elements);
        Assert.Equal("context_1", block.BlockId);
        Assert.Equal("context", block.Type);
    }

    [Fact]
    public void ParamsConstructor_CollectsElements()
    {
        var block = new ContextBlock(new PlainTextObject("a"), new MarkdownTextObject("b"), Image());

        Assert.Equal(3, block.Elements.Count);
    }

    [Fact]
    public void Serialize_WritesMixedElementsWithDiscriminators()
    {
        var json = ToJson(new ContextBlock([new PlainTextObject("a"), new MarkdownTextObject("*b*"), Image()],
            "context_1"));

        AssertProperties(json, "type", "elements", "block_id");
        Assert.Equal("context", json.GetProperty("type").GetString());
        Assert.Equal(["plain_text", "mrkdwn", "image"],
            json.GetProperty("elements").EnumerateArray().Select(e => e.GetProperty("type").GetString()));
    }

    [Fact]
    public void Serialize_WritesEachDiscriminatorOnce()
    {
        var json = ToJson(new ContextBlock(new PlainTextObject("a")));

        AssertProperties(json.GetProperty("elements")[0], "type", "text");
    }

    [Fact]
    public void Elements_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new ContextBlock((List<IContextElement>)null!));
        Assert.Throws<ArgumentException>(() => new ContextBlock());
    }

    [Fact]
    public void Elements_AllowsUpTo10()
    {
        var elements = Enumerable.Range(0, 10).Select(i => (IContextElement)new PlainTextObject($"{i}")).ToList();

        Assert.Equal(10, new ContextBlock(elements).Elements.Count);

        elements.Add(new PlainTextObject("extra"));
        Assert.Throws<ArgumentException>(() => new ContextBlock(elements));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new ContextBlock([new PlainTextObject("Text"), Image()], "context_1"));

        Assert.Equal("Text", Assert.IsType<PlainTextObject>(block.Elements[0]).Text);
        Assert.IsType<ImageElement>(block.Elements[1]);
        Assert.Equal("context_1", block.BlockId);
    }
}
