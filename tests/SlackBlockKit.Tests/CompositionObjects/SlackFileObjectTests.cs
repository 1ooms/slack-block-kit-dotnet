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
        Assert.Throws<ArgumentException>(() => new SlackFileObject(" ", " "));
    }

    [Fact]
    public void Constructor_ThrowsWhenBothProvided()
    {
        Assert.Throws<ArgumentException>(() => new SlackFileObject("F0123ABCD", "https://example.com"));
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
