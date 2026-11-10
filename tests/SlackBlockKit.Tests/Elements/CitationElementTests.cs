namespace SlackBlockKit.Tests.Elements;

public class CitationElementTests
{
    private const string Url = "https://example.com/source";

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var details = new WebCitationDetails("Example");

        var element = new CitationElement(Url, "Source", 2, details, true, false);

        Assert.Equal(Url, element.Url);
        Assert.Equal("Source", element.Text);
        Assert.Equal(2, element.Index);
        Assert.Same(details, element.Details);
        Assert.True(element.FromLlm);
        Assert.False(element.IsSlackUrl);
        Assert.Equal("citation", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new CitationElement(Url, "Source", 2, new WebCitationDetails("Example"), true, false));

        AssertProperties(json, "type", "url", "text", "index", "details", "from_llm", "is_slack_url");
        Assert.Equal("citation", json.GetProperty("type").GetString());
        Assert.Equal(2, json.GetProperty("index").GetInt32());
        Assert.Equal("web", json.GetProperty("details").GetProperty("citation_type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new CitationElement(Url, "Source", 0, new MemoryCitationDetails("M1"))), "type",
            "url", "text", "index", "details");
    }

    [Fact]
    public void Url_ThrowsWhenNullOrEmpty()
    {
        var details = new MemoryCitationDetails("M1");

        Assert.Throws<ArgumentNullException>(() => new CitationElement(null!, "Source", 0, details));
        Assert.Throws<ArgumentException>(() => new CitationElement("", "Source", 0, details));
    }

    [Fact]
    public void Text_ThrowsWhenNullOrEmpty()
    {
        var details = new MemoryCitationDetails("M1");

        Assert.Throws<ArgumentNullException>(() => new CitationElement(Url, null!, 0, details));
        Assert.Throws<ArgumentException>(() => new CitationElement(Url, "", 0, details));
    }
}
