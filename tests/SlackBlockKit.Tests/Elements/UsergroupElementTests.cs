namespace SlackBlockKit.Tests.Elements;

public class UsergroupElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new UsergroupElement("S123", style);

        Assert.Equal("S123", element.UsergroupId);
        Assert.Same(style, element.Style);
        Assert.Equal("usergroup", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new UsergroupElement("S123", new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "usergroup_id", "style");
        Assert.Equal("usergroup", json.GetProperty("type").GetString());
        Assert.Equal("S123", json.GetProperty("usergroup_id").GetString());
    }

    [Fact]
    public void Serialize_OmitsStyleWhenNull()
    {
        AssertProperties(ToJson(new UsergroupElement("S123")), "type", "usergroup_id");
    }

    [Fact]
    public void UsergroupId_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new UsergroupElement(null!));
        Assert.Throws<ArgumentException>(() => new UsergroupElement(""));
    }
}
