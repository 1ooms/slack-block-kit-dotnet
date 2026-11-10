namespace SlackBlockKit.Tests.Blocks;

public class HeaderBlockTests
{
    [Fact]
    public void TextObjectConstructor_SetsProperties()
    {
        var text = new PlainTextObject("Header", true);

        var block = new HeaderBlock(text, "header_1", 2);

        Assert.Same(text, block.Text);
        Assert.Equal("header_1", block.BlockId);
        Assert.Equal(2, block.Level);
        Assert.Equal("header", block.Type);
    }

    [Fact]
    public void StringConstructor_WrapsText()
    {
        var block = new HeaderBlock("Header", level: 1);

        Assert.Equal("Header", block.Text.Text);
        Assert.Equal(1, block.Level);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new HeaderBlock("Header", "header_1", 2));

        AssertProperties(json, "type", "text", "block_id", "level");
        Assert.Equal("header", json.GetProperty("type").GetString());
        Assert.Equal("plain_text", json.GetProperty("text").GetProperty("type").GetString());
        Assert.Equal(2, json.GetProperty("level").GetInt32());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new HeaderBlock("Header")), "type", "text");
    }

    [Fact]
    public void Text_ThrowsWhenNullOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new HeaderBlock((PlainTextObject)null!));
        Assert.NotNull(new HeaderBlock(Chars(150)));
        Assert.Throws<ArgumentException>(() => new HeaderBlock(Chars(151)));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    public void Level_AcceptsValuesBetween1And4(int level)
    {
        Assert.Equal(level, new HeaderBlock("Header", level: level).Level);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public void Level_ThrowsWhenOutOfRange(int level)
    {
        Assert.Throws<ArgumentException>(() => new HeaderBlock("Header", level: level));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new HeaderBlock("Title", "header_1", 2));

        Assert.Equal("Title", block.Text.Text);
        Assert.Equal("header_1", block.BlockId);
        Assert.Equal(2, block.Level);
    }
}
