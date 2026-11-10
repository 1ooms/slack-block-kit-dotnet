namespace SlackBlockKit.Tests.Elements;

public class UserElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new UserElement("U123", style, true);

        Assert.Equal("U123", element.UserId);
        Assert.Same(style, element.Style);
        Assert.True(element.FromLlm);
        Assert.Equal("user", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new UserElement("U123", new RichTextStyle(bold: true), true));

        AssertProperties(json, "type", "user_id", "style", "from_llm");
        Assert.Equal("user", json.GetProperty("type").GetString());
        Assert.Equal("U123", json.GetProperty("user_id").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new UserElement("U123")), "type", "user_id");
    }

    [Fact]
    public void UserId_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new UserElement(null!));
        Assert.Throws<ArgumentException>(() => new UserElement(""));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var element = RoundTrip(new UserElement("U123", new RichTextStyle(bold: true)));

        Assert.Equal("U123", element.UserId);
        Assert.True(element.Style!.Bold);
    }
}
