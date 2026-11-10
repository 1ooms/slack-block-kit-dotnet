namespace SlackBlockKit.Tests.Elements;

public class TextElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new TextElement("Hello", style);

        Assert.Equal("Hello", element.Text);
        Assert.Same(style, element.Style);
        Assert.Equal("text", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new TextElement("Hello", new RichTextStyle(bold: true, code: true)));

        AssertProperties(json, "type", "text", "style");
        Assert.Equal("text", json.GetProperty("type").GetString());
        Assert.Equal("Hello", json.GetProperty("text").GetString());
        Assert.True(json.GetProperty("style").GetProperty("code").GetBoolean());
    }

    [Fact]
    public void Serialize_OmitsStyleWhenNull()
    {
        AssertProperties(ToJson(new TextElement("Hello")), "type", "text");
    }

    [Fact]
    public void Text_AllowsEmptyButNotNull()
    {
        Assert.Equal("", new TextElement("").Text);
        Assert.Throws<ArgumentNullException>(() => new TextElement(null!));
    }

    [Fact]
    public void RoundTrip_AsSubElement_ReturnsTextElement()
    {
        var element = SlackSerializer.Deserialize<IRichTextSubElement>(
            SlackSerializer.Serialize(new TextElement("Hello", new RichTextStyle(italic: true))));

        var text = Assert.IsType<TextElement>(element);
        Assert.Equal("Hello", text.Text);
        Assert.True(text.Style!.Italic);
    }
}
