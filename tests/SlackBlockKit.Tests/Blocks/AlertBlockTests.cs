namespace SlackBlockKit.Tests.Blocks;

public class AlertBlockTests
{
    [Fact]
    public void TextObjectConstructor_SetsProperties()
    {
        var text = new PlainTextObject("Careful");

        var block = new AlertBlock(text, AlertLevel.Warning, "alert_1");

        Assert.Same(text, block.Text);
        Assert.Equal(AlertLevel.Warning, block.Level);
        Assert.Equal("alert_1", block.BlockId);
        Assert.Equal("alert", block.Type);
    }

    [Fact]
    public void StringConstructor_UsesMarkdownAndDefaultLevel()
    {
        var block = new AlertBlock("*Careful*");

        Assert.IsType<MarkdownTextObject>(block.Text);
        Assert.Equal(AlertLevel.Default, block.Level);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new AlertBlock("*Careful*", AlertLevel.Error, "alert_1"));

        AssertProperties(json, "type", "text", "level", "block_id");
        Assert.Equal("alert", json.GetProperty("type").GetString());
        Assert.Equal("mrkdwn", json.GetProperty("text").GetProperty("type").GetString());
        Assert.Equal("error", json.GetProperty("level").GetString());
    }

    [Theory]
    [InlineData(AlertLevel.Default, "default")]
    [InlineData(AlertLevel.Info, "info")]
    [InlineData(AlertLevel.Warning, "warning")]
    [InlineData(AlertLevel.Error, "error")]
    [InlineData(AlertLevel.Success, "success")]
    public void Level_SerializesAsSnakeCase(AlertLevel level, string expected)
    {
        Assert.Equal(expected, ToJson(new AlertBlock("Text", level)).GetProperty("level").GetString());
    }

    [Fact]
    public void Text_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AlertBlock((TextObject)null!));
    }

    [Fact]
    public void Text_AllowsUpTo200Characters()
    {
        Assert.NotNull(new AlertBlock(Chars(200)));
        Assert.Throws<ArgumentException>(() => new AlertBlock(Chars(201)));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new AlertBlock("*Careful*", AlertLevel.Warning, "alert_1"));

        var text = Assert.IsType<MarkdownTextObject>(block.Text);
        Assert.Equal("*Careful*", text.Text);
        Assert.Equal(AlertLevel.Warning, block.Level);
        Assert.Equal("alert_1", block.BlockId);
    }
}
