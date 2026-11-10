namespace SlackBlockKit.Tests.Elements;

public class LinkElementTests
{
    private const string Url = "https://example.com";

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new LinkElement(Url, "Example", true, false, true, false, style);

        Assert.Equal(Url, element.Url);
        Assert.Equal("Example", element.Text);
        Assert.True(element.Unsafe);
        Assert.False(element.FromLlm);
        Assert.True(element.IsSlackUrl);
        Assert.False(element.Truncated);
        Assert.Same(style, element.Style);
        Assert.Equal("link", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new LinkElement(Url, "Example", true, false, true, false, new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "url", "text", "unsafe", "from_llm", "is_slack_url", "truncated", "style");
        Assert.Equal("link", json.GetProperty("type").GetString());
        Assert.Equal(Url, json.GetProperty("url").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new LinkElement(Url)), "type", "url");
    }

    [Fact]
    public void Url_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new LinkElement(null!));
        Assert.Throws<ArgumentException>(() => new LinkElement(""));
    }

    [Fact]
    public void Serialize_AsPreformattedSubElement_WritesDiscriminatorOnce()
    {
        var json = ToJson(new List<IRichTextPreformattedSubElement> { new LinkElement(Url) })[0];

        AssertProperties(json, "type", "url");
        Assert.Equal("link", json.GetProperty("type").GetString());
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var element = RoundTrip(new LinkElement(Url, "Example", @unsafe: true));

        Assert.Equal(Url, element.Url);
        Assert.Equal("Example", element.Text);
        Assert.True(element.Unsafe);
    }
}
