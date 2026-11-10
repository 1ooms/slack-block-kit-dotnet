namespace SlackBlockKit.Tests.Elements;

public class ColorElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new ColorElement("#FF0000", style);

        Assert.Equal("#FF0000", element.Value);
        Assert.Same(style, element.Style);
        Assert.Equal("color", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ColorElement("#FF0000", new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "value", "style");
        Assert.Equal("color", json.GetProperty("type").GetString());
        Assert.Equal("#FF0000", json.GetProperty("value").GetString());
    }

    [Fact]
    public void Serialize_OmitsStyleWhenNull()
    {
        AssertProperties(ToJson(new ColorElement("#FF0000")), "type", "value");
    }

    [Fact]
    public void Value_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new ColorElement(null!));
        Assert.Throws<ArgumentException>(() => new ColorElement(""));
    }
}
