namespace SlackBlockKit.Tests.Elements;

public class TagElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new TagElement("Urgent", TagColor.Red, style);

        Assert.Equal("Urgent", element.Text);
        Assert.Equal(TagColor.Red, element.Color);
        Assert.Same(style, element.Style);
        Assert.Equal("tag", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new TagElement("Urgent", TagColor.Red, new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "text", "color", "style");
        Assert.Equal("tag", json.GetProperty("type").GetString());
        Assert.Equal("red", json.GetProperty("color").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new TagElement("Urgent")), "type", "text");
    }

    [Theory]
    [InlineData(TagColor.Gray, "gray")]
    [InlineData(TagColor.Brown, "brown")]
    [InlineData(TagColor.Purple, "purple")]
    [InlineData(TagColor.Indigo, "indigo")]
    [InlineData(TagColor.Blue, "blue")]
    [InlineData(TagColor.Green, "green")]
    [InlineData(TagColor.Yellow, "yellow")]
    [InlineData(TagColor.Orange, "orange")]
    [InlineData(TagColor.Red, "red")]
    public void Color_SerializesAsLowercase(TagColor color, string expected)
    {
        Assert.Equal(expected, ToJson(new TagElement("t", color)).GetProperty("color").GetString());
    }

    [Fact]
    public void Text_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new TagElement(null!));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var element = RoundTrip(new TagElement("Urgent", TagColor.Indigo));

        Assert.Equal("Urgent", element.Text);
        Assert.Equal(TagColor.Indigo, element.Color);
    }
}
