namespace SlackBlockKit.Tests.Elements;

public class ExternalCitationDetailsTests
{
    private const string IconUrl = "https://example.com/icon.png";

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var details = new ExternalCitationDetails("App", IconUrl);

        Assert.Equal("App", details.AppName);
        Assert.Equal(IconUrl, details.AppIconUrl);
        Assert.Equal("external", details.CitationType);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ExternalCitationDetails("App", IconUrl));

        AssertProperties(json, "citation_type", "app_name", "app_icon_url");
        Assert.Equal("external", json.GetProperty("citation_type").GetString());
        Assert.Equal("App", json.GetProperty("app_name").GetString());
        Assert.Equal(IconUrl, json.GetProperty("app_icon_url").GetString());
    }

    [Fact]
    public void RequiredProperties_ThrowWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ExternalCitationDetails(null!, IconUrl));
        Assert.Throws<ArgumentNullException>(() => new ExternalCitationDetails("App", null!));
    }
}
