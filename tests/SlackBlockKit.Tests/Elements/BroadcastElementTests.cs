namespace SlackBlockKit.Tests.Elements;

public class BroadcastElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new BroadcastElement(BroadcastRange.Channel, style);

        Assert.Equal(BroadcastRange.Channel, element.Range);
        Assert.Same(style, element.Style);
        Assert.Equal("broadcast", element.Type);
    }

    [Theory]
    [InlineData(BroadcastRange.Here, "here")]
    [InlineData(BroadcastRange.Channel, "channel")]
    [InlineData(BroadcastRange.Everyone, "everyone")]
    public void Serialize_WritesRangeAsSnakeCase(BroadcastRange range, string expected)
    {
        var json = ToJson(new BroadcastElement(range));

        AssertProperties(json, "type", "range");
        Assert.Equal("broadcast", json.GetProperty("type").GetString());
        Assert.Equal(expected, json.GetProperty("range").GetString());
    }

    [Fact]
    public void Serialize_WritesStyle()
    {
        var json = ToJson(new BroadcastElement(BroadcastRange.Here, new RichTextStyle(italic: true)));

        Assert.True(json.GetProperty("style").GetProperty("italic").GetBoolean());
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var element = RoundTrip(new BroadcastElement(BroadcastRange.Everyone, new RichTextStyle(bold: true)));

        Assert.Equal(BroadcastRange.Everyone, element.Range);
        Assert.True(element.Style!.Bold);
    }
}
