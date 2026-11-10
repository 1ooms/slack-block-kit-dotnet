namespace SlackBlockKit.Tests.Elements;

public class TeamElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new TeamElement("T123", style);

        Assert.Equal("T123", element.TeamId);
        Assert.Same(style, element.Style);
        Assert.Equal("team", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new TeamElement("T123", new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "team_id", "style");
        Assert.Equal("team", json.GetProperty("type").GetString());
        Assert.Equal("T123", json.GetProperty("team_id").GetString());
    }

    [Fact]
    public void Serialize_OmitsStyleWhenNull()
    {
        AssertProperties(ToJson(new TeamElement("T123")), "type", "team_id");
    }

    [Fact]
    public void TeamId_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new TeamElement(null!));
    }
}
