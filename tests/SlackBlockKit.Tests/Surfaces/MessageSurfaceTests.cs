namespace SlackBlockKit.Tests.Surfaces;

public class MessageSurfaceTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        List<IMessageBlock> blocks = [new HeaderBlock("Header")];

        var message = new MessageSurface("Text", blocks, "123.456", true);

        Assert.Equal("Text", message.Text);
        Assert.Same(blocks, message.Blocks);
        Assert.Equal("123.456", message.ThreadTs);
        Assert.True(message.Mrkdwn);
    }

    [Fact]
    public void ParamsConstructor_SetsBlocks()
    {
        var message = new MessageSurface(new HeaderBlock("Header"), new DividerBlock());

        Assert.Null(message.Text);
        Assert.Equal(2, message.Blocks!.Count);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new MessageSurface("Hello", [new HeaderBlock("Header"), new SectionBlock("Text")],
            "123.456", false));

        AssertProperties(json, "text", "blocks", "thread_ts", "mrkdwn");
        Assert.Equal("Hello", json.GetProperty("text").GetString());
        Assert.Equal("header", json.GetProperty("blocks")[0].GetProperty("type").GetString());
        Assert.Equal("section", json.GetProperty("blocks")[1].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFieldsAndType()
    {
        AssertProperties(ToJson(new MessageSurface("Hello")), "text");
    }

    [Fact]
    public void Blocks_ThrowsWhenTooMany()
    {
        Assert.NotNull(new MessageSurface(blocks: (List<IMessageBlock>?)null));
        Assert.NotNull(
            new MessageSurface(blocks: Enumerable.Range(0, 50).Select(_ => (IMessageBlock)new DividerBlock()).ToList()));
        Assert.Throws<ArgumentException>(() =>
            new MessageSurface(blocks: Enumerable.Range(0, 51).Select(_ => (IMessageBlock)new DividerBlock()).ToList()));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var message = RoundTrip(new MessageSurface("Text", [new HeaderBlock("Header")], "123.456", true));

        Assert.Equal("Text", message.Text);
        Assert.Equal("Header", Assert.IsType<HeaderBlock>(Assert.Single(message.Blocks!)).Text.Text);
        Assert.Equal("123.456", message.ThreadTs);
        Assert.True(message.Mrkdwn);
    }
}