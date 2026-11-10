namespace SlackBlockKit.Tests.Elements;

public class CanvasMessageUnfurlElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(code: true);

        var element = new CanvasMessageUnfurlElement("123.456", "C123", style);

        Assert.Equal("123.456", element.RootMessageTs);
        Assert.Equal("C123", element.RootMessageChannel);
        Assert.Same(style, element.Style);
        Assert.Equal("canvas_message_unfurl", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new CanvasMessageUnfurlElement("123.456", "C123", new RichTextStyle(code: true)));

        AssertProperties(json, "type", "root_message_ts", "root_message_channel", "style");
        Assert.Equal("canvas_message_unfurl", json.GetProperty("type").GetString());
        Assert.Equal("123.456", json.GetProperty("root_message_ts").GetString());
        Assert.Equal("C123", json.GetProperty("root_message_channel").GetString());
    }

    [Fact]
    public void Serialize_OmitsStyleWhenNull()
    {
        AssertProperties(ToJson(new CanvasMessageUnfurlElement("123.456", "C123")), "type", "root_message_ts",
            "root_message_channel");
    }

    [Fact]
    public void RequiredStrings_ThrowWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new CanvasMessageUnfurlElement(null!, "C123"));
        Assert.Throws<ArgumentException>(() => new CanvasMessageUnfurlElement("", "C123"));
        Assert.Throws<ArgumentNullException>(() => new CanvasMessageUnfurlElement("123.456", null!));
        Assert.Throws<ArgumentException>(() => new CanvasMessageUnfurlElement("123.456", ""));
    }
}
