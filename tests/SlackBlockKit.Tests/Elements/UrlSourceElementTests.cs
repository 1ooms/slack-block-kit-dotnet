namespace SlackBlockKit.Tests.Elements;

public class UrlSourceElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var element = new UrlSourceElement("https://example.com", "Example");

        Assert.Equal("https://example.com", element.Url);
        Assert.Equal("Example", element.Text);
        Assert.Equal("url", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new UrlSourceElement("https://example.com", "Example"));

        AssertProperties(json, "type", "url", "text");
        Assert.Equal("url", json.GetProperty("type").GetString());
    }

    [Fact]
    public void RequiredStrings_ThrowWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new UrlSourceElement(null!, "Example"));
        Assert.Throws<ArgumentNullException>(() => new UrlSourceElement("https://example.com", null!));
    }
}
