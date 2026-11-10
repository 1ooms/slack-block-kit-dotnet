namespace SlackBlockKit.Tests.CompositionObjects;

public class SlackIconObjectTests
{
    [Fact]
    public void Constructor_SetsName()
    {
        var icon = new SlackIconObject(SlackIcon.Rocket);

        Assert.Equal(SlackIcon.Rocket, icon.Name);
        Assert.Equal("icon", icon.Type);
    }

    [Fact]
    public void Serialize_WritesTypeAndKebabCaseName()
    {
        var json = ToJson(new SlackIconObject(SlackIcon.ThumbsUp));

        AssertProperties(json, "type", "name");
        Assert.Equal("icon", json.GetProperty("type").GetString());
        Assert.Equal("thumbs-up", json.GetProperty("name").GetString());
    }

    [Theory]
    [InlineData(SlackIcon.Archive, "archive")]
    [InlineData(SlackIcon.CaretLeft, "caret-left")]
    [InlineData(SlackIcon.EyeClosed, "eye-closed")]
    [InlineData(SlackIcon.NewWindow, "new-window")]
    [InlineData(SlackIcon.StarFilled, "star-filled")]
    public void Name_SerializesAsKebabCase(SlackIcon icon, string expected)
    {
        Assert.Equal(expected, ToJson(new SlackIconObject(icon)).GetProperty("name").GetString());
    }

    [Fact]
    public void ImplicitConversion_FromSlackIcon()
    {
        SlackIconObject icon = SlackIcon.Gear;

        Assert.Equal(SlackIcon.Gear, icon.Name);
    }

    [Fact]
    public void Deserialize_ReadsKebabCaseName()
    {
        var icon = SlackSerializer.Deserialize<SlackIconObject>("""{"type":"icon","name":"thumbs-down"}""");

        Assert.Equal(SlackIcon.ThumbsDown, icon!.Name);
    }
}
