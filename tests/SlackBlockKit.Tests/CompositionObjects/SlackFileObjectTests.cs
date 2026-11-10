namespace SlackBlockKit.Tests.CompositionObjects;

public class SlackFileObjectTests
{
    [Fact]
    public void FromId_SetsIdOnly()
    {
        var file = SlackFileObject.FromId("F0123ABCD");

        Assert.Equal("F0123ABCD", file.Id);
        Assert.Null(file.Url);
        AssertProperties(ToJson(file), "id");
    }

    [Fact]
    public void FromUrl_SetsUrlOnly()
    {
        var file = SlackFileObject.FromUrl("https://files.slack.com/file.png");

        Assert.Equal("https://files.slack.com/file.png", file.Url);
        Assert.Null(file.Id);
        var json = ToJson(file);
        AssertProperties(json, "url");
        Assert.Equal("https://files.slack.com/file.png", json.GetProperty("url").GetString());
    }

    [Fact]
    public void Constructor_ThrowsWhenNeitherProvided()
    {
        Assert.Throws<ArgumentException>(() => new SlackFileObject());
    }

    [Fact]
    public void Url_AcceptsWhitespaceButNotEmpty()
    {
        Assert.Equal(" ", SlackFileObject.FromUrl(" ").Url);
        Assert.Throws<ArgumentException>(() => SlackFileObject.FromUrl(""));
    }

    [Fact]
    public void Constructor_ThrowsWhenBothProvided()
    {
        Assert.Throws<ArgumentException>(() => new SlackFileObject("F0123ABCD", "https://example.com"));
    }

    [Fact]
    public void Constructor_TreatsWhitespaceUrlAsSet()
    {
        Assert.Throws<ArgumentException>(() => new SlackFileObject("F0123ABCD", " "));
    }

    [Fact]
    public void Initializer_ThrowsWhenBothEndUpSet()
    {
        Assert.Throws<ArgumentException>(() => new SlackFileObject(id: "F0123ABCD") { Url = "https://example.com" });
        Assert.Throws<ArgumentException>(() => new SlackFileObject(url: "https://example.com") { Id = "F0123ABCD" });
    }

    [Fact]
    public void Initializer_ThrowsWhenNeitherEndsUpSet()
    {
        Assert.Throws<ArgumentException>(() => new SlackFileObject(id: "F0123ABCD") { Id = null });
        Assert.Throws<ArgumentException>(() => new SlackFileObject(url: "https://example.com") { Url = null });
    }

    [Fact]
    public void Setters_AllowReplacingTheSameProperty()
    {
        var byId = SlackFileObject.FromId("F0123ABCD");
        byId.Id = "F9876WXYZ";
        Assert.Equal("F9876WXYZ", byId.Id);

        var byUrl = SlackFileObject.FromUrl("https://example.com/a.png");
        byUrl.Url = "https://example.com/b.png";
        Assert.Equal("https://example.com/b.png", byUrl.Url);
    }

    [Fact]
    public void Setters_ThrowWhenSettingTheOtherProperty()
    {
        var byId = SlackFileObject.FromId("F0123ABCD");
        Assert.Throws<ArgumentException>(() => byId.Url = "https://example.com");
        Assert.Throws<ArgumentException>(() => byId.Id = null);
        Assert.Equal("F0123ABCD", byId.Id);
        Assert.Null(byId.Url);
    }

    [Fact]
    public void Deserialize_RoundTripsIdAndUrl()
    {
        var byId = SlackSerializer.Deserialize<SlackFileObject>(SlackSerializer.Serialize(SlackFileObject.FromId("F0123ABCD")))!;
        var byUrl = SlackSerializer.Deserialize<SlackFileObject>(
            SlackSerializer.Serialize(SlackFileObject.FromUrl("https://example.com/a.png")))!;

        Assert.Equal("F0123ABCD", byId.Id);
        Assert.Null(byId.Url);
        Assert.Equal("https://example.com/a.png", byUrl.Url);
        Assert.Null(byUrl.Id);
    }

    [Theory]
    [InlineData("F12345678")]
    [InlineData("F0ABCDEFGHIJ")]
    public void Id_AcceptsValidFileIds(string id)
    {
        Assert.Equal(id, new SlackFileObject(id).Id);
    }

    [Theory]
    [InlineData("F1234567")]
    [InlineData("G12345678")]
    [InlineData("f12345678")]
    [InlineData("F1234abcd")]
    public void Id_ThrowsOnInvalidFileIds(string id)
    {
        Assert.Throws<ArgumentException>(() => new SlackFileObject(id));
    }
}
