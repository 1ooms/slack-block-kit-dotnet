namespace SlackBlockKit.Tests.Elements;

public class EmojiElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var element = new EmojiElement("smile", "1f604");

        Assert.Equal("smile", element.Name);
        Assert.Equal("1f604", element.Unicode);
        Assert.Equal("emoji", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new EmojiElement("smile", "1f604"));

        AssertProperties(json, "type", "name", "unicode");
        Assert.Equal("emoji", json.GetProperty("type").GetString());
        Assert.Equal("smile", json.GetProperty("name").GetString());
        Assert.Equal("1f604", json.GetProperty("unicode").GetString());
    }

    [Fact]
    public void Serialize_OmitsUnicodeWhenNull()
    {
        AssertProperties(ToJson(new EmojiElement("smile")), "type", "name");
    }

    [Fact]
    public void Name_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new EmojiElement(null!));
        Assert.Throws<ArgumentException>(() => new EmojiElement(""));
    }

    [Theory]
    [InlineData("1f604")]
    [InlineData("1f468-200d-1f4bb")]
    public void Unicode_AcceptsLowercaseHexWithDashes(string unicode)
    {
        Assert.Equal(unicode, new EmojiElement("x", unicode).Unicode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1F604")]
    [InlineData("u+1f604")]
    [InlineData("1f604 ")]
    public void Unicode_ThrowsOnInvalidValues(string unicode)
    {
        Assert.Throws<ArgumentException>(() => new EmojiElement("x", unicode));
    }
}
