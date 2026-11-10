namespace SlackBlockKit.Tests.Elements;

public class WebCitationDetailsTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var details = new WebCitationDetails("Example", "Title", "Snippet");

        Assert.Equal("Example", details.DisplayName);
        Assert.Equal("Title", details.Title);
        Assert.Equal("Snippet", details.Snippet);
        Assert.Equal("web", details.CitationType);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new WebCitationDetails("Example", "Title", "Snippet"));

        AssertProperties(json, "citation_type", "display_name", "title", "snippet");
        Assert.Equal("web", json.GetProperty("citation_type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new WebCitationDetails("Example")), "citation_type", "display_name");
    }

    [Fact]
    public void DisplayName_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new WebCitationDetails(null!));
    }
}
