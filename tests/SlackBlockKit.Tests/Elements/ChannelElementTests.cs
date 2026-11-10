namespace SlackBlockKit.Tests.Elements;

public class ChannelElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new ChannelElement("C123", "tab", style, true);

        Assert.Equal("C123", element.ChannelId);
        Assert.Equal("tab", element.TabId);
        Assert.Same(style, element.Style);
        Assert.True(element.FromLlm);
        Assert.Equal("channel", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ChannelElement("C123", "tab", new RichTextStyle(bold: true), true));

        AssertProperties(json, "type", "channel_id", "tab_id", "style", "from_llm");
        Assert.Equal("channel", json.GetProperty("type").GetString());
        Assert.Equal("C123", json.GetProperty("channel_id").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new ChannelElement("C123")), "type", "channel_id");
    }

    [Fact]
    public void ChannelId_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ChannelElement(null!));
    }

    [Fact]
    public void TabId_ThrowsWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new ChannelElement("C123", ""));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var element = RoundTrip(new ChannelElement("C123", "tab", fromLlm: false));

        Assert.Equal("C123", element.ChannelId);
        Assert.Equal("tab", element.TabId);
        Assert.False(element.FromLlm);
    }
}
